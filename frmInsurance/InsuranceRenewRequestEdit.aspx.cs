using onlineLegalWF.Class;
using onlineLegalWF.userControls;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static onlineLegalWF.Class.ReplaceInsRenew;

namespace onlineLegalWF.frmInsurance
{
    public partial class InsuranceRenewRequestEdit : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        public ReplaceInsRenew zreplaceinsrenew = new ReplaceInsRenew();
        public MargePDF zmergepdf = new MargePDF();
        public SendMail zsendmail = new SendMail();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string id = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(id))
                {
                    setDataEditRenewRequest(id);
                }

            }
        }

        private void setDataEditRenewRequest(string id)
        {
            ucHeader1.setHeader("Edit Renew Request");

            ddl_bu.DataSource = GetBusinessUnit();
            ddl_bu.DataBind();
            ddl_bu.DataTextField = "bu_desc";
            ddl_bu.DataValueField = "bu_code";
            ddl_bu.DataBind();

            string sql = "select * from li_insurance_request where req_no='" + id + "'";

            var res = zdb.ExecSql_DataTable(sql, zconnstr);

            if (res.Rows.Count > 0)
            {
                req_no.Text = res.Rows[0]["req_no"].ToString();
                req_date.Value = Convert.ToDateTime(res.Rows[0]["req_date"]).ToString("yyyy-MM-dd");
                type_req.SelectedValue = res.Rows[0]["toreq_code"].ToString();
                company_name.Text = res.Rows[0]["company_name"].ToString();
                doc_no.Text = res.Rows[0]["document_no"].ToString();
                subject.Text = res.Rows[0]["subject"].ToString();
                to.Text = res.Rows[0]["dear"].ToString();
                purpose.Text = res.Rows[0]["objective"].ToString();
                background.Text = res.Rows[0]["reason"].ToString();
                ddl_bu.SelectedValue = res.Rows[0]["bu_code"].ToString();
                prop_ins_name.Text = res.Rows[0]["property_insured_name"].ToString();
                lblPID.Text = res.Rows[0]["process_id"].ToString();
                hid_PID.Value = res.Rows[0]["process_id"].ToString();
                ucAttachment1.ini_object(hid_PID.Value = res.Rows[0]["process_id"].ToString());
                ucCommentlog1.ini_object(hid_PID.Value = res.Rows[0]["process_id"].ToString());

                //company_name.Text = GetCompanyNameByBuCode(ddl_bu.SelectedValue.ToString());
            }

            var dt = iniDataTable(id);
            gv1.DataSource = dt;
            gv1.DataBind();

            //check data and disable filed
            foreach (GridViewRow row in gv1.Rows)
            {
                string top_ins_code = (row.FindControl("gv1txttop_ins_code") as HiddenField).Value;
                (row.FindControl("gv1txtNo") as TextBox).Enabled = false;
                (row.FindControl("gv1txtPropertyInsured") as TextBox).Enabled = false;
                if (top_ins_code == "02")
                {
                    (row.FindControl("gv1txtIndemnityPeriod") as TextBox).Enabled = true;
                    (row.FindControl("gv1txtGop") as TextBox).Enabled = true;
                }
                else
                {
                    (row.FindControl("gv1txtIndemnityPeriod") as TextBox).Enabled = false;
                    (row.FindControl("gv1txtGop") as TextBox).Enabled = false;
                }

            }

        }
        public string GetCompanyNameByBuCode(string xbu_code)
        {
            string company_name = "";

            string sql = @"select * from li_business_unit where bu_code='" + xbu_code + "'";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            if (dt.Rows.Count > 0)
            {
                company_name = dt.Rows[0]["company_name"].ToString();

            }

            return company_name;
        }
        protected void ddl_bu_Changed(object sender, EventArgs e)
        {
            company_name.Text = GetCompanyNameByBuCode(ddl_bu.SelectedValue.ToString());
        }
        public DataTable GetBusinessUnit()
        {
            string sql = "select * from li_business_unit where isactive=1 order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }

        public DataTable iniDataTable(string id)
        {
            //getData
            var dt = iniDTStructure();
            var dr = dt.NewRow();

            var dt_top_ins = GetTypeOfPropertyInsured();
            var dt_prop_ins = GetDataPropertyInsuredByReqNo(id);

            if (dt_top_ins.Rows.Count > 0)
            {
                int no = 0;

                foreach (DataRow dr_ins in dt_top_ins.Rows)
                {
                    //init Data PropertyInsured
                    dr = dt.NewRow();
                    dr["No"] = (no + 1);
                    dr["PropertyInsured"] = dr_ins["top_ins_desc"].ToString();

                    //check Data from Db
                    if (dt_prop_ins.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt_prop_ins.Rows) 
                        {
                            //check data ins_code tb master == ins_code detail assign value
                            if (item["top_ins_code"].ToString() == dr_ins["top_ins_code"].ToString())
                            {
                                
                                dr["GOP"] = item["gop_fc"].ToString();
                                dr["IndemnityPeriod"] = item["indemnityperiod"].ToString();
                                dr["SumInsured"] = item["suminsured"].ToString();
                                dr["StartDate"] = Convert.ToDateTime(item["startdate"]).ToString("yyyy-MM-dd");
                                dr["EndDate"] = Convert.ToDateTime(item["enddate"]).ToString("yyyy-MM-dd");
                                
                            }
                        }
                        
                    }
                    else 
                    {
                        dr["GOP"] = "";
                        dr["IndemnityPeriod"] = "";
                        dr["SumInsured"] = "";
                        dr["StartDate"] = "";
                        dr["EndDate"] = "";
                    }

                    dr["Top_Ins_Code"] = dr_ins["top_ins_code"].ToString();
                    dt.Rows.Add(dr);



                    no++;
                }
            }

            return dt;
        }

        public DataTable iniDTStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("No", typeof(string));
            dt.Columns.Add("PropertyInsured", typeof(string));
            dt.Columns.Add("GOP", typeof(string));
            dt.Columns.Add("IndemnityPeriod", typeof(string));
            dt.Columns.Add("SumInsured", typeof(string));
            dt.Columns.Add("StartDate", typeof(string));
            dt.Columns.Add("EndDate", typeof(string));
            dt.Columns.Add("Top_Ins_Code", typeof(string));
            return dt;
        }

        public DataTable GetTypeOfPropertyInsured()
        {
            string sql = "select * from li_type_of_property_insured order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }

        public DataTable GetDataPropertyInsuredByReqNo(string id)
        {
            string sqlPropIns = "select * from li_insurance_req_property_insured where req_no='" + id + "'";
            DataTable dt = zdb.ExecSql_DataTable(sqlPropIns, zconnstr);

            return dt;
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            int res = UpdateRenewRequest();

            if (res > 0)
            {
                Response.Write("<script>alert('Successfully Updated');</script>");
                //Response.Redirect("frmInsurance/InsuranceRenewRequestList");
            }
            else
            {
                Response.Write("<script>alert('Error !!!');</script>");
            }
        }

        private int UpdateRenewRequest()
        {
            int ret = 0;

            var xreq_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var xreq_no = req_no.Text.Trim();
            var xtype_req = type_req.SelectedValue.ToString();
            var xbu_code = ddl_bu.SelectedValue.ToString();
            var xcompany_name = company_name.Text.ToString();
            var xdoc_no = doc_no.Text.Trim();
            var xsubject = subject.Text.Trim();
            var xto = to.Text.Trim();
            var xpurpose = purpose.Text.Trim();
            var xbackground = background.Text.Trim();
            var xprop_ins_name = prop_ins_name.Text.Trim();


            //Get Data from gv1 Insurance Detail
            List<InsurancePropData> listInsurancePropData = new List<InsurancePropData>();
            foreach (GridViewRow row in gv1.Rows)
            {
                InsurancePropData data = new InsurancePropData();
                data.TypeOfPropertyInsured = (row.FindControl("gv1txttop_ins_code") as HiddenField).Value;
                data.PropertyInsured = (row.FindControl("gv1txtPropertyInsured") as TextBox).Text;
                data.GOP = (row.FindControl("gv1txtGop") as TextBox).Text;
                data.IndemnityPeriod = (row.FindControl("gv1txtIndemnityPeriod") as TextBox).Text;
                data.SumInsured = (row.FindControl("gv1txtSumInsured") as TextBox).Text;
                data.StartDate = (row.FindControl("gv1txtSdate") as TextBox).Text;
                data.EndDate = (row.FindControl("gv1txtEdate") as TextBox).Text;

                if (!string.IsNullOrEmpty(data.SumInsured) && !string.IsNullOrEmpty(data.StartDate) && !string.IsNullOrEmpty(data.EndDate))
                {
                    listInsurancePropData.Add(data);
                }

            }

            string sql = @"UPDATE [dbo].[li_insurance_request]
                               SET [toreq_code] = '" + xtype_req + @"'
                                  ,[company_name] = '" + xcompany_name + @"'
                                  ,[document_no] = '" + xdoc_no + @"'
                                  ,[subject] = '" + xsubject + @"'
                                  ,[dear] = '" + xto + @"'
                                  ,[objective] = '" + xpurpose + @"'
                                  ,[reason] = '" + xbackground + @"'
                                  ,[updated_datetime] = '" + xreq_date + @"'
                                  ,[bu_code] = '" + xbu_code + @"'
                                  ,[property_insured_name] = '" + xprop_ins_name + @"'
                             WHERE [req_no] ='" + xreq_no + "'";

            ret = zdb.ExecNonQueryReturnID(sql, zconnstr);

            if (ret > 0)
            {
                if (listInsurancePropData.Count > 0)
                {
                    string sqlDeletePropIns = @"DELETE FROM [li_insurance_req_property_insured] WHERE req_no='" + xreq_no + "'";

                    ret = zdb.ExecNonQueryReturnID(sqlDeletePropIns, zconnstr);

                    if (ret > 0) 
                    {
                        foreach (var item in listInsurancePropData)
                        {
                            string sqlInsertPropIns = @"INSERT INTO [dbo].[li_insurance_req_property_insured]
                                                   ([req_no],[top_ins_code],[gop_fc],[indemnityperiod],[suminsured],[startdate],[enddate],[created_datetime],[updated_datetime])
                                             VALUES
                                                   ('" + xreq_no + @"'
                                                   ,'" + item.TypeOfPropertyInsured + @"'
                                                   ,'" + item.GOP + @"'
                                                   ,'" + item.IndemnityPeriod + @"'
                                                   ,'" + item.SumInsured + @"'
                                                   ,'" + item.StartDate + @"'
                                                   ,'" + item.EndDate + @"'
                                                   ,'" + xreq_date + @"'
                                                   ,'" + xreq_date + @"')";

                            ret = zdb.ExecNonQueryReturnID(sqlInsertPropIns, zconnstr);
                        }
                    }
                    
                }

            }

            return ret;
        }

        public class InsurancePropData
        {
            public string TypeOfPropertyInsured { get; set; }
            public string PropertyInsured { get; set; }
            public string GOP { get; set; }
            public string IndemnityPeriod { get; set; }
            public string SumInsured { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }

        protected void btn_gendocumnt_Click(object sender, EventArgs e)
        {
            GenDocumnet();
        }
        private void GenDocumnet()
        {
            // Replace Doc
            var xreq_date = Utillity.ConvertStringToDate(req_date.Value);
            var xreq_no = req_no.Text.Trim();
            var xbu_code = ddl_bu.SelectedValue.ToString();
            var xcompany_name = company_name.Text.ToString();
            var xdoc_no = doc_no.Text.Trim();
            var xsubject = subject.Text.Trim();
            var xto = to.Text.Trim();
            var xpurpose = purpose.Text.Trim();
            var xbackground = background.Text.Trim();
            var xapprove_des = "We, therefore, request for your approval to renew mentioned insurance policy.";

            var path_template = ConfigurationManager.AppSettings["WT_Template_insurance"].ToString();
            string templatefile = path_template + @"\InsuranceTemplateRenew.docx";
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\inreq_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            #region gentagstr data form
            ReplaceInsReNew_TagData data = new ReplaceInsReNew_TagData();
            data.docno = xdoc_no.Replace(",", "!comma");
            data.from = xcompany_name.Replace(",", "!comma");
            data.attn = xto.Replace(",", "!comma");
            data.re = xsubject.Replace(",", "!comma");
            data.reqdate = Utillity.ConvertDateToLongDateTime(xreq_date, "en");
            data.objective = xpurpose.Replace(",", "!comma");
            data.reason = xbackground.Replace(",", "!comma");
            data.approve = xapprove_des.Replace(",", "!comma");

            var requestordate = System.DateTime.Now.ToString("dd/MM/yyyy");

            ///get gm heam_am c_level
            string sqlbu = @"select * from li_business_unit where bu_code = '" + xbu_code + "'";
            var res = zdb.ExecSql_DataTable(sqlbu, zconnstr);

            var requestor = "";
            var requestorpos = "";
            var gmname = "";
            var gmpos = "GM";
            var amname = "";
            var headamname = "";
            var clevelname = "";
            if (res.Rows.Count > 0)
            {
                var empFunc = new EmpInfo();
                if (Session["user_login"] != null)
                {
                    var xlogin_name = Session["user_login"].ToString();
                    var emp = empFunc.getEmpInfo(xlogin_name);
                    string sqlwf = "select * from wf_routing where process_id = '" + lblPID.Text + "' and step_name = 'Start'";
                    var dtwf = zdb.ExecSql_DataTable(sqlwf, zconnstr);
                    if (dtwf.Rows.Count > 0)
                    {
                        DataRow drwf = dtwf.Rows[0];
                        var emprequester = empFunc.getEmpInfo(drwf["submit_by"].ToString());
                        if (emprequester != null)
                        {
                            requestor = emprequester.full_name_en;
                            requestorpos = emprequester.position_en;
                        }

                    }
                    else
                    {
                        requestor = emp.full_name_en;
                        requestorpos = emp.position_en;
                    }


                }
                string xgm = res.Rows[0]["gm"].ToString();
                string xam = res.Rows[0]["am"].ToString();
                string xhead_am = res.Rows[0]["head_am"].ToString();
                string xclevel = res.Rows[0]["c_level"].ToString();
                string xexternal_domain = res.Rows[0]["external_domain"].ToString();
                //get data am user
                if (!string.IsNullOrEmpty(xam))
                {
                    var empam = empFunc.getEmpInfo(xam);
                    if (empam.user_login != null)
                    {
                        amname = empam.full_name_en;
                    }
                }
                //get data head am user
                if (!string.IsNullOrEmpty(xhead_am))
                {
                    var empheadam = empFunc.getEmpInfo(xhead_am);
                    if (empheadam.user_login != null)
                    {
                        headamname = empheadam.full_name_en;
                    }
                }
                //get data gm user
                if (!string.IsNullOrEmpty(xgm))
                {
                    var empgm = empFunc.getEmpInfo(xgm);
                    if (empgm.user_login != null)
                    {
                        gmname = empgm.full_name_en;
                        if (xexternal_domain == "Y")
                        {
                            gmpos = empgm.position_en;
                        }
                    }
                }
                //get data c_level user
                if (!string.IsNullOrEmpty(xclevel))
                {
                    var empc = empFunc.getEmpInfo(xclevel);
                    if (empc.user_login != null)
                    {
                        clevelname = empc.full_name_en;
                    }
                }
            }

            var apv1 = gmname;
            var apv1pos = gmpos;
            var apv1date = "";
            var apv2 = amname;
            var apv2pos = "AM";
            var apv2date = "";
            var apv2_1 = headamname;
            var apv2pos_1 = "Head AM";
            var apv2date_1 = "";
            var apv3 = clevelname;
            var apv3pos = "C-Level";
            var apv3date = "";
            var signname1 = "";
            var signname2 = "";
            var signname3 = "";
            var signname3_1 = "";
            var signname4 = "";

            data.sign_name1 = signname1;
            data.name1 = requestor.Replace(",", "!comma");
            data.position1 = requestorpos.Replace(",", "!comma");
            data.date1 = requestordate.Replace(",", "!comma");

            data.sign_name2 = signname2;
            data.name2 = apv1.Replace(",", "!comma");
            data.position2 = apv1pos.Replace(",", "!comma");
            data.date2 = apv1date.Replace(",", "!comma");

            data.sign_name3 = signname3;
            data.name3 = apv2.Replace(",", "!comma");
            data.position3 = apv2pos.Replace(",", "!comma");
            data.date3 = apv2date.Replace(",", "!comma");

            data.sign_name3_1 = signname3_1;
            data.name3_1 = apv2_1.Replace(",", "!comma");
            data.position3_1 = apv2pos_1.Replace(",", "!comma");
            data.date3_1 = apv2date_1.Replace(",", "!comma");

            data.sign_name4 = signname4;
            data.name4 = apv3.Replace(",", "!comma");
            data.position4 = apv3pos.Replace(",", "!comma");
            data.date4 = apv3date.Replace(",", "!comma");


            DataTable dtStr = zreplaceinsrenew.genTagData(data);
            #endregion 

            #region Sample ReplaceTable

            //DataTable Column Properties
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
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
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
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Sum Insured";
            dr["col_width"] = "200";
            dr["col_align"] = "Center";
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
            //Get Data from gv1 Insurance Detail
            List<InsurancePropData> listInsurancePropData = new List<InsurancePropData>();
            foreach (GridViewRow row in gv1.Rows)
            {
                InsurancePropData datasum = new InsurancePropData();
                datasum.TypeOfPropertyInsured = (row.FindControl("gv1txttop_ins_code") as HiddenField).Value;
                datasum.PropertyInsured = (row.FindControl("gv1txtPropertyInsured") as TextBox).Text;
                datasum.IndemnityPeriod = (row.FindControl("gv1txtIndemnityPeriod") as TextBox).Text;
                datasum.SumInsured = (row.FindControl("gv1txtSumInsured") as TextBox).Text;
                datasum.StartDate = (row.FindControl("gv1txtSdate") as TextBox).Text;
                datasum.EndDate = (row.FindControl("gv1txtEdate") as TextBox).Text;

                if (!string.IsNullOrEmpty(datasum.SumInsured) && !string.IsNullOrEmpty(datasum.StartDate) && !string.IsNullOrEmpty(datasum.EndDate))
                {
                    listInsurancePropData.Add(datasum);
                }

            }
            //Assign Data From gv1
            var drGV = dt.NewRow();

            if (listInsurancePropData.Count > 0)
            {
                int no = 0;

                foreach (var item in listInsurancePropData)
                {
                    drGV = dt.NewRow();
                    drGV["tagname"] = "#table1#";
                    drGV["No"] = (no + 1);
                    drGV["Property Insured"] = item.PropertyInsured;
                    drGV["Indemnity Period"] = item.IndemnityPeriod;
                    drGV["Sum Insured"] = item.SumInsured;
                    drGV["Start Date"] = Utillity.ConvertDateToLongDateTime(Utillity.ConvertStringToDate(item.StartDate), "en");
                    drGV["End Date"] = Utillity.ConvertDateToLongDateTime(Utillity.ConvertStringToDate(item.EndDate), "en");
                    dt.Rows.Add(drGV);

                    no++;
                }
            }

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
            //// Dowload Word 
            //Response.Clear();
            //Response.ContentType = "text/xml";
            //Response.AddHeader("content-disposition", $"attachment; filename={outputfn}");
            //Response.BinaryWrite(outputbyte);
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.End();
            string filePath = outputfn.Replace(".docx", ".pdf");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalDoc();", true);
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            pdf_render.Attributes["src"] = host_url + "render/pdf?id=" + filePath.Replace("+", @"%2B");


            #endregion
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            // Sample Submit
            string process_code = "INR_RENEW";
            int version_no = 1;
            string xbu_code = ddl_bu.SelectedValue;

            // getCurrentStep
            var wfAttr = zwf.getCurrentStep(lblPID.Text, process_code, version_no);

            // check session_user
            if (Session["user_login"] != null)
            {
                //get check external domain
                string sql = @"select [row_id],[process_id],[req_no],[req_date],[toreq_code],ins.[company_name],[document_no],[subject],[dear],[objective]
                                  ,[reason],[approved_desc],[status],[updated_datetime], ins.[bu_code],bu.[external_domain],[property_insured_name] from li_insurance_request as ins
                              INNER JOIN li_business_unit as bu on ins.bu_code = bu.bu_code
                              where process_id = '" + wfAttr.process_id + "'";

                var resex = zdb.ExecSql_DataTable(sql, zconnstr);

                if (resex.Rows.Count > 0)
                {
                    wfAttr.external_domain = resex.Rows[0]["external_domain"].ToString();
                }

                var xlogin_name = Session["user_login"].ToString();
                var empFunc = new EmpInfo();

                //get data user
                var emp = empFunc.getEmpInfo(xlogin_name);

                // set WF Attributes
                wfAttr.subject = subject.Text.Trim();
                wfAttr.assto_login = emp.next_line_mgr_login;
                wfAttr.wf_status = "SUBMITTED";
                wfAttr.submit_answer = "SUBMITTED";
                wfAttr.submit_by = emp.user_login;
                wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, lblPID.Text, xbu_code);
                wfAttr.updated_by = emp.user_login;
                
                // wf.updateProcess
                var wfA_NextStep = zwf.updateProcess(wfAttr);
                //wfA_NextStep.next_assto_login = emp.next_line_mgr_login;
                wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by, lblPID.Text, xbu_code);
                string status = zwf.Insert_NextStep(wfA_NextStep);

                if (status == "Success")
                {
                    GenDocumnetInsRenew(lblPID.Text);
                    //send mail
                    string subject = "";
                    string body = "";
                    string sqlmail = @"select * from li_insurance_request where process_id = '" + wfAttr.process_id + "'";
                    var dt = zdb.ExecSql_DataTable(sqlmail, zconnstr);
                    if (dt.Rows.Count > 0)
                    {
                        var dr = dt.Rows[0];
                        string id = dr["req_no"].ToString();
                        subject = dr["subject"].ToString();
                        var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                        body = "คุณได้รับมอบหมายให้ตรวจสอบเอกสารเลขที่ " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='"+host_url_sendmail+"legalportal/legalportal?m=myworklist'>Click</a>";

                        string pathfileins = "";
                        string outputdirectory = "";

                        string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                        var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                        if (resfile.Rows.Count > 0)
                        {
                            pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                            outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                            List<string> listpdf = new List<string>();
                            listpdf.Add(pathfileins);

                            string sqlattachfile = "select * from wf_attachment where pid = '" + wfAttr.process_id + "' and e_form IS NULL";

                            var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                            if (resattachfile.Rows.Count > 0)
                            {
                                foreach (DataRow item in resattachfile.Rows)
                                {
                                    listpdf.Add(item["attached_filepath"].ToString());
                                }
                            }
                            //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                            string[] pdfFiles = listpdf.ToArray();

                            string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

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
                                _ = zsendmail.sendEmail(subject + " Mail To Next Appove", email, body, filepath);
                            }

                        }

                    }
                    var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                    Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                }

            }


        }

        private void GenDocumnetInsRenew(string pid)
        {

            // Replace Doc
            var xreq_no = "";
            var xapprove_des = "We, therefore, request for your approval to renew mentioned insurance policy.";

            var path_template = ConfigurationManager.AppSettings["WT_Template_insurance"].ToString();
            string templatefile = path_template + @"\InsuranceTemplateRenew.docx";
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\inreq_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            string sqlinsreq = "select * from li_insurance_request where process_id='" + pid + "'";
            var resinsreq = zdb.ExecSql_DataTable(sqlinsreq, zconnstr);

            #region gentagstr data form
            ReplaceInsReNew_TagData data = new ReplaceInsReNew_TagData();
            if (resinsreq.Rows.Count > 0)
            {
                xreq_no = resinsreq.Rows[0]["req_no"].ToString();
                data.approve = xapprove_des.Replace(",", "!comma");

                var requestordate = "";

                string xbu_code = resinsreq.Rows[0]["bu_code"].ToString();
                ///get moa
                string sqlbu = @"select * from li_business_unit where bu_code = '" + xbu_code + "'";
                var res = zdb.ExecSql_DataTable(sqlbu, zconnstr);

                var requestor = "";
                var requestorpos = "";
                var gmname = "";
                var gmpos = "GM";
                var amname = "";
                var headamname = "";
                var clevelname = "";
                if (res.Rows.Count > 0)
                {
                    var empFunc = new EmpInfo();
                    if (Session["user_login"] != null)
                    {
                        var xlogin_name = Session["user_login"].ToString();
                        var emp = empFunc.getEmpInfo(xlogin_name);
                        string sqlwf = "select * from wf_routing where process_id = '" + lblPID.Text + "' and step_name = 'Start'";
                        var dtwf = zdb.ExecSql_DataTable(sqlwf, zconnstr);
                        if (dtwf.Rows.Count > 0)
                        {
                            DataRow drwf = dtwf.Rows[0];
                            var emprequester = empFunc.getEmpInfo(drwf["submit_by"].ToString());
                            if (emprequester != null)
                            {
                                requestor = emprequester.full_name_en;
                                requestorpos = emprequester.position_en;
                            }

                        }
                        else
                        {
                            requestor = emp.full_name_en;
                            requestorpos = emp.position_en;
                        }


                    }
                    string xgm = res.Rows[0]["gm"].ToString();
                    string xam = res.Rows[0]["am"].ToString();
                    string xhead_am = res.Rows[0]["head_am"].ToString();
                    string xclevel = res.Rows[0]["c_level"].ToString();
                    string xexternal_domain = res.Rows[0]["external_domain"].ToString();
                    //get data am user
                    if (!string.IsNullOrEmpty(xam))
                    {
                        var empam = empFunc.getEmpInfo(xam);
                        if (empam.user_login != null)
                        {
                            amname = empam.full_name_en;
                        }
                    }
                    //get data head am user
                    if (!string.IsNullOrEmpty(xhead_am))
                    {
                        var empheadam = empFunc.getEmpInfo(xhead_am);
                        if (empheadam.user_login != null)
                        {
                            headamname = empheadam.full_name_en;
                        }
                    }
                    //get data gm user
                    if (!string.IsNullOrEmpty(xgm))
                    {
                        var empgm = empFunc.getEmpInfo(xgm);
                        if (empgm.user_login != null)
                        {
                            gmname = empgm.full_name_en;
                            if (xexternal_domain == "Y")
                            {
                                gmpos = empgm.position_en;
                            }
                        }
                    }
                    //get data c_level user
                    if (!string.IsNullOrEmpty(xclevel))
                    {
                        var empc = empFunc.getEmpInfo(xclevel);
                        if (empc.user_login != null)
                        {
                            clevelname = empc.full_name_en;
                        }
                    }
                }

                var apv1 = gmname;
                var apv1pos = gmpos;
                var apv1date = "";
                var apv2 = amname;
                var apv2pos = "AM";
                var apv2date = "";
                var apv2_1 = headamname;
                var apv2pos_1 = "Head AM";
                var apv2date_1 = "";
                var apv3 = clevelname;
                var apv3pos = "C-Level";
                var apv3date = "";
                var signname1 = "";
                var signname2 = "";
                var signname3 = "";
                var signname3_1 = "";
                var signname4 = "";

                data.sign_name1 = signname1;
                data.name1 = requestor.Replace(",", "!comma");
                data.position1 = requestorpos.Replace(",", "!comma");
                data.date1 = requestordate.Replace(",", "!comma");

                data.sign_name2 = signname2;
                data.name2 = apv1.Replace(",", "!comma");
                data.position2 = apv1pos.Replace(",", "!comma");
                data.date2 = apv1date.Replace(",", "!comma");

                data.sign_name3 = signname3;
                data.name3 = apv2.Replace(",", "!comma");
                data.position3 = apv2pos.Replace(",", "!comma");
                data.date3 = apv2date.Replace(",", "!comma");

                data.sign_name3_1 = signname3_1;
                data.name3_1 = apv2_1.Replace(",", "!comma");
                data.position3_1 = apv2pos_1.Replace(",", "!comma");
                data.date3_1 = apv2date_1.Replace(",", "!comma");

                data.sign_name4 = signname4;
                data.name4 = apv3.Replace(",", "!comma");
                data.position4 = apv3pos.Replace(",", "!comma");
                data.date4 = apv3date.Replace(",", "!comma");

            }

            DataTable dtStr = zreplaceinsrenew.BindTagData(lblPID.Text,data);
            #endregion 

            #region Sample ReplaceTable
            //DataTable Column Properties
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
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
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
            dr["header_align"] = "Center";
            dr["header_valign"] = "Middle";
            dr["header_fontcolor"] = "White";
            dr["row_height"] = "16";
            dtProperties1.Rows.Add(dr);

            dr = dtProperties1.NewRow();
            dr["tagname"] = "#table1#";
            dr["col_name"] = "Sum Insured";
            dr["col_width"] = "200";
            dr["col_align"] = "Center";
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

            DataTable dt = zreplaceinsrenew.genTagTableData(lblPID.Text);
            ////  DataTable dt = new DataTable();
            //DataTable dt = new DataTable();
            //dt.Columns.Add("tagname", typeof(string));
            //dt.Columns.Add("No", typeof(string));
            //dt.Columns.Add("Property Insured", typeof(string));
            //dt.Columns.Add("Indemnity Period", typeof(string));
            //dt.Columns.Add("Sum Insured", typeof(string));
            //dt.Columns.Add("Start Date", typeof(string));
            //dt.Columns.Add("End Date", typeof(string));

            ////DataTable for #table1#
            ////Get Data from gv1 Insurance Detail
            //List<InsurancePropData> listInsurancePropData = new List<InsurancePropData>();
            //foreach (GridViewRow row in gv1.Rows)
            //{
            //    InsurancePropData datasum = new InsurancePropData();
            //    datasum.TypeOfPropertyInsured = (row.FindControl("gv1txttop_ins_code") as HiddenField).Value;
            //    datasum.PropertyInsured = (row.FindControl("gv1txtPropertyInsured") as TextBox).Text;
            //    datasum.IndemnityPeriod = (row.FindControl("gv1txtIndemnityPeriod") as TextBox).Text;
            //    datasum.SumInsured = (row.FindControl("gv1txtSumInsured") as TextBox).Text;
            //    datasum.StartDate = (row.FindControl("gv1txtSdate") as TextBox).Text;
            //    datasum.EndDate = (row.FindControl("gv1txtEdate") as TextBox).Text;

            //    if (!string.IsNullOrEmpty(datasum.IndemnityPeriod) && !string.IsNullOrEmpty(datasum.SumInsured) && !string.IsNullOrEmpty(datasum.StartDate) && !string.IsNullOrEmpty(datasum.EndDate))
            //    {
            //        listInsurancePropData.Add(datasum);
            //    }

            //}
            ////Assign Data From gv1
            //var drGV = dt.NewRow();

            //if (listInsurancePropData.Count > 0)
            //{
            //    int no = 0;

            //    foreach (var item in listInsurancePropData)
            //    {
            //        drGV = dt.NewRow();
            //        drGV["tagname"] = "#table1#";
            //        drGV["No"] = (no + 1);
            //        drGV["Property Insured"] = item.PropertyInsured;
            //        drGV["Indemnity Period"] = item.IndemnityPeriod;
            //        drGV["Sum Insured"] = item.SumInsured;
            //        drGV["Start Date"] = Utillity.ConvertDateToLongDateTime(Utillity.ConvertStringToDate(item.StartDate), "en");
            //        drGV["End Date"] = Utillity.ConvertDateToLongDateTime(Utillity.ConvertStringToDate(item.EndDate), "en");
            //        dt.Rows.Add(drGV);

            //        no++;
            //    }
            //}

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
            // Dowload Word 
            //Response.Clear();
            //Response.ContentType = "text/xml";
            //Response.AddHeader("content-disposition", $"attachment; filename={outputfn}");
            //Response.BinaryWrite(outputbyte);
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.End();



        }

        protected void IndemnityPeriodChanged(object sender, EventArgs e)
        {
            int indem = 0;
            int gb = 0;
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            //NamingContainer return the container that the control sits in

            //get data textbox indemnity
            TextBox indemnitytb = (TextBox)row.FindControl("gv1txtIndemnityPeriod");
            //get data textbox gop
            TextBox goptb = (TextBox)row.FindControl("gv1txtGop");
            if (!string.IsNullOrEmpty(indemnitytb.Text))
            {
                indem = Int32.Parse(indemnitytb.Text);
                if (!string.IsNullOrEmpty(goptb.Text))
                {
                    gb = Int32.Parse(goptb.Text);

                    //get data textbox suminsured
                    TextBox suminsuredtb = (TextBox)row.FindControl("gv1txtSumInsured");

                    //set suminsured value
                    suminsuredtb.Text = (gb * (indem / 12)).ToString();
                    suminsuredtb.Focus();
                }
                else
                {
                    indemnitytb.Focus();
                }
            }
            else
            {
                indemnitytb.Focus();
            }
        }

        protected void GopChanged(object sender, EventArgs e)
        {
            int indem = 0;
            int gb = 0;
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            //NamingContainer return the container that the control sits in

            //get data textbox indemnity
            TextBox indemnitytb = (TextBox)row.FindControl("gv1txtIndemnityPeriod");
            //get data textbox gop
            TextBox goptb = (TextBox)row.FindControl("gv1txtGop");
            if (!string.IsNullOrEmpty(indemnitytb.Text))
            {
                indem = Int32.Parse(indemnitytb.Text);
                if (!string.IsNullOrEmpty(goptb.Text))
                {
                    gb = Int32.Parse(goptb.Text);

                    //get data textbox suminsured
                    TextBox suminsuredtb = (TextBox)row.FindControl("gv1txtSumInsured");

                    //set suminsured value
                    suminsuredtb.Text = (gb * (indem / 12)).ToString();
                    suminsuredtb.Focus();
                }
                else
                {
                    goptb.Focus();
                }
            }
            else
            {
                goptb.Focus();
            }
        }

        protected void btn_sendreview_Click(object sender, EventArgs e)
        {
            GenDocumnetInsRenew(lblPID.Text);

            string subject = "";
            string body = "";
            string sql = @"select * from li_insurance_request where process_id = '" + lblPID.Text + "'";
            var dt = zdb.ExecSql_DataTable(sql, zconnstr);
            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                string id = dr["req_no"].ToString();
                subject = dr["subject"].ToString();
                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                body = "คุณได้รับมอบหมายให้ตรวจสอบเอกสารเลขที่ " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='"+host_url_sendmail+"legalportal/legalportal?m=myworklist'>Click</a>";

                string pathfileins = "";
                string outputdirectory = "";

                string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                if (resfile.Rows.Count > 0)
                {
                    pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                    outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                    List<string> listpdf = new List<string>();
                    listpdf.Add(pathfileins);

                    string sqlattachfile = "select * from wf_attachment where pid = '" + lblPID.Text + "' and e_form IS NULL";

                    var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                    if (resattachfile.Rows.Count > 0)
                    {
                        foreach (DataRow item in resattachfile.Rows)
                        {
                            listpdf.Add(item["attached_filepath"].ToString());
                        }
                    }
                    //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                    string[] pdfFiles = listpdf.ToArray();

                    string email = "";

                    var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                    ////get mail from db
                    if (isdev != "true")
                    {
                        email = "jaroonsak.n@assetworldcorp-th.com";
                    }
                    else
                    {
                        //fix mail test
                        email = "legalwfuat2024@gmail.com";
                    }

                    string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

                    //send mail to Jaroonsak review
                    ////fix mail test
                    //string email = "legalwfuat2024@gmail.com";
                    _ = zsendmail.sendEmail(subject + " Mail To Jaroonsak.n Review", email, body, filepath);

                    Response.Write("<script>alert('SendEmail Successfully');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Error !!!');</script>");
            }
        }
    }
}