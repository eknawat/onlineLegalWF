using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace onlineLegalWF.userControls
{
    public partial class ucTaskList : System.Web.UI.UserControl
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["RPADB"].ToString();
        public string zconnstrbpm = ConfigurationManager.AppSettings["BPMDB"].ToString();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack) 
            {
                bind_gv();
            }
        }
        public void bindData(string xlogin_name, string xmode)
        {
            hidLogin.Value = xlogin_name;
            hidMode.Value = xmode; 

        }
        protected void gv1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv1.PageIndex = e.NewPageIndex;
            bind_gv();
        }
        public DataTable getDataStructure() 
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("No",typeof(string));
            dt.Columns.Add("Subject", typeof(string));
            dt.Columns.Add("RequestBy", typeof(string));
            dt.Columns.Add("SubmittedDate", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("LastUpdated", typeof(string));
            dt.Columns.Add("LastUpdatedBy", typeof(string));
            dt.Columns.Add("AssignTo", typeof(string));
            return dt; 
        }
        public DataTable ini_data()
        {
            var dt = getDataStructure(); 
            for (int i = 1; i<=1; i++)
            {
                var dr = dt.NewRow();
                dr["No"] = i.ToString();
                dt.Rows.Add(dr); 
            }
            return dt; 
        }
        public void bind_gv()
        {
            switch  (hidMode.Value)
            {
                case "myrequest": {
                        bind_gv1(getMyRequest()); 
                     };break;
                case "myworklist":
                    {
                        bind_gv1(getMyWorkList()); 
                    }; break;
                case "completelist":
                    {
                        bind_gv1(getCompleteList()); 
                    }; break;
                case "permitTracking":
                    {
                        bind_gv1(getPermitTrackingList());
                    }; break;
                case "insTracking":
                    {
                        bind_gv1(getInsTrackingList());
                    }; break;

            }
        }
        public DataTable getMyRequest()
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString(); 
            //string sql = "Select process_id,subject,submit_by,updated_by,created_datetime,wf_status,updated_datetime, ( '" + host_url+ "' + link_url_format) as link_url_format from " +
            //    "wf_routing where process_id in (Select process_id from wf_routing where submit_by = '"+ hidLogin.Value + "' and wf_status in ('SAVE','WAITATCH')) and wf_status in ('SAVE','WAITATCH')";
            //string sql = @"Select assto_login,process_id,subject,submit_by,updated_by,created_datetime,wf_status,updated_datetime,"+
            //                "CASE "+
            //                    "WHEN step_name = 'Start' or wf_status in ('SAVE', 'WAITATCH') THEN ('" + host_url+ "' + link_url_format) " +
            //                    "ELSE '"+host_url+"legalPortal/legalportal?m=myrequest#' " +
            //                "END AS link_url_format "+
            //                "from wf_routing where submit_by = '"+ hidLogin.Value + "'" +
            //                " and row_id in (select tb1.row_id from "+
            //                "(SELECT process_id, "+
            //                "MAX(row_id) as row_id "+
            //                "FROM wf_routing where submit_by = '"+ hidLogin.Value + "'" +
            //                "GROUP BY  process_id)as tb1) and step_name not in ('End')";
            string sql = @"Select wf.assto_login,wf.process_id,wf.subject,wf.submit_by,wf.updated_by,wf.created_datetime,wf.wf_status,wf.updated_datetime," +
                            "CASE "+
                                "WHEN step_name = 'Start' or wf_status in ('SAVE', 'WAITATCH') THEN ('" + host_url+ "' + link_url_format) " +
                                "ELSE '"+host_url+"legalPortal/legalportal?m=myrequest#' " +
                            "END AS link_url_format,"+
                            "CASE " +
                                "WHEN permitreq.document_no IS NOT NULL THEN permitreq.document_no " +
                                "WHEN commreq.document_no IS NOT NULL THEN commreq.document_no " +
                                "WHEN claimreq.document_no IS NOT NULL THEN claimreq.document_no " +
                                "WHEN insreq.document_no IS NOT NULL THEN insreq.document_no " +
                                "WHEN memoreq.document_no IS NOT NULL THEN memoreq.document_no " +
                                "WHEN lit.document_no IS NOT NULL THEN lit.document_no " +
                                "ELSE '' " +
                            "END AS document_no " +
                            "from wf_routing as wf " +
                            "left outer join li_permit_request as permitreq on permitreq.process_id = wf.process_id " +
                            "left outer join li_comm_regis_request as commreq on commreq.process_id = wf.process_id " +
                            "left outer join li_insurance_claim as claimreq on claimreq.process_id = wf.process_id " +
                            "left outer join li_insurance_request as insreq on insreq.process_id = wf.process_id " +
                            "left outer join li_insurance_renew_awc_memo as memoreq on memoreq.process_id = wf.process_id " +
                            "left outer join li_litigation_request as lit on lit.process_id = wf.process_id " +
                            "where submit_by = '" + hidLogin.Value + "'" +
                            " and wf.row_id in (select tb1.row_id from " +
                            "(SELECT process_id, "+
                            "MAX(row_id) as row_id "+
                            "FROM wf_routing where submit_by = '"+ hidLogin.Value + "'" +
                            "GROUP BY  process_id)as tb1) and step_name not in ('End')";

            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstrbpm);

            return dt;
        }
        public DataTable getMyWorkList()
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            string sql = "Select wf.assto_login,wf.process_id,wf.subject,wf.submit_by,wf.updated_by,wf.created_datetime,wf.wf_status,wf.updated_datetime, ( '" + host_url + "' + link_url_format) as link_url_format," +
                        "CASE " +
                            "WHEN permitreq.document_no IS NOT NULL THEN permitreq.document_no " +
                            "WHEN commreq.document_no IS NOT NULL THEN commreq.document_no " +
                            "WHEN claimreq.document_no IS NOT NULL THEN claimreq.document_no " +
                            "WHEN insreq.document_no IS NOT NULL THEN insreq.document_no " +
                            "WHEN memoreq.document_no IS NOT NULL THEN memoreq.document_no " +
                            "WHEN lit.document_no IS NOT NULL THEN lit.document_no " +
                            "ELSE '' " +
                        "END AS document_no " +
                        "from wf_routing as wf " +
                        "left outer join li_permit_request as permitreq on permitreq.process_id = wf.process_id " +
                        "left outer join li_comm_regis_request as commreq on commreq.process_id = wf.process_id " +
                        "left outer join li_insurance_claim as claimreq on claimreq.process_id = wf.process_id " +
                        "left outer join li_insurance_request as insreq on insreq.process_id = wf.process_id " +
                        "left outer join li_insurance_renew_awc_memo as memoreq on memoreq.process_id = wf.process_id " +
                        "left outer join li_litigation_request as lit on lit.process_id = wf.process_id " +
                        "where wf.process_id in (Select process_id from wf_routing where assto_login like '%" + hidLogin.Value + "%' and submit_answer = '') and submit_answer = ''";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstrbpm);

            return dt;
        }
        public DataTable getCompleteList()
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            string sql = "Select wf.assto_login,wf.process_id,wf.subject,wf.submit_by,wf.updated_by,wf.created_datetime,wf.wf_status,wf.updated_datetime, ( '" + host_url + "' + link_url_format) as link_url_format," +
                        "CASE " +
                            "WHEN permitreq.document_no IS NOT NULL THEN permitreq.document_no " +
                            "WHEN commreq.document_no IS NOT NULL THEN commreq.document_no " +
                            "WHEN claimreq.document_no IS NOT NULL THEN claimreq.document_no " +
                            "WHEN insreq.document_no IS NOT NULL THEN insreq.document_no " +
                            "WHEN memoreq.document_no IS NOT NULL THEN memoreq.document_no " +
                            "WHEN lit.document_no IS NOT NULL THEN lit.document_no " +
                            "ELSE '' " +
                        "END AS document_no " +
                        "from wf_routing as wf " +
                        "left outer join li_permit_request as permitreq on permitreq.process_id = wf.process_id " +
                        "left outer join li_comm_regis_request as commreq on commreq.process_id = wf.process_id " +
                        "left outer join li_insurance_claim as claimreq on claimreq.process_id = wf.process_id " +
                        "left outer join li_insurance_request as insreq on insreq.process_id = wf.process_id " +
                        "left outer join li_insurance_renew_awc_memo as memoreq on memoreq.process_id = wf.process_id " +
                        "left outer join li_litigation_request as lit on lit.process_id = wf.process_id " +
                "where wf.process_id in (Select process_id from wf_routing where submit_by = '" + hidLogin.Value + "' and step_name = 'End') and step_name = 'End' ";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstrbpm);

            return dt;
        }
        public DataTable getPermitTrackingList()
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            string sql = @"Select process_code,assto_login,process_id,subject,submit_by,updated_by,created_datetime,updated_datetime,
                            CASE 
                                WHEN wf_status = '' THEN 'IN PROGRESS' 
                                ELSE wf_status
                            END AS wf_status 
                            ,'" + host_url + @"forms/permitapv.aspx?req='+process_id+'&pc='+process_code+'&st='+step_name+'&mode=tracking' AS link_url_format
                            from wf_routing where process_code in ('PMT_LIC', 'PMT_TAX', 'PMT_TM')
                             and row_id in (select tb1.row_id from
                            (SELECT process_id,
                            MAX(row_id) as row_id
                            FROM wf_routing where process_code in ('PMT_LIC', 'PMT_TAX', 'PMT_TM')
                            GROUP BY process_id)as tb1)";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstrbpm);

            return dt;
        }
        public DataTable getInsTrackingList()
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            string sql = @"Select req.document_no,wf.process_code,wf.assto_login,wf.process_id,wf.subject,wf.submit_by,wf.updated_by,wf.created_datetime,wf.updated_datetime,
                            CASE 
                                WHEN wf_status = '' THEN 'IN PROGRESS' 
                                ELSE wf_status
                            END AS wf_status 
                            ,'" + host_url + @"forms/apv.aspx?req='+process_id+'&pc='+process_code+' AS link_url_format,bu.bu_desc
                            from wf_routing as wf
							left outer join li_insurance_claim as claimreq on claimreq.process_id = wf.process_id 
                            left outer join li_insurance_request as insreq on insreq.process_id = wf.process_id 
                            left outer join li_insurance_renew_awc_memo as memoreq on memoreq.process_id = wf.process_id 
                            left outer join li_business_unit as bu on bu.bu_code = wf.bu_code
							where wf.process_code in ('INR_AWC_RENEW', 'INR_CLAIM', 'INR_CLAIM_2', 'INR_CLAIM_3', 'INR_NEW')
                             and wf.row_id in (select tb1.row_id from
                            (SELECT process_id,
                            MAX(row_id) as row_id
                            FROM wf_routing where process_code in ('INR_AWC_RENEW', 'INR_CLAIM', 'INR_CLAIM_2', 'INR_CLAIM_3', 'INR_NEW')
                            GROUP BY process_id)as tb1) and wf.wf_status <> 'SAVE'";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstrbpm);

            return dt;
        }
        #region gv1
        public void bind_gv1(DataTable dt)
        {
            gv1.DataSource = dt;
            gv1.DataBind(); 
        }
        #endregion 
    }
}