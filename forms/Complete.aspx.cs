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
    public partial class Complete : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
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

            ucHeader1.setHeader(process_code + " Complete");
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
                    //subject.Text = resinsclaim.Rows[0]["subject"].ToString();

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
                pdf_render.Attributes["src"] = host_url+"render/pdf?id=" + pathfile.Replace("+", @"%2B");
            }
        }

        private void initDataAttachAndComment(string pid)
        {
            lblPID.Text = pid;
            hid_PID.Value = pid;
            ucAttachment1.ini_object(pid);
            ucCommentlog1.ini_object(pid);
        }
    }
}