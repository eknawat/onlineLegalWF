using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace onlineLegalWF.userControls
{
    public partial class UcAttachForm : System.Web.UI.UserControl
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public string zpath_attachment = ConfigurationManager.AppSettings["path_attachment"].ToString();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ini_object(string xPID)
        {
            hidPID.Value = xPID;
        }

        protected void uploadData(object sender, EventArgs e)
        {
            if (myFile.HasFile) 
            {
                string fileName = Path.GetFileName(myFile.PostedFile.FileName);
                string filePath = Server.MapPath("~/Temp/") + fileName;
                myFile.PostedFile.SaveAs(filePath);
                btn_upload.Value = "Update";
                lblFile.Text = fileName;
                lblFile.CommandArgument = filePath;
                lblFile.Visible = true;
            }

            //if (myFile.FileContent.Length <= 0)
            //{
            //    // file not found 
            //    Response.Write("<script> alert('Warning! Please input file.');</script>");

            //}
            //else
            //{
            //    // check existing folder => path_attachment + \\pid\\
            //    string xpath = zpath_attachment + "\\" + hidPID.Value;
            //    if (!Directory.Exists(xpath)) { Directory.CreateDirectory(xpath); }

            //    // check existing file
            //    string fn = xpath + "\\" + myFile.FileName;
            //    if (System.IO.File.Exists(fn))
            //    {
            //        Response.Write("<script> alert('Warning! File name is dupplicated. Please rename the filename.');</script>");
            //    }
            //    else
            //    {
            //        //Save file in Local
            //        if (myFile.HasFile)
            //        {
            //            string fileName = Path.GetFileName(myFile.PostedFile.FileName);
            //            myFile.SaveAs(fn);
            //            btn_upload.Value = "Update";
            //            lblFile.Text = fileName;
            //            lblFile.CommandArgument = fn;
            //            lblFile.Visible = true;

            //            string xpid = hidPID.Value;
            //            string xattach_fn = fileName;
            //            string xattach_fp = fn;
            //            //var xattach_ct_byte = myFile.FileBytes;
            //            string xattach_ctt = myFile.PostedFile.ContentType;
            //            string xcreate_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", new CultureInfo("en-US"));

            //            // insert into db
            //            string sql = @"INSERT INTO [dbo].[wf_attachment] 
            //                            ([pid],[attached_filename],[attached_filepath],[content_type],[created_datetime])
            //                             VALUES
            //                                   ('" + xpid + @"'
            //                                   ,'" + xattach_fn + @"'
            //                                   ,'" + xattach_fp + @"'
            //                                   ,'" + xattach_ctt + @"'
            //                                   ,'" + xcreate_date + @"')";
            //            zdb.ExecNonQuery(sql, zconnstr);



            //        }

            //    }

            //}


        }

        protected void DownloadData(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            var mimeType = MimeMapping.GetMimeMapping(Path.GetFileName(filePath));
            //Response.ContentType = ContentType;
            string extension = Path.GetExtension(filePath);

            if (extension == ".pdf")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalAttachFrm();", true);
                var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                frm_pdf_render.Attributes["src"] = host_url + "render/pdf?id=" + filePath.Replace("+", @"%2B");
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