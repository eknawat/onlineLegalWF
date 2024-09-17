using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Word;
using onlineLegalWF.Class;
using onlineLegalWF.userControls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static onlineLegalWF.Class.ReplaceLitigation;
using static onlineLegalWF.frmLitigation.LitigationDetail;

namespace onlineLegalWF.forms
{
    public partial class litapv : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public MargePDF zmergepdf = new MargePDF();
        public SendMail zsendmail = new SendMail();
        public ReplaceLitigation zreplacelitigation = new ReplaceLitigation();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string req = Request.QueryString["req"];
                string process_code = Request.QueryString["pc"];

                if (!string.IsNullOrEmpty(req) && !string.IsNullOrEmpty(process_code))
                {
                    setDataApprove(req, process_code);
                }

            }
        }
        private void setDataApprove(string req, string process_code)
        {
            string id = "";

            ucHeader1.setHeader(process_code + " Approve");
            if (process_code == "LIT" || process_code == "LIT_2")
            {
                string sqlinsreq = @"select [row_id],[process_id],[req_no],[document_no],[req_date],[lit_subject],[lit_desc]
                                  ,[tof_litigationreq_code],[status],[pro_occ_desc],li_req.[bu_code],li_req.[company_name],bu.[external_domain],[updated_datetime]
                                  from [li_litigation_request] as li_req
                                  left outer join li_business_unit as bu on li_req.bu_code = bu.bu_code
                                    where process_id='" + req + "'";
                var resinsreq = zdb.ExecSql_DataTable(sqlinsreq, zconnstr);

                //get data ins req
                if (resinsreq.Rows.Count > 0)
                {
                    id = resinsreq.Rows[0]["req_no"].ToString();
                    req_no.Value = resinsreq.Rows[0]["req_no"].ToString();
                    req_date.Text = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(resinsreq.Rows[0]["req_date"]), "en");
                    doc_no.Text = resinsreq.Rows[0]["document_no"].ToString();
                    subject.Text = resinsreq.Rows[0]["lit_subject"].ToString();
                    desc.Text = resinsreq.Rows[0]["lit_desc"].ToString();
                    var xtype_req = resinsreq.Rows[0]["tof_litigationreq_code"].ToString();
                    row_gv_data.Visible = false;
                    hid_external_domain.Value = resinsreq.Rows[0]["external_domain"].ToString();
                    hid_bucode.Value = resinsreq.Rows[0]["bu_code"].ToString();
                    string sql = @"select lit.[process_id],reqcase.[req_no],[case_no],[no],[contract_no],[bu_name],[customer_no],[customer_name],[customer_room],[overdue_desc],[outstanding_debt],[outstanding_debt_ack_of_debt],[fine_debt]
                                    ,[total_net],[retention_money],[total_after_retention_money],[remark],reqcase.[status],[assto_login],reqcase.[updated_datetime] 
                                    from [li_litigation_req_case] as reqcase 
                                    inner join li_litigation_request as lit on lit.req_no = reqcase.req_no 
                                    where reqcase.[req_no]='" + id + "'";

                    var res = zdb.ExecSql_DataTable(sql, zconnstr);
                    if (res.Rows.Count > 0)
                    {
                        row_gv_data.Visible = true;
                        List<LitigationCivilCaseData> listCivilCaseData = new List<LitigationCivilCaseData>();

                        foreach (DataRow item in res.Rows)
                        {
                            LitigationCivilCaseData civilCaseData = new LitigationCivilCaseData();
                            civilCaseData.req_no = item["req_no"].ToString();
                            civilCaseData.case_no = item["case_no"].ToString();
                            civilCaseData.no = item["no"].ToString();
                            civilCaseData.contract_no = item["contract_no"].ToString();
                            civilCaseData.bu_name = item["bu_name"].ToString();
                            civilCaseData.customer_no = item["customer_no"].ToString();
                            civilCaseData.customer_name = item["customer_name"].ToString();
                            civilCaseData.customer_room = item["customer_room"].ToString();
                            civilCaseData.overdue_desc = item["overdue_desc"].ToString();
                            civilCaseData.outstanding_debt = item["outstanding_debt"].ToString();
                            civilCaseData.outstanding_debt_ack_of_debt = item["outstanding_debt_ack_of_debt"].ToString();
                            civilCaseData.fine_debt = item["fine_debt"].ToString();
                            civilCaseData.total_net = item["total_net"].ToString();
                            civilCaseData.retention_money = item["retention_money"].ToString();
                            civilCaseData.total_after_retention_money = item["total_after_retention_money"].ToString();
                            civilCaseData.remark = item["remark"].ToString();
                            civilCaseData.status = item["status"].ToString();
                            civilCaseData.assto_login = item["assto_login"].ToString();
                            listCivilCaseData.Add(civilCaseData);
                        }
                        gvExcelFile.DataSource = listCivilCaseData;
                        gvExcelFile.DataBind();
                    }

                    //init data UcAttachAndCommentLogs
                    initDataAttachAndComment(resinsreq.Rows[0]["process_id"].ToString());

                    getDocument(id);
                }

                string st_name = Request.QueryString["st"];
                if (st_name == "Head of Litigation Assign") 
                {
                    btn_Approve.Visible = false;
                    btn_assign.Visible = true;
                }
            }
        }

        private void getDocument(string id)
        {
            string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

            var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

            if (resfile.Rows.Count > 0)
            {
                string pathfile = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                pdf_render.Attributes["src"] = host_url + "render/pdf?id=" + pathfile.Replace("+", @"%2B").Replace("&", @"%26");
            }
        }

        private void initDataAttachAndComment(string pid)
        {
            lblPID.Text = pid;
            hid_PID.Value = pid;
            ucAttachment1.ini_object(pid);
            ucCommentlog1.ini_object(pid);
        }

        protected void btn_Reject_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalReject();", true);
        }
        protected void btn_reject_submit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comment.Text))
            {
                // comment not found 
                Response.Write("<script> alert('Warning! Please input comment');</script>");

            }
            else
            {
                //insert comment
                string xpid = hid_PID.Value;
                string xcomment = comment.Text.Trim();
                string xcreate_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", new CultureInfo("en-US"));
                string xby_login = Session["user_login"].ToString();

                // insert into db
                string sql = @"INSERT INTO [dbo].[wf_comment_log] 
                                        ([pid],[comment],[by_login],[created_datetime])
                                         VALUES
                                               ('" + xpid + @"'
                                               ,'" + xcomment + @"'
                                               ,'" + xby_login + @"'
                                               ,'" + xcreate_date + @"')";
                zdb.ExecNonQuery(sql, zconnstr);

                //submit workflow
                string process_code = Request.QueryString["pc"];
                int version_no = 1;
                string xbu_code = hid_bucode.Value;

                // getCurrentStep
                var wfAttr = zwf.getCurrentStep(lblPID.Text, process_code, version_no);

                // check session_user
                if (Session["user_login"] != null)
                {
                    var xlogin_name = Session["user_login"].ToString();
                    var empFunc = new EmpInfo();

                    //get data user
                    var emp = empFunc.getEmpInfo(xlogin_name);

                    // set WF Attributes
                    wfAttr.subject = "เรื่อง " + subject.Text.Trim();
                    wfAttr.assto_login = emp.next_line_mgr_login;
                    wfAttr.wf_status = "REJECT";
                    wfAttr.submit_answer = "REJECT";
                    wfAttr.submit_by = wfAttr.submit_by;
                    wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, lblPID.Text, xbu_code);
                    wfAttr.updated_by = emp.user_login;

                    // wf.updateProcess
                    var wfA_NextStep = zwf.updateProcess(wfAttr);
                    wfA_NextStep.wf_status = "SAVE";
                    wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by, lblPID.Text, xbu_code);
                    string status = zwf.Insert_NextStep(wfA_NextStep);

                    if (status == "Success")
                    {
                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                    }

                }
            }

        }

        protected void btn_Approve_Click(object sender, EventArgs e)
        {
            string process_code = Request.QueryString["pc"];
            int version_no = 1;

            if (!string.IsNullOrEmpty(process_code))
            {

                // getCurrentStep
                var wfAttr = zwf.getCurrentStep(lblPID.Text, process_code, version_no);

                // check session_user
                if (Session["user_login"] != null)
                {
                    var xlogin_name = Session["user_login"].ToString();
                    var empFunc = new EmpInfo();

                    //get data user
                    var emp = empFunc.getEmpInfo(xlogin_name);

                    // set WF Attributes
                    wfAttr.external_domain = hid_external_domain.Value;
                    wfAttr.subject = subject.Text.Trim();
                    wfAttr.assto_login = emp.next_line_mgr_login;
                    wfAttr.wf_status = wfAttr.step_name + " Approved";
                    wfAttr.submit_answer = "APPROVED";
                    wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, wfAttr.process_id, hid_bucode.Value);
                    wfAttr.updated_by = emp.user_login;
                    wfAttr.submit_by = wfAttr.submit_by;
                    // wf.updateProcess
                    var wfA_NextStep = zwf.updateProcess(wfAttr);
                    wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by, wfAttr.process_id, hid_bucode.Value);
                    
                    if (wfAttr.step_name == "End")
                    {
                        wfA_NextStep.wf_status = "COMPLETED";
                    }

                    wfA_NextStep.submit_by = wfA_NextStep.submit_by;
                    string status = zwf.Insert_NextStep(wfA_NextStep);

                    if (status == "Success")
                    {
                        // check processcode loop gendocument
                        if (wfAttr.step_name == "Head of Treasury Operation Approve" || wfAttr.step_name == "Head AM Approve")
                        {
                            GenDocumnetLitigation(lblPID.Text, wfAttr.submit_by);
                            string subject = "";
                            string body = "";
                            string sql = @"select * from li_litigation_request where process_id = '" + wfAttr.process_id + "'";
                            var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                            if (dt.Rows.Count > 0)
                            {
                                var dr = dt.Rows[0];
                                string id = dr["req_no"].ToString();
                                subject = wfAttr.subject;
                                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                                body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้รับการอนุมัติผ่านระบบแล้ว กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

                                string pathfile = "";

                                string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                if (resfile.Rows.Count > 0)
                                {
                                    pathfile = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");

                                    string[] emailLitigation;

                                    var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                                    ////get mail from db
                                    if (isdev != "true")
                                    {
                                        //emailLitigation = new string[] { "aram.r@assetworldcorp-th.com", "supoj.k@assetworldcorp-th.com", "peeranat.u@assetworldcorp-th.com", "nuttanun.su@assetworldcorp-th.com", "supat.ku@assetworldcorp-th.com", "wiwek.s@assetworldcorp-th.com", "phooriwit.l@assetworldcorp-th.com", "nares.l@assetworldcorp-th.com" };
                                        emailLitigation = new string[] { "aram.r@assetworldcorp-th.com" };
                                    }
                                    else
                                    {
                                        ////fix mail test
                                        emailLitigation = new string[] { "legalwfuat2024@gmail.com", "manit.ch@assetworldcorp-th.com" };
                                    }

                                    if (emailLitigation.Length > 0)
                                    {
                                        _ = zsendmail.sendEmails(subject + " Mail To Head Of Litigation", emailLitigation, body, pathfile);
                                    }
                                }

                            }
                        }
                        else if (wfAttr.step_name == "GM Approve" && hid_external_domain.Value == "N")
                            {
                                GenDocumnetLitigation(lblPID.Text, wfAttr.submit_by);
                                string subject = "";
                                string body = "";
                                string sql = @"select * from li_litigation_request where process_id = '" + wfAttr.process_id + "'";
                                var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                                if (dt.Rows.Count > 0)
                                {
                                    var dr = dt.Rows[0];
                                    string id = dr["req_no"].ToString();
                                    subject = wfAttr.subject;
                                    var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                                    body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้รับการอนุมัติผ่านระบบแล้ว กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

                                    string pathfile = "";

                                    string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                    var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                    if (resfile.Rows.Count > 0)
                                    {
                                        pathfile = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");

                                        string[] emailLitigation;

                                        var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                                        ////get mail from db
                                        if (isdev != "true")
                                        {
                                            //emailLitigation = new string[] { "aram.r@assetworldcorp-th.com", "supoj.k@assetworldcorp-th.com", "peeranat.u@assetworldcorp-th.com", "nuttanun.su@assetworldcorp-th.com", "supat.ku@assetworldcorp-th.com", "wiwek.s@assetworldcorp-th.com", "phooriwit.l@assetworldcorp-th.com", "nares.l@assetworldcorp-th.com" };
                                            emailLitigation = new string[] { "aram.r@assetworldcorp-th.com" };
                                        }
                                        else
                                        {
                                            ////fix mail test
                                            emailLitigation = new string[] { "legalwfuat2024@gmail.com", "manit.ch@assetworldcorp-th.com" };
                                        }

                                        if (emailLitigation.Length > 0)
                                        {
                                            _ = zsendmail.sendEmails(subject + " Mail To Head Of Litigation", emailLitigation, body, pathfile);
                                        }
                                    }

                                }
                            }
                        else if (wfAttr.step_name == "GM Approve" || wfAttr.step_name == "AM Approve" && hid_external_domain.Value == "Y") 
                        {
                            GenDocumnetLitigation(lblPID.Text, wfAttr.submit_by);
                            //send mail
                            string subject = "";
                            string body = "";
                            string sqlmail = @"select * from li_litigation_request
                                        where process_id = '" + wfAttr.process_id + "'";
                            var dt = zdb.ExecSql_DataTable(sqlmail, zconnstr);
                            if (dt.Rows.Count > 0)
                            {
                                var dr = dt.Rows[0];
                                string id = dr["req_no"].ToString();
                                subject = wfAttr.subject;
                                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                                body = "คุณได้รับมอบหมายให้ตรวจสอบเอกสารเลขที่ " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";



                                string pathfileins = "";

                                string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                if (resfile.Rows.Count > 0)
                                {
                                    pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");

                                    string email = "";

                                    var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                                    ////get mail from db
                                    /////send mail to next_approve
                                    if (isdev != "true")
                                    {
                                        string sqlbpm = "select * from li_user where user_login = '" + wfA_NextStep.next_assto_login + "' ";
                                        System.Data.DataTable dtbpm = zdb.ExecSql_DataTable(sqlbpm, zconnstr);

                                        if (dtbpm.Rows.Count > 0)
                                        {
                                            email = dtbpm.Rows[0]["email"].ToString();

                                        }
                                        else
                                        {
                                            string sqlpra = "select * from Rpa_Mst_HrNameList where Login = 'ASSETWORLDCORP-\\" + wfA_NextStep.next_assto_login + "' ";
                                            System.Data.DataTable dtrpa = zdb.ExecSql_DataTable(sqlpra, zconnstrrpa);

                                            if (dtrpa.Rows.Count > 0)
                                            {
                                                email = dtrpa.Rows[0]["Email"].ToString();
                                            }
                                            else
                                            {
                                                email = "";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        ////fix mail test
                                        email = "legalwfuat2024@gmail.com";
                                    }

                                    if (!string.IsNullOrEmpty(email))
                                    {
                                        _ = zsendmail.sendEmail(subject + " Mail To Next Appove", email, body, pathfileins);
                                    }

                                }

                            }
                        }

                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                    }

                }
            }


        }

        private void GenDocumnetLitigation(string pid, string submit_by)
        {
            string xreq_no = "";
            var path_template = ConfigurationManager.AppSettings["WT_Template_litigation"].ToString();
            string templatefile = "";
            ReplaceLitigation_TagData data = new ReplaceLitigation_TagData();

            string sqllit = "select * from li_litigation_request where process_id='" + pid + "'";
            var reslit = zdb.ExecSql_DataTable(sqllit, zconnstr);

            if (reslit.Rows.Count > 0)
            {
                xreq_no = reslit.Rows[0]["req_no"].ToString();
                if (reslit.Rows[0]["tof_litigationreq_code"].ToString() == "01")
                {
                    templatefile = path_template + @"\LitigationTemplate.docx";
                    #region gentagstr data form

                    var requestor = "";
                    var requestorpos = "";
                    var supervisor = "";
                    var supervisorpos = "";

                    var empFunc = new EmpInfo();

                    //get data user
                    var emp = empFunc.getEmpInfo(submit_by);
                    if (!string.IsNullOrEmpty(emp.full_name_en))
                    {
                        requestor = emp.full_name_en;
                        requestorpos = emp.position_en;
                    }

                    //get supervisor data
                    var empSupervisor = empFunc.getEmpInfo("sarawut.l");
                    if (!string.IsNullOrEmpty(empSupervisor.full_name_en))
                    {
                        supervisor = empSupervisor.full_name_en;
                        supervisorpos = empSupervisor.position_en;
                    }

                    data.sign_name1 = "";
                    data.name1 = requestor;
                    data.position1 = requestorpos;
                    data.date1 = "";

                    data.sign_name2 = "";
                    data.name2 = supervisor;
                    data.position2 = supervisorpos;
                    data.date2 = "";
                    #endregion
                }
                else
                {
                    templatefile = path_template + @"\LitigationTemplate2.docx";
                    #region gentagstr data form

                    var requestor = "";
                    var requestorpos = "";
                    var supervisor = "";
                    var supervisorpos = "";

                    ///get gm heam_am c_level
                    string sqlbu = @"select * from li_business_unit where bu_code = '" + reslit.Rows[0]["bu_code"] + "'";
                    var res = zdb.ExecSql_DataTable(sqlbu, zconnstr);

                    if (res.Rows.Count > 0)
                    {
                        // check session_user
                        if (Session["user_login"] != null)
                        {
                            var xlogin_name = Session["user_login"].ToString();
                            var empFunc = new EmpInfo();

                            string xgm = res.Rows[0]["gm"].ToString();
                            string xam = res.Rows[0]["am"].ToString();
                            string xhead_am = res.Rows[0]["head_am"].ToString();
                            string xexternal_domain = res.Rows[0]["external_domain"].ToString();

                            if (xexternal_domain == "Y")
                            {
                                //get data am user
                                if (!string.IsNullOrEmpty(xam))
                                {
                                    var empam = empFunc.getEmpInfo(xam);
                                    if (empam.user_login != null)
                                    {
                                        requestor = empam.full_name_en;
                                        requestorpos = empam.position_en;
                                    }
                                }
                                //get data head am user
                                if (!string.IsNullOrEmpty(xhead_am))
                                {
                                    var empheadam = empFunc.getEmpInfo(xhead_am);
                                    if (empheadam.user_login != null)
                                    {
                                        supervisor = empheadam.full_name_en;
                                        supervisorpos = empheadam.position_en;
                                    }
                                }
                            }
                            else
                            {
                                //get data user
                                var emp = empFunc.getEmpInfo(xlogin_name);
                                if (!string.IsNullOrEmpty(emp.full_name_en))
                                {
                                    requestor = emp.full_name_en;
                                    requestorpos = emp.position_en;
                                }

                                //get data gm user
                                if (!string.IsNullOrEmpty(xgm))
                                {
                                    var empgm = empFunc.getEmpInfo(xgm);
                                    if (empgm.user_login != null)
                                    {
                                        supervisor = empgm.full_name_en;
                                        supervisorpos = empgm.position_en;
                                    }
                                }
                            }
                        }
                    }

                    data.sign_name1 = "";
                    data.name1 = requestor;
                    data.position1 = requestorpos;
                    data.date1 = "";

                    data.sign_name2 = "";
                    data.name2 = supervisor;
                    data.position2 = supervisorpos;
                    data.date2 = "";
                    #endregion
                }
            }

            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\litigation_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            System.Data.DataTable dtStr = zreplacelitigation.BindTagData(pid, data);

            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
            var jsonDTProperties1 = "";
            var jsonDTdata = "";
            //end prepare data

            // Save to Database z_replacedocx_log
            string sql = @"insert into z_replacedocx_log (replacedocx_reqno,jsonTagString, jsonTableProp, jsonTableData,template_filepath , output_directory,output_filepath, delete_output ) 
                        values('" + xreq_no + @"',
                               '" + jsonDTStr + @"', 
                                '" + jsonDTProperties1 + @"', 
                                '" + jsonDTdata + @"', 
                                '" + templatefile + @"', 
                                '" + outputfolder + @"', 
                                '" + outputfn + @"',  
                                '" + "0" + @"'
                            ) ";

            zdb.ExecNonQuery(sql, zconnstr);

            var outputbyte = rdoc.ReplaceData2(jsonDTStr, jsonDTProperties1, jsonDTdata, templatefile, outputfolder, outputfn, false);

            repl.convertDOCtoPDF(outputfn, outputfn.Replace(".docx", ".pdf"), false);

        }

        protected void btn_assign_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopupAssign", "showModalAssign();", true);
        }
        protected void Assign_Update_Click(object sender, EventArgs e)
        {
            hid_assto_login.Value = ddlAssign_NameList.SelectedValue;

            string process_code = Request.QueryString["pc"];
            int version_no = 1;

            if (!string.IsNullOrEmpty(process_code))
            {

                // getCurrentStep
                var wfAttr = zwf.getCurrentStep(lblPID.Text, process_code, version_no);

                // check session_user
                if (Session["user_login"] != null)
                {
                    var xlogin_name = Session["user_login"].ToString();
                    var empFunc = new EmpInfo();

                    //get data user
                    var emp = empFunc.getEmpInfo(xlogin_name);

                    // set WF Attributes
                    wfAttr.external_domain = hid_external_domain.Value;
                    wfAttr.subject = subject.Text.Trim();
                    wfAttr.assto_login = emp.next_line_mgr_login;
                    wfAttr.wf_status = wfAttr.step_name + " Assigned";
                    wfAttr.submit_answer = "ASSIGNED";
                    wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, wfAttr.process_id, hid_bucode.Value);
                    wfAttr.updated_by = emp.user_login;
                    wfAttr.submit_by = wfAttr.submit_by;
                    // wf.updateProcess
                    var wfA_NextStep = zwf.updateProcess(wfAttr);
                    wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, hid_assto_login.Value, wfAttr.process_id, hid_bucode.Value);
                    wfA_NextStep.submit_by = wfA_NextStep.submit_by;
                    string status = zwf.Insert_NextStep(wfA_NextStep);

                    if (status == "Success")
                    {
                        // check processcode loop gendocument
                        if (wfAttr.step_name == "Head of Litigation Assign")
                        {
                            GenDocumnetLitigation(lblPID.Text, wfAttr.submit_by);
                            string subject = "";
                            string body = "";
                            string sql = @"select * from li_litigation_request where process_id = '" + wfAttr.process_id + "'";
                            var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                            if (dt.Rows.Count > 0)
                            {
                                var dr = dt.Rows[0];
                                string id = dr["req_no"].ToString();
                                subject = wfAttr.subject;
                                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                                body = "คุณได้รับมอบหมายให้ดำเนินการเอกสารเลขที่ " + dr["document_no"].ToString() + " ได้รับการอนุมัติผ่านระบบแล้ว กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

                                string pathfile = "";

                                string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                if (resfile.Rows.Count > 0)
                                {
                                    pathfile = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");

                                    string email;

                                    var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                                    ////get mail from db
                                    if (isdev != "true")
                                    {
                                        string sqlbpm = "select * from li_user where user_login = '" + wfA_NextStep.next_assto_login + "' ";
                                        System.Data.DataTable dtbpm = zdb.ExecSql_DataTable(sqlbpm, zconnstr);

                                        if (dtbpm.Rows.Count > 0)
                                        {
                                            email = dtbpm.Rows[0]["email"].ToString();

                                        }
                                        else
                                        {
                                            string sqlpra = "select * from Rpa_Mst_HrNameList where Login = 'ASSETWORLDCORP-\\" + wfA_NextStep.next_assto_login + "' ";
                                            System.Data.DataTable dtrpa = zdb.ExecSql_DataTable(sqlpra, zconnstrrpa);

                                            if (dtrpa.Rows.Count > 0)
                                            {
                                                email = dtrpa.Rows[0]["Email"].ToString();
                                            }
                                            else
                                            {
                                                email = "";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        ////fix mail test
                                        email = "legalwfuat2024@gmail.com";
                                    }

                                    if (!string.IsNullOrEmpty(email))
                                    {
                                        _ = zsendmail.sendEmail(subject + " Mail To Litigation", email, body, pathfile);
                                    }
                                }

                            }
                        }

                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                    }

                }
            }
        }
    }
}