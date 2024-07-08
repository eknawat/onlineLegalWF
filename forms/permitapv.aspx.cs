using DocumentFormat.OpenXml.Presentation;
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
using static onlineLegalWF.Class.ReplacePermit;

namespace onlineLegalWF.forms
{
    public partial class permitapv : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public SendMail zsendmail = new SendMail();
        public ReplacePermit zreplacepermit = new ReplacePermit();
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
            string sqlpermit = @"select * from li_permit_request where process_id = '" + req + "'";
            var respermit = zdb.ExecSql_DataTable(sqlpermit, zconnstr);

            //get data ins req
            if (respermit.Rows.Count > 0)
            {
                id = respermit.Rows[0]["permit_no"].ToString();
                req_no.Value = respermit.Rows[0]["permit_no"].ToString();
                req_date.Text = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(respermit.Rows[0]["permit_date"]), "th");
                doc_no.Text = respermit.Rows[0]["document_no"].ToString();
                subject.Text = respermit.Rows[0]["permit_subject"].ToString();
                desc.Text = respermit.Rows[0]["permit_desc"].ToString();
                hid_bucode.Value = respermit.Rows[0]["bu_code"].ToString();
                hid_islandtax.Value = "false";
                hid_issignagetax.Value = "false";
                var xtype_of_permitrequest = respermit.Rows[0]["tof_permitreq_code"].ToString();

                if (xtype_of_permitrequest == "05") 
                {
                    hid_issignagetax.Value = "true";
                }
                else if (xtype_of_permitrequest == "06")
                {
                    hid_islandtax.Value = "true";
                    section_comcode.Visible = true;
                }

                string sqlbu = "select * from li_business_unit where bu_code = '" + hid_bucode.Value + "'";
                var resbu = zdb.ExecSql_DataTable(sqlbu, zconnstr);
                if (resbu.Rows.Count > 0) 
                {
                    DataRow dr = resbu.Rows[0];
                    hid_external_domain.Value = dr["external_domain"].ToString();
                    hid_permit_license_external.Value = dr["permit_license_external"].ToString();
                    hid_permit_landtax_external.Value = dr["permit_landtax_external"].ToString();
                    hid_permit_signagetax_external.Value = dr["permit_signagetax_external"].ToString();
                    hid_permit_tradmark_external.Value = dr["permit_tradmark_external"].ToString();
                    hid_permit_energy_external.Value = dr["permit_energy_external"].ToString();
                    hid_permit_utility_external.Value = dr["permit_utility_external"].ToString();
                }

                //init data UcAttachAndCommentLogs
                initDataAttachAndComment(respermit.Rows[0]["process_id"].ToString());

                getDocument(id);

                int version_no = 1;

