using DocumentFormat.OpenXml.Wordprocessing;
using onlineLegalWF.Class;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static onlineLegalWF.Class.ReplaceInsRenewAWC;

namespace onlineLegalWF.frmInsurance
{
    public partial class InsuranceRenewAWC : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        public ReplaceInsRenewAWC zreplaceinsrenewawc = new ReplaceInsRenewAWC();
        public MargePDF zmergepdf = new MargePDF();
        public SendMail zsendmail = new SendMail();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string id = Request.QueryString["id"];

                //if (!string.IsNullOrEmpty(id))
                //{
                //    setData(id);
                //}
                setData();
            }
        }

        //private void setData(string id)
        private void setData()
        {
            ucHeader1.setHeader("AWCRenew Insurance Memo");

            //iniData(id);
            iniData();

            string pid = zwf.iniPID("LEGALWF");
            lblPID.Text = pid;
            hid_PID.Value = pid;
            ucAttachment1.ini_object(pid);
            ucCommentlog1.ini_object(pid);
            hid_reqno.Value = System.DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");

            //bindDataListPropTable(id);

        }

        #region gv1
        public void iniData()
        {
            var dt = iniDataTable();
            gv1.DataSource = dt;
            gv1.DataBind();
        }
        public void iniData(string id)
        {
            var dt = iniDataTable(id);
            gv1.DataSource = dt;
            gv1.DataBind();

        }
        public DataTable iniDataTable()
        {
            //getData
            var dt = iniDTStructure();
            var dr = dt.NewRow();
            dr["TYPE_PROP"] = "Commercial";
            dr["IAR"] = "0";
            dr["BI"] = "0";
            dr["CGL"] = "0";
            dr["PL"] = "0";
            dr["PV"] = "0";
            dr["LPG"] = "0";   
            dr["D_O"] = "0";
            dr["Row_Sort"] = "01";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["TYPE_PROP"] = "Retail&Wholesale";
            dr["IAR"] = "0";
            dr["BI"] = "0";
            dr["CGL"] = "0";
            dr["PL"] = "0";
            dr["PV"] = "0";
            dr["LPG"] = "0";
            dr["D_O"] = "0";
            dr["Row_Sort"] = "02";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["TYPE_PROP"] = "Hospitality";
            dr["IAR"] = "0";
            dr["BI"] = "0";
            dr["CGL"] = "0";
            dr["PL"] = "0";
            dr["PV"] = "0";
            dr["LPG"] = "0";
            dr["D_O"] = "0";
            dr["Row_Sort"] = "03";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["TYPE_PROP"] = "AWC";
            dr["IAR"] = "0";
            dr["BI"] = "0";
            dr["CGL"] = "0";
            dr["PL"] = "0";
            dr["PV"] = "0";
            dr["LPG"] = "0";
            dr["D_O"] = "0";
            dr["Row_Sort"] = "04";
            dt.Rows.Add(dr);

            return dt;
        }
        public DataTable iniDataTable(string id)
        {
            //getData
            var dt = iniDTStructure();
            var dr = dt.NewRow();

            var dt_sum = GetSumInsurance(id);

            if (dt_sum.Rows.Count > 0)
            {
                foreach (DataRow dr_sum in dt_sum.Rows)
                {
                    dr = dt.NewRow();
                    dr["TYPE_PROP"] = dr_sum["TYPE_PROP"].ToString();
                    dr["IAR"] = dr_sum["IAR"].ToString();
                    dr["BI"] = dr_sum["BI"].ToString();
                    dr["CGL"] = dr_sum["CGL"].ToString();
                    dr["PL"] = dr_sum["PL"].ToString();
                    dr["PV"] = dr_sum["PV"].ToString();
                    dr["LPG"] = dr_sum["LPG"].ToString();
                    dr["D_O"] = dr_sum["D_O"].ToString();
                    dr["Row_Sort"] = dr_sum["Row_Sort"].ToString();
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
        public DataTable iniDTStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TYPE_PROP", typeof(string));
            dt.Columns.Add("IAR", typeof(string));
            dt.Columns.Add("BI", typeof(string));
            dt.Columns.Add("CGL", typeof(string));
            dt.Columns.Add("PL", typeof(string));
            dt.Columns.Add("PV", typeof(string));
            dt.Columns.Add("LPG", typeof(string));
            dt.Columns.Add("D_O", typeof(string));
            dt.Columns.Add("Row_Sort", typeof(string));
            return dt;
        }

        public DataTable GetSumInsurance(string id)
        {
            string sql = @"SELECT * FROM 
                                (
                                VALUES ('Commercial',
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as IAR from li_insurance_req_property_insured where top_ins_code = '01' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '01' and '06' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as BI from li_insurance_req_property_insured where top_ins_code = '02' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '01' and '06' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as CGL from li_insurance_req_property_insured where top_ins_code = '03' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '01' and '06' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as PL from li_insurance_req_property_insured where top_ins_code = '04' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '01' and '06' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as PV from li_insurance_req_property_insured where top_ins_code = '05' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '01' and '06' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as LPG from li_insurance_req_property_insured where top_ins_code = '06' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '01' and '06' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as D_O from li_insurance_req_property_insured where top_ins_code = '07' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '01' and '06' and req_no in (" + id + @"))),'01'
	                                    ),('Retail&Wholesale',
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as IAR from li_insurance_req_property_insured where top_ins_code = '01' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '07' and '14' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as BI from li_insurance_req_property_insured where top_ins_code = '02' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '07' and '14' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as CGL from li_insurance_req_property_insured where top_ins_code = '03' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '07' and '14' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as PL from li_insurance_req_property_insured where top_ins_code = '04' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '07' and '14' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as PV from li_insurance_req_property_insured where top_ins_code = '05' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '07' and '14' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as LPG from li_insurance_req_property_insured where top_ins_code = '06' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '07' and '14' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as D_O from li_insurance_req_property_insured where top_ins_code = '07' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '07' and '14' and req_no in (" + id + @"))),'02'
	                                    ),('Hospitality',
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as IAR from li_insurance_req_property_insured where top_ins_code = '01' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '15' and '46' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as BI from li_insurance_req_property_insured where top_ins_code = '02' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '15' and '46' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as CGL from li_insurance_req_property_insured where top_ins_code = '03' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '15' and '46' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as PL from li_insurance_req_property_insured where top_ins_code = '04' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '15' and '46' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as PV from li_insurance_req_property_insured where top_ins_code = '05' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '15' and '46' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as LPG from li_insurance_req_property_insured where top_ins_code = '06' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '15' and '46' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as D_O from li_insurance_req_property_insured where top_ins_code = '07' and req_no in (select req_no from li_insurance_request where toreq_code='07' and bu_code between '15' and '46' and req_no in (" + id + @"))),'03'
	                                    ),('AWC',
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as IAR from li_insurance_req_property_insured where top_ins_code = '01' and req_no in (select req_no from li_insurance_request where toreq_code='07' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as BI from li_insurance_req_property_insured where top_ins_code = '02' and req_no in (select req_no from li_insurance_request where toreq_code='07' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as CGL from li_insurance_req_property_insured where top_ins_code = '03' and req_no in (select req_no from li_insurance_request where toreq_code='07' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as PL from li_insurance_req_property_insured where top_ins_code = '04' and req_no in (select req_no from li_insurance_request where toreq_code='07' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as PV from li_insurance_req_property_insured where top_ins_code = '05' and req_no in (select req_no from li_insurance_request where toreq_code='07' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as LPG from li_insurance_req_property_insured where top_ins_code = '06' and req_no in (select req_no from li_insurance_request where toreq_code='07' and req_no in (" + id + @"))),
		                                (select format(isnull(sum(cast(suminsured as int)),0), '##,##0') as D_O from li_insurance_req_property_insured where top_ins_code = '07' and req_no in (select req_no from li_insurance_request where toreq_code='07' and req_no in (" + id + @"))),'04'
	                                    )
                                )sum_renew_Insurance(TYPE_PROP,IAR,BI,CGL,PL,PV,LPG,D_O,Row_Sort)";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }
        #endregion

        protected void btn_save_Click(object sender, EventArgs e)
        {
            int res = SaveRenewRequest();

            if (res > 0)
            {
                // wf save draft
                string process_code = "INR_AWC_RENEW";
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
                    wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by,lblPID.Text);

                    // wf.updateProcess
                    var wfA_NextStep = zwf.updateProcess(wfAttr);

                }
                Response.Write("<script>alert('Successfully added');</script>");
                var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                Response.Redirect(host_url + "frmInsurance/InsuranceRenewAWCEdit.aspx?id=" + lblPID.Text.Trim());
            }
            else
            {
                Response.Write("<script>alert('Error !!!');</script>");
            }

        }

        private int SaveRenewRequest()
        {
            int ret = 0;

            if (doc_no.Text.Trim() == "")
            {
                doc_no.Text = zwf.genDocNo("MEMO-" + System.DateTime.Now.ToString("yyyy", new CultureInfo("en-US")) + "-", 5);
            }
            var xreq_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //var xprocess_id = string.Format("{0:000000}", (GetMaxProcessID() + 1));
            var xreq_no = hid_reqno.Value.Trim();
            var xprocess_id = hid_PID.Value.ToString();
            var xfrom = company_name.Text.ToString();
            var xdoc_no = doc_no.Text.Trim();
            var xsubject = subject.Text.Trim().Replace("'", "’");
            var xto = to.Text.Trim();
            var xdescription = description.Text.Trim().Replace("'", "’");


            //Get Data from gv1 Sum Insurance Detail
            List<SummaryInsurance> listInsuranceSumData = new List<SummaryInsurance>();
            foreach (GridViewRow row in gv1.Rows)
            {
                SummaryInsurance data = new SummaryInsurance();
                data.TYPE_PROP = (row.FindControl("gv1txtTYPE_PROP") as TextBox).Text;
                data.IAR = (row.FindControl("gv1txtIAR") as TextBox).Text;
                data.BI = (row.FindControl("gv1txtBI") as TextBox).Text;
                data.CGL = (row.FindControl("gv1txtCGL") as TextBox).Text;
                data.PL = (row.FindControl("gv1txtPL") as TextBox).Text;
                data.PV = (row.FindControl("gv1txtPV") as TextBox).Text;
                data.LPG = (row.FindControl("gv1txtLPG") as TextBox).Text;
                data.D_O = (row.FindControl("gv1txtD_O") as TextBox).Text;
                data.Row_Sort = (row.FindControl("gv1txtRow_Sort") as HiddenField).Value;

                listInsuranceSumData.Add(data);
            }

            string sqlmemo = @"INSERT INTO [dbo].[li_insurance_renew_awc_memo]
                               ([process_id],[req_no],[req_date],[document_no],[dear],[company_name],[subject],[description])
                         VALUES
                               ('" + xprocess_id+@"'
                               ,'"+ xreq_no + @"'
                               ,'"+xreq_date+@"'
                               ,'"+xdoc_no+@"'
                               ,'"+xto+@"'
                               ,'"+xfrom+@"'
                               ,'"+xsubject+@"'
                               ,'"+xdescription+"')";

            ret = zdb.ExecNonQueryReturnID(sqlmemo, zconnstr);

            if (ret > 0)
            {

                if (listInsuranceSumData.Count > 0)
                {
                    foreach (var item in listInsuranceSumData)
                    {
                        string sqlsum = @"INSERT INTO [dbo].[li_insurance_renew_awc_memo_sumins]
                                           ([process_id],[row_sort],[type_prop],[sum_iar],[sum_bi],[sum_cgl],[sum_pl],[sum_pv],[sum_lpg],[sum_d_o])
                                     VALUES
                                           ('" + xprocess_id+ @"'
                                           ,'" +item.Row_Sort+ @"'
                                           ,'" +item.TYPE_PROP+ @"'
                                           ,'" + float.Parse(item.IAR) + @"'
                                           ,'" + float.Parse(item.BI) + @"'
                                           ,'" + float.Parse(item.CGL) + @"'
                                           ,'" + float.Parse(item.PL) + @"'
                                           ,'" + float.Parse(item.PV) + @"'
                                           ,'" + float.Parse(item.LPG) + @"'
                                           ,'" + float.Parse(item.D_O) + "')";

                        ret = zdb.ExecNonQueryReturnID(sqlsum, zconnstr);

                    }
                }

                //insert memo ref renew_req
                //if (ret > 0)
                //{
                //    string id = Request.QueryString["id"];
                //    if (!string.IsNullOrEmpty(id))
                //    {
                //        var listid = id.Split(',');

                //        foreach (var item_id in listid)
                //        {
                //            string sql = @"INSERT INTO [dbo].[li_insurance_renew_awc_memo_req]
		              //                 ([process_id],[req_no])
                //                 VALUES
                //                       ('" + xprocess_id + @"'
                //                       ," + item_id + ")";
                //            zdb.ExecNonQuery(sql, zconnstr);
                //        }
                //    }

                //    //update status renew ins
                //    string sqlupdate = @"update li_insurance_request set status = 'genmemo' , updated_datetime = '"+xreq_date+"' where req_no in (" + id + ")";
                //    ret = zdb.ExecNonQueryReturnID(sqlupdate, zconnstr);
                //}

            }

            return ret;
        }

        public void bindDataListPropTable(string id) 
        {
            string sqlreqres = "select req.property_insured_name,req.req_no,req.req_date,req.[status],bu.bu_desc from li_insurance_request as req inner join li_business_unit as bu on bu.bu_code = req.bu_code where req.toreq_code='07' and req_no in (" + id+ ") order by req.bu_code desc";

            var reqres = zdb.ExecSql_DataTable(sqlreqres, zconnstr);

            if (reqres.Rows.Count > 0)
            {
                List<InsuranceRequestResponse> listRequestResponse = new List<InsuranceRequestResponse>();

                int no = 1;
                foreach (DataRow drReq in reqres.Rows)
                {
                    InsuranceRequestResponse requestResponse = new InsuranceRequestResponse();
                    requestResponse.No = no.ToString();
                    requestResponse.PropertyInsured = drReq["property_insured_name"].ToString();

                    string sqlreqinsres = "select reqpropins.req_no,format(isnull(cast(reqpropins.suminsured as int),0), '##,##0') as suminsured,tofins.top_ins_code,top_ins_desc from [dbo].[li_insurance_req_property_insured] as reqpropins inner join li_type_of_property_insured as tofins on tofins.top_ins_code = reqpropins.top_ins_code where req_no='" + drReq["req_no"].ToString() + "'";

                    var reqinsres = zdb.ExecSql_DataTable(sqlreqinsres, zconnstr);

                    if (reqinsres.Rows.Count > 0)
                    {
                        foreach (DataRow drReqIns in reqinsres.Rows)
                        {
                            var topInsCode = drReqIns["top_ins_code"].ToString();
                            if (topInsCode == "01")
                            {
                                requestResponse.IARSumInsured = drReqIns["suminsured"].ToString();
                            }
                            else if (topInsCode == "02")
                            {
                                requestResponse.BISumInsured = drReqIns["suminsured"].ToString();
                            }
                            else if (topInsCode == "03")
                            {
                                requestResponse.CGLSumInsured = drReqIns["suminsured"].ToString();
                            }
                            else if (topInsCode == "04")
                            {
                                requestResponse.PLSumInsured = drReqIns["suminsured"].ToString();
                            }
                            else if (topInsCode == "05")
                            {
                                requestResponse.PVSumInsured = drReqIns["suminsured"].ToString();
                            }
                            else if (topInsCode == "06")
                            {
                                requestResponse.LPGSumInsured = drReqIns["suminsured"].ToString();
                            }
                            else if (topInsCode == "07")
                            {
                                requestResponse.DOSumInsured = drReqIns["suminsured"].ToString();
                            }
                        }
                    }

                    listRequestResponse.Add(requestResponse);

                    no++;
                }

                //gvList.DataSource = listRequestResponse;
                //gvList.DataBind();
            }
        }

        private void GenDocumnet()
        {
            // Replace Doc
            var xreq_date = System.DateTime.Now;
            var xreq_no = hid_reqno.Value;
            var xcompany_name = company_name.Text.ToString();
            var xdoc_no = doc_no.Text.Trim();
            var xsubject = subject.Text.Trim();
            var xto = to.Text.Trim();
            var xdescription = description.Text.Trim();

            var path_template = ConfigurationManager.AppSettings["WT_Template_insurance"].ToString();

            string templatefile = path_template + @"\InsuranceTemplateRenewAWC.docx";
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\inreq_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            ReplaceInsReNewAWC_TagData data = new ReplaceInsReNewAWC_TagData();

            #region prepare data
            data.docno = xdoc_no.Replace(",", "!comma");
            data.to = xto.Replace(",", "!comma");
            data.company = xcompany_name.Replace(",", "!comma");
            data.subject = xsubject.Replace(",", "!comma");
            data.reqdate = Utillity.ConvertDateToLongDateTime(xreq_date, "en");
            data.description = xdescription.Replace(",", "!comma");

            var requestor = "";
            var requestorpos = "";

            var apv1 = "คุณจรูญศักดิ์ นามะฮง";
            var apv1pos = "Insurance Specialist";
            var apv1_2 = "คุณวารินทร์ เกลียวไพศาล";
            var apv2 = "คุณชโลทร ศรีสมวงษ์";
            var apv2pos = "Head of Legal";
            var apv3 = "คุณชยุต อมตวนิช";
            var apv3pos = "Head of Risk Management";

            var apv4 = "ดร.สิเวศ โรจนสุนทร";
            var apv4pos = "CCO";

            data.sign_name1 = "";
            data.name1 = requestor;
            data.position1 = requestorpos;
            data.date1 = "";

            data.sign_name2 = "";
            data.name2 = apv1;
            data.position2 = apv1pos;
            data.date2 = "";

            data.sign_name22 = "";
            data.name22 = apv1_2;
            data.date22 = "";

            data.sign_name3 = "";
            data.name3 = apv2;
            data.position3 = apv2pos;
            data.date3 = "";

            data.sign_name4 = "";
            data.name4 = apv3;
            data.position4 = apv3pos;
            data.date4 = "";

            data.sign_name5 = "";
            data.name5 = apv4;
            data.position5 = apv4pos;
            data.date5 = "";
            data.cb1 = "";
            data.cb2 = "";
            data.remark5 = "";

            DataTable dtStr = zreplaceinsrenewawc.genTagData(data);

            #endregion 

            #region Sample ReplaceTable
            ////DataTable Column Properties
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
            // Replace #tablesum# ------------------------------------------------------
            DataRow dr = dtProperties1.NewRow();
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "กลุ่มธุรกิจ";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "IAR";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "BI";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "CGL($)";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "PL";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "PV";
            dr["col_width"] = "150";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "LPG";
            dr["col_width"] = "150";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "D&O";
            dr["col_width"] = "150";
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

            ////  DataTable dt = new DataTable();
            DataTable dt = new DataTable();
            dt.Columns.Add("tagname", typeof(string));
            dt.Columns.Add("TYPE_PROP", typeof(string));
            dt.Columns.Add("IAR", typeof(string));
            dt.Columns.Add("BI", typeof(string));
            dt.Columns.Add("CGL", typeof(string));
            dt.Columns.Add("PL", typeof(string));
            dt.Columns.Add("PV", typeof(string));
            dt.Columns.Add("LPG", typeof(string));
            dt.Columns.Add("D_O", typeof(string));
            dt.Columns.Add("Row_Sort", typeof(string));

            ////DataTable for #tablesum#
            //Get Data from gv1 Insurance sum Detail
            List<SummaryInsurance> listInsuranceSum = new List<SummaryInsurance>();
            foreach (GridViewRow row in gv1.Rows)
            {
                SummaryInsurance datasum = new SummaryInsurance();
                datasum.TYPE_PROP = (row.FindControl("gv1txtTYPE_PROP") as TextBox).Text;
                datasum.IAR = (row.FindControl("gv1txtIAR") as TextBox).Text;
                datasum.BI = (row.FindControl("gv1txtBI") as TextBox).Text;
                datasum.CGL = (row.FindControl("gv1txtCGL") as TextBox).Text;
                datasum.PL = (row.FindControl("gv1txtPL") as TextBox).Text;
                datasum.PV = (row.FindControl("gv1txtPV") as TextBox).Text;
                datasum.LPG = (row.FindControl("gv1txtLPG") as TextBox).Text;
                datasum.D_O = (row.FindControl("gv1txtD_O") as TextBox).Text;
                datasum.Row_Sort = (row.FindControl("gv1txtRow_Sort") as HiddenField).Value;

                listInsuranceSum.Add(datasum);

            }
            ////Assign Data From gv1
            var drGV = dt.NewRow();

            if (listInsuranceSum.Count > 0)
            {
                foreach (var item in listInsuranceSum)
                {
                    drGV = dt.NewRow();
                    drGV["tagname"] = "#tablesum#";
                    drGV["TYPE_PROP"] = item.TYPE_PROP.Replace(",", "!comma");
                    drGV["IAR"] = item.IAR.Replace(",", "!comma");
                    drGV["BI"] = item.BI.Replace(",", "!comma");
                    drGV["CGL"] = item.CGL.Replace(",", "!comma");
                    drGV["PL"] = item.PL.Replace(",", "!comma");
                    drGV["PV"] = item.PV.Replace(",", "!comma");
                    drGV["LPG"] = item.LPG.Replace(",", "!comma");
                    drGV["D_O"] = item.LPG.Replace(",", "!comma");
                    drGV["Row_Sort"] = item.Row_Sort.Replace(",", "!comma");
                    dt.Rows.Add(drGV);
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
            var jsonDTdata = repl.DataTableToJSONWithStringBuilder(dt);
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

        public class InsuranceRequestResponse
        {
            public string No { get; set; }
            public string PropertyInsured { get; set; }
            public string IARSumInsured { get; set; }
            public string BISumInsured { get; set; }
            public string CGLSumInsured { get; set; }
            public string PLSumInsured { get; set; }
            public string PVSumInsured { get; set; }
            public string LPGSumInsured { get; set; }
            public string DOSumInsured { get; set; }
        }

        public class SummaryInsurance
        {
            public string TYPE_PROP { get; set; }
            public string IAR { get; set; }
            public string BI { get; set; }
            public string CGL { get; set; }
            public string PL { get; set; }
            public string PV { get; set; }
            public string LPG { get; set; }
            public string D_O { get; set; }
            public string Row_Sort { get; set; }
        }

        protected void btn_gendocumnt_Click(object sender, EventArgs e)
        {
            GenDocumnet();
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string sqlreq = "select * from li_insurance_renew_awc_memo where req_no='" + hid_reqno.Value + "'";

            var res = zdb.ExecSql_DataTable(sqlreq, zconnstr);

            if (res.Rows.Count == 0)
            {
                SaveRenewRequest();
            }

            string process_code = "INR_AWC_RENEW";
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
                wfAttr.wf_status = "SUBMITTED";
                wfAttr.submit_answer = "SUBMITTED";
                wfAttr.submit_by = emp.user_login;
                wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by);
                wfAttr.updated_by = emp.user_login;

                // wf.updateProcess Start
                var wfA_NextStep = zwf.updateProcess(wfAttr);
                wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by);
                string status = zwf.Insert_NextStep(wfA_NextStep);

                // wf.updateProcess Insurance Specialist Approve
                wfA_NextStep.subject = subject.Text.Trim();
                wfA_NextStep.wf_status = wfA_NextStep.step_name + " Approved";
                wfA_NextStep.submit_answer = "APPROVED";
                wfA_NextStep.submit_by = emp.user_login;
                wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by);
                wfA_NextStep.updated_by = emp.user_login;

                // wf.updateProcess Next Step
                var wfA_NextStep2 = zwf.updateProcess(wfA_NextStep);
                wfA_NextStep2.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep2.process_code, wfA_NextStep2.step_name, emp.user_login, wfAttr.submit_by);
                status = zwf.Insert_NextStep(wfA_NextStep2);

                if (status == "Success")
                {
                    GenDocumnetInsRenewAWC(lblPID.Text);
                    //send mail
                    string subject = "";
                    string body = "";
                    string sqlmail = @"select * from li_insurance_renew_awc_memo where process_id = '" + wfAttr.process_id + "'";
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

                            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                            Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);

                        }

                    }
                    else 
                    {
                        Response.Write("<script>alert('Error !!!');</script>");
                    }
                    
                }

            }
        }

        private void GenDocumnetInsRenewAWC(string pid)
        {
            // Replace Doc
            string xreq_no = "";

            var path_template = ConfigurationManager.AppSettings["WT_Template_insurance"].ToString();
            string templatefile = path_template + @"\InsuranceTemplateRenewAWC.docx";
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\inreq_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            ReplaceInsReNewAWC_TagData data = new ReplaceInsReNewAWC_TagData();

            #region prepare data

            string sqlinsmemo = "select * from li_insurance_renew_awc_memo where process_id='" + pid + "'";
            var resinsmemo = zdb.ExecSql_DataTable(sqlinsmemo, zconnstr);

            if (resinsmemo.Rows.Count > 0)
            {
                xreq_no = resinsmemo.Rows[0]["req_no"].ToString();
            }

            var requestor = "";
            var requestorpos = "";

            var apv1 = "คุณจรูญศักดิ์ นามะฮง";
            var apv1pos = "Insurance Specialist";
            var apv1_2 = "คุณวารินทร์ เกลียวไพศาล";
            var apv2 = "คุณชโลทร ศรีสมวงษ์";
            var apv2pos = "Head of Legal";
            var apv3 = "คุณชยุต อมตวนิช";
            var apv3pos = "Head of Risk Management";

            var apv4 = "ดร.สิเวศ โรจนสุนทร";
            var apv4pos = "CCO";

            data.sign_name1 = "";
            data.name1 = requestor;
            data.position1 = requestorpos;
            data.date1 = "";

            data.sign_name2 = "";
            data.name2 = apv1;
            data.position2 = apv1pos;
            data.date2 = "";

            data.sign_name22 = "";
            data.name22 = apv1_2;
            data.date22 = "";

            data.sign_name3 = "";
            data.name3 = apv2;
            data.position3 = apv2pos;
            data.date3 = "";

            data.sign_name4 = "";
            data.name4 = apv3;
            data.position4 = apv3pos;
            data.date4 = "";

            data.sign_name5 = "";
            data.name5 = apv4;
            data.position5 = apv4pos;
            data.date5 = "";
            data.cb1 = "";
            data.cb2 = "";
            data.remark5 = "";

            DataTable dtStr = zreplaceinsrenewawc.BindTagData(pid, data);

            #endregion 

            #region Sample ReplaceTable
            ////DataTable Column Properties
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
            // Replace #tablesum# ------------------------------------------------------
            DataRow dr = dtProperties1.NewRow();
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "กลุ่มธุรกิจ";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "IAR";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "BI";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "CGL($)";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "PL";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "PV";
            dr["col_width"] = "150";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "LPG";
            dr["col_width"] = "150";
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
            dr["tagname"] = "#tablesum#";
            dr["col_name"] = "D&O";
            dr["col_width"] = "150";
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

            ////DataTable for #tablesum#
            //Get Data from gv1 Insurance sum Detail
            DataTable dt = zreplaceinsrenewawc.genTagTableData(lblPID.Text);

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
            var jsonDTdata = repl.DataTableToJSONWithStringBuilder(dt);
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


            #endregion
        }

        protected void btn_export_doc_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];

            if (!string.IsNullOrEmpty(id))
            {
                string sql = "select * from li_insurance_request where req_no in (" + id + ") order by bu_code desc";
                DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);

                if (dt.Rows.Count > 0) 
                {
                    List<string> listpdf = new List<string>();
                    string[] pdfFiles = new string[] { };
                    string pathfileins = "";
                    string outputdirectory = "";

                    foreach (DataRow dr in dt.Rows) 
                    {
                        string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + dr["req_no"].ToString() + "' order by row_id desc";

                        var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                        if (resfile.Rows.Count > 0)
                        {
                            pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");
                            outputdirectory = resfile.Rows[0]["output_directory"].ToString();

                            
                            listpdf.Add(pathfileins);

                            string sqlattachfile = "select * from wf_attachment where pid = '" + dr["process_id"].ToString() + "' and e_form IS NULL";

                            var resattachfile = zdb.ExecSql_DataTable(sqlattachfile, zconnstr);

                            if (resattachfile.Rows.Count > 0)
                            {
                                foreach (DataRow item in resattachfile.Rows)
                                {
                                    listpdf.Add(item["attached_filepath"].ToString());
                                }
                            }
                            //get list pdf file from tb z_replacedocx_log where replacedocx_reqno
                            pdfFiles = listpdf.ToArray();
                        }
                    }

                    //
                    string filepath = zmergepdf.mergefilePDF(pdfFiles, outputdirectory);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalDoc();", true);
                    var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                    pdf_render.Attributes["src"] = host_url + "render/pdf?id=" + filepath.Replace("+", @"%2B");
                }
            }
        }
    }
}