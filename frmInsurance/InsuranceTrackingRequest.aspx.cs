using onlineLegalWF.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace onlineLegalWF.frmInsurance
{
    public partial class InsuranceTrackingRequest : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
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
            ucHeader1.setHeader("Insurance Tracking");
            // Bind Worklist
            if (Session["user_login"] != null)
            {
                var xlogin_name = Session["user_login"].ToString();

                bindData(xlogin_name, "insTracking");

                bind_gv();

            }
        }

        public void bindData(string xlogin_name, string xmode)
        {
            hidLogin.Value = xlogin_name;
            hidMode.Value = xmode;

        }

        public void bind_gv()
        {
            switch (hidMode.Value)
            {
                case "insTracking":
                    {
                        bind_gv1(getInsTrackingList());
                    }; break;

            }
        }
        protected void gv1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv1.PageIndex = e.NewPageIndex;
            bind_gv();
        }
        public DataTable getInsTrackingList()
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            string sql = @"Select wf.process_code,wf.assto_login,wf.process_id,wf.subject,wf.submit_by,wf.updated_by,wf.created_datetime,wf.updated_datetime,
                            CASE 
                                WHEN wf_status = '' THEN 'IN PROGRESS' 
                                ELSE wf_status
                            END AS wf_status 
                            ,'" + host_url + @"forms/apv.aspx?req='+wf.process_id+'&pc='+process_code+'&mode=tracking' AS link_url_format, " +
                            "CASE " +
                                "WHEN claimreq.document_no IS NOT NULL THEN claimreq.document_no " +
                                "WHEN insreq.document_no IS NOT NULL THEN insreq.document_no " +
                                "WHEN memoreq.document_no IS NOT NULL THEN memoreq.document_no " +
                                "ELSE '' " +
                            "END AS document_no " +
                            "from wf_routing as wf " +
							"left outer join li_insurance_claim as claimreq on claimreq.process_id = wf.process_id " +
                            "left outer join li_insurance_request as insreq on insreq.process_id = wf.process_id "+
                            "left outer join li_insurance_renew_awc_memo as memoreq on memoreq.process_id = wf.process_id "+
							"where wf.process_code in ('INR_AWC_RENEW', 'INR_CLAIM', 'INR_CLAIM_2', 'INR_CLAIM_3', 'INR_NEW') "+
                             "and wf.row_id in (select tb1.row_id from "+
                            "(SELECT process_id,"+
                            "MAX(row_id) as row_id "+
                            "FROM wf_routing where process_code in ('INR_AWC_RENEW', 'INR_CLAIM', 'INR_CLAIM_2', 'INR_CLAIM_3', 'INR_NEW')"+
                            "GROUP BY process_id)as tb1) and wf.wf_status <> 'SAVE'";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);

            return dt;
        }
        #region gv1
        public void bind_gv1(DataTable dt)
        {
            gv1.DataSource = dt;
            gv1.DataBind();
        }
        #endregion
        //public void setDataTrackingRenew()
        //{
        //    ucHeader1.setHeader("Tracking Renew");
        //    string sqlreqres = "select req.process_id,req.req_no,req.req_date,req.[status],bu.bu_desc from li_insurance_request as req inner join li_business_unit as bu on bu.bu_code = req.bu_code where req.toreq_code not in (07)";

        //    var reqres = zdb.ExecSql_DataTable(sqlreqres, zconnstr);

        //    if (reqres.Rows.Count > 0)
        //    {
        //        List<InsuranceRequestResponse> listRequestResponse = new List<InsuranceRequestResponse>();

        //        foreach (DataRow drReq in reqres.Rows)
        //        {
        //            InsuranceRequestResponse requestResponse = new InsuranceRequestResponse();
        //            requestResponse.ProcressID = drReq["process_id"].ToString();
        //            requestResponse.RequestNo = drReq["req_no"].ToString();
        //            requestResponse.BuName = drReq["bu_desc"].ToString();
        //            requestResponse.Status = drReq["status"].ToString();
        //            requestResponse.RequestDate = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(drReq["req_date"]), "en");

        //            string sqlreqinsres = "select reqpropins.req_no,reqpropins.suminsured,tofins.top_ins_code,top_ins_desc from [dbo].[li_insurance_req_property_insured] as reqpropins inner join li_type_of_property_insured as tofins on tofins.top_ins_code = reqpropins.top_ins_code where req_no='" + drReq["req_no"].ToString() + "'";

        //            var reqinsres = zdb.ExecSql_DataTable(sqlreqinsres, zconnstr);

        //            if (reqinsres.Rows.Count > 0)
        //            {
        //                foreach (DataRow drReqIns in reqinsres.Rows)
        //                {
        //                    var topInsCode = drReqIns["top_ins_code"].ToString();
        //                    if (topInsCode == "01")
        //                    {
        //                        requestResponse.IARSumInsured = drReqIns["suminsured"].ToString();
        //                    }
        //                    else if (topInsCode == "02")
        //                    {
        //                        requestResponse.BISumInsured = drReqIns["suminsured"].ToString();
        //                    }
        //                    else if (topInsCode == "03")
        //                    {
        //                        requestResponse.CGLPLSumInsured = drReqIns["suminsured"].ToString();
        //                    }
        //                    else if (topInsCode == "04")
        //                    {
        //                        requestResponse.PVSumInsured = drReqIns["suminsured"].ToString();
        //                    }
        //                    else if (topInsCode == "05")
        //                    {
        //                        requestResponse.LPGSumInsured = drReqIns["suminsured"].ToString();
        //                    }
        //                    else if (topInsCode == "06")
        //                    {
        //                        requestResponse.DOSumInsured = drReqIns["suminsured"].ToString();
        //                    }
        //                }
        //            }

        //            listRequestResponse.Add(requestResponse);


        //        }

        //        ListView1.DataSource = listRequestResponse;
        //        ListView1.DataBind();
        //    }
        //}

        //public class InsuranceRequestResponse
        //{
        //    public string ProcressID { get; set; }
        //    public string RequestNo { get; set; }
        //    public string BuName { get; set; }
        //    public string Status { get; set; }
        //    public string RequestDate { get; set; }
        //    public string IARSumInsured { get; set; }
        //    public string BISumInsured { get; set; }
        //    public string CGLPLSumInsured { get; set; }
        //    public string PVSumInsured { get; set; }
        //    public string LPGSumInsured { get; set; }
        //    public string DOSumInsured { get; set; }
        //}

        //protected void Approve_Click(object sender, EventArgs e)
        //{
        //    List<string> listreq_no = new List<string>();
        //    foreach (ListViewItem row in ListView1.Items)
        //    {
        //        CheckBox cb = (CheckBox)row.FindControl("CheckBox1");
        //        if (cb != null)
        //        {
        //            if (cb.Checked == true)
        //            {
        //                HiddenField xreq_no = (HiddenField)row.FindControl("req_no");
        //                if (xreq_no != null)
        //                {
        //                    string refreq_no = xreq_no.Value;
        //                    listreq_no.Add(refreq_no);
        //                }
        //            }
        //        }
        //    }

        //    string reslistreq_no = string.Join(", ", listreq_no);
        //}
    }
}