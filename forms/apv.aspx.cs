using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Word;
using onlineLegalWF.Class;
using Org.BouncyCastle.Ocsp;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static onlineLegalWF.Class.ReplaceInsClaim;
using static onlineLegalWF.Class.ReplaceInsRenew;
using static onlineLegalWF.Class.ReplaceInsRenewAWC;

namespace onlineLegalWF.forms
{
    public partial class apv : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public MargePDF zmergepdf = new MargePDF();
        public SendMail zsendmail = new SendMail();
        public ReplaceInsNew zreplaceinsnew = new ReplaceInsNew();
        public ReplaceInsClaim zreplaceinsclaim = new ReplaceInsClaim();
        public ReplaceInsRenew zreplaceinsrenew = new ReplaceInsRenew();
        public ReplaceInsRenewAWC zreplaceinsrenewawc = new ReplaceInsRenewAWC();
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
            if (process_code == "INR_NEW" || process_code == "INR_RENEW")
            {
                string sqlinsreq = "select * from li_insurance_request where process_id='" + req + "'";
                var resinsreq = zdb.ExecSql_DataTable(sqlinsreq, zconnstr);

                //get data ins req
                if (resinsreq.Rows.Count > 0)
                {
                    id = resinsreq.Rows[0]["req_no"].ToString();
                    req_no.Value = resinsreq.Rows[0]["req_no"].ToString();
                    req_date.Text = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(resinsreq.Rows[0]["req_date"]), "en");
                    from.Text = resinsreq.Rows[0]["company_name"].ToString();
                    doc_no.Text = resinsreq.Rows[0]["document_no"].ToString();
                    subject.Text = resinsreq.Rows[0]["subject"].ToString();
                    hid_bucode.Value = resinsreq.Rows[0]["bu_code"].ToString();

                    string sqlbu = @"select * from li_business_unit where bu_code = '" + hid_bucode.Value + "'";
                    var resbu = zdb.ExecSql_DataTable(sqlbu, zconnstr);
                    if (resbu.Rows.Count > 0)
                    {
                        DataRow dr = resbu.Rows[0];
                        hid_external_domain.Value = dr["external_domain"].ToString();
                    }

                    //init data UcAttachAndCommentLogs
                    initDataAttachAndComment(resinsreq.Rows[0]["process_id"].ToString());

                    getDocument(id);
                }
            }
            else if (process_code == "INR_CLAIM" || process_code == "INR_CLAIM_2" || process_code == "INR_CLAIM_3") 
            {
                string sqlinsclaim = "select * from li_insurance_claim where process_id='" + req + "'";
                var resinsclaim = zdb.ExecSql_DataTable(sqlinsclaim, zconnstr);

                //get data ins req
                if (resinsclaim.Rows.Count > 0)
                {
                    id = resinsclaim.Rows[0]["claim_no"].ToString();
                    req_no.Value = resinsclaim.Rows[0]["claim_no"].ToString();
                    req_date.Text = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(resinsclaim.Rows[0]["claim_date"]), "en");
                    from.Text = resinsclaim.Rows[0]["company_name"].ToString();
                    doc_no.Text = resinsclaim.Rows[0]["document_no"].ToString();
                    subject.Text = resinsclaim.Rows[0]["incident"].ToString();
                    hid_bucode.Value = resinsclaim.Rows[0]["bu_code"].ToString();

                    string sqlbu = @"select * from li_business_unit where bu_code = '" + hid_bucode.Value + "'";
                    var resbu = zdb.ExecSql_DataTable(sqlbu, zconnstr);
                    if (resbu.Rows.Count > 0)
                    {
                        DataRow dr = resbu.Rows[0];
                        hid_external_domain.Value = dr["external_domain"].ToString();
                    }

                    //init data UcAttachAndCommentLogs
                    initDataAttachAndComment(resinsclaim.Rows[0]["process_id"].ToString());

                    getDocument(id);
                }
            }
            else if (process_code == "INR_AWC_RENEW")
            {
                string sqlinsmemo = "select * from li_insurance_renew_awc_memo where process_id='" + req + "'";
                var resinsmemo = zdb.ExecSql_DataTable(sqlinsmemo, zconnstr);

                //get data ins req
                if (resinsmemo.Rows.Count > 0)
                {
                    id = resinsmemo.Rows[0]["req_no"].ToString();
                    req_no.Value = resinsmemo.Rows[0]["req_no"].ToString();
                    req_date.Text = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(resinsmemo.Rows[0]["req_date"]), "en");
                    from.Text = resinsmemo.Rows[0]["company_name"].ToString();
                    doc_no.Text = resinsmemo.Rows[0]["document_no"].ToString();
                    subject.Text = resinsmemo.Rows[0]["subject"].ToString();
                    hid_bucode.Value = "";

                    string sqlbu = @"select * from li_business_unit where bu_code = '" + hid_bucode.Value + "'";
                    var resbu = zdb.ExecSql_DataTable(sqlbu, zconnstr);
                    if (resbu.Rows.Count > 0)
                    {
                        DataRow dr = resbu.Rows[0];
                        hid_external_domain.Value = dr["external_domain"].ToString();
                    }

                    //init data UcAttachAndCommentLogs
                    initDataAttachAndComment(resinsmemo.Rows[0]["process_id"].ToString());

                    getDocument(id);
                }
            }

            string mode = Request.QueryString["mode"];
            if (mode == "tracking") 
            {
                btn_Approve.Enabled = false;
                btn_Reject.Enabled = false;
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
                pdf_render.Attributes["src"] = host_url+"render/pdf?id=" + pathfile;
            }
        }

        private void initDataAttachAndComment(string pid)
        {
            lblPID.Text = pid;
            hid_PID.Value = pid;
            ucAttachment1.ini_object(pid);
            ucCommentlog1.ini_object(pid);
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
                    //if (wfAttr.step_name == "CCO Approve" && wfAttr.process_code == "INR_NEW")
                    //{
                    //    wfA_NextStep.wf_status = "WAITATCH";
                    //    string sqlupdate = @"update li_insurance_request set status='approve',updated_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where process_id = '" + wfAttr.process_id + "'";
                    //    zdb.ExecNonQuery(sqlupdate, zconnstr);
                    //}
                    if (wfAttr.step_name == "Insurance Specialist Confirm Approve")
                    {
                        if (wfAttr.process_code == "INR_CLAIM" || wfAttr.process_code == "INR_CLAIM_2" || wfAttr.process_code == "INR_CLAIM_3") 
                        {
                            wfA_NextStep.wf_status = "COMPLETED";
                            string sqlupdate = @"update li_insurance_claim set status='approve',updated_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where process_id = '" + wfAttr.process_id + "'";
                            zdb.ExecNonQuery(sqlupdate, zconnstr);
                        }

                    }
                    //else if (wfAttr.step_name == "BU Approve" && wfAttr.process_code == "INR_RENEW")
                    //{
                    //    wfA_NextStep.wf_status = "WAITATCH";
                    //    string sqlupdate = @"update li_insurance_request set status='approve',updated_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where process_id = '" + wfAttr.process_id + "'";
                    //    zdb.ExecNonQuery(sqlupdate, zconnstr);
                    //}
                    //else if (wfAttr.step_name == "End") 
                    if (wfA_NextStep.step_name == "End") 
                    {
                        wfA_NextStep.wf_status = "COMPLETED";
                    }
                    
                    wfA_NextStep.submit_by = wfA_NextStep.submit_by;
                    string status = zwf.Insert_NextStep(wfA_NextStep);

                    if (status == "Success") 
                    {
                        // check processcode loop gendocument
                        if (process_code == "INR_NEW")
                        {
                            GenDocumnetInsNew(lblPID.Text);
                            //check step not in step_name cco approve and end send email notification user assign_to approve or review
                            if (wfAttr.step_name != "End" && wfAttr.step_name != "CCO Approve")
                            {
                                string subject = "";
                                string body = "";
                                string sql = @"select * from li_insurance_request where process_id = '" + wfAttr.process_id + "'";
                                var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                                if (dt.Rows.Count > 0)
                                {
                                    var dr = dt.Rows[0];
                                    string id = dr["req_no"].ToString();
                                    subject = dr["subject"].ToString();
                                    var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                                    body = "คุณได้รับมอบหมายให้ตรวจสอบเอกสารเลขที่ " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='"+host_url_sendmail+"legalportal/legalportal?m=myworklist'>Click</a>";

                                    string pathfileins = "";
                                    string outputdirectory = "";

                                    string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                    var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                    if (resfile.Rows.Count > 0)
                                    {
                                        pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                                        outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                                        List<string> listpdf = new List<string>();
                                        listpdf.Add(pathfileins);

                                        string sqlattachfile = "select * from wf_attachment where pid = '" + wfAttr.process_id + "' and e_form IS NULL";

                                        var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                                        if (resattachfile.Rows.Count > 0)
                                        {
                                            foreach (DataRow item in resattachfile.Rows)
                                            {
                                                listpdf.Add(item["attached_filepath"].ToString());
                                            }
                                        }
                                        //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                                        string[] pdfFiles = listpdf.ToArray();

                                        string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

                                        string email = "";
                                        string ccemail = "";

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
                                            ccemail = "ruangsuk.k@assetworldcorp-th.com";
                                        }
                                        else 
                                        {
                                            ////fix mail test
                                            email = "legalwfuat2024@gmail.com";
                                            ccemail = "legalwfuat2024@gmail.com";
                                        }

                                        if (!string.IsNullOrEmpty(email)) 
                                        {
                                            //_ = zsendmail.sendEmail(subject + " Mail To Next Appove", email, body, filepath);

                                            //check send email cco cc to khun ruangsuk.k@assetworldcorp-th.com
                                            if (wfA_NextStep.step_name == "CCO Approve")
                                            {
                                                //string ccemail = "ruangsuk.k@assetworldcorp-th.com";
                                                //string ccemail = "legalwfuat2024@gmail.com";
                                                _ = zsendmail.sendEmailCC(subject + " Mail To Next Appove", email, ccemail, body, filepath);
                                            }
                                            else 
                                            {
                                                _ = zsendmail.sendEmail(subject + " Mail To Next Appove", email, body, filepath);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        else if (process_code == "INR_CLAIM" || process_code == "INR_CLAIM_2" || process_code == "INR_CLAIM_3")
                        {
                            GenDocumnetInsClaim(lblPID.Text, wfAttr.submit_by);
                            //check step not in step_name awc approval approve and end send email notification user assign_to approve or review
                            if (wfAttr.step_name != "End" && wfAttr.step_name != "Insurance Specialist Confirm Approve")
                            {
                                string subject = "";
                                string body = "";
                                string sql = @"select * from li_insurance_claim where process_id = '" + wfAttr.process_id + "'";
                                var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                                if (dt.Rows.Count > 0)
                                {
                                    var dr = dt.Rows[0];
                                    string id = dr["claim_no"].ToString();
                                    subject = dr["incident"].ToString();
                                    var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                                    body = "คุณได้รับมอบหมายให้ตรวจสอบเอกสารเลขที่ " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='"+host_url_sendmail+"legalportal/legalportal?m=myworklist'>Click</a>";

                                    string pathfileins = "";
                                    string outputdirectory = "";

                                    string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                    var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                    if (resfile.Rows.Count > 0)
                                    {
                                        pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                                        outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                                        List<string> listpdf = new List<string>();
                                        listpdf.Add(pathfileins);

                                        string sqlattachfile = "select * from wf_attachment where pid = '" + wfAttr.process_id + "' and e_form IS NULL";

                                        var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                                        if (resattachfile.Rows.Count > 0)
                                        {
                                            foreach (DataRow item in resattachfile.Rows)
                                            {
                                                listpdf.Add(item["attached_filepath"].ToString());
                                            }
                                        }
                                        //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                                        string[] pdfFiles = listpdf.ToArray();

                                        string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

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
                                            _ = zsendmail.sendEmail(subject + " Mail To Next Appove", email, body, filepath);
                                        }

                                    }

                                }
                            }
                        }
                        else if (process_code == "INR_RENEW")
                        {
                            GenDocumnetInsRenew(lblPID.Text);
                            //check step not in step_name bu approve and end send email notification user assign_to approve or review
                            if (wfAttr.step_name != "End" && wfAttr.step_name != "BU Approve")
                            {
                                string subject = "";
                                string body = "";
                                string sql = @"select * from li_insurance_request where process_id = '" + wfAttr.process_id + "'";
                                var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                                if (dt.Rows.Count > 0)
                                {
                                    var dr = dt.Rows[0];
                                    string id = dr["req_no"].ToString();
                                    subject = dr["subject"].ToString();
                                    var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                                    body = "คุณได้รับมอบหมายให้ตรวจสอบเอกสารเลขที่ " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='"+host_url_sendmail+"legalportal/legalportal?m=myworklist'>Click</a>";

                                    string pathfileins = "";
                                    string outputdirectory = "";

                                    string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                    var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                    if (resfile.Rows.Count > 0)
                                    {
                                        pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                                        outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                                        List<string> listpdf = new List<string>();
                                        listpdf.Add(pathfileins);

                                        string sqlattachfile = "select * from wf_attachment where pid = '" + wfAttr.process_id + "' and e_form IS NULL";

                                        var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                                        if (resattachfile.Rows.Count > 0)
                                        {
                                            foreach (DataRow item in resattachfile.Rows)
                                            {
                                                listpdf.Add(item["attached_filepath"].ToString());
                                            }
                                        }
                                        //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                                        string[] pdfFiles = listpdf.ToArray();

                                        string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

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
                                            _ = zsendmail.sendEmail(subject + " Mail To Next Appove", email, body, filepath);
                                        }
                                    }

                                }
                            }
                        }
                        else if (process_code == "INR_AWC_RENEW")
                        {
                            GenDocumnetInsRenewAWC(lblPID.Text);
                            //check step not in step_name cco approve and end send email notification user assign_to approve or review
                            if (wfAttr.step_name != "End" && wfAttr.step_name != "CCO Approve")
                            {
                                string subject = "";
                                string body = "";
                                string sql = @"select * from li_insurance_renew_awc_memo where process_id = '" + wfAttr.process_id + "'";
                                var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                                if (dt.Rows.Count > 0)
                                {
                                    var dr = dt.Rows[0];
                                    string id = dr["req_no"].ToString();
                                    subject = dr["subject"].ToString();
                                    var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                                    body = "คุณได้รับมอบหมายให้ตรวจสอบเอกสารเลขที่ " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='"+host_url_sendmail+"legalportal/legalportal?m=myworklist'>Click</a>";

                                    string pathfileins = "";
                                    string outputdirectory = "";

                                    string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                    var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                    if (resfile.Rows.Count > 0)
                                    {
                                        pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                                        outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                                        List<string> listpdf = new List<string>();
                                        listpdf.Add(pathfileins);

                                        string sqlattachfile = "select * from wf_attachment where pid = '" + wfAttr.process_id + "' and e_form IS NULL";

                                        var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                                        if (resattachfile.Rows.Count > 0)
                                        {
                                            foreach (DataRow item in resattachfile.Rows)
                                            {
                                                listpdf.Add(item["attached_filepath"].ToString());
                                            }
                                        }
                                        //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                                        string[] pdfFiles = listpdf.ToArray();

                                        string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

                                        string email = "";
                                        string ccemail = "";
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
                                            ccemail = "ruangsuk.k@assetworldcorp-th.com";
                                        }
                                        else
                                        {
                                            ////fix mail test
                                            email = "legalwfuat2024@gmail.com";
                                            ccemail = "legalwfuat2024@gmail.com";
                                        }

                                        if (!string.IsNullOrEmpty(email))
                                        {
                                            //_ = zsendmail.sendEmail(subject + " Mail To Next Appove", email, body, filepath);

                                            //check send email to cco cc khun ruangsuk.k@assetworldcorp-th.com
                                            if (wfA_NextStep.step_name == "CCO Approve")
                                            {
                                                //string ccemail = "ruangsuk.k@assetworldcorp-th.com";
                                                //string ccemail = "legalwfuat2024@gmail.com";
                                                _ = zsendmail.sendEmailCC(subject + " Mail To Next Appove", email, ccemail, body, filepath);
                                            }
                                            else
                                            {
                                                _ = zsendmail.sendEmail(subject + " Mail To Next Appove", email, body, filepath);
                                            }
                                        }
                                    }

                                }
                            }
                        }

                        if (wfAttr.step_name == "CCO Approve" && wfAttr.process_code == "INR_NEW")
                        {
                            string subject = "";
                            string body = "";
                            string sql = @"select * from li_insurance_request where process_id = '" + wfAttr.process_id + "'";
                            var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                            if (dt.Rows.Count > 0)
                            {
                                var dr = dt.Rows[0];
                                string id = dr["req_no"].ToString();
                                subject = dr["subject"].ToString();
                                body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้รับการอนุมัติผ่านระบบแล้ว";

                                string pathfileins = "";
                                string outputdirectory = "";

                                string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                if (resfile.Rows.Count > 0)
                                {
                                    pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                                    outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                                    List<string> listpdf = new List<string>();
                                    listpdf.Add(pathfileins);

                                    string sqlattachfile = "select * from wf_attachment where pid = '" + wfAttr.process_id + "' and e_form IS NULL";

                                    var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                                    if (resattachfile.Rows.Count > 0)
                                    {
                                        foreach (DataRow item in resattachfile.Rows)
                                        {
                                            listpdf.Add(item["attached_filepath"].ToString());
                                        }
                                    }
                                    //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                                    string[] pdfFiles = listpdf.ToArray();

                                    string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

                                    string email = "";
                                    string emailxclv = "";
                                    string[] emailInsurance;
                                    string emailpro = "";

                                    var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                                    ////get mail from db
                                    /////send mail 
                                    if (isdev != "true")
                                    {
                                        var empreq = empFunc.getEmpInfo(wfAttr.submit_by);
                                        if (empreq.user_login != null)
                                        {
                                            email = empreq.email;
                                        }
                                        emailInsurance = new string[] { "jaroonsak.n@assetworldcorp-th.com", "warin.k@assetworldcorp-th.com" };
                                        emailpro = "pornnipa.u@assetworldcorp-th.com";

                                        string sqlbu = @"select * from li_business_unit where bu_code = '" + hid_bucode.Value + "'";
                                        var resbu = zdb.ExecSql_DataTable(sqlbu, zconnstr);
                                        if (resbu.Rows.Count > 0)
                                        {
                                            DataRow drbu = resbu.Rows[0];
                                            string xclv = drbu["c_level"].ToString();
                                            //get data am user
                                            if (!string.IsNullOrEmpty(xclv))
                                            {
                                                var empclv = empFunc.getEmpInfo(xclv);
                                                if (empclv.user_login != null)
                                                {
                                                    emailxclv = empclv.email;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ////fix mail test
                                        email = "legalwfuat2024@gmail.com";
                                        emailxclv = "legalwfuat2024@gmail.com";
                                        emailInsurance = new string[] { "legalwfuat2024@gmail.com" };
                                        emailpro = "legalwfuat2024@gmail.com";
                                    }

                                    if (!string.IsNullOrEmpty(email))
                                    {
                                        //send mail to requester
                                        _ = zsendmail.sendEmail(subject + " Mail To Requester", email, body, filepath);

                                        //send mait to Procurement
                                        _ = zsendmail.sendEmail(subject + " Mail To Procurement", emailpro, body, filepath);


                                        ////send mail to Insurance
                                        //string[] emailInsurance = new string[] { "jaroonsak.n@assetworldcorp-th.com", "warin.k@assetworldcorp-th.com" };
                                        _ = zsendmail.sendEmails(subject + " Mail To Insurance", emailInsurance, body, filepath);
                                        //_ = zsendmail.sendEmail(subject + " Mail To Jaroonsak.n", email, body, filepath);
                                        //_ = zsendmail.sendEmail(subject + " Mail To Warin.k", email, body, filepath);

                                        ////send mail to C-Level
                                        _ = zsendmail.sendEmail(subject + " Mail To C-Level", emailxclv, body, filepath);
                                    }

                                }

                            }
                        }


                        else if (wfAttr.step_name == "Insurance Specialist Confirm Approve")
                        {
                            if (wfAttr.process_code == "INR_CLAIM" || wfAttr.process_code == "INR_CLAIM_2" || wfAttr.process_code == "INR_CLAIM_3") 
                            {
                                string subject = "";
                                string body = "";
                                string sql = @"select * from li_insurance_claim where process_id = '" + wfAttr.process_id + "'";
                                var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                                if (dt.Rows.Count > 0)
                                {
                                    var dr = dt.Rows[0];
                                    string id = dr["claim_no"].ToString();
                                    subject = dr["incident"].ToString();
                                    body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้รับการอนุมัติผ่านระบบแล้ว";

                                    string pathfileins = "";
                                    string outputdirectory = "";

                                    string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                    var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                    if (resfile.Rows.Count > 0)
                                    {
                                        pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                                        outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                                        List<string> listpdf = new List<string>();
                                        listpdf.Add(pathfileins);

                                        string sqlattachfile = "select * from wf_attachment where pid = '" + wfAttr.process_id + "' and e_form IS NULL";

                                        var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                                        if (resattachfile.Rows.Count > 0)
                                        {
                                            foreach (DataRow item in resattachfile.Rows)
                                            {
                                                listpdf.Add(item["attached_filepath"].ToString());
                                            }
                                        }
                                        //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                                        string[] pdfFiles = listpdf.ToArray();

                                        string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

                                        string email = "";
                                        string emailpro = "";
                                        string[] emailIndara;
                                        string[] emailInsurance;

                                        var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                                        ////get mail from db
                                        /////send mail to next_approve
                                        if (isdev != "true")
                                        {
                                            var empreq = empFunc.getEmpInfo(wfAttr.submit_by);
                                            if (empreq.user_login != null)
                                            {
                                                email = empreq.email;
                                            }
                                            //emailIndara = new string[] { "teerapat.w@tgh.co.th", "phakorn.s@tgh.co.th" };
                                            emailIndara = new string[] { "jaroonsak.n@assetworldcorp-th.com" };
                                            emailInsurance = new string[] { "jaroonsak.n@assetworldcorp-th.com", "warin.k@assetworldcorp-th.com" };
                                            emailpro = "pornnipa.u@assetworldcorp-th.com";
                                        }
                                        else
                                        {
                                            ////fix mail test
                                            email = "legalwfuat2024@gmail.com";
                                            emailIndara = new string[] { "legalwfuat2024@gmail.com", "manit.ch@assetworldcorp-th.com" };
                                            emailInsurance = new string[] { "legalwfuat2024@gmail.com", "manit.ch@assetworldcorp-th.com" };
                                            emailpro = "legalwfuat2024@gmail.com";
                                        }

                                        if (!string.IsNullOrEmpty(email))
                                        {
                                            //send mail to requester
                                            _ = zsendmail.sendEmail(subject + " Mail To Requester", email, body, filepath);

                                            //send mait to Procurement
                                            //_ = zsendmail.sendEmail(subject + " Mail To Procurement", emailpro, body, filepath);


                                            ////send mail to Insurance
                                            //string[] emailInsurance = new string[] { "jaroonsak.n@assetworldcorp-th.com", "warin.k@assetworldcorp-th.com" };
                                            _ = zsendmail.sendEmails(subject + " Mail To Insurance", emailInsurance, body, filepath);
                                            //_ = zsendmail.sendEmail(subject + " Mail To Jaroonsak.n", email, body, filepath);
                                            //_ = zsendmail.sendEmail(subject + " Mail To Warin.k", email, body, filepath);

                                            //send mail to indara
                                            //get file eform and attach first attachfile
                                            //string[] pdfFilesIndara = new string[] { resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf"), resattachfile.Rows[resattachfile.Rows.Count-1]["attached_filepath"].ToString() };
                                            //string filepathIndara = zmergepdf.mergefilePDF(pdfFilesIndara, outputdirectory);
                                            //string[] emailIndara = new string[] { "teerapat.w@tgh.co.th", "phakorn.s@tgh.co.th" };
                                            //_ = zsendmail.sendEmails(subject + " Mail To indara", emailIndara, body, filepathIndara);
                                        }

                                    }

                                }
                            }
                            
                        }
                        else if (wfAttr.step_name == "BU Approve" && wfAttr.process_code == "INR_RENEW")
                        {
                            string subject = "";
                            string body = "";
                            string sql = @"select * from li_insurance_request where process_id = '" + wfAttr.process_id + "'";
                            var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                            if (dt.Rows.Count > 0)
                            {
                                var dr = dt.Rows[0];
                                string id = dr["req_no"].ToString();
                                subject = dr["subject"].ToString();
                                body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้รับการอนุมัติผ่านระบบแล้ว";

                                string pathfileins = "";
                                string outputdirectory = "";

                                string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                if (resfile.Rows.Count > 0)
                                {
                                    pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                                    outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                                    List<string> listpdf = new List<string>();
                                    listpdf.Add(pathfileins);

                                    string sqlattachfile = "select * from wf_attachment where pid = '" + wfAttr.process_id + "' and e_form IS NULL";

                                    var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                                    if (resattachfile.Rows.Count > 0)
                                    {
                                        foreach (DataRow item in resattachfile.Rows)
                                        {
                                            listpdf.Add(item["attached_filepath"].ToString());
                                        }
                                    }
                                    //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                                    string[] pdfFiles = listpdf.ToArray();

                                    string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

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
                                        //send mail to requester
                                        _ = zsendmail.sendEmail(subject + " Mail To Requester", email, body, filepath);

                                        //send mait to Procurement
                                        //_ = zsendmail.sendEmail(subject + " Mail To Procurement", email, body, filepath);

                                        ////send mail to Insurance
                                        //string[] emailInsurance = new string[] { "jaroonsak.n@assetworldcorp-th.com", "warin.k@assetworldcorp-th.com" };
                                        //_ = zsendmail.sendEmails(subject + " Mail To Insurance", emailInsurance, body, filepath);
                                        _ = zsendmail.sendEmail(subject + " Mail To Jaroonsak.n", email, body, filepath);
                                        _ = zsendmail.sendEmail(subject + " Mail To Warin.k", email, body, filepath);
                                    }
                                    

                                }

                            }
                        }
                        else if (wfAttr.step_name == "CCO Approve" && wfAttr.process_code == "INR_AWC_RENEW")
                        {
                            string subject = "";
                            string body = "";
                            string sql = @"select * from li_insurance_renew_awc_memo where process_id = '" + wfAttr.process_id + "'";
                            var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                            if (dt.Rows.Count > 0)
                            {
                                var dr = dt.Rows[0];
                                string id = dr["req_no"].ToString();
                                subject = dr["subject"].ToString();
                                body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้รับการอนุมัติผ่านระบบแล้ว";

                                string pathfileins = "";
                                string outputdirectory = "";

                                string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                if (resfile.Rows.Count > 0)
                                {
                                    pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                                    outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                                    List<string> listpdf = new List<string>();
                                    listpdf.Add(pathfileins);

                                    string sqlattachfile = "select * from wf_attachment where pid = '" + wfAttr.process_id + "' and e_form IS NULL";

                                    var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                                    if (resattachfile.Rows.Count > 0)
                                    {
                                        foreach (DataRow item in resattachfile.Rows)
                                        {
                                            listpdf.Add(item["attached_filepath"].ToString());
                                        }
                                    }
                                    //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                                    string[] pdfFiles = listpdf.ToArray();

                                    string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

                                    //string email = "";
                                    string[] emailInsurance;
                                    string emailpro = "";

                                    var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                                    ////get mail from db
                                    /////send mail to next_approve
                                    if (isdev != "true")
                                    {
                                        ////fix mail test 
                                        //email = "legalwfuat2024@gmail.com";
                                        emailpro = "pornnipa.u@assetworldcorp-th.com";
                                        emailInsurance = new string[] { "jaroonsak.n@assetworldcorp-th.com", "warin.k@assetworldcorp-th.com" };
                                    }
                                    else 
                                    {
                                        ////get req_no by process_id from li_insurance_renew_awc_memo_req from db loop for send mail
                                        //string sqlmemo = "select * from li_insurance_renew_awc_memo_req where process_id = '" + wfAttr.process_id + "'";
                                        //System.Data.DataTable dtmemo = zdb.ExecSql_DataTable(sqlmemo, zconnstr);

                                        //List<string> listEmails = new List<string>();
                                        //if (dtmemo.Rows.Count > 0)
                                        //{
                                        //    foreach (DataRow row in dtmemo.Rows)
                                        //    {
                                        //        var req_id = row["req_no"].ToString();

                                        //        string sql_login = @"select top 1 submit_by from wf_routing where process_id in (select process_id from li_insurance_request where req_no ='" + req_id + "')";
                                        //        System.Data.DataTable dt_login = zdb.ExecSql_DataTable(sql_login, zconnstr);

                                        //        if (dt_login.Rows.Count > 0)
                                        //        {
                                        //            var submitby = dt_login.Rows[0]["submit_by"].ToString();
                                        //            string sqlbpm = "select * from li_user where user_login = '" + submitby + "' ";
                                        //            System.Data.DataTable dtbpm = zdb.ExecSql_DataTable(sqlbpm, zconnstr);

                                        //            if (dtbpm.Rows.Count > 0)
                                        //            {
                                        //                listEmails.Add(dtbpm.Rows[0]["email"].ToString());
                                        //                //email = dtbpm.Rows[0]["email"].ToString();

                                        //            }
                                        //            else
                                        //            {
                                        //                string sqlpra = "select * from Rpa_Mst_HrNameList where Login = 'ASSETWORLDCORP-\\" + submitby + "' ";
                                        //                System.Data.DataTable dtrpa = zdb.ExecSql_DataTable(sqlpra, zconnstrrpa);

                                        //                if (dtrpa.Rows.Count > 0)
                                        //                {
                                        //                    listEmails.Add(dtbpm.Rows[0]["Email"].ToString());
                                        //                    //email = dtrpa.Rows[0]["Email"].ToString();
                                        //                }

                                        //            }
                                        //        }
                                        //    }
                                        //}

                                        //string[] emails = listEmails.ToArray();
                                        ////send mail to requester loop for all bu or prop
                                        //_ = zsendmail.sendEmails(subject + "Mail To Requester", emails, body, filepath);

                                        //send mait to Procurement
                                        ////fix mail test 
                                        //email = "legalwfuat2024@gmail.com";
                                        emailpro = "legalwfuat2024@gmail.com";
                                        emailInsurance = new string[] { "legalwfuat2024@gmail.com" };
                                    }

                                    //_ = zsendmail.sendEmail(subject + " Mail To Requester", email, body, filepath);

                                    //send mait to Procurement
                                    _ = zsendmail.sendEmail(subject + " Mail To Procurement", emailpro, body, filepath);

                                    ////send mail to Insurance
                                    //string[] emailInsurance = new string[] { "jaroonsak.n@assetworldcorp-th.com", "warin.k@assetworldcorp-th.com" };
                                    _ = zsendmail.sendEmails(subject + " Mail To Insurance", emailInsurance, body, filepath);
                                }

                            }
                        }
                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        Response.Redirect(host_url+"legalportal/legalportal.aspx?m=myworklist",false);
                    }

                }
            }

            
        }

        private void GenDocumnetInsNew(string pid)
        {
            string xreq_no = "";
            var path_template = ConfigurationManager.AppSettings["WT_Template_insurance"].ToString();
            string templatefile = path_template + @"\InsuranceTemplateRequest.docx";
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\inreq_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            string sqlinsreq = "select * from li_insurance_request where process_id='" + pid + "'";
            var resinsreq = zdb.ExecSql_DataTable(sqlinsreq, zconnstr);

            ReplaceInsNew_TagData data = new ReplaceInsNew_TagData();
            //get data ins req
            if (resinsreq.Rows.Count > 0)
            {
                xreq_no = resinsreq.Rows[0]["req_no"].ToString();
                
                ////get gm or am check external domain
                string xbu_code = resinsreq.Rows[0]["bu_code"].ToString();
                string sqlbu = @"select * from li_business_unit where bu_code = '" + xbu_code + "'";

                var res = zdb.ExecSql_DataTable(sqlbu, zconnstr);

                var requestor = "";
                var requestorpos = "";
                if (res.Rows.Count > 0)
                {
                    string xexternal_domain = res.Rows[0]["external_domain"].ToString();
                    string xgm = res.Rows[0]["gm"].ToString();
                    string xam = res.Rows[0]["head_am"].ToString();
                    var empFunc = new EmpInfo();

                    if (xexternal_domain == "Y")
                    {
                        //get data user
                        var emp = empFunc.getEmpInfo(xam);
                        requestor = emp.full_name_en;
                        requestorpos = emp.position_en;
                    }
                    else
                    {
                        //get data user
                        var emp = empFunc.getEmpInfo(xgm);
                        requestor = emp.full_name_en;
                        requestorpos = emp.position_en;
                    }

                }

                var apv1 = "คุณจรูญศักดิ์ นามะฮง";
                var apv1pos = "Insurance Specialist";
                var apv1_2 = "คุณวารินทร์ เกลียวไพศาล";
                var apv2 = "คุณชโลทร ศรีสมวงษ์";
                var apv2pos = "Head of Legal";
                var apv3 = "คุณชยุต อมตวนิช";
                var apv3pos = "Head of Risk Management";

                var apv4 = "ดร.สิเวศ โรจนสุนทร";
                var apv4pos = "CCO";

                data.sign_name1 = "";
                data.name1 = requestor;
                data.position1 = requestorpos;
                data.date1 = "";

                data.sign_name2 = "";
                data.name2 = apv1;
                data.position2 = apv1pos;
                data.date2 = "";

                data.sign_name22 = "";
                data.name22 = apv1_2;
                data.date22 = "";

                data.sign_name3 = "";
                data.name3 = apv2;
                data.position3 = apv2pos;
                data.date3 = "";

                data.sign_name4 = "";
                data.name4 = apv3;
                data.position4 = apv3pos;
                data.date4 = "";

                data.sign_name5 = "";
                data.name5 = apv4;
                data.position5 = apv4pos;
                data.date5 = "";
                data.cb1 = "";
                data.cb2 = "";
                data.remark5 = "";
            }
            System.Data.DataTable dtStr = zreplaceinsnew.BindTagData(pid,data);

            #region Sample ReplaceTable
            System.Data.DataTable dtProperties1 = new System.Data.DataTable();
            dtProperties1.Columns.Add("tagname", typeof(string));
            dtProperties1.Columns.Add("col_name", typeof(string));
            dtProperties1.Columns.Add("col_width", typeof(string));
            dtProperties1.Columns.Add("col_align", typeof(string)); //Left, Right, Center
            dtProperties1.Columns.Add("col_valign", typeof(string)); //Top, Middle, Bottom
            dtProperties1.Columns.Add("col_font", typeof(string));
            dtProperties1.Columns.Add("col_fontsize", typeof(string));
            dtProperties1.Columns.Add("col_fontcolor", typeof(string));
            dtProperties1.Columns.Add("col_color", typeof(string));
            dtProperties1.Columns.Add("header_height", typeof(string));
            dtProperties1.Columns.Add("header_color", typeof(string));
            dtProperties1.Columns.Add("header_font", typeof(string));
            dtProperties1.Columns.Add("header_fontsize", typeof(string));
            dtProperties1.Columns.Add("header_fontbold", typeof(string));
            dtProperties1.Columns.Add("header_align", typeof(string)); //Left, Right, Center
            dtProperties1.Columns.Add("header_valign", typeof(string)); //Top, Middle, Bottom
            dtProperties1.Columns.Add("header_fontcolor", typeof(string));
            dtProperties1.Columns.Add("row_height", typeof(string));
            // Replace #table1# ------------------------------------------------------
            DataRow dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "No";
            dr["col_width"] = "100";
            dr["col_align"] = "Center";
            dr["col_valign"] = "Top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Property Insured";
            dr["col_width"] = "200";
            dr["col_align"] = "Left";
            dr["col_valign"] = "Top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Indemnity Period";
            dr["col_width"] = "200";
            dr["col_align"] = "Center";
            dr["col_valign"] = "Top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Sum Insured";
            dr["col_width"] = "200";
            dr["col_align"] = "Center";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Start Date";
            dr["col_width"] = "150";
            dr["col_align"] = "left";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "End Date";
            dr["col_width"] = "150";
            dr["col_align"] = "left";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            System.Data.DataTable dt = zreplaceinsnew.genTagTableData(lblPID.Text);
            #endregion

            // Convert to JSONString
            System.Data.DataTable dtTagPropsTable = new System.Data.DataTable();
            dtTagPropsTable.Columns.Add("tagname", typeof(string));
            dtTagPropsTable.Columns.Add("jsonstring", typeof(string));

            System.Data.DataTable dtTagDataTable = new System.Data.DataTable();
            dtTagDataTable.Columns.Add("tagname", typeof(string));
            dtTagDataTable.Columns.Add("jsonstring", typeof(string));
            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
            var jsonDTProperties1 = repl.DataTableToJSONWithStringBuilder(dtProperties1);
            var jsonDTdata = repl.DataTableToJSONWithStringBuilder(dt);

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
            //// Dowload Word 
            //Response.Clear();
            //Response.ContentType = "text/xml";
            //Response.AddHeader("content-disposition", $"attachment; filename={outputfn}");
            //Response.BinaryWrite(outputbyte);
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.End();
        }

        private void GenDocumnetInsClaim(string pid, string submitby)
        {
            string xclaim_no = "";
            var xdocref = GetListDoc(hid_PID.Value);
            var xiar_pfc = "";
            var xiar_uatc = "";

            var path_template = ConfigurationManager.AppSettings["WT_Template_insurance"].ToString();

            string templatefile = path_template + @"\InsuranceTemplateClaim.docx";
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\inreq_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            ReplaceInsClaim_TagData data = new ReplaceInsClaim_TagData();

            #region prepare data

            data.docref = xdocref;

            string sqlinsclaim = "select * from li_insurance_claim where process_id='" + pid + "'";
            var resinsclaim = zdb.ExecSql_DataTable(sqlinsclaim, zconnstr);

            //get data ins req
            if (resinsclaim.Rows.Count > 0)
            {
                xclaim_no = resinsclaim.Rows[0]["claim_no"].ToString();
                xiar_pfc = (!string.IsNullOrEmpty(resinsclaim.Rows[0]["iar_pfc"].ToString()) ? resinsclaim.Rows[0]["iar_pfc"].ToString() : null);
                xiar_uatc = (!string.IsNullOrEmpty(resinsclaim.Rows[0]["iar_uatc"].ToString()) ? resinsclaim.Rows[0]["iar_uatc"].ToString() : null);

                ////get moa claim get gm or am check external domain
                string xbu_code = resinsclaim.Rows[0]["bu_code"].ToString();
                string sqlbu = @"select * from li_business_unit where bu_code = '" + xbu_code + "'";
                var res = zdb.ExecSql_DataTable(sqlbu, zconnstr);

                var requestor = "";
                var requestorpos = "";
                var gmname = "";
                var gmpos = "";
                var amname = "";
                var ampos = "";
                var headamname = "";
                var headampos = "";
                if (res.Rows.Count > 0)
                {
                    var empFunc = new EmpInfo();
                    if (Session["user_login"] != null)
                    {
                        //var xlogin_name = Session["user_login"].ToString();
                        var emp = empFunc.getEmpInfo(submitby);
                        requestor = emp.full_name_en;
                        requestorpos = emp.position_en;
                    }
                    string xexternal_domain = res.Rows[0]["external_domain"].ToString();
                    string xgm = res.Rows[0]["gm"].ToString();
                    string xam = res.Rows[0]["am"].ToString();
                    string xhead_am = res.Rows[0]["head_am"].ToString();
                    //get data am user
                    if (!string.IsNullOrEmpty(xam))
                    {
                        var empam = empFunc.getEmpInfo(xam);
                        if (empam.user_login != null)
                        {
                            amname = empam.full_name_en;
                            ampos = "AM";
                        }
                    }
                    //get data head am user
                    if (!string.IsNullOrEmpty(xhead_am))
                    {
                        var empheadam = empFunc.getEmpInfo(xhead_am);
                        if (empheadam.user_login != null)
                        {
                            headamname = empheadam.full_name_en;
                            headampos = "/ Head AM";
                        }
                    }
                    //get data gm user
                    if (!string.IsNullOrEmpty(xgm))
                    {
                        var empgm = empFunc.getEmpInfo(xgm);
                        if (empgm.user_login != null)
                        {
                            gmname = empgm.full_name_en;
                            gmpos = empgm.position_en;
                        }
                    }
                }
                data.sign_propname1 = "";
                data.propname1 = requestor;
                data.propposition1 = requestorpos;
                data.propdate1 = "";

                data.sign_propname2 = "";
                data.propname2 = gmname;
                data.propposition2 = gmpos;
                data.propdate2 = "";

                data.sign_propname3 = "";
                data.propname3 = amname;
                data.propposition3 = ampos;
                data.propdate3 = "";

                data.sign_propname3_1 = "";
                data.propname3_1 = headamname;
                data.propposition3_1 = headampos;
                data.propdate3_1 = "";

                data.sign_awcname1 = "";
                data.awcname1 = "คุณจรูญศักดิ์ นามะฮง";
                data.awcposition1 = "Insurance Specialist";
                data.awcdate1 = "";

                data.sign_awcname1_2 = "";
                data.awcname1_2 = "";
                data.awcposition1_2 = "";
                data.awcdate1_2 = "";

                data.sign_awcname1_3 = "";
                data.awcname1_3 = "";
                data.awcposition1_3 = "";
                data.awcdate1_3 = "";

                data.sign_awcname2 = "";
                data.awcname2 = "คุณวารินทร์ เกลียวไพศาล";
                data.awcposition2 = "Head of Compliance";
                data.awcdate2 = "";

                data.sign_awcname3 = "";
                data.awcname3 = "คุณชโลทร ศรีสมวงษ์";
                data.awcposition3 = "Head of Legal";
                data.awcdate3 = "";

                ////check corditon deviation claim
                //float deviation = 0;
                //float cal_iar_uatc = (!string.IsNullOrEmpty(xiar_uatc) ? float.Parse(xiar_uatc) : 0);
                //float cal_iar_pfc = (!string.IsNullOrEmpty(xiar_pfc) ? float.Parse(xiar_pfc) : 0);
                //float int_iar_pfc = (!string.IsNullOrEmpty(xiar_pfc) ? float.Parse(xiar_pfc) : 0);
                //deviation = cal_iar_uatc / cal_iar_pfc;
                //if (int_iar_pfc <= 100000)
                //{
                //    data.sign_awcname1 = "";
                //    data.awcname1 = "คุณจรูญศักดิ์ นามะฮง";
                //    data.awcposition1 = "Insurance Specialist";
                //    data.awcdate1 = "";

                //    data.sign_awcname1_2 = "";
                //    data.awcname1_2 = "";
                //    data.awcposition1_2 = "";
                //    data.awcdate1_2 = "";

                //    data.sign_awcname1_3 = "";
                //    data.awcname1_3 = "";
                //    data.awcposition1_3 = "";
                //    data.awcdate1_3 = "";

                //    data.sign_awcname2 = "";
                //    data.awcname2 = "คุณวารินทร์ เกลียวไพศาล";
                //    data.awcposition2 = "Head of Compliance";
                //    data.awcdate2 = "";

                //    data.sign_awcname3 = "";
                //    data.awcname3 = "คุณชโลทร ศรีสมวงษ์";
                //    data.awcposition3 = "Head of Legal";
                //    data.awcdate3 = "";
                //}
                //else if (int_iar_pfc > 100000 && int_iar_pfc <= 1000000 && deviation <= 0.1)
                //{
                //    data.sign_awcname1 = "";
                //    data.awcname1 = "คุณจรูญศักดิ์ นามะฮง";
                //    data.awcposition1 = "Insurance Specialist";
                //    data.awcdate1 = "";

                //    data.sign_awcname1_2 = "";
                //    data.awcname1_2 = "";
                //    data.awcposition1_2 = "";
                //    data.awcdate1_2 = "";

                //    data.sign_awcname1_3 = "";
                //    data.awcname1_3 = "";
                //    data.awcposition1_3 = "";
                //    data.awcdate1_3 = "";

                //    data.sign_awcname2 = "";
                //    data.awcname2 = "คุณวารินทร์ เกลียวไพศาล";
                //    data.awcposition2 = "Head of Compliance";
                //    data.awcdate2 = "";

                //    data.sign_awcname3 = "";
                //    data.awcname3 = "คุณชโลทร ศรีสมวงษ์";
                //    data.awcposition3 = "Head of Legal";
                //    data.awcdate3 = "";
                //}
                //else if (int_iar_pfc > 100000 && int_iar_pfc <= 1000000 && deviation > 0.1)
                //{
                //    data.sign_awcname1 = "";
                //    data.awcname1 = "คุณจรูญศักดิ์ นามะฮง";
                //    data.awcposition1 = "Insurance Specialist";
                //    data.awcdate1 = "";

                //    data.sign_awcname1_2 = "";
                //    data.awcname1_2 = "คุณวารินทร์ เกลียวไพศาล";
                //    data.awcposition1_2 = "/Head of Compliance";
                //    data.awcdate1_2 = "";

                //    data.sign_awcname1_3 = "";
                //    data.awcname1_3 = "";
                //    data.awcposition1_3 = "";
                //    data.awcdate1_3 = "";

                //    data.sign_awcname2 = "";
                //    data.awcname2 = "คุณชโลทร ศรีสมวงษ์";
                //    data.awcposition2 = "Head of Legal";
                //    data.awcdate2 = "";

                //    data.sign_awcname3 = "";
                //    data.awcname3 = "ดร.สิเวศ โรจนสุนทร";
                //    data.awcposition3 = "CCO";
                //    data.awcdate3 = "";
                //}
                //else if (int_iar_pfc > 1000000 && deviation <= 0.2)
                //{
                //    data.sign_awcname1 = "";
                //    data.awcname1 = "คุณจรูญศักดิ์ นามะฮง";
                //    data.awcposition1 = "Insurance Specialist";
                //    data.awcdate1 = "";

                //    data.sign_awcname1_2 = "";
                //    data.awcname1_2 = "คุณวารินทร์ เกลียวไพศาล";
                //    data.awcposition1_2 = "/Head of Compliance";
                //    data.awcdate1_2 = "";

                //    data.sign_awcname1_3 = "";
                //    data.awcname1_3 = "";
                //    data.awcposition1_3 = "";
                //    data.awcdate1_3 = "";

                //    data.sign_awcname2 = "";
                //    data.awcname2 = "คุณชโลทร ศรีสมวงษ์";
                //    data.awcposition2 = "Head of Legal";
                //    data.awcdate2 = "";

                //    data.sign_awcname3 = "";
                //    data.awcname3 = "ดร.สิเวศ โรจนสุนทร";
                //    data.awcposition3 = "CCO";
                //    data.awcdate3 = "";
                //}
                //else if (int_iar_pfc > 1000000 && deviation > 0.2)
                //{
                //    //data.sign_awcname1 = "";
                //    //data.awcname1 = "คุณชโลทร ศรีสมวงษ์";
                //    //data.awcposition1 = "Head of Legal";
                //    //data.awcdate1 = "";

                //    data.sign_awcname1 = "";
                //    data.awcname1 = "คุณจรูญศักดิ์ นามะฮง";
                //    data.awcposition1 = "Insurance Specialist";
                //    data.awcdate1 = "";

                //    data.sign_awcname1_2 = "";
                //    data.awcname1_2 = "คุณวารินทร์ เกลียวไพศาล";
                //    data.awcposition1_2 = "/Head of Compliance";
                //    data.awcdate1_2 = "";

                //    data.sign_awcname1_3 = "";
                //    data.awcname1_3 = "คุณชโลทร ศรีสมวงษ์";
                //    data.awcposition1_3 = "/Head of Legal";
                //    data.awcdate1_3 = "";

                //    data.sign_awcname2 = "";
                //    data.awcname2 = "ดร.สิเวศ โรจนสุนทร";
                //    data.awcposition2 = "CCO";
                //    data.awcdate2 = "";

                //    data.sign_awcname3 = "";
                //    data.awcname3 = "คุณวัลลภา ไตรโสรัส";
                //    data.awcposition3 = "CEO";
                //    data.awcdate3 = "";
                //}
            }
            System.Data.DataTable dtStr = zreplaceinsclaim.bindTagData(lblPID.Text, data);

            #endregion

            //DataTable Column Properties
            System.Data.DataTable dtProperties1 = new System.Data.DataTable();
            dtProperties1.Columns.Add("tagname", typeof(string));
            dtProperties1.Columns.Add("col_name", typeof(string));
            dtProperties1.Columns.Add("col_width", typeof(string));
            dtProperties1.Columns.Add("col_align", typeof(string)); //Left, Right, Center
            dtProperties1.Columns.Add("col_valign", typeof(string)); //Top, Middle, Bottom
            dtProperties1.Columns.Add("col_font", typeof(string));
            dtProperties1.Columns.Add("col_fontsize", typeof(string));
            dtProperties1.Columns.Add("col_fontcolor", typeof(string));
            dtProperties1.Columns.Add("col_color", typeof(string));
            dtProperties1.Columns.Add("header_height", typeof(string));
            dtProperties1.Columns.Add("header_color", typeof(string));
            dtProperties1.Columns.Add("header_font", typeof(string));
            dtProperties1.Columns.Add("header_fontsize", typeof(string));
            dtProperties1.Columns.Add("header_fontbold", typeof(string));
            dtProperties1.Columns.Add("header_align", typeof(string)); //Left, Right, Center
            dtProperties1.Columns.Add("header_valign", typeof(string)); //Top, Middle, Bottom
            dtProperties1.Columns.Add("header_fontcolor", typeof(string));
            dtProperties1.Columns.Add("row_height", typeof(string));

            // Replace #tableeltc# ------------------------------------------------------
            DataRow dr = dtProperties1.NewRow();
            dr["tagname"] = "#tableeltc#";
            //dr["col_name"] = "Estimated losses to claim \r\n มูลค่าความเสียหาย";
            dr["col_name"] = "Estimated losses to claim";
            dr["col_width"] = "450";
            dr["col_align"] = "Left";
            dr["col_valign"] = "Bottom";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "8";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "15";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "8";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "15";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#tableeltc#";
            //dr["col_name"] = "IAR \r\n ทรัพย์สินเสียหาย";
            dr["col_name"] = "IAR";
            dr["col_width"] = "150";
            dr["col_align"] = "Right";
            dr["col_valign"] = "Bottom";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "8";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "15";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "8";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "15";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#tableeltc#";
            //dr["col_name"] = "BI \r\n ธุรกิจหยุดชะงัก";
            dr["col_name"] = "BI";
            dr["col_width"] = "150";
            dr["col_align"] = "Right";
            dr["col_valign"] = "Bottom";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "8";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "15";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "8";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "15";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#tableeltc#";
            //dr["col_name"] = "PL/CGL \r\n รับผิดบุคคลภายนอก";
            dr["col_name"] = "PL/CGL";
            dr["col_width"] = "150";
            dr["col_align"] = "Right";
            dr["col_valign"] = "Bottom";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "8";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "15";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "8";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "15";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#tableeltc#";
            //dr["col_name"] = "PV \r\n สาเหตุทางการเมือง";
            dr["col_name"] = "PV";
            dr["col_width"] = "150";
            dr["col_align"] = "Right";
            dr["col_valign"] = "Bottom";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "8";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "15";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "8";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "15";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#tableeltc#";
            //dr["col_name"] = "Total \r\n รวม";
            dr["col_name"] = "Total";
            dr["col_width"] = "150";
            dr["col_align"] = "Right";
            dr["col_valign"] = "Bottom";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "8";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "15";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "8";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "15";
            dtProperties1.Rows.Add(dr);

            System.Data.DataTable dt = zreplaceinsclaim.genTagTableData(lblPID.Text);

            // Convert to JSONString
            System.Data.DataTable dtTagPropsTable = new System.Data.DataTable();
            dtTagPropsTable.Columns.Add("tagname", typeof(string));
            dtTagPropsTable.Columns.Add("jsonstring", typeof(string));

            System.Data.DataTable dtTagDataTable = new System.Data.DataTable();
            dtTagDataTable.Columns.Add("tagname", typeof(string));
            dtTagDataTable.Columns.Add("jsonstring", typeof(string));
            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
            var jsonDTProperties1 = repl.DataTableToJSONWithStringBuilder(dtProperties1);
            var jsonDTdata = repl.DataTableToJSONWithStringBuilder(dt);
            //end prepare data

            // Save to Database z_replacedocx_log
            string sql = @"insert into z_replacedocx_log (replacedocx_reqno,jsonTagString, jsonTableProp, jsonTableData,template_filepath , output_directory,output_filepath, delete_output ) 
                        values('" + xclaim_no + @"',
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
            // Dowload Word 
            //Response.Clear();
            //Response.ContentType = "text/xml";
            //Response.AddHeader("content-disposition", $"attachment; filename={outputfn}");
            //Response.BinaryWrite(outputbyte);
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.End();
        }

        private string GetListDoc(string pid)
        {
            string res = "";

            string sql = @"select * from wf_attachment where pid = '" + pid + "'";
            var dtattach = zdb.ExecSql_DataTable(sql, zconnstr);

            if (dtattach.Rows.Count > 0)
            {
                int no = 0;
                foreach (DataRow dr in dtattach.Rows)
                {
                    res += (no + 1) + "." + dr["attached_filename"] + "\r\n";
                    no++;
                }
            }

            return res;
        }

        private void GenDocumnetInsRenew(string pid)
        {

            // Replace Doc
            var xreq_no = "";
            var xapprove_des = "We, therefore, request for your approval to renew mentioned insurance policy.";

            var path_template = ConfigurationManager.AppSettings["WT_Template_insurance"].ToString();

            string templatefile = path_template + @"\InsuranceTemplateRenew.docx";
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\inreq_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            string sqlinsreq = "select * from li_insurance_request where process_id='" + pid + "'";
            var resinsreq = zdb.ExecSql_DataTable(sqlinsreq, zconnstr);

            #region gentagstr data form
            ReplaceInsReNew_TagData data = new ReplaceInsReNew_TagData();
            if (resinsreq.Rows.Count > 0)
            {
                xreq_no = resinsreq.Rows[0]["req_no"].ToString();
                data.approve = xapprove_des.Replace(",", "!comma");

                var requestordate = "";

                string xbu_code = resinsreq.Rows[0]["bu_code"].ToString();
                ///get moa
                string sqlbu = @"select * from li_business_unit where bu_code = '" + xbu_code + "'";
                var res = zdb.ExecSql_DataTable(sqlbu, zconnstr);

                var requestor = "";
                var requestorpos = "";
                var gmname = "";
                var gmpos = "GM";
                var amname = "";
                var headamname = "";
                var clevelname = "";
                if (res.Rows.Count > 0)
                {
                    var empFunc = new EmpInfo();
                    if (Session["user_login"] != null)
                    {
                        var xlogin_name = Session["user_login"].ToString();
                        var emp = empFunc.getEmpInfo(xlogin_name);
                        string sqlwf = "select * from wf_routing where process_id = '" + lblPID.Text + "' and step_name = 'Start'";
                        var dtwf = zdb.ExecSql_DataTable(sqlwf, zconnstr);
                        if (dtwf.Rows.Count > 0)
                        {
                            DataRow drwf = dtwf.Rows[0];
                            var emprequester = empFunc.getEmpInfo(drwf["submit_by"].ToString());
                            if (emprequester != null)
                            {
                                requestor = emprequester.full_name_en;
                                requestorpos = emprequester.position_en;
                            }

                        }
                        else
                        {
                            requestor = emp.full_name_en;
                            requestorpos = emp.position_en;
                        }


                    }
                    string xgm = res.Rows[0]["gm"].ToString();
                    string xam = res.Rows[0]["am"].ToString();
                    string xhead_am = res.Rows[0]["head_am"].ToString();
                    string xclevel = res.Rows[0]["c_level"].ToString();
                    string xexternal_domain = res.Rows[0]["external_domain"].ToString();
                    //get data am user
                    if (!string.IsNullOrEmpty(xam))
                    {
                        var empam = empFunc.getEmpInfo(xam);
                        if (empam.user_login != null)
                        {
                            amname = empam.full_name_en;
                        }
                    }
                    //get data head am user
                    if (!string.IsNullOrEmpty(xhead_am))
                    {
                        var empheadam = empFunc.getEmpInfo(xhead_am);
                        if (empheadam.user_login != null)
                        {
                            headamname = empheadam.full_name_en;
                        }
                    }
                    //get data gm user
                    if (!string.IsNullOrEmpty(xgm))
                    {
                        var empgm = empFunc.getEmpInfo(xgm);
                        if (empgm.user_login != null)
                        {
                            gmname = empgm.full_name_en;
                            if (xexternal_domain == "Y")
                            {
                                gmpos = empgm.position_en;
                            }
                        }
                    }
                    //get data c_level user
                    if (!string.IsNullOrEmpty(xclevel))
                    {
                        var empc = empFunc.getEmpInfo(xclevel);
                        if (empc.user_login != null)
                        {
                            clevelname = empc.full_name_en;
                        }
                    }
                }

                var apv1 = gmname;
                var apv1pos = gmpos;
                var apv1date = "";
                var apv2 = amname;
                var apv2pos = "AM";
                var apv2date = "";
                var apv2_1 = headamname;
                var apv2pos_1 = "Head AM";
                var apv2date_1 = "";
                var apv3 = clevelname;
                var apv3pos = "C-Level";
                var apv3date = "";
                var signname1 = "";
                var signname2 = "";
                var signname3 = "";
                var signname3_1 = "";
                var signname4 = "";

                data.sign_name1 = signname1;
                data.name1 = requestor.Replace(",", "!comma");
                data.position1 = requestorpos.Replace(",", "!comma");
                data.date1 = requestordate.Replace(",", "!comma");

                data.sign_name2 = signname2;
                data.name2 = apv1.Replace(",", "!comma");
                data.position2 = apv1pos.Replace(",", "!comma");
                data.date2 = apv1date.Replace(",", "!comma");

                data.sign_name3 = signname3;
                data.name3 = apv2.Replace(",", "!comma");
                data.position3 = apv2pos.Replace(",", "!comma");
                data.date3 = apv2date.Replace(",", "!comma");

                data.sign_name3_1 = signname3_1;
                data.name3_1 = apv2_1.Replace(",", "!comma");
                data.position3_1 = apv2pos_1.Replace(",", "!comma");
                data.date3_1 = apv2date_1.Replace(",", "!comma");

                data.sign_name4 = signname4;
                data.name4 = apv3.Replace(",", "!comma");
                data.position4 = apv3pos.Replace(",", "!comma");
                data.date4 = apv3date.Replace(",", "!comma");

            }

            System.Data.DataTable dtStr = zreplaceinsrenew.BindTagData(lblPID.Text, data);
            #endregion 

            #region Sample ReplaceTable
            //DataTable Column Properties
            System.Data.DataTable dtProperties1 = new System.Data.DataTable();
            dtProperties1.Columns.Add("tagname", typeof(string));
            dtProperties1.Columns.Add("col_name", typeof(string));
            dtProperties1.Columns.Add("col_width", typeof(string));
            dtProperties1.Columns.Add("col_align", typeof(string)); //Left, Right, Center
            dtProperties1.Columns.Add("col_valign", typeof(string)); //Top, Middle, Bottom
            dtProperties1.Columns.Add("col_font", typeof(string));
            dtProperties1.Columns.Add("col_fontsize", typeof(string));
            dtProperties1.Columns.Add("col_fontcolor", typeof(string));
            dtProperties1.Columns.Add("col_color", typeof(string));
            dtProperties1.Columns.Add("header_height", typeof(string));
            dtProperties1.Columns.Add("header_color", typeof(string));
            dtProperties1.Columns.Add("header_font", typeof(string));
            dtProperties1.Columns.Add("header_fontsize", typeof(string));
            dtProperties1.Columns.Add("header_fontbold", typeof(string));
            dtProperties1.Columns.Add("header_align", typeof(string)); //Left, Right, Center
            dtProperties1.Columns.Add("header_valign", typeof(string)); //Top, Middle, Bottom
            dtProperties1.Columns.Add("header_fontcolor", typeof(string));
            dtProperties1.Columns.Add("row_height", typeof(string));
            // Replace #table1# ------------------------------------------------------
            DataRow dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "No";
            dr["col_width"] = "100";
            dr["col_align"] = "Center";
            dr["col_valign"] = "Top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Property Insured";
            dr["col_width"] = "200";
            dr["col_align"] = "Left";
            dr["col_valign"] = "Top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Indemnity Period";
            dr["col_width"] = "200";
            dr["col_align"] = "Center";
            dr["col_valign"] = "Top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Sum Insured";
            dr["col_width"] = "200";
            dr["col_align"] = "Center";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Start Date";
            dr["col_width"] = "150";
            dr["col_align"] = "left";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "End Date";
            dr["col_width"] = "150";
            dr["col_align"] = "left";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            System.Data.DataTable dt = zreplaceinsrenew.genTagTableData(lblPID.Text);

            #endregion

            // Convert to JSONString
            System.Data.DataTable dtTagPropsTable = new System.Data.DataTable();
            dtTagPropsTable.Columns.Add("tagname", typeof(string));
            dtTagPropsTable.Columns.Add("jsonstring", typeof(string));

            System.Data.DataTable dtTagDataTable = new System.Data.DataTable();
            dtTagDataTable.Columns.Add("tagname", typeof(string));
            dtTagDataTable.Columns.Add("jsonstring", typeof(string));
            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
            var jsonDTProperties1 = repl.DataTableToJSONWithStringBuilder(dtProperties1);
            //var jsonDTProperties2 = repl.DataTableToJSONWithStringBuilder(dtProperties2);
            var jsonDTdata = repl.DataTableToJSONWithStringBuilder(dt);
            //var jsonDTdata2 = repl.DataTableToJSONWithStringBuilder(dt2);
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
            // Dowload Word 
            //Response.Clear();
            //Response.ContentType = "text/xml";
            //Response.AddHeader("content-disposition", $"attachment; filename={outputfn}");
            //Response.BinaryWrite(outputbyte);
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.End();



        }

        private void GenDocumnetInsRenewAWC(string pid)
        {
            // Replace Doc
            string xreq_no = "";

            var path_template = ConfigurationManager.AppSettings["WT_Template_insurance"].ToString();

            string templatefile = path_template + @"\InsuranceTemplateRenewAWC.docx";
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\inreq_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            ReplaceInsReNewAWC_TagData data = new ReplaceInsReNewAWC_TagData();

            #region prepare data

            string sqlinsmemo = "select * from li_insurance_renew_awc_memo where process_id='" + pid + "'";
            var resinsmemo = zdb.ExecSql_DataTable(sqlinsmemo, zconnstr);

            if (resinsmemo.Rows.Count > 0)
            {
                xreq_no = resinsmemo.Rows[0]["req_no"].ToString();
            }

            var requestor = "";
            var requestorpos = "";

            var apv1 = "คุณจรูญศักดิ์ นามะฮง";
            var apv1pos = "Insurance Specialist";
            var apv1_2 = "คุณวารินทร์ เกลียวไพศาล";
            var apv2 = "คุณชโลทร ศรีสมวงษ์";
            var apv2pos = "Head of Legal";
            var apv3 = "คุณชยุต อมตวนิช";
            var apv3pos = "Head of Risk Management";

            var apv4 = "ดร.สิเวศ โรจนสุนทร";
            var apv4pos = "CCO";

            data.sign_name1 = "";
            data.name1 = requestor;
            data.position1 = requestorpos;
            data.date1 = "";

            data.sign_name2 = "";
            data.name2 = apv1;
            data.position2 = apv1pos;
            data.date2 = "";

            data.sign_name22 = "";
            data.name22 = apv1_2;
            data.date22 = "";

            data.sign_name3 = "";
            data.name3 = apv2;
            data.position3 = apv2pos;
            data.date3 = "";

            data.sign_name4 = "";
            data.name4 = apv3;
            data.position4 = apv3pos;
            data.date4 = "";

            data.sign_name5 = "";
            data.name5 = apv4;
            data.position5 = apv4pos;
            data.date5 = "";
            data.cb1 = "";
            data.cb2 = "";
            data.remark5 = "";

            System.Data.DataTable dtStr = zreplaceinsrenewawc.BindTagData(pid, data);

            #endregion 

            #region Sample ReplaceTable
            ////DataTable Column Properties
            System.Data.DataTable dtProperties1 = new System.Data.DataTable();
            dtProperties1.Columns.Add("tagname", typeof(string));
            dtProperties1.Columns.Add("col_name", typeof(string));
            dtProperties1.Columns.Add("col_width", typeof(string));
            dtProperties1.Columns.Add("col_align", typeof(string)); //Left, Right, Center
            dtProperties1.Columns.Add("col_valign", typeof(string)); //Top, Middle, Bottom
            dtProperties1.Columns.Add("col_font", typeof(string));
            dtProperties1.Columns.Add("col_fontsize", typeof(string));
            dtProperties1.Columns.Add("col_fontcolor", typeof(string));
            dtProperties1.Columns.Add("col_color", typeof(string));
            dtProperties1.Columns.Add("header_height", typeof(string));
            dtProperties1.Columns.Add("header_color", typeof(string));
            dtProperties1.Columns.Add("header_font", typeof(string));
            dtProperties1.Columns.Add("header_fontsize", typeof(string));
            dtProperties1.Columns.Add("header_fontbold", typeof(string));
            dtProperties1.Columns.Add("header_align", typeof(string)); //Left, Right, Center
            dtProperties1.Columns.Add("header_valign", typeof(string)); //Top, Middle, Bottom
            dtProperties1.Columns.Add("header_fontcolor", typeof(string));
            dtProperties1.Columns.Add("row_height", typeof(string));
            // Replace #tablesum# ------------------------------------------------------
            DataRow dr = dtProperties1.NewRow();
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "กลุ่มธุรกิจ";
            dr["col_width"] = "100";
            dr["col_align"] = "Center";
            dr["col_valign"] = "Top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "IAR";
            dr["col_width"] = "200";
            dr["col_align"] = "Center";
            dr["col_valign"] = "Top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "BI";
            dr["col_width"] = "200";
            dr["col_align"] = "Center";
            dr["col_valign"] = "Top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "CGL($)";
            dr["col_width"] = "200";
            dr["col_align"] = "Center";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "PL";
            dr["col_width"] = "200";
            dr["col_align"] = "Center";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "PV";
            dr["col_width"] = "150";
            dr["col_align"] = "Center";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "LPG";
            dr["col_width"] = "150";
            dr["col_align"] = "Center";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "D&O";
            dr["col_width"] = "150";
            dr["col_align"] = "Center";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            ////DataTable for #tablesum#
            //Get Data from gv1 Insurance sum Detail
            System.Data.DataTable dt = zreplaceinsrenewawc.genTagTableData(lblPID.Text);

            // Convert to JSONString
            System.Data.DataTable dtTagPropsTable = new System.Data.DataTable();
            dtTagPropsTable.Columns.Add("tagname", typeof(string));
            dtTagPropsTable.Columns.Add("jsonstring", typeof(string));

            System.Data.DataTable dtTagDataTable = new System.Data.DataTable();
            dtTagDataTable.Columns.Add("tagname", typeof(string));
            dtTagDataTable.Columns.Add("jsonstring", typeof(string));
            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
            var jsonDTProperties1 = repl.DataTableToJSONWithStringBuilder(dtProperties1);
            var jsonDTdata = repl.DataTableToJSONWithStringBuilder(dt);
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
            // Dowload Word 
            //Response.Clear();
            //Response.ContentType = "text/xml";
            //Response.AddHeader("content-disposition", $"attachment; filename={outputfn}");
            //Response.BinaryWrite(outputbyte);
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.End();


            #endregion
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

                if (process_code == "INR_NEW" || process_code == "INR_RENEW")
                {
                    string sqlinsreq = "select * from li_insurance_request where process_id='" + lblPID.Text + "'";
                    var resinsreq = zdb.ExecSql_DataTable(sqlinsreq, zconnstr);

                    if (resinsreq.Rows.Count > 0)
                    {
                        xbu_code = resinsreq.Rows[0]["bu_code"].ToString();
                    }
                }
                else if (process_code == "INR_CLAIM" || process_code == "INR_CLAIM_2" || process_code == "INR_CLAIM_3")
                {
                    string sqlinsreq = "select * from li_insurance_claim where process_id='" + lblPID.Text + "'";
                    var resinsreq = zdb.ExecSql_DataTable(sqlinsreq, zconnstr);

                    if (resinsreq.Rows.Count > 0)
                    {
                        xbu_code = resinsreq.Rows[0]["bu_code"].ToString();
                    }

                }

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
                    wfAttr.subject = subject.Text.Trim();
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
                        if (process_code == "INR_NEW")
                        {
                            GenDocumnetInsNew(lblPID.Text);
                            string subject = "";
                            string body = "";
                            string sqlins = @"select * from li_insurance_request where process_id = '" + wfAttr.process_id + "'";
                            var dt = zdb.ExecSql_DataTable(sqlins, zconnstr);
                            if (dt.Rows.Count > 0)
                            {
                                var dr = dt.Rows[0];
                                string id = dr["req_no"].ToString();
                                subject = dr["subject"].ToString();
                                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                                body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้ถูก Reject กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

                                string pathfileins = "";
                                string outputdirectory = "";

                                string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                if (resfile.Rows.Count > 0)
                                {
                                    pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                                    outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                                    List<string> listpdf = new List<string>();
                                    listpdf.Add(pathfileins);

                                    string sqlattachfile = "select * from wf_attachment where pid = '" + wfAttr.process_id + "' and e_form IS NULL";

                                    var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                                    if (resattachfile.Rows.Count > 0)
                                    {
                                        foreach (DataRow item in resattachfile.Rows)
                                        {
                                            listpdf.Add(item["attached_filepath"].ToString());
                                        }
                                    }
                                    //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                                    string[] pdfFiles = listpdf.ToArray();

                                    string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

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
                                        _ = zsendmail.sendEmail("Reject "+ subject + " Mail To Requester", email, body, filepath);
                                    }
                                }

                            }
                        }
                        else if (process_code == "INR_CLAIM" || process_code == "INR_CLAIM_2" || process_code == "INR_CLAIM_3")
                        {
                            GenDocumnetInsClaim(lblPID.Text, wfAttr.submit_by);
                            string subject = "";
                            string body = "";
                            string sqlclaim = @"select * from li_insurance_claim where process_id = '" + wfAttr.process_id + "'";
                            var dt = zdb.ExecSql_DataTable(sqlclaim, zconnstr);
                            if (dt.Rows.Count > 0)
                            {
                                var dr = dt.Rows[0];
                                string id = dr["claim_no"].ToString();
                                subject = dr["incident"].ToString();
                                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                                body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้ถูก Reject กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

                                string pathfileins = "";
                                string outputdirectory = "";

                                string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                if (resfile.Rows.Count > 0)
                                {
                                    pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                                    outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                                    List<string> listpdf = new List<string>();
                                    listpdf.Add(pathfileins);

                                    string sqlattachfile = "select * from wf_attachment where pid = '" + wfAttr.process_id + "' and e_form IS NULL";

                                    var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                                    if (resattachfile.Rows.Count > 0)
                                    {
                                        foreach (DataRow item in resattachfile.Rows)
                                        {
                                            listpdf.Add(item["attached_filepath"].ToString());
                                        }
                                    }
                                    //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                                    string[] pdfFiles = listpdf.ToArray();

                                    string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

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
                                        _ = zsendmail.sendEmail("Reject " + subject + " Mail To Requester", email, body, filepath);
                                    }

                                }

                            }
                        }
                        else if (process_code == "INR_RENEW")
                        {
                            GenDocumnetInsRenew(lblPID.Text);
                            string subject = "";
                            string body = "";
                            string sqlre = @"select * from li_insurance_request where process_id = '" + wfAttr.process_id + "'";
                            var dt = zdb.ExecSql_DataTable(sqlre, zconnstr);
                            if (dt.Rows.Count > 0)
                            {
                                var dr = dt.Rows[0];
                                string id = dr["req_no"].ToString();
                                subject = dr["subject"].ToString();
                                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                                body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้ถูก Reject กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

                                string pathfileins = "";
                                string outputdirectory = "";

                                string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                                var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                                if (resfile.Rows.Count > 0)
                                {
                                    pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                                    outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                                    List<string> listpdf = new List<string>();
                                    listpdf.Add(pathfileins);

                                    string sqlattachfile = "select * from wf_attachment where pid = '" + wfAttr.process_id + "' and e_form IS NULL";

                                    var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                                    if (resattachfile.Rows.Count > 0)
                                    {
                                        foreach (DataRow item in resattachfile.Rows)
                                        {
                                            listpdf.Add(item["attached_filepath"].ToString());
                                        }
                                    }
                                    //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                                    string[] pdfFiles = listpdf.ToArray();

                                    string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

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
                                        _ = zsendmail.sendEmail("Reject " + subject + " Mail To Requester", email, body, filepath);
                                    }
                                }

                            }
                        }

                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        Response.Redirect(host_url+"legalportal/legalportal.aspx?m=myworklist", false);
                    }

                }
            }
            
        }
    }
}