                var wfAttr = zwf.getCurrentStep(lblPID.Text, process_code, version_no);
                step_name.Text = wfAttr.step_name;
            }

            string mode = Request.QueryString["mode"];

            //check switch button approve 
            if (st_name == "GM Approve" || st_name == "AM Approve" || st_name == "Head AM Approve")
            {
                btn_Approve.Visible = true;
                btn_Reject.Visible = true;
                btn_Accept.Visible = false;
                btn_Submit.Visible = false;
                btn_send_requester.Visible = false;
            }
            else if (st_name == "Permit Receive")
            {
                ucHeader1.setHeader(process_code + " "+ st_name);
                btn_Approve.Visible = false;
                btn_Reject.Visible = true;
                btn_Accept.Visible = true;
                btn_Submit.Visible = false;
                btn_send_requester.Visible = false;
            }
            else if (st_name == "Permit Update" || st_name == "Requester Update")
            {
                ucHeader1.setHeader(process_code + " " + st_name);
                btn_Approve.Visible = false;
                btn_Reject.Visible = false;
                btn_Accept.Visible = false;
                btn_Submit.Visible = true;
                btn_send_requester.Visible = false;
            }
            else if (st_name == "Permit Check Update")
            {
                ucHeader1.setHeader(process_code + " " + st_name);
                btn_Approve.Visible = false;
                btn_Reject.Visible = false;
                btn_Accept.Visible = false;
                btn_Submit.Visible = true;
                btn_send_requester.Visible = true;
            }

            if (mode == "tracking") 
            {
                btn_Approve.Visible = false;
                btn_Reject.Visible = true;

                if (Session["user_login"] != null)
                {
                    var xlogin_name = Session["user_login"].ToString();
                    if (xlogin_name == "pornsawan.s") 
                    {
                        if (st_name == "Permit Update" || st_name == "Permit Check Update") 
                        {
                            btn_assign.Visible = true;
                        }
                        
                    }
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
                pdf_render.Attributes["src"] = host_url + "render/pdf?id=" + pathfile;
            }
        }

        private void initDataAttachAndComment(string pid)
        {
            lblPID.Text = pid;
            hid_PID.Value = pid;
            ucAttachment1.ini_object(pid);
            ucAttachment2.ini_object(pid,"POA","1");
            ucAttachment3.ini_object(pid,"License","2");
            ucCommentlog1.ini_object(pid);
        }

        private void GenDocumnetPermit(string pid, string submit_by)
        {
            string xreq_no = "";
            var path_template = ConfigurationManager.AppSettings["WT_Template_permit"].ToString();
            string templatefile = path_template + @"\PermitTemplate.docx";
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\permit_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            string sqlpermit = "select * from li_permit_request where process_id='" + pid + "'";
            var respermit = zdb.ExecSql_DataTable(sqlpermit, zconnstr);

            #region gentagstr data form
            ReplacePermit_TagData data = new ReplacePermit_TagData();

            if (respermit.Rows.Count > 0)
            {
                xreq_no = respermit.Rows[0]["permit_no"].ToString();
                string xbu_code = respermit.Rows[0]["bu_code"].ToString();
                var proceed_by = "";
                var approved_by = "";
                ///get gm am heam_am
                string sqlbu = @"select * from li_business_unit where bu_code = '" + xbu_code + "'";
                var resbu = zdb.ExecSql_DataTable(sqlbu, zconnstr);
                if (resbu.Rows.Count > 0)
                {
                    string xexternal_domain = resbu.Rows[0]["external_domain"].ToString();
                    string xgm = resbu.Rows[0]["gm"].ToString();
                    string xam = resbu.Rows[0]["am"].ToString();
                    string xhead_am = resbu.Rows[0]["head_am"].ToString();

                    var empFunc = new EmpInfo();

                    //get data user
                    if (xexternal_domain == "Y")
                    {
                        //Hotel get am
                        var empam = empFunc.getEmpInfo(xam);
                        if (!string.IsNullOrEmpty(empam.full_name_en))
                        {
                            proceed_by = empam.full_name_en;
                        }

                        //Hotel get head am
                        var empheam_am = empFunc.getEmpInfo(xhead_am);
                        if (!string.IsNullOrEmpty(empheam_am.full_name_en))
                        {
                            approved_by = empheam_am.full_name_en;
                        }
                    }
                    else
                    {
                        //get requester
                        var emp = empFunc.getEmpInfo(submit_by);
                        if (!string.IsNullOrEmpty(emp.full_name_en))
                        {
                            proceed_by = emp.full_name_en;
                        }

                        //get gm
                        var empgm = empFunc.getEmpInfo(xgm);
                        if (!string.IsNullOrEmpty(empgm.full_name_en))
                        {
                            approved_by = empgm.full_name_en;
                        }
                    }
                }

                data.name1 = proceed_by;
                data.signdate1 = "";
                data.name2 = approved_by;
                data.signdate2 = "";
            }

            DataTable dtStr = zreplacepermit.BindTagData(pid, data);
            #endregion

            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
            var jsonDTProperties1 = "";
            var jsonDTdata = "";

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
                    wfAttr.subject = "เรื่อง " + subject.Text.Trim();
                    wfAttr.assto_login = emp.next_line_mgr_login;
                    wfAttr.wf_status = wfAttr.step_name + " Approved";
                    wfAttr.submit_answer = "APPROVED";
                    wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, wfAttr.process_id, hid_bucode.Value);
                    wfAttr.updated_by = emp.user_login;
                    wfAttr.submit_by = wfAttr.submit_by;
                    wfAttr.external_domain = hid_external_domain.Value;
                    wfAttr.islandtax = Convert.ToBoolean(hid_islandtax.Value);
                    wfAttr.issignagetax = Convert.ToBoolean(hid_issignagetax.Value);
                    wfAttr.permit_license_external = hid_permit_license_external.Value;
                    wfAttr.permit_landtax_external = hid_permit_landtax_external.Value;
                    wfAttr.permit_tradmark_external = hid_permit_tradmark_external.Value;
                    wfAttr.permit_signagetax_external = hid_permit_signagetax_external.Value;
                    wfAttr.permit_energy_external = hid_permit_energy_external.Value;
                    wfAttr.permit_utility_external = hid_permit_utility_external.Value;
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
                        //Gendoc
                        GenDocumnetPermit(lblPID.Text, wfAttr.submit_by);
                        //check Send email to next approve for hotel and ccm 
                        if (wfAttr.step_name == "GM Approve" || wfAttr.step_name == "AM Approve")
                        {
                            if (wfAttr.external_domain == "Y")
                            {
                                sendMailNextApprove(wfAttr.process_id, wfAttr.subject, wfA_NextStep.next_assto_login);
                            }
                            else
                            {
                                sendMailToPermit(wfAttr.process_id, wfAttr.subject);
                            }

                        }
                        else if (wfAttr.step_name == "Head AM Approve")
                        {
                            sendMailToPermit(wfAttr.process_id, wfAttr.subject);
                        }

                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                    }

                }
            }
        }

        private void sendMailNextApprove(string pid, string xsubject, string xassign_to)
        {
            string subject = "";
            string body = "";
            string sqlmail = @"select * from li_permit_request where process_id = '" + pid + "'";
            var dt = zdb.ExecSql_DataTable(sqlmail, zconnstr);
            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                string id = dr["permit_no"].ToString();
                subject = xsubject;
                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                body = "คุณได้รับมอบหมายให้ตรวจสอบเอกสารเลขที่ " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a> <br/>" +
                                "You have been assigned to check document no " + dr["document_no"].ToString() + " Please check and proceed through the system <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

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
                        string sqlbpm = "select * from li_user where user_login = '" + xassign_to + "' ";
                        System.Data.DataTable dtbpm = zdb.ExecSql_DataTable(sqlbpm, zconnstr);

                        if (dtbpm.Rows.Count > 0)
                        {
                            email = dtbpm.Rows[0]["email"].ToString();

                        }
                        else
                        {
                            string sqlpra = "select * from Rpa_Mst_HrNameList where Login = 'ASSETWORLDCORP-\\" + xassign_to + "' ";
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

        private void sendMailToPermit(string pid, string xsubject)
        {
            string subject = "";
            string body = "";
            string sql = @"select * from li_permit_request where process_id = '" + pid + "'";
            var dt = zdb.ExecSql_DataTable(sql, zconnstr);
            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                string id = dr["permit_no"].ToString();
                subject = xsubject;
                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้รับการอนุมัติผ่านระบบแล้ว กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a><br/>" +
                    "Document no" + dr["document_no"].ToString() + " It has been approved from the system. Please check and proceed through the system. <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

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
                        emailCommregis = new string[] { "pornsawan.s@assetworldcorp-th.com", "naruemol.w@assetworldcorp-th.com", "kanita.s@assetworldcorp-th.com", "pattanis.r@assetworldcorp-th.com", "suradach.k@assetworldcorp-th.com" };
                    }
                    else
                    {
                        ////fix mail test
                        emailCommregis = new string[] { "legalwfuat2024@gmail.com", "manit.ch@assetworldcorp-th.com" };
                    }

                    if (emailCommregis.Length > 0)
                    {
                        _ = zsendmail.sendEmails(subject + " Mail To Permit", emailCommregis, body, pathfilecommregis);
                    }
                }

            }
        }

        protected void btn_Accept_Click(object sender, EventArgs e)
        {
            string req = Request.QueryString["req"];
            string st_name = Request.QueryString["st"];
            string sqlcheckreceive = @"select * from wf_routing where process_id='" + req + "' and step_name='Registration Receive' and wf_status='RECEIVED'";
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
                        wfAttr.subject = "เรื่อง " + subject.Text.Trim();
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
                            //update status li_permit_request
                            string sqlupdate = @"update li_permit_request set status='inprogress',updated_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where process_id = '" + wfAttr.process_id + "'";
                            zdb.ExecNonQuery(sqlupdate, zconnstr);

                            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                            Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                        }

                    }
                }
            }
            
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
                        GenDocumnetPermit(lblPID.Text, wfAttr.submit_by);
                        string subject = "";
                        string body = "";
                        string sqlmail = @"select * from li_permit_request where process_id = '" + wfAttr.process_id + "'";
                        var dt = zdb.ExecSql_DataTable(sqlmail, zconnstr);
                        if (dt.Rows.Count > 0)
                        {
                            var dr = dt.Rows[0];
                            string id = dr["permit_no"].ToString();
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

                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
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
                    wfAttr.subject = "เรื่อง " + subject.Text.Trim();
                    wfAttr.assto_login = emp.next_line_mgr_login;
                    wfAttr.wf_status = "COMPLETED";
                    wfAttr.submit_answer = "COMPLETED";
                    wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, "");
                    wfAttr.updated_by = emp.user_login;
                    wfAttr.submit_by = wfAttr.submit_by;
                    wfAttr.external_domain = hid_external_domain.Value;
                    wfAttr.islandtax = Convert.ToBoolean(hid_islandtax.Value);
                    wfAttr.issignagetax = Convert.ToBoolean(hid_issignagetax.Value);
                    wfAttr.permit_license_external = hid_permit_license_external.Value;
                    wfAttr.permit_landtax_external = hid_permit_landtax_external.Value;
                    wfAttr.permit_tradmark_external = hid_permit_tradmark_external.Value;
                    wfAttr.permit_signagetax_external = hid_permit_signagetax_external.Value;
                    wfAttr.permit_energy_external = hid_permit_energy_external.Value;
                    wfAttr.permit_utility_external = hid_permit_utility_external.Value;
                    // wf.updateProcess
                    var wfA_NextStep = zwf.updateProcess(wfAttr);
                    wfA_NextStep.submit_by = wfAttr.submit_by;
                    if (wfA_NextStep.step_name == "End")
                    {
                        wfA_NextStep.wf_status = "COMPLETED";
                    }
                    else 
                    {
                        wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by, wfAttr.process_id, hid_bucode.Value);
                    }
                    
                    string status = zwf.Insert_NextStep(wfA_NextStep);

                    if (status == "Success")
                    {
                        if (wfA_NextStep.step_name == "Requester Update")
                        {
                            sendMailUserClosejob(wfAttr.process_id, wfAttr.subject, wfA_NextStep.next_assto_login);
                        }
                        else if (wfA_NextStep.step_name == "Permit Check Update")
                        {
                            sendMailToPermitColseJob(wfAttr.process_id, wfAttr.subject);
                        }
                        else if(wfA_NextStep.step_name == "End")
                        {
                            sendMailToRequester(wfAttr.process_id, wfAttr.subject, wfAttr.submit_by);
                        }
                        
                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                    }

                }
            }
        }

        protected void btn_send_requester_Click(object sender, EventArgs e)
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
                    wfAttr.subject = "เรื่อง " + subject.Text.Trim();
                    wfAttr.assto_login = emp.next_line_mgr_login;
                    wfAttr.wf_status = "Reject";
                    wfAttr.submit_answer = "Reject";
                    wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, "");
                    wfAttr.updated_by = emp.user_login;
                    wfAttr.submit_by = wfAttr.submit_by;
                    wfAttr.external_domain = hid_external_domain.Value;
                    wfAttr.islandtax = Convert.ToBoolean(hid_islandtax.Value);
                    wfAttr.issignagetax = Convert.ToBoolean(hid_issignagetax.Value);
                    wfAttr.permit_license_external = hid_permit_license_external.Value;
                    wfAttr.permit_landtax_external = hid_permit_landtax_external.Value;
                    wfAttr.permit_tradmark_external = hid_permit_tradmark_external.Value;
                    wfAttr.permit_signagetax_external = hid_permit_signagetax_external.Value;
                    wfAttr.permit_energy_external = hid_permit_energy_external.Value;
                    wfAttr.permit_utility_external = hid_permit_utility_external.Value;
                    // wf.updateProcess
                    var wfA_NextStep = zwf.updateProcess(wfAttr);
                    wfA_NextStep.submit_by = wfAttr.submit_by;
                    if (wfA_NextStep.step_name == "End")
                    {
                        wfA_NextStep.wf_status = "COMPLETED";
                    }
                    else
                    {
                        wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by, wfAttr.process_id, hid_bucode.Value);
                    }

                    string status = zwf.Insert_NextStep(wfA_NextStep);

                    if (status == "Success")
                    {
                        if (wfA_NextStep.step_name == "Requester Update")
                        {
                            sendMailUserClosejob(wfAttr.process_id, wfAttr.subject, wfA_NextStep.next_assto_login);
                        }
                        else if (wfA_NextStep.step_name == "Permit Check Update")
                        {
                            sendMailToPermitColseJob(wfAttr.process_id, wfAttr.subject);
                        }
                        else if (wfA_NextStep.step_name == "End")
                        {
                            sendMailToRequester(wfAttr.process_id, wfAttr.subject, wfAttr.submit_by);
                        }

                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                    }

                }
            }
        }
        private void sendMailToPermitColseJob(string pid, string xsubject)
        {
            string subject = "";
            string body = "";
            string sql = @"select * from li_permit_request where process_id = '" + pid + "'";
            var dt = zdb.ExecSql_DataTable(sql, zconnstr);
            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                string id = dr["permit_no"].ToString();
                subject = xsubject;
                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                body = "คุณได้รับมอบหมายให้ตรวจสอบเอกสารเลขที่ " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a> <br/>" +
                                "You have been assigned to check document no " + dr["document_no"].ToString() + " Please check and proceed through the system <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

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
                        emailCommregis = new string[] { "pornsawan.s@assetworldcorp-th.com", "naruemol.w@assetworldcorp-th.com", "kanita.s@assetworldcorp-th.com", "pattanis.r@assetworldcorp-th.com", "suradach.k@assetworldcorp-th.com" };
                    }
                    else
                    {
                        ////fix mail test
                        emailCommregis = new string[] { "legalwfuat2024@gmail.com", "manit.ch@assetworldcorp-th.com" };
                    }

                    if (emailCommregis.Length > 0)
                    {
                        _ = zsendmail.sendEmails(subject + " Mail To Permit", emailCommregis, body, pathfilecommregis);
                    }
                }

            }
        }

        private void sendMailUserClosejob(string pid, string xsubject, string xassign_to)
        {
            string subject = "";
            string body = "";
            string sqlmail = @"select * from li_permit_request where process_id = '" + pid + "'";
            var dt = zdb.ExecSql_DataTable(sqlmail, zconnstr);
            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                string id = dr["permit_no"].ToString();
                subject = xsubject;
                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                body = "คุณได้รับมอบหมายให้ตรวจสอบเอกสารเลขที่ " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a> <br/>" +
                                "You have been assigned to check document no " + dr["document_no"].ToString() + " Please check and proceed through the system <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

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
                        string sqlbpm = "select * from li_user where user_login = '" + xassign_to + "' ";
                        System.Data.DataTable dtbpm = zdb.ExecSql_DataTable(sqlbpm, zconnstr);

                        if (dtbpm.Rows.Count > 0)
                        {
                            email = dtbpm.Rows[0]["email"].ToString();

                        }
                        else
                        {
                            string sqlpra = "select * from Rpa_Mst_HrNameList where Login = 'ASSETWORLDCORP-\\" + xassign_to + "' ";
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
                        _ = zsendmail.sendEmail(subject + " Mail To Requester", email, body, pathfileins);
                    }

                }

            }
        }

        private void sendMailChangeAssign(string pid, string xsubject, string xassign_to)
        {
            string subject = "";
            string body = "";
            string sqlmail = @"select * from li_permit_request where process_id = '" + pid + "'";
            var dt = zdb.ExecSql_DataTable(sqlmail, zconnstr);
            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                string id = dr["permit_no"].ToString();
                subject = xsubject;
                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                body = "คุณได้รับมอบหมายให้ตรวจสอบเอกสารเลขที่ " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a> <br/>" +
                                "You have been assigned to check document no " + dr["document_no"].ToString() + " Please check and proceed through the system <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

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
                        string sqlbpm = "select * from li_user where user_login = '" + xassign_to + "' ";
                        System.Data.DataTable dtbpm = zdb.ExecSql_DataTable(sqlbpm, zconnstr);

                        if (dtbpm.Rows.Count > 0)
                        {
                            email = dtbpm.Rows[0]["email"].ToString();

                        }
                        else
                        {
                            string sqlpra = "select * from Rpa_Mst_HrNameList where Login = 'ASSETWORLDCORP-\\" + xassign_to + "' ";
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
                        _ = zsendmail.sendEmail(subject + " Mail To Permit ChangeAssign", email, body, pathfileins);
                    }

                }

            }
        }

        private void sendMailToRequester(string pid, string xsubject, string xsumit_by)
        {
            string sqlupdate = @"update li_permit_request set status='completed',updated_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where process_id = '" + pid + "'";
            zdb.ExecNonQuery(sqlupdate, zconnstr);

            string subject = "";
            string body = "";
            string sql = @"select * from li_permit_request where process_id = '" + pid + "'";
            var dt = zdb.ExecSql_DataTable(sql, zconnstr);
            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                string id = dr["permit_no"].ToString();
                subject = xsubject;
                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                body = "เอกสารเลขที่" + dr["document_no"].ToString() + " ได้รับการดำเนินการเสร็จสิ้นแล้ว กรุณาตรวจสอบผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a><br/>" + 
                        "Document no" + dr["document_no"].ToString() + " It has been completed. Please check through the system. <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

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
                        string sqlbpm = "select * from li_user where user_login = '" + xsumit_by + "' ";
                        System.Data.DataTable dtbpm = zdb.ExecSql_DataTable(sqlbpm, zconnstr);

                        if (dtbpm.Rows.Count > 0)
                        {
                            email = dtbpm.Rows[0]["email"].ToString();

                        }
                        else
                        {
                            string sqlpra = "select * from Rpa_Mst_HrNameList where Login = 'ASSETWORLDCORP-\\" + xsumit_by + "' ";
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
                        //send mail to requester
                        _ = zsendmail.sendEmail(subject + " Mail To Requester", email, body, pathfileCommregis);
                    }

                }

            }

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
                    wfAttr.assto_login = hid_assto_login.Value;
                    wfAttr.updated_by = emp.user_login;
                    wfAttr.submit_by = wfAttr.submit_by;
                    // wf.updateAssignto
                    string status = zwf.updateAssignto(wfAttr);

                    if (status == "Success")
                    {
                        sendMailChangeAssign(wfAttr.process_id, wfAttr.subject, hid_assto_login.Value);

                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                    }

                }
            }
        }
    }
}