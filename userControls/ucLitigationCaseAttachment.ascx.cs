using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace onlineLegalWF.userControls
{
    public partial class ucLitigationCaseAttachment : System.Web.UI.UserControl
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
        public void ini_object(string xcase_no, string xcontract_no, string xcustomer_no, string xcustomer_name)
        {
            customer_no.Text = xcase_no;
            contract_no.Text = xcontract_no;
            customer_no.Text = xcustomer_no;
            customer_name.Text = xcustomer_name;
            hid_caseNo.Value = xcase_no;

            type_doc.DataSource = GetTypeDocument();
            type_doc.DataBind();
            type_doc.DataTextField = "lit_type_doc_desc";
            type_doc.DataValueField = "lit_type_doc_code";
            type_doc.DataBind();

            ini_data_wf_attach();
        }
        public DataTable GetTypeDocument()
        {
            string sql = "select * from li_litigation_type_document order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }
        private void ini_data_wf_attach()
        {
            string sql = @"select [row_id],[case_no],li_litigation_case_attachment.[lit_type_doc_code],li_litigation_type_document.[lit_type_doc_desc],[attached_filename]
                          ,[attached_filepath],[content_type],[note],[created_datetime]
                          from [li_litigation_case_attachment]
                          inner join li_litigation_type_document on li_litigation_type_document.lit_type_doc_code = li_litigation_case_attachment.lit_type_doc_code
                          where case_no = '" + hid_caseNo.Value + "' order by li_litigation_case_attachment.[lit_type_doc_code] asc";

            var dt = zdb.ExecSql_DataTable(sql, zconnstr);

            fileGridview.DataSource = dt;
            fileGridview.DataBind();

            //check data and disable filed
            foreach (GridViewRow row in fileGridview.Rows)
            {
                string filename = (row.FindControl("DownloadLink") as LinkButton).Text;
                if (!string.IsNullOrEmpty(filename))
                {
                    (row.FindControl("DownloadLink") as LinkButton).Visible = true;
                }
                else
                {
                    (row.FindControl("DownloadLink") as LinkButton).Visible = false;
                }

            }

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            // insert into db

            string sql = "";
            string xcaseno = "";
            string xtype_doc = "";
            string xnote = "";
            string xcreate_date = "";
            if (FileUpload1.FileContent.Length <= 0)
            {
                xcaseno = hid_caseNo.Value;
                xtype_doc = type_doc.SelectedValue;
                xnote = note.Text.Trim();
                xcreate_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", new CultureInfo("en-US"));

                sql = @"INSERT INTO [dbo].[li_litigation_case_attachment]
                                   ([case_no]
                                   ,[lit_type_doc_code]
                                   ,[note]
                                   ,[created_datetime])
                             VALUES
                                   ('" + xcaseno + @"'
                                   ,'" + xtype_doc + @"'
                                   ,'" + xnote + @"'
                                   ,'" + xcreate_date + "')";

                zdb.ExecNonQuery(sql, zconnstr);
                note.Text = "";
                Response.Write("<script>alert('Successfully Updated');</script>");
            }
            else
            {
                // check existing folder => path_attachment + \\caseno\\
                string xpath = zpath_attachment + "\\" + hid_caseNo.Value;
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

                    xcaseno = hid_caseNo.Value;
                    xtype_doc = type_doc.SelectedValue;
                    string xattach_fn = FileUpload1.FileName;
                    string xattach_fp = fn;
                    string xattach_ctt = FileUpload1.PostedFile.ContentType;
                    xnote = note.Text.Trim();
                    xcreate_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", new CultureInfo("en-US"));

                    sql = @"INSERT INTO [dbo].[li_litigation_case_attachment]
                                   ([case_no]
                                   ,[lit_type_doc_code]
                                   ,[attached_filename]
                                   ,[attached_filepath]
                                   ,[content_type]
                                   ,[note]
                                   ,[created_datetime])
                             VALUES
                                   ('" + xcaseno + @"'
                                   ,'" + xtype_doc + @"'
                                   ,'" + xattach_fn + @"'
                                   ,'" + xattach_fp + @"'
                                   ,'" + xattach_ctt + @"'
                                   ,'" + xnote + @"'
                                   ,'" + xcreate_date + "')";

                    zdb.ExecNonQuery(sql, zconnstr);
                    note.Text = "";
                    Response.Write("<script>alert('Successfully Updated');</script>");
                }
            }

            ini_data_wf_attach();


        }
        protected void DownloadData(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "hideModalEditData();", true);
            string filePath = (sender as LinkButton).CommandArgument;
            var mimeType = MimeMapping.GetMimeMapping(Path.GetFileName(filePath));
            //Response.ContentType = ContentType;
            //string extension = Path.GetExtension(filePath);

            //if (extension == ".pdf")
            //{
            //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "hideModalEditData();", true);
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalAttachLitigation();", true);
            //    var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            //    pdf_render_liti.Attributes["src"] = host_url + "render/pdf?id=" + filePath.Replace("+", @"%2B").Replace("&", @"%26");
            //}
            //else
            //{
            Response.ContentType = mimeType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                Response.WriteFile(filePath);
                Response.End();
            //}
        }

        //delete Local
        protected void DeleteData(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            string filename = Path.GetFileName(filePath);

            string sql = "";
            string sqldelete = "";
            if (!string.IsNullOrEmpty(filename))
            {
                sql = "select * from li_litigation_case_attachment where case_no = '" + hid_caseNo.Value + "' and attached_filename= '" + filename + "'";
                sqldelete = "delete li_litigation_case_attachment where case_no = '" + hid_caseNo.Value + "' and attached_filename= '" + filename + "'";
            }
            else 
            {
                sql = "select * from li_litigation_case_attachment where case_no = '" + hid_caseNo.Value + "' and attached_filename is null";
                sqldelete = "delete li_litigation_case_attachment where case_no = '" + hid_caseNo.Value + "' and attached_filename is null";
            }
            var dt = zdb.ExecSql_DataTable(sql, zconnstr);

            if (dt.Rows.Count > 0)
            {
                zdb.ExecNonQuery(sqldelete, zconnstr);
                if (!string.IsNullOrEmpty(filePath)) 
                {
                    File.Delete(filePath);
                }
                ini_data_wf_attach();

            }

        }
    }
}