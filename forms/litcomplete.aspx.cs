using onlineLegalWF.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static onlineLegalWF.frmLitigation.LitigationRequestEdit;

namespace onlineLegalWF.forms
{
    public partial class litcomplete : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public MargePDF zmergepdf = new MargePDF();
        public SendMail zsendmail = new SendMail();
        public ReplaceLitigation zreplacelitigation = new ReplaceLitigation();
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
            if (process_code == "LIT" || process_code == "LIT_2")
            {
                string sqlinsreq = "select * from li_litigation_request where process_id='" + req + "'";
                var resinsreq = zdb.ExecSql_DataTable(sqlinsreq, zconnstr);

                //get data ins req
                if (resinsreq.Rows.Count > 0)
                {
                    id = resinsreq.Rows[0]["req_no"].ToString();
                    req_no.Value = resinsreq.Rows[0]["req_no"].ToString();
                    req_date.Text = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(resinsreq.Rows[0]["req_date"]), "en");
                    doc_no.Text = resinsreq.Rows[0]["document_no"].ToString();
                    subject.Text = resinsreq.Rows[0]["lit_subject"].ToString();
                    desc.Text = resinsreq.Rows[0]["lit_desc"].ToString();
                    var xtype_req = resinsreq.Rows[0]["tof_litigationreq_code"].ToString();
                    row_gv_data.Visible = false;
                    if (xtype_req == "01")
                    {
                        string sql = @"select lit.[process_id],reqcase.[req_no],[case_no],[no],[contract_no],[bu_name],[customer_no],[customer_name],[customer_room],[overdue_desc],[outstanding_debt],[outstanding_debt_ack_of_debt],[fine_debt]
                                    ,[total_net],[retention_money],[total_after_retention_money],[remark],reqcase.[status],[assto_login],reqcase.[updated_datetime] 
                                    from [li_litigation_req_case] as reqcase 
                                    inner join li_litigation_request as lit on lit.req_no = reqcase.req_no 
                                    where reqcase.[req_no]='" + id + "'";

                        var res = zdb.ExecSql_DataTable(sql, zconnstr);
                        if (res.Rows.Count > 0)
                        {
                            row_gv_data.Visible = true;
                            List<LitigationCivilCaseData> listCivilCaseData = new List<LitigationCivilCaseData>();

                            foreach (DataRow item in res.Rows)
                            {
                                LitigationCivilCaseData civilCaseData = new LitigationCivilCaseData();
                                civilCaseData.req_no = item["req_no"].ToString();
                                civilCaseData.case_no = item["case_no"].ToString();
                                civilCaseData.no = item["no"].ToString();
                                civilCaseData.contract_no = item["contract_no"].ToString();
                                civilCaseData.bu_name = item["bu_name"].ToString();
                                civilCaseData.customer_no = item["customer_no"].ToString();
                                civilCaseData.customer_name = item["customer_name"].ToString();
                                civilCaseData.customer_room = item["customer_room"].ToString();
                                civilCaseData.overdue_desc = item["overdue_desc"].ToString();
                                civilCaseData.outstanding_debt = item["outstanding_debt"].ToString();
                                civilCaseData.outstanding_debt_ack_of_debt = item["outstanding_debt_ack_of_debt"].ToString();
                                civilCaseData.fine_debt = item["fine_debt"].ToString();
                                civilCaseData.total_net = item["total_net"].ToString();
                                civilCaseData.retention_money = item["retention_money"].ToString();
                                civilCaseData.total_after_retention_money = item["total_after_retention_money"].ToString();
                                civilCaseData.remark = item["remark"].ToString();
                                civilCaseData.status = item["status"].ToString();
                                civilCaseData.assto_login = item["assto_login"].ToString();
                                listCivilCaseData.Add(civilCaseData);
                            }
                            gvExcelFile.DataSource = listCivilCaseData;
                            gvExcelFile.DataBind();
                        }
                    }

                    //init data UcAttachAndCommentLogs
                    initDataAttachAndComment(resinsreq.Rows[0]["process_id"].ToString());

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
                pdf_render.Attributes["src"] = host_url + "render/pdf?id=" + pathfile.Replace("+", @"%2B");
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