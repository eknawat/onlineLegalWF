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
    public partial class ccrcomplete : System.Web.UI.Page
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
            string sqlcommregis = @"SELECT [process_id],[req_no],[req_date],commreg.[toc_regis_code],toc.[toc_regis_desc],commreg.[subsidiary_code],[document_no],[mt_res_desc],[mt_res_no],[mt_res_date],
                                      CASE
                                        WHEN commreg.subsidiary_code IS NULL THEN commreg.company_name_th
                                        ELSE comsub.subsidiary_name_th
                                      END AS company_name_th,
                                      CASE
                                        WHEN commreg.subsidiary_code IS NULL THEN commreg.company_name_en
                                        ELSE comsub.subsidiary_name_en
                                      END AS company_name_en,
                                      [isrdregister],[status],[updated_datetime]
                                      FROM li_comm_regis_request AS commreg
                                      LEFT OUTER JOIN li_comm_regis_subsidiary AS comsub ON commreg.subsidiary_code = comsub.subsidiary_code
                                      INNER JOIN li_type_of_comm_regis AS toc ON commreg.toc_regis_code = toc.toc_regis_code
                              where process_id = '" + req + "'";
            var rescommregis = zdb.ExecSql_DataTable(sqlcommregis, zconnstr);

            //get data ins req
            if (rescommregis.Rows.Count > 0)
            {
                id = rescommregis.Rows[0]["req_no"].ToString();
                req_no.Value = rescommregis.Rows[0]["req_no"].ToString();
                req_date.Text = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(rescommregis.Rows[0]["req_date"]), "th");
                doc_no.Text = rescommregis.Rows[0]["document_no"].ToString();
                subject.Text = rescommregis.Rows[0]["toc_regis_desc"].ToString();
                companyname_th.Text = rescommregis.Rows[0]["company_name_th"].ToString();
                companyname_en.Text = rescommregis.Rows[0]["company_name_en"].ToString();

                //init data UcAttachAndCommentLogs
                initDataAttachAndComment(rescommregis.Rows[0]["process_id"].ToString());

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
            ucCommentlog1.ini_object(pid);
        }
    }
}