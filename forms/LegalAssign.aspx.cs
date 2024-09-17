using onlineLegalWF.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace onlineLegalWF.forms
{
    public partial class LegalAssign : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string req = Request.QueryString["req"];
                string process_code = Request.QueryString["pc"];

                if (!string.IsNullOrEmpty(req) && !string.IsNullOrEmpty(process_code))
                {
                    setData(req, process_code);
                }

            }
        }

        private void setData(string req, string process_code)
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

                    //init data UcAttachAndCommentLogs
                    initDataAttachAndComment(resinsreq.Rows[0]["process_id"].ToString());

                    getDocument(id);
                }
            }
            else if (process_code == "INR_CLAIM")
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

                    //init data UcAttachAndCommentLogs
                    initDataAttachAndComment(resinsclaim.Rows[0]["process_id"].ToString());

                    getDocument(id);
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

        protected void btn_SendMail_Click(object sender, EventArgs e)
        {
            modal_document_no.Text = doc_no.Text;
            modal_subject.Text = subject.Text;
            modal_submitted_date.Text = req_date.Text;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
        }

        //protected void btn_Approve_Click(object sender, EventArgs e)
        //{
        //    string process_code = Request.QueryString["pc"];
        //    int version_no = 1;

        //    if (!string.IsNullOrEmpty(process_code))
        //    {
        //        // getCurrentStep
        //        var wfAttr = zwf.getCurrentStep(lblPID.Text, process_code, version_no);

        //        // check session_user
        //        if (Session["user_login"] != null)
        //        {
        //            var xlogin_name = Session["user_login"].ToString();
        //            var empFunc = new EmpInfo();

        //            //get data user
        //            var emp = empFunc.getEmpInfo(xlogin_name);

        //            // set WF Attributes
        //            wfAttr.subject = subject.Text.Trim();
        //            wfAttr.assto_login = emp.next_line_mgr_login;
        //            wfAttr.wf_status = wfAttr.step_name + " Approved";
        //            wfAttr.submit_answer = "APPROVED";
        //            //wfAttr.next_assto_login = emp.next_line_mgr_login;
        //            wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by);
        //            wfAttr.updated_by = emp.user_login;
        //            wfAttr.submit_by = wfAttr.submit_by;
        //            // wf.updateProcess
        //            var wfA_NextStep = zwf.updateProcess(wfAttr);
        //            //wfA_NextStep.next_assto_login = emp.next_line_mgr_login;
        //            wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by);
        //            //wfA_NextStep.submit_by = emp.user_login;
        //            wfA_NextStep.submit_by = wfA_NextStep.submit_by;
        //            string status = zwf.Insert_NextStep(wfA_NextStep);

        //            if (status == "Success")
        //            {
        //                Response.Redirect("legalportal/legalportal.aspx?m=myworklist");
        //            }

        //        }
        //    }


        //}
    }
}