using Microsoft.Office.Interop.Word;
using onlineLegalWF.Class;
using onlineLegalWF.userControls;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace onlineLegalWF.frmInsurance
{
    public partial class InsuranceApprove : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string id = Request.QueryString["id"];
                string type = Request.QueryString["type"];

                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type))
                {
                    setDataApprove(id, type);
                }

            }
        }

        private void setDataApprove(string id, string type) {
            ucHeader1.setHeader("Insurance Approve");

            if (type == "req")
            {
                string sql = "select * from li_insurance_request where req_no='" + id + "'";

                var res = zdb.ExecSql_DataTable(sql, zconnstr);

                if (res.Rows.Count > 0)
                {
                    req_no.Value = res.Rows[0]["req_no"].ToString();
                    req_date.Text = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(res.Rows[0]["req_date"]), "en");
                    from.Text = res.Rows[0]["company_name"].ToString();
                    doc_no.Text = res.Rows[0]["document_no"].ToString();
                    subject.Text = res.Rows[0]["subject"].ToString();

                    //init data UcAttachAndCommentLogs
                    initDataAttachAndComment(res.Rows[0]["process_id"].ToString());
                }

                string sqlfile = "select top 1 *  from  z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                if (resfile.Rows.Count > 0)
                {
                    string pathfile = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                    var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                    pdf_render.Attributes["src"] = host_url + "render/pdf?id=" + pathfile.Replace("+", @"%2B").Replace("&", @"%26");
                }
            }
            else if (type == "claim")
            {

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