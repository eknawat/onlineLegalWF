using onlineLegalWF.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static onlineLegalWF.Class.ReplaceCommRegis;

namespace onlineLegalWF.forms
{
    public partial class ccrapv : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public SendMail zsendmail = new SendMail();
        public ReplaceCommRegis zreplacecommregis = new ReplaceCommRegis();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string req = Request.QueryString["req"];
                string process_code = Request.QueryString["pc"];
                string st_name = Request.QueryString["st"];

                if (!string.IsNullOrEmpty(req) && !string.IsNullOrEmpty(process_code) && !string.IsNullOrEmpty(st_name))
                {
                    setDataApprove(req, process_code, st_name);
                }

            }
        }

        private void setDataApprove(string req, string process_code, string st_name)
        {
            string id = "";

            ucHeader1.setHeader(process_code + " Approve");
            string sqlcommregis = @"SELECT [process_id],[req_no],[req_date],commreg.[toc_regis_code],toc.[toc_regis_desc],commreg.[subsidiary_code],[document_no],[mt_res_desc],[mt_res_no],[mt_res_date],
                                      CASE
                                        WHEN commreg.subsidiary_code IS NULL THEN commreg.company_name_th
                                        ELSE comsub.subsidiary_name_th
                                      END AS company_name_th,
                                      CASE
                                        WHEN commreg.subsidiary_code IS NULL THEN commreg.company_name_en
                                        ELSE comsub.subsidiary_name_en
                                      END AS company_name_en,
                                      [isrdregister],[ismoresubsidiary],[status],[updated_datetime]
                                      FROM li_comm_regis_request AS commreg
                                      LEFT OUTER JOIN li_comm_regis_subsidiary AS comsub ON commreg.subsidiary_code = comsub.subsidiary_code
                                      INNER JOIN li_type_of_comm_regis AS toc ON commreg.toc_regis_code = toc.toc_regis_code
                              where process_id = '" + req + "'";
            var rescommregis = zdb.ExecSql_DataTable(sqlcommregis, zconnstr);

            //get data ins req
            if (rescommregis.Rows.Count > 0)
            {
                id = rescommregis.Rows[0]["req_no"].ToString();
                req_no.Value = rescommregis.Rows[0]["req_no"].ToString();
                req_date.Text = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(rescommregis.Rows[0]["req_date"]), "th");
                doc_no.Text = rescommregis.Rows[0]["document_no"].ToString();
                subject.Text = rescommregis.Rows[0]["toc_regis_desc"].ToString();
                companyname_th.Text = rescommregis.Rows[0]["company_name_th"].ToString();
                companyname_en.Text = rescommregis.Rows[0]["company_name_en"].ToString();

                if (Convert.ToBoolean(rescommregis.Rows[0]["ismoresubsidiary"].ToString())) 
                {
                    if (st_name == "Registration Update")
                    {
                        btn_Edit.Visible = true;
                    }
                    string sqladditional = @"select [req_no],commaddi.[subsidiary_code],commsub.[subsidiary_name_th],[assto_login],[status],[created_datetime]
                                                ,[updated_datetime] from li_comm_regis_request_additional as commaddi
                                                inner join li_comm_regis_subsidiary as commsub on commaddi.subsidiary_code = commsub.subsidiary_code
                                                where req_no='" + id + "'";
                    var resadditional = zdb.ExecSql_DataTable(sqladditional, zconnstr);



                    if (resadditional.Rows.Count > 0)
                    {
                        string js = "$('.moresubsidiary').show();";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSection", js, true);

                        var dt = iniDTStructure();
                        int no = 0;

                        foreach (DataRow item in resadditional.Rows) 
                        {
                            var dr = dt.NewRow();
                            dr = dt.NewRow();
                            dr["No"] = (no + 1);
                            dr["req_no"] = item["req_no"].ToString();
                            dr["subsidiary_code"] = item["subsidiary_code"].ToString();
                            dr["subsidiary_name_th"] = item["subsidiary_name_th"].ToString();
                            dr["assto_login"] = item["assto_login"].ToString();
                            dr["status"] = item["status"].ToString();
                            dr["created_datetime"] = Convert.ToDateTime(item["created_datetime"]).ToString("yyyy-MM-dd");
                            dr["updated_datetime"] = (!string.IsNullOrEmpty(item["updated_datetime"].ToString()) ? Convert.ToDateTime(item["updated_datetime"]).ToString("yyyy-MM-dd") : "");
                            dt.Rows.Add(dr);
                            no++;
                        }

                        gv1.DataSource = dt;
                        gv1.DataBind();

                    }
                }

                //init data UcAttachAndCommentLogs
                initDataAttachAndComment(rescommregis.Rows[0]["process_id"].ToString());

                getDocument(id);
            }

            //check switch button approve 
            if (st_name == "Supervisor Approve") 
            {
                btn_Approve.Visible = true;
                btn_Reject.Visible = true;
                btn_Accept.Visible = false;
                btn_Submit.Visible = false;
                btn_Edit.Visible = false;
            }
            else if (st_name == "Registration Receive")
            {
                ucHeader1.setHeader(process_code + " " + st_name);
                btn_Approve.Visible = false;
                btn_Reject.Visible = false;
                btn_Accept.Visible = true;
                btn_Submit.Visible = false;
                btn_Edit.Visible = false;
            }
            else if (st_name == "Registration Update")
            {
                ucHeader1.setHeader(process_code + " " + st_name);
                btn_Approve.Visible = false;
                btn_Reject.Visible = false;
                btn_Accept.Visible = false;
                btn_Submit.Visible = true;
            }
        }

        public DataTable iniDTStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("no", typeof(string));
            dt.Columns.Add("req_no", typeof(string));
            dt.Columns.Add("subsidiary_code", typeof(string));
            dt.Columns.Add("subsidiary_name_th", typeof(string));
            dt.Columns.Add("assto_login", typeof(string));
            dt.Columns.Add("status", typeof(string));
            dt.Columns.Add("created_datetime", typeof(string));
            dt.Columns.Add("updated_datetime", typeof(string));
            return dt;
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
                string xbu_code = "";

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
                    wfAttr.subject = "เรื่อง " + subject.Text.Trim() + " " + companyname_th.Text.Trim();
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
                        if (process_code == "CCR")
                        {
                            GenDocumnetCCRRegis(lblPID.Text, wfAttr.submit_by);
                            string subject = "";
                            string body = "";
                            string sqlccr = @"SELECT [process_id],[req_no],[req_date],commreg.[toc_regis_code],toc.[toc_regis_desc],commreg.[subsidiary_code],[document_no],[mt_res_desc],[mt_res_no],[mt_res_date],
                                                CASE
                                                WHEN commreg.subsidiary_code IS NULL THEN commreg.company_name_th
                                                ELSE comsub.subsidiary_name_th
                                                END AS company_name_th,
                                                CASE
                                                WHEN commreg.subsidiary_code IS NULL THEN commreg.company_name_en
                                                ELSE comsub.subsidiary_name_en
                                                END AS company_name_en,
                                                [isrdregister],[status],[updated_datetime]
                                                FROM li_comm_regis_request AS commreg
                                                LEFT OUTER JOIN li_comm_regis_subsidiary AS comsub ON commreg.subsidiary_code = comsub.subsidiary_code
                                                INNER JOIN li_type_of_comm_regis AS toc ON commreg.toc_regis_code = toc.toc_regis_code  
                                                where process_id = '" + wfAttr.process_id + "'";
                            var dt = zdb.ExecSql_DataTable(sqlccr, zconnstr);
                            if (dt.Rows.Count > 0)
                            {
                                var dr = dt.Rows[0];
                                string id = dr["req_no"].ToString();
                                subject = wfAttr.subject;
                                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                                body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้ถูก Reject กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

                                string pathfilecommregis = "";

                                string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                if (resfile.Rows.Count > 0)
                                {
                                    pathfilecommregis = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");

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
                                        _ = zsendmail.sendEmail("Reject " + subject + " Mail To Requester", email, body, pathfilecommregis);
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

        private void GenDocumnetCCRRegis(string pid, string submit_by)
        {
            string xreq_no = "";
            var path_template = ConfigurationManager.AppSettings["WT_Template_commregistration"].ToString();
            string templatefile = "";

            string sqlcommregis = "select * from li_comm_regis_request where process_id='" + pid + "'";
            var rescommregis = zdb.ExecSql_DataTable(sqlcommregis, zconnstr);

            if (rescommregis.Rows.Count > 0)
            {
                xreq_no = rescommregis.Rows[0]["req_no"].ToString();
                if (rescommregis.Rows[0]["toc_regis_code"].ToString() == "12" || rescommregis.Rows[0]["toc_regis_code"].ToString() == "13" || rescommregis.Rows[0]["toc_regis_code"].ToString() == "14" || rescommregis.Rows[0]["toc_regis_code"].ToString() == "99")
                {
                    templatefile = path_template + @"\InsuranceComregis2.docx";
                }
                else
                {
                    templatefile = path_template + @"\InsuranceComregis.docx";
                }
            }

            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\commregis_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            #region gentagstr data form
            ReplaceCommRegis_TagData data = new ReplaceCommRegis_TagData();

            var requestor = "";
            var requestorpos = "";
            var supervisor = "";
            var supervisorpos = "";

            // check session_user
            //if (Session["user_login"] != null)
            //{
            //    var xlogin_name = Session["user_login"].ToString();
            //    var empFunc = new EmpInfo();

            //    //get data user
            //    var emp = empFunc.getEmpInfo(xlogin_name);
            //    if (!string.IsNullOrEmpty(emp.full_name_en))
            //    {
            //        requestor = emp.full_name_en;
            //        requestorpos = emp.position_en;
            //    }

            //    //get supervisor data
            //    var empSupervisor = empFunc.getEmpInfo(emp.next_line_mgr_login);
            //    if (!string.IsNullOrEmpty(empSupervisor.full_name_en))
            //    {
            //        supervisor = empSupervisor.full_name_en;
            //        supervisorpos = empSupervisor.position_en;
            //    }

            //}
            var empFunc = new EmpInfo();

            //get data user
            var emp = empFunc.getEmpInfo(submit_by);
            if (!string.IsNullOrEmpty(emp.full_name_en))
            {
                requestor = emp.full_name_en;
                requestorpos = emp.position_en;
            }

            //get supervisor data
            var empSupervisor = empFunc.getEmpInfo(emp.next_line_mgr_login);
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


            DataTable dtStr = zreplacecommregis.BindTagData(pid, data);
            #endregion


            // Convert to JSONString
            //DataTable dtTagPropsTable = new DataTable();
            //dtTagPropsTable.Columns.Add("tagname", typeof(string));
            //dtTagPropsTable.Columns.Add("jsonstring", typeof(string));

            //DataTable dtTagDataTable = new DataTable();
            //dtTagDataTable.Columns.Add("tagname", typeof(string));
            //dtTagDataTable.Columns.Add("jsonstring", typeof(string));
            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
            //var jsonDTProperties1 = repl.DataTableToJSONWithStringBuilder(dtProperties1);
            //var jsonDTdata = repl.DataTableToJSONWithStringBuilder(dt);
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
                    wfAttr.subject = "เรื่อง "+ subject.Text.Trim() +" "+ companyname_th.Text.Trim();
                    wfAttr.assto_login = emp.next_line_mgr_login;
                    wfAttr.wf_status = wfAttr.step_name + " Approved";
                    wfAttr.submit_answer = "APPROVED";
                    wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, wfAttr.process_id, "");
                    wfAttr.updated_by = emp.user_login;
                    wfAttr.submit_by = wfAttr.submit_by;
                    // wf.updateProcess
                    var wfA_NextStep = zwf.updateProcess(wfAttr);
                    wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by, wfAttr.process_id, "");
                    if (wfAttr.step_name == "End")
                    {
                        wfA_NextStep.wf_status = "COMPLETED";
                    }

                    wfA_NextStep.submit_by = wfA_NextStep.submit_by;
                    string status = zwf.Insert_NextStep(wfA_NextStep);

                    if (status == "Success")
                    {
                        //check li_type_of_comm_regis == 01 Insert li_comm_regis_subsidiary and //check li_type_of_comm_regis == 02 update li_comm_regis_subsidiary
                        string sqlcommregis = "select * from li_comm_regis_request where process_id = '" + wfAttr.process_id + "'";
                        var rescommregis = zdb.ExecSql_DataTable(sqlcommregis, zconnstr);

                        if (rescommregis.Rows.Count > 0)
                        {
                            string xtoc_regis_code = rescommregis.Rows[0]["toc_regis_code"].ToString();
                            string xcompany_name_th = rescommregis.Rows[0]["company_name_th"].ToString();
                            string xcompany_name_en = rescommregis.Rows[0]["company_name_en"].ToString();
                            var xsubcode = (GetMaxSubsidiaryCode() + 1).ToString();
                            if (xtoc_regis_code == "01")
                            {
                                string sqlnew = @"INSERT INTO [dbo].[li_comm_regis_subsidiary] 
                                                    ([subsidiary_code],[subsidiary_name_th],[subsidiary_name_en],[row_sort])
                                                    VALUES ('" + xsubcode + "','" + xcompany_name_th + "','" + xcompany_name_en + "','" + xsubcode + "')";
                                zdb.ExecNonQuery(sqlnew, zconnstr);
                            }
                            else if (xtoc_regis_code == "02") 
                            {
                                string sqlupdate = @"UPDATE [dbo].[li_comm_regis_subsidiary]
                                                       SET [subsidiary_name_th] = '"+ xcompany_name_th +@"'
                                                          ,[subsidiary_name_en] = '"+ xcompany_name_en +@"'
                                                     WHERE subsidiary_code = '"+ xtoc_regis_code +"'";
                                zdb.ExecNonQuery(sqlupdate, zconnstr);
                            }
                        }

                        // check processcode loop gendocument
                        if (process_code == "CCR")
                        {
                            //check step in step_name Supervisor Approve send email notification to Registration team
                            if (wfAttr.step_name == "Supervisor Approve")
                            {
                                GenDocumnetCCRRegis(lblPID.Text, wfAttr.submit_by);
                                string subject = "";
                                string body = "";
                                string sql = @"SELECT [process_id],[req_no],[req_date],commreg.[toc_regis_code],toc.[toc_regis_desc],commreg.[subsidiary_code],[document_no],[mt_res_desc],[mt_res_no],[mt_res_date],
                                                CASE
                                                WHEN commreg.subsidiary_code IS NULL THEN commreg.company_name_th
                                                ELSE comsub.subsidiary_name_th
                                                END AS company_name_th,
                                                CASE
                                                WHEN commreg.subsidiary_code IS NULL THEN commreg.company_name_en
                                                ELSE comsub.subsidiary_name_en
                                                END AS company_name_en,
                                                [isrdregister],[status],[updated_datetime]
                                                FROM li_comm_regis_request AS commreg
                                                LEFT OUTER JOIN li_comm_regis_subsidiary AS comsub ON commreg.subsidiary_code = comsub.subsidiary_code
                                                INNER JOIN li_type_of_comm_regis AS toc ON commreg.toc_regis_code = toc.toc_regis_code  
                                                where process_id = '" + wfAttr.process_id + "'";
                                var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                                if (dt.Rows.Count > 0)
                                {
                                    var dr = dt.Rows[0];
                                    string id = dr["req_no"].ToString();
                                    subject = wfAttr.subject;
                                    var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                                    body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้รับการอนุมัติผ่านระบบแล้ว กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='"+host_url_sendmail+"legalportal/legalportal?m=myworklist'>Click</a>";

                                    string pathfilecommregis = "";

                                    string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                    var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                    if (resfile.Rows.Count > 0)
                                    {
                                        pathfilecommregis = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");

                                        string[] emailCommregis;

                                        var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                                        ////get mail from db
                                        if (isdev != "true")
                                        {
                                            emailCommregis = new string[] { "aram.r@assetworldcorp-th.com", "apicha.w@assetworldcorp-th.com", "thunchanok.i@assetworldcorp-th.com" };
                                        }
                                        else
                                        {
                                            ////fix mail test
                                            emailCommregis = new string[] { "legalwfuat2024@gmail.com", "manit.ch@assetworldcorp-th.com" };
                                        }

                                        if (emailCommregis.Length > 0)
                                        {
                                            _ = zsendmail.sendEmails(subject + " Mail To Commercial Registration", emailCommregis, body, pathfilecommregis);
                                        }
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

        protected void btn_Accept_Click(object sender, EventArgs e)
        {
            string req = Request.QueryString["req"];
            string st_name = Request.QueryString["st"];
            string sqlcheckreceive = @"select * from wf_routing where process_id='"+req+"' and step_name='Registration Receive' and wf_status='RECEIVED'";
            var rescheckreceive = zdb.ExecSql_DataTable(sqlcheckreceive, zconnstr);
            //check receive 
            if (rescheckreceive.Rows.Count > 0)
            {
                //has receive Alert
                Response.Write("<script> alert('This task has already been received by another user');</script>");

                var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
            }
            else 
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
                        wfAttr.subject = "เรื่อง " + subject.Text.Trim() + " " + companyname_th.Text.Trim();
                        wfAttr.assto_login = emp.next_line_mgr_login;
                        wfAttr.wf_status = "RECEIVED";
                        wfAttr.submit_answer = "RECEIVED";
                        wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, "");
                        wfAttr.updated_by = emp.user_login;
                        wfAttr.submit_by = wfAttr.submit_by;
                        // wf.updateProcess
                        var wfA_NextStep = zwf.updateProcess(wfAttr);
                        wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by, wfAttr.process_id, "");
                        wfA_NextStep.submit_by = wfAttr.submit_by;
                        wfA_NextStep.wf_status = "";
                        string status = zwf.Insert_NextStep(wfA_NextStep);

                        if (status == "Success")
                        {
                            //update status li_comm_regis_request
                            string sqlupdate = @"update li_comm_regis_request set status='inprogress',updated_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where process_id = '" + wfAttr.process_id + "'";
                            zdb.ExecNonQuery(sqlupdate, zconnstr);

                            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                            Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                        }

                    }
                }
            }

            
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
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
                    wfAttr.subject = "เรื่อง " + subject.Text.Trim() +" "+ companyname_th.Text.Trim();
                    wfAttr.assto_login = emp.next_line_mgr_login;
                    wfAttr.wf_status = "COMPLETED";
                    wfAttr.submit_answer = "COMPLETED";
                    wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, "");
                    wfAttr.updated_by = emp.user_login;
                    wfAttr.submit_by = wfAttr.submit_by;
                    // wf.updateProcess
                    var wfA_NextStep = zwf.updateProcess(wfAttr);
                    wfA_NextStep.submit_by = wfAttr.submit_by;
                    wfA_NextStep.wf_status = "COMPLETED";
                    string status = zwf.Insert_NextStep(wfA_NextStep);

                    if (status == "Success")
                    {
                        string sqlupdate = @"update li_comm_regis_request set status='completed',updated_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where process_id = '" + wfAttr.process_id + "'";
                        zdb.ExecNonQuery(sqlupdate, zconnstr);

                        string subject = "";
                        string body = "";
                        string sql = @"SELECT [process_id],[req_no],[req_date],commreg.[toc_regis_code],toc.[toc_regis_desc],commreg.[subsidiary_code],[document_no],[mt_res_desc],[mt_res_no],[mt_res_date],
                                                CASE
                                                WHEN commreg.subsidiary_code IS NULL THEN commreg.company_name_th
                                                ELSE comsub.subsidiary_name_th
                                                END AS company_name_th,
                                                CASE
                                                WHEN commreg.subsidiary_code IS NULL THEN commreg.company_name_en
                                                ELSE comsub.subsidiary_name_en
                                                END AS company_name_en,
                                                [isrdregister],[status],[updated_datetime]
                                                FROM li_comm_regis_request AS commreg
                                                LEFT OUTER JOIN li_comm_regis_subsidiary AS comsub ON commreg.subsidiary_code = comsub.subsidiary_code
                                                INNER JOIN li_type_of_comm_regis AS toc ON commreg.toc_regis_code = toc.toc_regis_code  
                                                where process_id = '" + wfAttr.process_id + "'";
                        var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                        if (dt.Rows.Count > 0)
                        {
                            var dr = dt.Rows[0];
                            string id = dr["req_no"].ToString();
                            subject = wfAttr.subject;
                            body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้รับการดำเนินการเสร็จสิ้นแล้ว";

                            string pathfileCommregis = "";

                            string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                            var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                            if (resfile.Rows.Count > 0)
                            {
                                pathfileCommregis = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");

                                string email = "";

                                var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                                ////get mail from db
                                if (isdev != "true")
                                {
                                    var empreq = empFunc.getEmpInfo(wfAttr.submit_by);
                                    if (empreq.user_login != null)
                                    {
                                        email = empreq.email;
                                    }
                                    //string sqlbpm = "select * from li_user where user_login = '" + wfA_NextStep.next_assto_login + "' ";
                                    //System.Data.DataTable dtbpm = zdb.ExecSql_DataTable(sqlbpm, zconnstr);

                                    //if (dtbpm.Rows.Count > 0)
                                    //{
                                    //    email = dtbpm.Rows[0]["email"].ToString();

                                    //}
                                    //else
                                    //{
                                    //    string sqlpra = "select * from Rpa_Mst_HrNameList where Login = 'ASSETWORLDCORP-\\" + wfA_NextStep.next_assto_login + "' ";
                                    //    System.Data.DataTable dtrpa = zdb.ExecSql_DataTable(sqlpra, zconnstrrpa);

                                    //    if (dtrpa.Rows.Count > 0)
                                    //    {
                                    //        email = dtrpa.Rows[0]["Email"].ToString();
                                    //    }
                                    //    else
                                    //    {
                                    //        email = "";
                                    //    }

                                    //}
                                }
                                else
                                {
                                    ////fix mail test
                                    email = "legalwfuat2024@gmail.com";
                                }

                                if (!string.IsNullOrEmpty(email))
                                {
                                    //send mail to requester
                                    _ = zsendmail.sendEmail(subject + " Mail To Requester", email, body, pathfileCommregis);
                                }

                            }

                        }

                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                    }

                }
            }
        }

        public int GetMaxSubsidiaryCode()
        {
            string sql = "select isnull(max(subsidiary_code),0) as id from li_comm_regis_subsidiary";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return Convert.ToInt32(dt.Rows[0][0]);
        }

        protected void btn_Edit_Click(object sender, EventArgs e)
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            Response.Redirect(host_url + "frmCommregis/CommRegisRequestEditByAdmin.aspx?id=" + req_no.Value.Trim());
        }
    }
}