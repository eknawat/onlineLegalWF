using DocumentFormat.OpenXml.ExtendedProperties;
using iTextSharp.text.pdf;
using onlineLegalWF.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static onlineLegalWF.Class.ReplaceCommRegis;

namespace onlineLegalWF.frmCommregis
{
    public partial class CommRegisRequestEdit : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        public ReplaceCommRegis zreplacecommregis = new ReplaceCommRegis();
        public SendMail zsendmail = new SendMail();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setDataDDL();

                string id = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(id))
                {
                    setDataEditRequest(id);
                }

            }

            string js = "$('#section1').hide();";
            if (type_comm_regis.SelectedValue == "01")
            {
                js += "$('.meeting').show();$('#section1').show();$('.subsidiary').hide();$('.company').show();$('.moresubsidiary').hide();$('.other').hide();";
            }
            else if (type_comm_regis.SelectedValue == "02")
            {
                js += "$('.meeting').show();$('#section2').show();$('.subsidiary').show();$('.company').show();$('.moresubsidiary').hide();$('.other').hide();";
            }
            else if (type_comm_regis.SelectedValue == "03")
            {
                js += "$('.meeting').show();$('#section3').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "04")
            {
                js += "$('.meeting').show();$('#section4').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "05")
            {
                js += "$('.meeting').show();$('#section5').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "06")
            {
                js += "$('.meeting').show();$('#section6').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "07")
            {
                js += "$('.meeting').show();$('#section7').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "08")
            {
                js += "$('.meeting').show();$('#section8').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "09")
            {
                js += "$('.meeting').show();$('#section9').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "10")
            {
                js += "$('.meeting').show();$('#section10').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "11")
            {
                js += "$('.meeting').show();$('#section11').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "12")
            {
                js += "$('.meeting').hide();$('#section12').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "13")
            {
                js += "$('.meeting').hide();$('#section13').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "14")
            {
                js += "$('.meeting').hide();$('#section14').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "15")
            {
                js += "$('.meeting').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "99")
            {
                js += "$('.meeting').hide();$('#section99').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').hide();$('.other').show();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "changeType", js, true);
        }

        private void setDataDDL()
        {
            ucHeader1.setHeader("Edit Request");

            type_comm_regis.DataSource = GetTypeOfRequest();
            type_comm_regis.DataBind();
            type_comm_regis.DataTextField = "toc_regis_desc";
            type_comm_regis.DataValueField = "toc_regis_code";
            type_comm_regis.DataBind();

            ddl_subsidiary.DataSource = GetSubsidiary();
            ddl_subsidiary.DataBind();
            ddl_subsidiary.DataTextField = "subsidiary_name_th";
            ddl_subsidiary.DataValueField = "subsidiary_code";
            ddl_subsidiary.DataBind();

            cb_subsidiary_multi.DataSource = GetSubsidiary();
            cb_subsidiary_multi.DataBind();
            cb_subsidiary_multi.DataTextField = "subsidiary_name_th";
            cb_subsidiary_multi.DataValueField = "subsidiary_code";
            cb_subsidiary_multi.DataBind();
        }

        public DataTable GetTypeOfRequest()
        {
            string sql = "select * from li_type_of_comm_regis order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }

        public DataTable GetSubsidiary()
        {
            string sql = "select * from li_comm_regis_subsidiary order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }

        private void setDataEditRequest(string id)
        {
            string sql = "select * from li_comm_regis_request where req_no='" + id + "'";

            var res = zdb.ExecSql_DataTable(sql, zconnstr);

            if (res.Rows.Count > 0)
            {
                req_no.Text = res.Rows[0]["req_no"].ToString();
                req_date.Value = Convert.ToDateTime(res.Rows[0]["req_date"]).ToString("yyyy-MM-dd");
                type_comm_regis.SelectedValue = res.Rows[0]["toc_regis_code"].ToString();
                if (!string.IsNullOrEmpty(res.Rows[0]["subsidiary_code"].ToString())) 
                {
                    ddl_subsidiary.SelectedValue = res.Rows[0]["subsidiary_code"].ToString();
                }
                doc_no.Text = res.Rows[0]["document_no"].ToString();
                mt_res_no.Text = res.Rows[0]["mt_res_no"].ToString();
                mt_res_desc.Text = res.Rows[0]["mt_res_desc"].ToString();
                mt_res_date.Text = Convert.ToDateTime(res.Rows[0]["mt_res_date"]).ToString("yyyy-MM-dd");
                company_name_th.Text = res.Rows[0]["company_name_th"].ToString();
                company_name_en.Text = res.Rows[0]["company_name_en"].ToString();
                lblPID.Text = res.Rows[0]["process_id"].ToString();
                hid_PID.Value = res.Rows[0]["process_id"].ToString();
                ucAttachment1.ini_object(res.Rows[0]["process_id"].ToString());
                ucCommentlog1.ini_object(res.Rows[0]["process_id"].ToString());
                cb_more.Checked = Convert.ToBoolean(res.Rows[0]["ismoresubsidiary"].ToString());
                if (!string.IsNullOrEmpty(res.Rows[0]["toc_regis_desc_other"].ToString()))
                {
                    toc_regis_desc_other.Text = res.Rows[0]["toc_regis_desc_other"].ToString();
                }
                if (!string.IsNullOrEmpty(res.Rows[0]["other_desc"].ToString()))
                {
                    other_desc.Text = res.Rows[0]["other_desc"].ToString();
                }
                if (cb_more.Checked) 
                {
                    string sqladditional = "select * from li_comm_regis_request_additional where req_no='" + id + "'";
                    var resadditional = zdb.ExecSql_DataTable(sqladditional, zconnstr);

                    if (resadditional.Rows.Count > 0) 
                    {
                        foreach (DataRow dtrow in resadditional.Rows) 
                        {
                            string subsidiarycode = dtrow["subsidiary_code"].ToString();
                            foreach (ListItem checkbox in cb_subsidiary_multi.Items)
                            {
                                if (checkbox.Value == subsidiarycode)
                                {
                                    checkbox.Selected = true;
                                }
                            }
                        }
                        
                    }
                }

                string js = "$('#section1').hide();";
                if (type_comm_regis.SelectedValue == "01")
                {
                    js += "$('.meeting').show();$('#section1').show();$('.subsidiary').hide();$('.company').show();$('.moresubsidiary').hide();$('.other').hide();";
                }
                else if (type_comm_regis.SelectedValue == "02")
                {
                    js += "$('.meeting').show();$('#section2').show();$('.subsidiary').show();$('.company').show();$('.moresubsidiary').hide();$('.other').hide();";
                }
                else if (type_comm_regis.SelectedValue == "03")
                {
                    js += "$('.meeting').show();$('#section3').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }
                else if (type_comm_regis.SelectedValue == "04")
                {
                    js += "$('.meeting').show();$('#section4').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }
                else if (type_comm_regis.SelectedValue == "05")
                {
                    js += "$('.meeting').show();$('#section5').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }
                else if (type_comm_regis.SelectedValue == "06")
                {
                    js += "$('.meeting').show();$('#section6').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }
                else if (type_comm_regis.SelectedValue == "07")
                {
                    js += "$('.meeting').show();$('#section7').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }
                else if (type_comm_regis.SelectedValue == "08")
                {
                    js += "$('.meeting').show();$('#section8').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }
                else if (type_comm_regis.SelectedValue == "09")
                {
                    js += "$('.meeting').show();$('#section9').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }
                else if (type_comm_regis.SelectedValue == "10")
                {
                    js += "$('.meeting').show();$('#section10').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }
                else if (type_comm_regis.SelectedValue == "11")
                {
                    js += "$('.meeting').show();$('#section11').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }
                else if (type_comm_regis.SelectedValue == "12")
                {
                    js += "$('.meeting').hide();$('#section12').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }
                else if (type_comm_regis.SelectedValue == "13")
                {
                    js += "$('.meeting').hide();$('#section13').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }
                else if (type_comm_regis.SelectedValue == "14")
                {
                    js += "$('.meeting').hide();$('#section14').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }
                else if (type_comm_regis.SelectedValue == "15")
                {
                    js += "$('.meeting').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }
                else if (type_comm_regis.SelectedValue == "99")
                {
                    js += "$('.meeting').hide();$('#section99').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').hide();$('.other').show();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "changeType", js, true);
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            int res = UpdateRequest();

            if (res > 0)
            {
                Response.Write("<script>alert('Successfully Updated');</script>");
            }
            else
            {
                Response.Write("<script>alert('Error !!!');</script>");
            }
        }

        private int UpdateRequest()
        {
            int ret = 0;

            string xtype_comm_regis = type_comm_regis.SelectedValue;
            var xreq_no = req_no.Text.Trim();
            var xupdate_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var xmt_res_desc = mt_res_desc.Text.Trim().Replace("'", "’");
            var xmt_res_no = mt_res_no.Text.Trim();
            var xmt_res_date = mt_res_date.Text.Trim();
            var xddl_subsidiary = ddl_subsidiary.SelectedValue;

            //check is cbmore loop subsidiary_code
            var xiscb_more = cb_more.Checked;
            List<string> selected = new List<string>();
            foreach (ListItem item in cb_subsidiary_multi.Items)
            {
                if (item.Selected)
                {
                    selected.Add(item.Value);
                }
            }

            string sql = "";

            if (xtype_comm_regis == "02")
            {
                var xcompany_name_th = company_name_th.Text.Trim();
                var xcompany_name_en = company_name_en.Text.Trim();

                sql = @"UPDATE [dbo].[li_comm_regis_request]
                           SET [mt_res_desc] = '"+ xmt_res_desc +@"'
                              ,[mt_res_no] = '" +xmt_res_no+ @"'
                              ,[mt_res_date] = '"+ xmt_res_date + @"'
                              ,[toc_regis_code] = '" + xtype_comm_regis + @"'
                              ,[subsidiary_code] = '" + xddl_subsidiary + @"'
                              ,[company_name_th] = '"+ xcompany_name_th +@"'
                              ,[company_name_en] = '"+ xcompany_name_en +@"'
                              ,[updated_datetime] = '" + xupdate_date + @"'
                         WHERE [req_no] = '"+ xreq_no + "'";

                ret = zdb.ExecNonQueryReturnID(sql, zconnstr);
            }
            else
            {
                if (xtype_comm_regis == "01")
                {
                    var xisrdregister = sec1_cb_rd.Checked;
                    var xcompany_name_th = company_name_th.Text.Trim();
                    var xcompany_name_en = company_name_en.Text.Trim();
                    sql = @"UPDATE [dbo].[li_comm_regis_request]
                           SET [mt_res_desc] = '" + xmt_res_desc + @"'
                              ,[mt_res_no] = '" + xmt_res_no + @"'
                              ,[mt_res_date] = '" + xmt_res_date + @"'
                              ,[toc_regis_code] = '" + xtype_comm_regis + @"'
                              ,[company_name_th] = '"+ xcompany_name_th +@"'
                              ,[company_name_en] = '"+ xcompany_name_en +@"'
                              ,[isrdregister] = '" + xisrdregister + @"'
                              ,[updated_datetime] = '" + xupdate_date + @"'
                         WHERE [req_no] = '" + xreq_no + "'";

                    ret = zdb.ExecNonQueryReturnID(sql, zconnstr);
                }
                else
                {
                    var xisrdregister = false;

                    if (xtype_comm_regis == "06")
                    {
                        xisrdregister = sec6_cb_rd.Checked;
                    }

                    if (xtype_comm_regis == "08")
                    {
                        xisrdregister = sec8_cb_rd.Checked;
                    }

                    if (xtype_comm_regis == "99")
                    {
                        var xtoc_regis_desc_other = toc_regis_desc_other.Text.Trim();
                        var xother_desc = other_desc.Text.Trim();
                        sql = @"UPDATE [dbo].[li_comm_regis_request]
                           SET [mt_res_desc] = '" + xmt_res_desc + @"'
                              ,[mt_res_no] = '" + xmt_res_no + @"'
                              ,[mt_res_date] = '" + xmt_res_date + @"'
                              ,[toc_regis_code] = '" + xtype_comm_regis + @"'
                              ,[subsidiary_code] = '" + xddl_subsidiary + @"'
                              ,[isrdregister] = '" + xisrdregister + @"'
                              ,[updated_datetime] = '" + xupdate_date + @"'
                              ,[toc_regis_desc_other] = '" + xtoc_regis_desc_other + @"'
                              ,[other_desc] = '" + xother_desc + @"'
                         WHERE [req_no] = '" + xreq_no + "'";
                    }
                    else 
                    {
                        sql = @"UPDATE [dbo].[li_comm_regis_request]
                           SET [mt_res_desc] = '" + xmt_res_desc + @"'
                              ,[mt_res_no] = '" + xmt_res_no + @"'
                              ,[mt_res_date] = '" + xmt_res_date + @"'
                              ,[toc_regis_code] = '" + xtype_comm_regis + @"'
                              ,[subsidiary_code] = '" + xddl_subsidiary + @"'
                              ,[isrdregister] = '" + xisrdregister + @"'
                              ,[updated_datetime] = '" + xupdate_date + @"'
                         WHERE [req_no] = '" + xreq_no + "'";
                    }

                    

                    ret = zdb.ExecNonQueryReturnID(sql, zconnstr);
                    //check cb_more == true and cb_subsidiary_multi > 0 insert tb li_comm_regis_request_additional
                    if (xiscb_more && selected.Count > 0)
                    {
                        //delete li_comm_regis_request_additional and insert new

                        string sqlDeletePropIns = @"delete from li_comm_regis_request_additional where req_no='" + xreq_no + "'";

                        ret = zdb.ExecNonQueryReturnID(sqlDeletePropIns, zconnstr);

                        if (ret > 0) 
                        {
                            string xassign = "";
                            string xadditionalstatus = "wait assign";
                            foreach (var item in selected)
                            {
                                string sqladditional = @"INSERT INTO [dbo].[li_comm_regis_request_additional]
                                                       ([req_no]
                                                       ,[subsidiary_code]
                                                       ,[assto_login]
                                                       ,[status]
                                                       ,[created_datetime])
                                                 VALUES
                                                       ('" + xreq_no + @"'
                                                       ,'" + item + @"'
                                                       ,'" + xassign + @"'
                                                       ,'" + xadditionalstatus + @"'
                                                       ,'" + xupdate_date + "')";
                                ret = zdb.ExecNonQueryReturnID(sqladditional, zconnstr);
                            }
                        }
                    }

                }
            }


            return ret;
        }

        protected void btn_gendocumnt_Click(object sender, EventArgs e)
        {
            GenDocumnet();
        }

        private void GenDocumnet()
        {
            // Replace Doc
            var xtype_comm_regis = type_comm_regis.SelectedValue;
            var xtype_comm_regis_text = type_comm_regis.SelectedItem.Text.Trim() + " " + toc_regis_desc_other.Text.Trim();
            var xdoc_no = doc_no.Text.Trim();
            var xprocess_id = hid_PID.Value.ToString();
            var xreq_date = Utillity.ConvertStringToDate(req_date.Value);
            var xmt_res_desc = mt_res_desc.Text.Trim();
            var xmt_res_no = mt_res_no.Text.Trim();
            var xmt_res_date = mt_res_date.Text.Trim();
            var xcompany_name_th = "";
            var xcompany_name_en = "";
            var xddl_subsidiary = ddl_subsidiary.SelectedValue;

            if (xtype_comm_regis == "01" || xtype_comm_regis == "02")
            {
                xcompany_name_th = company_name_th.Text.Trim();
                xcompany_name_en = company_name_en.Text.Trim();
            }
            else
            {
                string sql_commsub = "SELECT * FROM [BPM].[dbo].[li_comm_regis_subsidiary] where subsidiary_code = '" + xddl_subsidiary + "'";
                var rescommsub = zdb.ExecSql_DataTable(sql_commsub, zconnstr);

                if (rescommsub.Rows.Count > 0)
                {
                    xcompany_name_th = rescommsub.Rows[0]["subsidiary_name_th"].ToString().Trim();
                    xcompany_name_en = rescommsub.Rows[0]["subsidiary_name_en"].ToString().Trim();
                }
            }

            var path_template = ConfigurationManager.AppSettings["WT_Template_commregistration"].ToString();
            string templatefile = "";
            if (xtype_comm_regis == "12" || xtype_comm_regis == "13" || xtype_comm_regis == "14" || xtype_comm_regis == "99")
            {
                templatefile = path_template + @"\InsuranceComregis2.docx";
            }
            else
            {
                templatefile = path_template + @"\InsuranceComregis.docx";
            }
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\commregis_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            #region gentagstr data form
            ReplaceCommRegis_TagData data = new ReplaceCommRegis_TagData();

            data.docno = xdoc_no.Replace(",", "!comma");
            data.subject = xtype_comm_regis_text.Replace(",", "!comma");
            data.companyname_th = xcompany_name_th.Replace(",", "!comma");
            data.companyname_en = xcompany_name_en.Replace(",", "!comma");
            data.reqdate = Utillity.ConvertDateToLongDateTime(xreq_date, "th");
            data.mt_res_desc = xmt_res_desc.Replace(",", "!comma");
            data.mt_res_no = xmt_res_no.Replace(",", "!comma");
            data.mt_res_date = Utillity.ConvertDateToLongDateTime(Utillity.ConvertStringToDate(xmt_res_date), "th").Replace(",", "!comma");

            var requestor = "";
            var requestorpos = "";
            var supervisor = "";
            var supervisorpos = "";

            // check session_user
            if (Session["user_login"] != null)
            {
                var xlogin_name = Session["user_login"].ToString();
                var empFunc = new EmpInfo();

                //get data user
                var emp = empFunc.getEmpInfo(xlogin_name);
                if (!string.IsNullOrEmpty(emp.full_name_en))
                {
                    requestor = emp.full_name_en;
                    requestorpos = emp.position_en;
                }

                //get supervisor data
                var empSupervisor = empFunc.getEmpInfo(emp.next_line_mgr_login);
                if (!string.IsNullOrEmpty(empSupervisor.full_name_en))
                {
                    supervisor = empSupervisor.full_name_en;
                    supervisorpos = empSupervisor.position_en;
                }

            }

            data.sign_name1 = "";
            data.name1 = requestor;
            data.position1 = requestorpos;
            data.date1 = "";

            data.sign_name2 = "";
            data.name2 = supervisor;
            data.position2 = supervisorpos;
            data.date2 = "";


            DataTable dtStr = zreplacecommregis.genTagData(data);
            #endregion


            // Convert to JSONString
            //DataTable dtTagPropsTable = new DataTable();
            //dtTagPropsTable.Columns.Add("tagname", typeof(string));
            //dtTagPropsTable.Columns.Add("jsonstring", typeof(string));

            //DataTable dtTagDataTable = new DataTable();
            //dtTagDataTable.Columns.Add("tagname", typeof(string));
            //dtTagDataTable.Columns.Add("jsonstring", typeof(string));
            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
            //var jsonDTProperties1 = repl.DataTableToJSONWithStringBuilder(dtProperties1);
            //var jsonDTdata = repl.DataTableToJSONWithStringBuilder(dt);
            var jsonDTProperties1 = "";
            var jsonDTdata = "";
            //end prepare data

            // Save to Database z_replacedocx_log
            string xreq_no = req_no.Text.Trim();
            string sql = @"insert into z_replacedocx_log (replacedocx_reqno,jsonTagString, jsonTableProp, jsonTableData,template_filepath , output_directory,output_filepath, delete_output ) 
                        values('" + xreq_no + @"',
                               '" + jsonDTStr + @"', 
                                '" + jsonDTProperties1 + @"', 
                                '" + jsonDTdata + @"', 
                                '" + templatefile + @"', 
                                '" + outputfolder + @"', 
                                '" + outputfn + @"',  
                                '" + "0" + @"'
                            ) ";

            zdb.ExecNonQuery(sql, zconnstr);

            var outputbyte = rdoc.ReplaceData2(jsonDTStr, jsonDTProperties1, jsonDTdata, templatefile, outputfolder, outputfn, false);

            repl.convertDOCtoPDF(outputfn, outputfn.Replace(".docx", ".pdf"), false);

            string filePath = outputfn.Replace(".docx", ".pdf");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalDoc();", true);
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            pdf_render.Attributes["src"] = host_url + "render/pdf?id=" + filePath.Replace("+", @"%2B").Replace("&", @"%26");
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            // Sample Submit
            string process_code = "CCR";
            int version_no = 1;

            // getCurrentStep
            var wfAttr = zwf.getCurrentStep(lblPID.Text, process_code, version_no);

            // check session_user
            if (Session["user_login"] != null)
            {
                var xlogin_name = Session["user_login"].ToString();
                var empFunc = new EmpInfo();

                //get data user
                var emp = empFunc.getEmpInfo(xlogin_name);

                string xsubject = "";
                if (type_comm_regis.SelectedValue == "01")
                {
                    xsubject = "เรื่อง " + type_comm_regis.SelectedItem.Text.Trim() + " " + company_name_th.Text.Trim();
                }
                else
                {
                    xsubject = "เรื่อง " + type_comm_regis.SelectedItem.Text.Trim() + " " + ddl_subsidiary.SelectedItem.Text.Trim();
                }

                // set WF Attributes
                wfAttr.subject = xsubject;
                wfAttr.assto_login = emp.next_line_mgr_login;
                wfAttr.wf_status = "SUBMITTED";
                wfAttr.submit_answer = "SUBMITTED";
                wfAttr.submit_by = emp.user_login;

                wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, lblPID.Text, "");
                wfAttr.updated_by = emp.user_login;

                // wf.updateProcess
                var wfA_NextStep = zwf.updateProcess(wfAttr);
                //wfA_NextStep.next_assto_login = emp.next_line_mgr_login;
                wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by, lblPID.Text, "");
                string status = zwf.Insert_NextStep(wfA_NextStep);

                if (status == "Success")
                {
                    GenDocumnetCCRRegis(lblPID.Text);
                    //send mail
                    string subject = "";
                    string body = "";
                    string sqlmail = @"SELECT [process_id],[req_no],[req_date],commreg.[toc_regis_code],toc.[toc_regis_desc],commreg.[subsidiary_code],[document_no],[mt_res_desc],[mt_res_no],[mt_res_date],
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
                                        where process_id = '" + wfAttr.process_id + "'";
                    var dt = zdb.ExecSql_DataTable(sqlmail, zconnstr);
                    if (dt.Rows.Count > 0)
                    {
                        var dr = dt.Rows[0];
                        string id = dr["req_no"].ToString();
                        subject = wfAttr.subject;
                        var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                        body = "คุณได้รับมอบหมายให้ตรวจสอบเอกสารเลขที่ " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='"+host_url_sendmail+"legalportal/legalportal?m=myworklist'>Click</a>";



                        string pathfileins = "";

                        string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                        var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                        if (resfile.Rows.Count > 0)
                        {
                            pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");

                            string email = "";

                            var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                            ////get mail from db
                            /////send mail to next_approve
                            if (isdev != "true")
                            {
                                string sqlbpm = "select * from li_user where user_login = '" + wfA_NextStep.next_assto_login + "' ";
                                System.Data.DataTable dtbpm = zdb.ExecSql_DataTable(sqlbpm, zconnstr);

                                if (dtbpm.Rows.Count > 0)
                                {
                                    email = dtbpm.Rows[0]["email"].ToString();

                                }
                                else
                                {
                                    string sqlpra = "select * from Rpa_Mst_HrNameList where Login = 'ASSETWORLDCORP-\\" + wfA_NextStep.next_assto_login + "' ";
                                    System.Data.DataTable dtrpa = zdb.ExecSql_DataTable(sqlpra, zconnstrrpa);

                                    if (dtrpa.Rows.Count > 0)
                                    {
                                        email = dtrpa.Rows[0]["Email"].ToString();
                                    }
                                    else
                                    {
                                        email = "";
                                    }

                                }
                            }
                            else
                            {
                                ////fix mail test
                                email = "legalwfuat2024@gmail.com";
                            }

                            if (!string.IsNullOrEmpty(email))
                            {
                                _ = zsendmail.sendEmail(subject + " Mail To Next Appove", email, body, pathfileins);
                            }

                        }

                    }
                    var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                    Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                }

            }
        }

        private void GenDocumnetCCRRegis(string pid)
        {
            var path_template = ConfigurationManager.AppSettings["WT_Template_commregistration"].ToString();
            string templatefile = "";

            string sqlcommregis = "select * from li_comm_regis_request where process_id='" + pid + "'";
            var rescommregis = zdb.ExecSql_DataTable(sqlcommregis, zconnstr);

            if (rescommregis.Rows.Count > 0) 
            {
                if (rescommregis.Rows[0]["toc_regis_code"].ToString() == "12" || rescommregis.Rows[0]["toc_regis_code"].ToString() == "13" || rescommregis.Rows[0]["toc_regis_code"].ToString() == "14" || rescommregis.Rows[0]["toc_regis_code"].ToString() == "99")
                {
                    templatefile = path_template + @"\InsuranceComregis2.docx";
                }
                else
                {
                    templatefile = path_template + @"\InsuranceComregis.docx";
                }
            }
            
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\commregis_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            #region gentagstr data form
            ReplaceCommRegis_TagData data = new ReplaceCommRegis_TagData();

            var requestor = "";
            var requestorpos = "";
            var supervisor = "";
            var supervisorpos = "";

            // check session_user
            if (Session["user_login"] != null)
            {
                var xlogin_name = Session["user_login"].ToString();
                var empFunc = new EmpInfo();

                //get data user
                var emp = empFunc.getEmpInfo(xlogin_name);
                if (!string.IsNullOrEmpty(emp.full_name_en))
                {
                    requestor = emp.full_name_en;
                    requestorpos = emp.position_en;
                }

                //get supervisor data
                var empSupervisor = empFunc.getEmpInfo(emp.next_line_mgr_login);
                if (!string.IsNullOrEmpty(empSupervisor.full_name_en))
                {
                    supervisor = empSupervisor.full_name_en;
                    supervisorpos = empSupervisor.position_en;
                }

            }

            data.sign_name1 = "";
            data.name1 = requestor;
            data.position1 = requestorpos;
            data.date1 = "";

            data.sign_name2 = "";
            data.name2 = supervisor;
            data.position2 = supervisorpos;
            data.date2 = "";


            DataTable dtStr = zreplacecommregis.BindTagData(pid,data);
            #endregion


            // Convert to JSONString
            //DataTable dtTagPropsTable = new DataTable();
            //dtTagPropsTable.Columns.Add("tagname", typeof(string));
            //dtTagPropsTable.Columns.Add("jsonstring", typeof(string));

            //DataTable dtTagDataTable = new DataTable();
            //dtTagDataTable.Columns.Add("tagname", typeof(string));
            //dtTagDataTable.Columns.Add("jsonstring", typeof(string));
            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
            //var jsonDTProperties1 = repl.DataTableToJSONWithStringBuilder(dtProperties1);
            //var jsonDTdata = repl.DataTableToJSONWithStringBuilder(dt);
            var jsonDTProperties1 = "";
            var jsonDTdata = "";
            //end prepare data

            // Save to Database z_replacedocx_log
            string xreq_no = req_no.Text.Trim();
            string sql = @"insert into z_replacedocx_log (replacedocx_reqno,jsonTagString, jsonTableProp, jsonTableData,template_filepath , output_directory,output_filepath, delete_output ) 
                        values('" + xreq_no + @"',
                               '" + jsonDTStr + @"', 
                                '" + jsonDTProperties1 + @"', 
                                '" + jsonDTdata + @"', 
                                '" + templatefile + @"', 
                                '" + outputfolder + @"', 
                                '" + outputfn + @"',  
                                '" + "0" + @"'
                            ) ";

            zdb.ExecNonQuery(sql, zconnstr);

            var outputbyte = rdoc.ReplaceData2(jsonDTStr, jsonDTProperties1, jsonDTdata, templatefile, outputfolder, outputfn, false);

            repl.convertDOCtoPDF(outputfn, outputfn.Replace(".docx", ".pdf"), false);

        }

        protected void CheckAll(object sender, EventArgs e)
        {
            CheckBox btnCheckAll = sender as CheckBox;
            if (btnCheckAll.Checked == true)
            {
                foreach (ListItem checkbox in cb_subsidiary_multi.Items)
                {
                    checkbox.Selected = true;
                }
            }
            else
            {
                foreach (ListItem checkbox in cb_subsidiary_multi.Items)
                {
                    checkbox.Selected = false;
                }
            }

            string js = "$('#section1').hide();";
            if (type_comm_regis.SelectedValue == "01")
            {
                js += "$('.meeting').show();$('#section1').show();$('.subsidiary').hide();$('.company').show();$('.moresubsidiary').hide();$('.other').hide();";
            }
            else if (type_comm_regis.SelectedValue == "02")
            {
                js += "$('.meeting').show();$('#section2').show();$('.subsidiary').show();$('.company').show();$('.moresubsidiary').hide();$('.other').hide();";
            }
            else if (type_comm_regis.SelectedValue == "03")
            {
                js += "$('.meeting').show();$('#section3').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "04")
            {
                js += "$('.meeting').show();$('#section4').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "05")
            {
                js += "$('.meeting').show();$('#section5').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "06")
            {
                js += "$('.meeting').show();$('#section6').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "07")
            {
                js += "$('.meeting').show();$('#section7').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "08")
            {
                js += "$('.meeting').show();$('#section8').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "09")
            {
                js += "$('.meeting').show();$('#section9').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "10")
            {
                js += "$('.meeting').show();$('#section10').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "11")
            {
                js += "$('.meeting').show();$('#section11').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "12")
            {
                js += "$('.meeting').hide();$('#section12').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "13")
            {
                js += "$('.meeting').hide();$('#section13').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "14")
            {
                js += "$('.meeting').hide();$('#section14').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "15")
            {
                js += "$('.meeting').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').show();$('.other').hide();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }
            else if (type_comm_regis.SelectedValue == "99")
            {
                js += "$('.meeting').hide();$('#section99').show();$('.subsidiary').show();$('.company').hide();$('.moresubsidiary').hide();$('.other').show();if ($('#ContentPlaceHolder1_cb_more').is(':checked') == true) {$('.more_cb_sub').show();}else {$('.more_cb_sub').hide();}";
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "changeType", js, true);
        }
    }
}