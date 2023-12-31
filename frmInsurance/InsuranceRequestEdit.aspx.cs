﻿using onlineLegalWF.Class;
using onlineLegalWF.userControls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace onlineLegalWF.frmInsurance
{
    public partial class InsuranceRequestEdit : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public WFFunctions zwf = new WFFunctions();
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
        }
        private void setDataDDL()
        {
            ucHeader1.setHeader("Edit Request");

            type_req.DataSource = GetTypeOfRequest();
            type_req.DataBind();
            type_req.DataTextField = "toreq_desc";
            type_req.DataValueField = "toreq_code";
            type_req.DataBind();

            type_pi.DataSource = GetTypeOfPropertyInsured();
            type_pi.DataBind();
            type_pi.DataTextField = "top_ins_desc";
            type_pi.DataValueField = "top_ins_code";
            type_pi.DataBind();

            ddl_bu.DataSource = GetBusinessUnit();
            ddl_bu.DataBind();
            ddl_bu.DataTextField = "bu_desc";
            ddl_bu.DataValueField = "bu_code";
            ddl_bu.DataBind();
        }

        private void setDataEditRequest(string id) 
        {
            string sql = "select * from li_insurance_request where req_no='"+ id + "'";

            var res = zdb.ExecSql_DataTable(sql, zconnstr);

            if (res.Rows.Count > 0) 
            {
                req_no.Text = res.Rows[0]["req_no"].ToString();
                req_date.Value = Convert.ToDateTime(res.Rows[0]["req_date"]).ToString("yyyy-MM-dd");
                type_req.SelectedValue = res.Rows[0]["toreq_code"].ToString();
                company.Text = res.Rows[0]["company_name"].ToString();
                doc_no.Text = res.Rows[0]["document_no"].ToString();
                subject.Text = res.Rows[0]["subject"].ToString();
                to.Text = res.Rows[0]["dear"].ToString();
                purpose.Text = res.Rows[0]["objective"].ToString();
                background.Text = res.Rows[0]["reason"].ToString();
                approve_des.Text = res.Rows[0]["approved_desc"].ToString();
                ddl_bu.SelectedValue = res.Rows[0]["bu_code"].ToString();
                prop_ins_name.Text = res.Rows[0]["property_insured_name"].ToString();
                lblPID.Text = res.Rows[0]["process_id"].ToString();
                hid_PID.Value = res.Rows[0]["process_id"].ToString();
                ucAttachment1.ini_object(res.Rows[0]["process_id"].ToString());
                ucCommentlog1.ini_object(res.Rows[0]["process_id"].ToString());
            }

            string sqlPropIns = "select  top 1 * from li_insurance_req_property_insured where req_no='"+ id + "'";

            var resPropIns = zdb.ExecSql_DataTable(sqlPropIns, zconnstr);

            if (resPropIns.Rows.Count > 0)
            {
                type_pi.SelectedValue = resPropIns.Rows[0]["top_ins_code"].ToString();
                indemnity_period.Text = resPropIns.Rows[0]["indemnityperiod"].ToString();
                sum_insured.Text = resPropIns.Rows[0]["suminsured"].ToString();
                start_date.Text = Convert.ToDateTime(resPropIns.Rows[0]["startdate"]).ToString("yyyy-MM-dd");
                end_date.Text = Convert.ToDateTime(resPropIns.Rows[0]["enddate"]).ToString("yyyy-MM-dd");
            }
        }

        public DataTable GetBusinessUnit()
        {
            string sql = "select * from li_business_unit order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }

        public DataTable GetTypeOfRequest()
        {
            string sql = "select * from li_type_of_request order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }

        public DataTable GetTypeOfPropertyInsured()
        {
            string sql = "select * from li_type_of_property_insured order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            int res = UpdateRequest();

            if (res > 0)
            {
                //// wf save draft
                //string process_code = "INR_NEW";
                //int version_no = 1;

                //// getCurrentStep
                //var wfAttr = zwf.getCurrentStep(lblPID.Text, process_code, version_no);

                //// check session_user
                //if (Session["user_login"] != null)
                //{
                //    var xlogin_name = Session["user_login"].ToString();
                //    var empFunc = new EmpInfo();

                //    //get data user
                //    var emp = empFunc.getEmpInfo(xlogin_name);

                //    // set WF Attributes
                //    wfAttr.subject = subject.Text.Trim();
                //    //wfAttr.assto_login = emp.next_line_mgr_login;
                //    wfAttr.wf_status = "DRAFT";
                //    wfAttr.submit_answer = "DRAFT";
                //    //wfAttr.next_assto_login = emp.next_line_mgr_login;
                //    wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login);
                //    //wfAttr.submit_by = emp.user_login;
                //    wfAttr.submit_by = wfAttr.submit_by;

                //    // wf.updateProcess
                //    var wfA_NextStep = zwf.updateProcess(wfAttr);

                //}
                Response.Write("<script>alert('Successfully Updated');</script>");
                //Response.Redirect("/frmInsurance/InsuranceRequestList");
            }
            else
            {
                Response.Write("<script>alert('Error !!!');</script>");
            }
        }

        private int UpdateRequest() 
        {
            int ret = 0;

            var xreq_no = req_no.Text.Trim();
            var xtype_req = type_req.SelectedValue.ToString();
            var xcompany = company.Text.Trim();
            var xdoc_no = doc_no.Text.Trim();
            var xsubject = subject.Text.Trim();
            var xto = to.Text.Trim();
            var xpurpose = purpose.Text.Trim();
            var xbackground = background.Text.Trim();
            var xtype_pi = type_pi.SelectedValue.ToString();
            var xindemnity_period = indemnity_period.Text.Trim();
            var xsum_insured = sum_insured.Text.Trim();
            var xstart_date = start_date.Text.Trim();
            var xend_date = end_date.Text.Trim();
            var xapprove_des = approve_des.Text.Trim();
            var xupdate_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var xbu_code = ddl_bu.SelectedValue.ToString();
            var xprop_ins_name = prop_ins_name.Text.Trim();

            string sql = @"UPDATE [dbo].[li_insurance_request]
                           SET [toreq_code] = '"+ xtype_req + @"'
                              ,[company_name] = '"+ xcompany + @"'
                              ,[document_no] = '"+ xdoc_no + @"'
                              ,[subject] = '"+ xsubject + @"'
                              ,[dear] = '"+ xto + @"'
                              ,[objective] = '"+ xpurpose + @"'
                              ,[reason] = '"+ xbackground + @"'
                              ,[approved_desc] = '"+ xapprove_des + @"'
                              ,[updated_datetime] = '" + xupdate_date + @"'
                              ,[bu_code] = '" + xbu_code + @"'
                              ,[property_insured_name] = '" + xprop_ins_name + @"'
                         WHERE [req_no] = '" + xreq_no + "'";

            ret = zdb.ExecNonQueryReturnID(sql, zconnstr);

            if (ret > 0)
            {
                
                string sqlUpdaePropIns = @"UPDATE [dbo].[li_insurance_req_property_insured]
                                               SET [top_ins_code] = '" + xtype_pi + @"'
                                                  ,[indemnityperiod] = '" + xindemnity_period + @"'
                                                  ,[suminsured] = '" + xsum_insured + @"'
                                                  ,[startdate] = '" + xstart_date + @"'
                                                  ,[enddate] = '" + xend_date + @"'
                                                  ,[updated_datetime] = '" + xupdate_date + @"'
                                             WHERE [req_no] = '" + xreq_no + "'";

                ret = zdb.ExecNonQueryReturnID(sqlUpdaePropIns, zconnstr);
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
            //var xtype_req = type_req.SelectedValue.ToString();
            var xcompany = company.Text.Trim();
            var xdoc_no = doc_no.Text.Trim();
            var xreq_date = Utillity.ConvertStringToDate(req_date.Value);
            var xsubject = subject.Text.Trim();
            var xto = to.Text.Trim();
            var xpurpose = purpose.Text.Trim();
            var xbackground = background.Text.Trim();
            var xtype_pi = type_pi.SelectedItem.Text.ToString();
            var xindemnity_period = indemnity_period.Text.Trim();
            var xsum_insured = sum_insured.Text.Trim();
            var xstart_date = start_date.Text.Trim();
            var xend_date = end_date.Text.Trim();
            var xapprove_des = approve_des.Text.Trim();

            string templatefile = @"C:\WordTemplate\Insurance\InsuranceTemplateRequest.docx";
            string outputfoler = @"C:\WordTemplate\Insurance\Output";
            string outputfn = outputfoler + @"\inreq_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            #region prepare data
            //Replace TAG STRING
            DataTable dtStr = new DataTable();
            dtStr.Columns.Add("tagname", typeof(string));
            dtStr.Columns.Add("tagvalue", typeof(string));

            DataRow dr0 = dtStr.NewRow();
            dr0["tagname"] = "#docno#";
            dr0["tagvalue"] = xdoc_no.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#company#";
            dr0["tagvalue"] = xcompany.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#to#";
            dr0["tagvalue"] = xto.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#subject#";
            dr0["tagvalue"] = xsubject.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#reqdate#";
            dr0["tagvalue"] = Utillity.ConvertDateToLongDateTime(xreq_date, "en");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#objective#";
            dr0["tagvalue"] = xpurpose.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#reason#";
            dr0["tagvalue"] = xbackground.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#approve#";
            dr0["tagvalue"] = xapprove_des.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            #endregion

            //DOA
            #region DOA 
            var requestor = "คุณรุ่งเรือง วิโรจน์ชีวัน";
            var requestorpos = "Head of Operations";
            var requestordate = System.DateTime.Now.ToString("dd/MM/yyyy");
            var apv1 = "คุณจรูณศักดิ์ นามะฮง";
            var apv1pos = "Insurance Specialist";
            var apv1date = "";
            var apv2 = "คุณชโลทร ศรีสมวงษ์";
            var apv2pos = "Head of Legal";
            var apv2date = "";
            var apv3 = "คุณชยุต อมตวนิช";
            var apv3pos = "Head of Risk Management";
            var apv3date = "";

            var apv4 = "ดร.สิเวศ โรจนสุนทร";
            var apv4pos = "CCO";
            var apv4date = "";
            var apv4cb1 = "";
            var apv4cb2 = "";
            var apv4remark = "";

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name1#";
            dr0["tagvalue"] = requestor.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#position1#";
            dr0["tagvalue"] = requestorpos.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#date1#";
            dr0["tagvalue"] = requestordate.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_name1#";
            dr0["tagvalue"] = "อนุมัติผ่านระบบ";
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name2#";
            dr0["tagvalue"] = apv1.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#position2#";
            dr0["tagvalue"] = apv1pos.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#date2#";
            dr0["tagvalue"] = apv1date.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_name2#";
            dr0["tagvalue"] = apv4remark.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name3#";
            dr0["tagvalue"] = apv2.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#position3#";
            dr0["tagvalue"] = apv2pos.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#date3#";
            dr0["tagvalue"] = apv2date.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_name3#";
            dr0["tagvalue"] = apv4remark.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);


            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name4#";
            dr0["tagvalue"] = apv3.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#position4#";
            dr0["tagvalue"] = apv3pos.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#date4#";
            dr0["tagvalue"] = apv3date.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_name4#";
            dr0["tagvalue"] = apv4remark.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name5#";
            dr0["tagvalue"] = apv4.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#position5#";
            dr0["tagvalue"] = apv4pos.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#date5#";
            dr0["tagvalue"] = apv4date.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_name5#";
            dr0["tagvalue"] = apv4remark.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#cb1#";
            dr0["tagvalue"] = apv4cb1.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#cb2#";
            dr0["tagvalue"] = apv4cb2.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#remark5#";
            dr0["tagvalue"] = apv4remark.Replace(",", "!comma");
            dtStr.Rows.Add(dr0);
            #endregion 


            #region Sample ReplaceTable

            //DataTable Column Properties
            //col_name, col_width, col_align, col_valign,
            DataTable dtProperties1 = new DataTable();
            dtProperties1.Columns.Add("tagname", typeof(string));
            dtProperties1.Columns.Add("col_name", typeof(string));
            dtProperties1.Columns.Add("col_width", typeof(string));
            dtProperties1.Columns.Add("col_align", typeof(string)); //Left, Right, Center
            dtProperties1.Columns.Add("col_valign", typeof(string)); //Top, Middle, Bottom
            dtProperties1.Columns.Add("col_font", typeof(string));
            dtProperties1.Columns.Add("col_fontsize", typeof(string));
            dtProperties1.Columns.Add("col_fontcolor", typeof(string));
            dtProperties1.Columns.Add("col_color", typeof(string));
            dtProperties1.Columns.Add("header_height", typeof(string));
            dtProperties1.Columns.Add("header_color", typeof(string));
            dtProperties1.Columns.Add("header_font", typeof(string));
            dtProperties1.Columns.Add("header_fontsize", typeof(string));
            dtProperties1.Columns.Add("header_fontbold", typeof(string));
            dtProperties1.Columns.Add("header_align", typeof(string)); //Left, Right, Center
            dtProperties1.Columns.Add("header_valign", typeof(string)); //Top, Middle, Bottom
            dtProperties1.Columns.Add("header_fontcolor", typeof(string));
            dtProperties1.Columns.Add("row_height", typeof(string));
            // Replace #table1# ------------------------------------------------------
            DataRow dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "No";
            dr["col_width"] = "100";
            dr["col_align"] = "Center";
            dr["col_valign"] = "Top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Middle";
            dr["header_valign"] = "Center";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Property Insured";
            dr["col_width"] = "200";
            dr["col_align"] = "Left";
            dr["col_valign"] = "Top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Indemnity Period";
            dr["col_width"] = "200";
            dr["col_align"] = "Left";
            dr["col_valign"] = "Top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Sum Insured";
            dr["col_width"] = "200";
            dr["col_align"] = "left";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Start Date";
            dr["col_width"] = "150";
            dr["col_align"] = "left";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "End Date";
            dr["col_width"] = "150";
            dr["col_align"] = "left";
            dr["col_valign"] = "top";
            dr["col_font"] = "Tahoma";
            dr["col_fontsize"] = "9";
            dr["col_fontcolor"] = "Black";
            dr["col_color"] = "Transparent";
            dr["header_height"] = "20";
            dr["header_color"] = "Gray";
            dr["header_font"] = "Tahoma";
            dr["header_fontsize"] = "9";
            dr["header_fontbold"] = "true";
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            //  DataTable dt = new DataTable();
            DataTable dt = new DataTable();
            dt.Columns.Add("tagname", typeof(string));
            dt.Columns.Add("No", typeof(string));
            dt.Columns.Add("Property Insured", typeof(string));
            dt.Columns.Add("Indemnity Period", typeof(string));
            dt.Columns.Add("Sum Insured", typeof(string));
            dt.Columns.Add("Start Date", typeof(string));
            dt.Columns.Add("End Date", typeof(string));

            //DataTable for #table1#
            //var dataGV = iniDataTable();

            //for (int i = 0; i < dataGV.Rows.Count; i++)
            //{
            //    var drGV = dataGV.Rows[i];

            //    DataRow dr1 = dt.NewRow();
            //    dr1["tagname"] = "#table1#";
            //    dr1["No"] = drGV["No"].ToString();
            //    dr1["Property Insured"] = drGV["PropertyInsured"].ToString();  // "xxxxx";//.Text.Replace(",", "!comma");
            //    dr1["Indemnity Period"] = drGV["IndemnityPeriod"].ToString(); // "1,000,000".Replace(",", "!comma"); ;
            //    dr1["Sum Insured"] = drGV["SumInsured"].ToString();  // "15,000".Replace(",", "!comma"); ;
            //    dr1["Start Date"] = drGV["StartDate"].ToString();
            //    dr1["End Date"] = drGV["EndDate"].ToString();
            //    dt.Rows.Add(dr1);
            //}

            //Assign DataTable for #table#
            DataRow dr1 = dt.NewRow();
            dr1["tagname"] = "#table1#";
            dr1["No"] = "1";
            dr1["Property Insured"] = xtype_pi.Replace(",", "!comma");  // "xxxxx";//.Text.Replace(",", "!comma");
            dr1["Indemnity Period"] = xindemnity_period.Replace(",", "!comma"); // "1,000,000".Replace(",", "!comma"); ;
            dr1["Sum Insured"] = xsum_insured.Replace(",", "!comma");  // "15,000".Replace(",", "!comma"); ;
            dr1["Start Date"] = Utillity.ConvertDateToLongDateTime(Utillity.ConvertStringToDate(xstart_date), "en");
            dr1["End Date"] = Utillity.ConvertDateToLongDateTime(Utillity.ConvertStringToDate(xend_date), "en");
            dt.Rows.Add(dr1);
            #endregion

            // Convert to JSONString
            DataTable dtTagPropsTable = new DataTable();
            dtTagPropsTable.Columns.Add("tagname", typeof(string));
            dtTagPropsTable.Columns.Add("jsonstring", typeof(string));

            DataTable dtTagDataTable = new DataTable();
            dtTagDataTable.Columns.Add("tagname", typeof(string));
            dtTagDataTable.Columns.Add("jsonstring", typeof(string));
            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
            var jsonDTProperties1 = repl.DataTableToJSONWithStringBuilder(dtProperties1);
            //var jsonDTProperties2 = repl.DataTableToJSONWithStringBuilder(dtProperties2);
            var jsonDTdata = repl.DataTableToJSONWithStringBuilder(dt);
            //var jsonDTdata2 = repl.DataTableToJSONWithStringBuilder(dt2);
            //end prepare data

            // Save to Database z_replacedocx_log
            string xreq_no = req_no.Text.Trim();
            string sql = @"insert into z_replacedocx_log (replacedocx_reqno,jsonTagString, jsonTableProp, jsonTableData,template_filepath , output_directory,output_filepath, delete_output ) 
                        values('" + xreq_no + @"',
                               '" + jsonDTStr + @"', 
                                '" + jsonDTProperties1 + @"', 
                                '" + jsonDTdata + @"', 
                                '" + templatefile + @"', 
                                '" + outputfoler + @"', 
                                '" + outputfn + @"',  
                                '" + "0" + @"'
                            ) ";

            zdb.ExecNonQuery(sql, zconnstr);

            var outputbyte = rdoc.ReplaceData2(jsonDTStr, jsonDTProperties1, jsonDTdata, templatefile, outputfoler, outputfn, false);

            repl.convertDOCtoPDF(outputfn, outputfn.Replace(".docx", ".pdf"), false);
            // Dowload Word 
            Response.Clear();
            Response.ContentType = "text/xml";
            Response.AddHeader("content-disposition", $"attachment; filename={outputfn}");
            Response.BinaryWrite(outputbyte);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.End();
        }
        protected void btn_submit_Click(object sender, EventArgs e)
        {
            // Sample Submit
            string process_code = "INR_NEW";
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

                // set WF Attributes
                wfAttr.subject = subject.Text.Trim();
                wfAttr.assto_login = emp.next_line_mgr_login;
                wfAttr.wf_status = "SUBMITTED";
                wfAttr.submit_answer = "SUBMITTED";
                //wfAttr.next_assto_login = emp.next_line_mgr_login;
                wfAttr.submit_by = emp.user_login;
                wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by);
                wfAttr.updated_by = emp.user_login;
                
                // wf.updateProcess
                var wfA_NextStep = zwf.updateProcess(wfAttr);
                //wfA_NextStep.next_assto_login = emp.next_line_mgr_login;
                wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by);
                string status = zwf.Insert_NextStep(wfA_NextStep);

                if (status == "Success")
                {
                    Response.Redirect("/legalportal/legalportal.aspx?m=myworklist");
                }

            }


        }
    }
}