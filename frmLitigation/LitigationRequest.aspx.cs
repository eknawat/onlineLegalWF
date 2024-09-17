using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using onlineLegalWF.Class;
using System.Configuration;
using System.Globalization;
using static onlineLegalWF.Class.ReplaceLitigation;

namespace onlineLegalWF.frmLitigation
{
    public partial class LitigationRequest : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        public ReplaceLitigation zreplacelitigation = new ReplaceLitigation();

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setData();
            }
        }

        private void setData()
        {
            ucHeader1.setHeader("Litigation Request");
            string xreq_no = System.DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            req_no.Text = xreq_no;

            string pid = zwf.iniPID("LEGALWF");
            lblPID.Text = pid;
            hid_PID.Value = pid;
            ucAttachment1.ini_object(pid);
            ucCommentlog1.ini_object(pid);

            type_req.DataSource = GetTypeOfRequest();
            type_req.DataBind();
            type_req.DataTextField = "tof_litigationreq_desc";
            type_req.DataValueField = "tof_litigationreq_code";
            type_req.DataBind();

            ddl_bu.DataSource = GetBusinessUnit();
            ddl_bu.DataBind();
            ddl_bu.DataTextField = "bu_desc";
            ddl_bu.DataValueField = "bu_code";
            ddl_bu.DataBind();

            company.Text = GetCompanyNameByBuCode(ddl_bu.SelectedValue.ToString());
        }
        public DataTable GetBusinessUnit()
        {
            string sql = "select * from li_business_unit where isactive=1 order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
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
            company.Text = GetCompanyNameByBuCode(ddl_bu.SelectedValue.ToString());
        }
        public DataTable GetTypeOfRequest()
        {
            string sql = "select * from li_type_of_litigationrequest order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }
        protected void type_req_Changed(object sender, EventArgs e)
        {
            if (type_req.SelectedValue.ToString() == "01")
            {
                row_tp_download.Visible = true;
                row_tp_upload.Visible = true;
                row_gv_data.Visible = true;
                pro_occ_section.Visible = false;
                section_bu.Visible = false;
                section_company.Visible = false;

            }
            else
            {
                row_tp_download.Visible = true;
                row_tp_upload.Visible = true;
                row_gv_data.Visible = true;
                pro_occ_section.Visible = true;
                section_bu.Visible = true;
                section_company.Visible = true;
            }
        }
        private int SaveRequest()
        {
            int ret = 0;

            if (doc_no.Text.Trim() == "")
            {
                doc_no.Text = zwf.genDocNo("LIT-" + System.DateTime.Now.ToString("yyyy", new CultureInfo("en-US")) + "-", 4);
            }

            var xreq_no = req_no.Text.Trim();
            var xreq_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var xprocess_id = hid_PID.Value.ToString();
            var xdoc_no = doc_no.Text.Trim();
            var xtype_req = type_req.SelectedValue.ToString();
            var xsubject = subject.Text.Trim().Replace("'", "’");
            var xdesc = desc.Text.Trim();
            var xstatus = "verify";
            var xbucode = ddl_bu.SelectedValue.ToString();
            var xcompany_name = company.Text.Trim();
            var xpro_occ_desc = pro_occ_desc.Text.Trim().Replace("'", "’");

            string sqlLit = @"select * from li_litigation_request where req_no = '"+ xreq_no + "'";
            var resLit = zdb.ExecSql_DataTable(sqlLit, zconnstr);

            //check data from li_litigation_request if has data Update else insert data
            if (resLit.Rows.Count > 0)
            {
                string sqlUpdate = "";
                if (xtype_req == "01")
                {
                    sqlUpdate = @"UPDATE [li_litigation_request]
                                       SET [lit_subject] = '" + xsubject + @"'
                                          ,[lit_desc] = '" + xdesc + @"'
                                          ,[tof_litigationreq_code] = '" + xtype_req + @"'
                                          ,[bu_code] = NULL
                                          ,[company_name] = NULL
                                          ,[updated_datetime] = '" + xreq_date + @"'
                                     WHERE [req_no] = '" + xreq_no + "'";
                }
                else 
                {
                    sqlUpdate = @"UPDATE [li_litigation_request]
                                       SET [lit_subject] = '" + xsubject + @"'
                                          ,[lit_desc] = '" + xdesc + @"'
                                          ,[tof_litigationreq_code] = '" + xtype_req + @"'
                                          ,[bu_code] = '" + xbucode + @"'
                                          ,[company_name] = '" + xcompany_name + @"'
                                          ,[updated_datetime] = '" + xreq_date + @"'
                                          ,[pro_occ_desc] = '" + xpro_occ_desc + @"'
                                     WHERE [req_no] = '" + xreq_no + "'";
                }
                
                ret = zdb.ExecNonQueryReturnID(sqlUpdate, zconnstr);
            }
            else 
            {
                string sqlInsert = "";
                if (xtype_req == "01")
                {
                    sqlInsert = @"INSERT INTO [dbo].[li_litigation_request]
                                           ([process_id],[req_no],[document_no],[req_date],[lit_subject],[lit_desc],[tof_litigationreq_code],[status])
                                     VALUES
                                           ('" + xprocess_id + @"'
                                           ,'" + xreq_no + @"'
                                           ,'" + xdoc_no + @"'
                                           ,'" + xreq_date + @"'
                                           ,'" + xsubject + @"'
                                           ,'" + xdesc + @"'
                                           ,'" + xtype_req + @"'
                                           ,'" + xstatus + "')";
                }
                else 
                {
                    sqlInsert = @"INSERT INTO [dbo].[li_litigation_request]
                                           ([process_id],[req_no],[document_no],[req_date],[lit_subject],[lit_desc],[tof_litigationreq_code],[status],[pro_occ_desc],[bu_code],[company_name])
                                     VALUES
                                           ('" + xprocess_id + @"'
                                           ,'" + xreq_no + @"'
                                           ,'" + xdoc_no + @"'
                                           ,'" + xreq_date + @"'
                                           ,'" + xsubject + @"'
                                           ,'" + xdesc + @"'
                                           ,'" + xtype_req + @"'
                                           ,'" + xstatus + @"'
                                           ,'" + xpro_occ_desc + @"'
                                           ,'" + xbucode + @"'
                                           ,'" + xcompany_name + "')";
                }
                
                ret = zdb.ExecNonQueryReturnID(sqlInsert, zconnstr);
            }

            if (ret > 0) 
            {
                //delete li_litigation_req_case by req_id and loopinsert from gridview data
                string sqlDelCase = @"delete from [li_litigation_req_case] where [req_no] = '" + xreq_no + "'";
                zdb.ExecNonQuery(sqlDelCase, zconnstr);

                if (ret > 0)
                {
                    //get data from gridview
                    List<LitigationCivilCaseData> listCivilCaseData = new List<LitigationCivilCaseData>();
                    foreach (GridViewRow row in gvExcelFile.Rows)
                    {
                        LitigationCivilCaseData civilCaseData = new LitigationCivilCaseData();
                        civilCaseData.req_no = (row.FindControl("gv_req_no") as HiddenField).Value.ToString();
                        civilCaseData.case_no = (row.FindControl("gv_case_no") as HiddenField).Value.ToString();
                        civilCaseData.no = (row.FindControl("gv_no") as Label).Text.ToString();
                        civilCaseData.contract_no = (row.FindControl("gv_contract_no") as Label).Text.ToString();
                        civilCaseData.bu_name = (row.FindControl("gv_bu_name") as Label).Text.ToString();
                        civilCaseData.customer_no = (row.FindControl("gv_customer_no") as Label).Text.ToString();
                        civilCaseData.customer_name = (row.FindControl("gv_customer_name") as Label).Text.ToString();
                        civilCaseData.customer_room = (row.FindControl("gv_customer_room") as Label).Text.ToString();
                        civilCaseData.overdue_desc = (row.FindControl("gv_overdue_desc") as Label).Text.ToString();
                        civilCaseData.outstanding_debt = (row.FindControl("gv_outstanding_debt") as Label).Text.ToString();
                        civilCaseData.outstanding_debt_ack_of_debt = (row.FindControl("gv_outstanding_debt_ack_of_debt") as Label).Text.ToString();
                        civilCaseData.fine_debt = (row.FindControl("gv_fine_debt") as Label).Text.ToString();
                        civilCaseData.total_net = (row.FindControl("gv_total_net") as Label).Text.ToString();
                        civilCaseData.retention_money = (row.FindControl("gv_retention_money") as Label).Text.ToString();
                        civilCaseData.total_after_retention_money = (row.FindControl("gv_total_after_retention_money") as Label).Text.ToString();
                        civilCaseData.remark = (row.FindControl("gv_remark") as Label).Text.ToString();
                        civilCaseData.status = xstatus;
                        listCivilCaseData.Add(civilCaseData);
                    }

                    //check length > 0 insert
                    if (listCivilCaseData.Count > 0)
                    {
                        foreach (var item in listCivilCaseData)
                        {
                            string sqlCaseReq = @"INSERT INTO [li_litigation_req_case]
                                                           ([req_no],[case_no],[no],[contract_no],[bu_name],[customer_no],[customer_name],[customer_room]
                                                           ,[overdue_desc],[outstanding_debt],[outstanding_debt_ack_of_debt],[fine_debt],[total_net],[retention_money],[total_after_retention_money],[remark])
                                                     VALUES
                                                           ('" + item.req_no + @"'
                                                           ,'" + item.case_no + @"'
                                                           ,'" + item.no + @"'
                                                           ,'" + item.contract_no + @"'
                                                           ,'" + item.bu_name + @"'
                                                           ,'" + item.customer_no + @"'
                                                           ,'" + item.customer_name + @"'
                                                           ,'" + item.customer_room + @"'
                                                           ,'" + item.overdue_desc + @"'
                                                           ,'" + item.outstanding_debt + @"'
                                                           ,'" + item.outstanding_debt_ack_of_debt + @"'
                                                           ,'" + item.fine_debt + @"'
                                                           ,'" + item.total_net + @"'
                                                           ,'" + item.retention_money + @"'
                                                           ,'" + item.total_after_retention_money + @"'
                                                           ,'" + item.remark + "')";
                            ret = zdb.ExecNonQueryReturnID(sqlCaseReq, zconnstr);
                        }
                    }
                }
            }

            return ret;
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            int res = SaveRequest();

            if (res > 0)
            {
                // wf save draft
                string process_code = "";
                var xtype_req = type_req.SelectedValue;
                string xbu_code = "";
                if (xtype_req == "01")
                {
                    process_code = "LIT";
                }
                else 
                {
                    process_code = "LIT_2";
                    xbu_code = ddl_bu.SelectedValue.ToString();

                }
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
                    wfAttr.wf_status = "SAVE";
                    wfAttr.submit_answer = "SAVE";
                    wfAttr.submit_by = emp.user_login;
                    wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, lblPID.Text, xbu_code);

                    // wf.updateProcess
                    var wfA_NextStep = zwf.updateProcess(wfAttr);

                }
                Response.Write("<script>alert('Successfully added');</script>");
                //Response.Redirect("frmInsurance/InsuranceRenewRequestList");
                var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                Response.Redirect(host_url + "frmLitigation/LitigationRequestEdit.aspx?id=" + req_no.Text.Trim());
            }
            else
            {
                Response.Write("<script>alert('Error !!!');</script>");
            }
        }

        protected void btn_gendocumnt_Click(object sender, EventArgs e)
        {
            GenDocumnet();
        }

        private void GenDocumnet()
        {
            // Replace Doc
            var xtype_req = type_req.SelectedValue;
            var xdoc_no = doc_no.Text.Trim();
            var xprocess_id = hid_PID.Value.ToString();
            var xreq_date = System.DateTime.Now;
            var xsubject = subject.Text.Trim();
            var xdesc = desc.Text.Trim();
            var xpro_occ_desc = pro_occ_desc.Text.Trim();
            var xbu_code = ddl_bu.SelectedValue.ToString();

            var path_template = ConfigurationManager.AppSettings["WT_Template_litigation"].ToString();
            string templatefile = "";
            ReplaceLitigation_TagData data = new ReplaceLitigation_TagData();
            if (xtype_req == "01")
            {
                templatefile = path_template + @"\LitigationTemplate.docx";

                #region gentagstr data form
                data.docno = xdoc_no.Replace(",", "!comma");
                data.subject = xsubject.Replace(",", "!comma");
                data.desc = xdesc.Replace(",", "!comma");
                data.reqdate = Utillity.ConvertDateToLongDateTime(xreq_date, "en");
                data.to = "คุณอร่าม รัตนโชติ Head of Litigation and Registration";

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
                    var empSupervisor = empFunc.getEmpInfo("sarawut.l");
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
                #endregion
            }
            else
            {
                templatefile = path_template + @"\LitigationTemplate2.docx";

                #region gentagstr data form
                data.docno = xdoc_no.Replace(",", "!comma");
                data.subject = xsubject.Replace(",", "!comma");
                data.desc = xdesc.Replace(",", "!comma");
                data.pro_occ_desc = xpro_occ_desc.Replace(",", "!comma");
                data.reqdate = Utillity.ConvertDateToLongDateTime(xreq_date, "en");
                data.to = "คุณอร่าม รัตนโชติ Head of Litigation and Registration";

                var requestor = "";
                var requestorpos = "";
                var supervisor = "";
                var supervisorpos = "";

                ///get gm heam_am c_level
                string sqlbu = @"select * from li_business_unit where bu_code = '" + xbu_code + "'";
                var res = zdb.ExecSql_DataTable(sqlbu, zconnstr);

                if (res.Rows.Count > 0) 
                {
                    // check session_user
                    if (Session["user_login"] != null)
                    {
                        var xlogin_name = Session["user_login"].ToString();
                        var empFunc = new EmpInfo();

                        string xgm = res.Rows[0]["gm"].ToString();
                        string xam = res.Rows[0]["am"].ToString();
                        string xhead_am = res.Rows[0]["head_am"].ToString();
                        string xexternal_domain = res.Rows[0]["external_domain"].ToString();

                        if (xexternal_domain == "Y")
                        {
                            //get data am user
                            if (!string.IsNullOrEmpty(xam))
                            {
                                var empam = empFunc.getEmpInfo(xam);
                                if (empam.user_login != null)
                                {
                                    requestor = empam.full_name_en;
                                    requestorpos = empam.position_en;
                                }
                            }
                            //get data head am user
                            if (!string.IsNullOrEmpty(xhead_am))
                            {
                                var empheadam = empFunc.getEmpInfo(xhead_am);
                                if (empheadam.user_login != null)
                                {
                                    supervisor = empheadam.full_name_en;
                                    supervisorpos = empheadam.position_en;
                                }
                            }
                        }
                        else 
                        {
                            //get data user
                            var emp = empFunc.getEmpInfo(xlogin_name);
                            if (!string.IsNullOrEmpty(emp.full_name_en))
                            {
                                requestor = emp.full_name_en;
                                requestorpos = emp.position_en;
                            }

                            //get data gm user
                            if (!string.IsNullOrEmpty(xgm))
                            {
                                var empgm = empFunc.getEmpInfo(xgm);
                                if (empgm.user_login != null)
                                {
                                    supervisor = empgm.full_name_en;
                                    supervisorpos = empgm.position_en;
                                }
                            }
                        }
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
                #endregion
            }
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\litigation_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            DataTable dtStr = zreplacelitigation.genTagData(data);

            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);

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

        protected void btnUploadExcel_Click(object sender, EventArgs e)
        {
            //Coneection String by default empty  
            string ConStr = "";

            if (FileUploadExcel.HasFile) 
            {
                //Extantion of the file upload control saving into ext because   
                //there are two types of extation .xls and .xlsx of Excel   
                string ext = Path.GetExtension(FileUploadExcel.FileName).ToLower();
                //getting the path of the file   
                string path = Server.MapPath("~/Temp/" + FileUploadExcel.FileName);
                //saving the file inside the Temp of the server  
                FileUploadExcel.SaveAs(path);
                Label1.Text = FileUploadExcel.FileName + "\'s Data showing into the GridView";
                //checking that extantion is .xls or .xlsx  
                if (ext.Trim() == ".xls")
                {
                    //connection string for that file which extantion is .xls  
                    ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                else if (ext.Trim() == ".xlsx")
                {
                    //connection string for that file which extantion is .xlsx  
                    ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }
                //making query  
                //string query = "SELECT * FROM [Sheet1$]";
                string query = "";
                //Providing connection  
                OleDbConnection conn = new OleDbConnection(ConStr);
                //checking that connection state is closed or not if closed the   
                //open the connection  
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                //get sheetname
                DataTable dtExcelSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string getExcelSheetName = dtExcelSheetName.Rows[0]["Table_Name"].ToString();
                query = "SELECT * FROM [" + getExcelSheetName + "]";

                //create command object  
                OleDbCommand cmd = new OleDbCommand(query, conn);
                // create a data adapter and get the data into dataadapter  
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                //fill the Excel data to data set  
                da.Fill(ds);

                DataTable dt = ds.Tables[0];

                List<LitigationCivilCaseData> listCivilCaseData = new List<LitigationCivilCaseData>();
                if (dt.Rows.Count > 0) 
                {
                    string xreq_no = req_no.Text.Trim();
                    string xcase_no = "";
                    foreach (DataRow dr in dt.Rows) 
                    {
                        xcase_no = System.DateTime.Now.ToString("yyyyMMdd_HHmmss_ffffff");
                        LitigationCivilCaseData civilCaseData = new LitigationCivilCaseData();
                        civilCaseData.req_no = xreq_no;
                        civilCaseData.case_no = xcase_no+"_"+ dr["ลำดับ"].ToString();
                        civilCaseData.no = dr["ลำดับ"].ToString();
                        civilCaseData.contract_no = dr["เลขที่สัญญา"].ToString();
                        civilCaseData.bu_name = dr["Bu"].ToString();
                        civilCaseData.customer_no = dr["รหัสลูกค้า"].ToString();
                        civilCaseData.customer_name = dr["ชื่อ"].ToString();
                        civilCaseData.customer_room = dr["ห้อง"].ToString();
                        civilCaseData.overdue_desc = dr["ช่วงเวลาที่ค้าง"].ToString();
                        civilCaseData.outstanding_debt = dr["หนี้ค้างชำระ"].ToString();
                        civilCaseData.outstanding_debt_ack_of_debt = dr["หนี้ค้างชำระตามรับสภาพหนี้"].ToString();
                        civilCaseData.fine_debt = dr["ค่าเบี้ยปรับ"].ToString();
                        civilCaseData.total_net = dr["ยอดรวมสุทธิ"].ToString();
                        civilCaseData.retention_money = dr["เงินประกัน"].ToString();
                        civilCaseData.total_after_retention_money = dr["ยอดหลังหักเงินประกัน"].ToString();
                        civilCaseData.remark = dr["หมายเหตุ"].ToString();


                        listCivilCaseData.Add(civilCaseData);
                    }
                }
                //set data source of the grid view  
                gvExcelFile.DataSource = listCivilCaseData;
                //binding the gridview  
                gvExcelFile.DataBind();
                //close the connection  
                conn.Close();

                //delete file inside the Temp of the server 
                File.Delete(path);

                //save request
                SaveRequest();
            }
                
        }

        public class LitigationCivilCaseData
        {
            public string req_no { get; set; }
            public string case_no { get; set; }
            public string no { get; set; }
            public string contract_no { get; set; }
            public string bu_name { get; set; }
            public string customer_no { get; set; }
            public string customer_name { get; set; }
            public string customer_room { get; set; }
            public string overdue_desc { get; set; }
            public string outstanding_debt { get; set; }
            public string outstanding_debt_ack_of_debt { get; set; }
            public string fine_debt { get; set; }
            public string total_net { get; set; }
            public string retention_money { get; set; }
            public string total_after_retention_money { get; set; }
            public string remark { get; set; }
            public string status { get; set; }
            public string assto_login { get; set; }
        }

        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "openModal")
            {
                int i = System.Convert.ToInt32(e.CommandArgument);
                //var xreq_no = ((HiddenField)gvExcelFile.Rows[i].FindControl("gv_req_no")).Value;
                var xcase_no = ((HiddenField)gvExcelFile.Rows[i].FindControl("gv_case_no")).Value;
                var xcontract_no = ((Label)gvExcelFile.Rows[i].FindControl("gv_contract_no")).Text;
                var xcustomer_no = ((Label)gvExcelFile.Rows[i].FindControl("gv_customer_no")).Text;
                var xcustomer_name = ((Label)gvExcelFile.Rows[i].FindControl("gv_customer_name")).Text;

                ucLitigationCaseAttachment1.ini_object(xcase_no,xcontract_no,xcustomer_no,xcustomer_name);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalEditData();", true);
            }
        }

        protected void btnDownload_template_Click(object sender, EventArgs e)
        {
            var WT_Template_litigation = ConfigurationManager.AppSettings["WT_Template_litigation"].ToString();
            string filePath = WT_Template_litigation + @"\TemplateLitigation.xlsx";
            var mimeType = MimeMapping.GetMimeMapping(Path.GetFileName(filePath));

            Response.ContentType = mimeType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }
    }
}