using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace onlineLegalWF.userControls
{
    public partial class ucAttachment : System.Web.UI.UserControl
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
        public void ini_object(string xPID,string xEform = "", string xEfomSecNo = "") 
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
                lblSecAttach.Text = "Attachments "+ eformID.Value;
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
                sql = "select * from wf_attachment where pid = '" + hidPID.Value + "' and e_form = '"+ eformID.Value + "' and e_form_sec_no ="+ eformSecNo.Value;
            }
            

            var ds = zdb.ExecSql_DataSet(sql, zconnstr);

            fileGridview.DataSource = ds;
            fileGridview.DataBind();

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.FileContent.Length <= 0)
            {
                // file not found 
                Response.Write("<script> alert('Warning! Please input file.');</script>");

            }
            else
            {
                // check existing folder => path_attachment + \\pid\\
                string xpath = zpath_attachment + "\\" + hidPID.Value;
                if (!Directory.Exists(xpath)) 
                { 
                    Directory.CreateDirectory(xpath); 
                }

                // check existing file
                string fn = xpath + "\\" + FileUpload1.FileName;
                if (System.IO.File.Exists(fn))
                {
                    Response.Write("<script> alert('Warning! File name is dupplicated. Please rename the filename.');</script>");
                }
                else
                {
                    //Save file in Local
                    FileUpload1.SaveAs(fn);

                    string xpid = hidPID.Value;
                    string xattach_fn = FileUpload1.FileName;
                    string xattach_fp = fn;
                    //var xattach_ct_byte = FileUpload1.FileBytes;
                    string xeform = eformID.Value;
                    string xeform_sec_no = eformSecNo.Value;
                    string xattach_ctt = FileUpload1.PostedFile.ContentType;
                    string xcreate_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", new CultureInfo("en-US"));

                    // insert into db

                    string sql = "";
                    if (string.IsNullOrEmpty(eformID.Value))
                    {
                        sql = @"INSERT INTO [dbo].[wf_attachment] 
                                        ([pid],[attached_filename],[attached_filepath],[content_type],[created_datetime])
                                         VALUES
                                               ('" + xpid + @"'
                                               ,'" + xattach_fn + @"'
                                               ,'" + xattach_fp + @"'
                                               ,'" + xattach_ctt + @"'
                                               ,'" + xcreate_date + @"')";
                    }
                    else
                    {
                        sql = @"INSERT INTO [dbo].[wf_attachment] 
                                        ([pid],[e_form],[e_form_sec_no],[attached_filename],[attached_filepath],[content_type],[created_datetime])
                                         VALUES
                                               ('" + xpid + @"'
                                               ,'" + xeform + @"'
                                               ,'" + xeform_sec_no + @"'
                                               ,'" + xattach_fn + @"'
                                               ,'" + xattach_fp + @"'
                                               ,'" + xattach_ctt + @"'
                                               ,'" + xcreate_date + @"')";
                    }
                    zdb.ExecNonQuery(sql, zconnstr);


                    //string xpid = hidPID.Value;
                    //string xattach_fn = FileUpload1.FileName;
                    //string xattach_fp = fn;
                    //var xattach_ct_byte = FileUpload1.FileBytes;
                    //string xattach_ctt = FileUpload1.PostedFile.ContentType;
                    //string xcreate_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", new CultureInfo("en-US"));

                    //string sql = @"INSERT INTO [dbo].[wf_attachment] 
                    //                    ([pid],[attached_filename],[attached_filepath],[attached_content],[content_type],[created_datetime])
                    //                     VALUES
                    //                           ('" + xpid + @"'
                    //                           ,'" + xattach_fn + @"'
                    //                           ,'" + xattach_fp + @"'
                    //                           ,@bytes
                    //                           ,'" + xattach_ctt + @"'
                    //                           ,'" + xcreate_date + @"')";

                    //using (SqlConnection conn = new SqlConnection(zconnstr))
                    //{
                    //    conn.Open();
                    //    SqlCommand command = new SqlCommand(sql, conn);
                    //    //command.Parameters.Add("@bytes", SqlDbType.VarBinary);
                    //    //command.Parameters["@bytes"].Value = xattach_ct_byte;
                    //    // Replace 8000, below, with the correct size of the field
                    //    command.Parameters.Add("@bytes", SqlDbType.VarBinary, 8000).Value = xattach_ct_byte;
                    //    command.ExecuteNonQuery();
                    //    command.Dispose();
                    //    conn.Close();
                    //}

                    ini_data_wf_attach();



                }
                
            }

        }
        protected void DownloadData(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            var mimeType = MimeMapping.GetMimeMapping(Path.GetFileName(filePath));
            //Response.ContentType = ContentType;
            string extension = Path.GetExtension(filePath);
            var xeform = eformID.Value.ToString();

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

        ////delete from db
        //protected void DeleteData(object sender, EventArgs e)
        //{
        //    string filePath = (sender as LinkButton).CommandArgument;
        //    string filename = Path.GetFileName(filePath);
        //    string sql = "select * from wf_attachment where pid = '" + hidPID.Value + "' and  attached_filename= '" + filename + "'";

        //    var dt = zdb.ExecSql_DataTable(sql, zconnstr);

        //    if(dt.Rows.Count > 0) 
        //    {
        //        string sqldelete = "delete wf_attachment where pid = '" + hidPID.Value + "' and  attached_filename= '" + filename + "'";

        //        zdb.ExecNonQuery(sql, zconnstr);
        //        File.Delete(filePath);
        //        //Response.Redirect(Request.Url.AbsoluteUri);
        //        ini_data();

        //    }

        //}

        //delete Local
        protected void DeleteData(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            //File.Delete(filePath);
            //ini_data();
            string filename = Path.GetFileName(filePath);
            string sql = "";

            if (string.IsNullOrEmpty(eformID.Value))
            {
                sql = "select * from wf_attachment where pid = '" + hidPID.Value + "' and  attached_filename= '" + filename + "'";
            }
            else 
            {
                sql = "select * from wf_attachment where pid = '" + hidPID.Value + "' and  attached_filename= '" + filename + "' and e_form = '" + eformID.Value + "' and e_form_sec_no =" + eformSecNo.Value;
            }
            

            var dt = zdb.ExecSql_DataTable(sql, zconnstr);

            if (dt.Rows.Count > 0)
            {
                string sqldelete = "";
                if (string.IsNullOrEmpty(eformID.Value))
                {
                    sqldelete = "delete wf_attachment where pid = '" + hidPID.Value + "' and  attached_filename= '" + filename + "'";
                }
                else 
                {
                    sqldelete = "delete wf_attachment where pid = '" + hidPID.Value + "' and  attached_filename= '" + filename + "' and e_form = '" + eformID.Value + "' and e_form_sec_no =" + eformSecNo.Value;
                }
                

                zdb.ExecNonQuery(sqldelete, zconnstr);
                File.Delete(filePath);
                //Response.Redirect(Request.Url.AbsoluteUri);
                ini_data_wf_attach();

            }

            

        }
    }
}