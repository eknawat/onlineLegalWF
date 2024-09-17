using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace onlineLegalWF.userControls
{
    public partial class ucAttachmentdata : System.Web.UI.UserControl
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public string zpath_attachment = ConfigurationManager.AppSettings["path_attachment"].ToString();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ini_data_wf_attach();
            }
        }
        public void ini_object(string xPID, string xEform = "", string xEfomSecNo = "")
        {
            hidPID.Value = xPID;
            eformID.Value = xEform;
            eformSecNo.Value = xEfomSecNo;
            ////ini_data_wf_attach();
            if (!string.IsNullOrEmpty(eformID.Value))
            {
                lblSecAttach.CssClass = "Label_lg";
                //seal_attach.Visible = false;
                //lbltitleAttach.Visible = false;
                lblSecAttach.Text = "Attachments " + eformID.Value;
            }
        }
        private void ini_data_wf_attach()
        {
            string sql = "";
            if (string.IsNullOrEmpty(eformID.Value))
            {
                sql = "select * from wf_attachment where pid = '" + hidPID.Value + "' and e_form IS NULL ";
            }
            else
            {
                sql = "select * from wf_attachment where pid = '" + hidPID.Value + "' and e_form = '" + eformID.Value + "' and e_form_sec_no =" + eformSecNo.Value;
            }


            var ds = zdb.ExecSql_DataSet(sql, zconnstr);

            fileGridview.DataSource = ds;
            fileGridview.DataBind();

        }

        protected void DownloadData(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            var mimeType = MimeMapping.GetMimeMapping(Path.GetFileName(filePath));
            //Response.ContentType = ContentType;
            string extension = Path.GetExtension(filePath);

            if (extension == ".pdf")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", string.Format("$('#{0}').modal('show');", modalattach.ClientID), true);
                var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                pdf_render.Attributes["src"] = host_url + "render/pdf?id=" + filePath.Replace("+", @"%2B").Replace("&", @"%26");
            }
            else
            {
                Response.ContentType = mimeType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                Response.WriteFile(filePath);
                Response.End();
            }
        }
    }
}