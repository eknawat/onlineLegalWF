using onlineLegalWF.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace onlineLegalWF.forms
{
    public partial class permitcomplete : System.Web.UI.Page
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
                //var xtype_of_permitrequest = respermit.Rows[0]["tof_permitreq_code"].ToString();

                //init data UcAttachAndCommentLogs
                initDataAttachAndComment(respermit.Rows[0]["process_id"].ToString());

                getDocument(id);
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
            ucAttachment2.ini_object(pid, "POA", "1");
            ucAttachment3.ini_object(pid, "License", "2");
            ucCommentlog1.ini_object(pid);
        }
    }
}