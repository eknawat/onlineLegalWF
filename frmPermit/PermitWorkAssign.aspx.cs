﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace onlineLegalWF.frmPermit
{
    public partial class PermitWorkAssign : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["RPADB"].ToString();
        public string zconnstrbpm = ConfigurationManager.AppSettings["BPMDB"].ToString();
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
            ucHeader1.setHeader("Permit Tracking");
            // Bind Worklist
            if (Session["user_login"] != null)
            {
                var xlogin_name = Session["user_login"].ToString();

                bindData(xlogin_name, "permitTracking");

                bind_gv();

                ddlType_of_request.DataSource = GetTypeOfRequest();
                ddlType_of_request.DataTextField = "tof_permitreq_desc";
                ddlType_of_request.DataValueField = "tof_permitreq_code";
                ddlType_of_request.DataBind();
                ddlType_of_request.Items.Insert(0, new ListItem("All", "0"));

                ddl_license.DataSource = GetTypeOfLicenseRequest();
                ddl_license.DataTextField = "license_desc_en";
                ddl_license.DataValueField = "license_code";
                ddl_license.DataBind();
                ddl_license.Items.Insert(0, new ListItem("All", "0"));

                //ddl_status.SelectedValue = "IN PROGRESS";

                //getSearchPermitTracking("", "0", "IN PROGRESS");

                gv1.DataSource = getSearchPermitTracking("", "0", "IN PROGRESS", ddl_license.SelectedValue);
                gv1.DataBind();
            }
        }
        public DataTable GetTypeOfRequest()
        {
            string sql = "select * from li_type_of_permitrequest order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstrbpm);
            return dt;
        }

        public DataTable GetTypeOfLicenseRequest()
        {
            string sql = "select * from li_permit_license order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstrbpm);
            return dt;
        }

        protected void Search(object sender, EventArgs e)
        {
            gv1.DataSource = getSearchPermitTracking(txtSearch.Text, ddlType_of_request.SelectedValue, "", ddl_license.SelectedValue);
            gv1.DataBind();
            
        }

        protected void SearchByTOR(object sender, EventArgs e)
        {
            gv1.DataSource = getSearchPermitTracking(txtSearch.Text, ddlType_of_request.SelectedValue,"", ddl_license.SelectedValue);
            gv1.DataBind();
        }
        protected void SearchByStatus(object sender, EventArgs e)
        {
            gv1.DataSource = getSearchPermitTracking(txtSearch.Text, ddlType_of_request.SelectedValue, ddl_status.SelectedValue, ddl_license.SelectedValue);
            gv1.DataBind();
        }

        protected void SearchByLicense(object sender, EventArgs e)
        {
            gv1.DataSource = getSearchPermitTracking(txtSearch.Text, ddlType_of_request.SelectedValue, ddl_status.SelectedValue, ddl_license.SelectedValue);
            gv1.DataBind();
        }

        protected void gv1_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            DataTable dt = getSearchPermitTracking(txtSearch.Text, ddlType_of_request.SelectedValue, ddl_status.SelectedValue, ddl_license.SelectedValue);
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gv1.DataSource = dt.DefaultView;
                gv1.DataBind();
            }
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        protected void gv1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv1.PageIndex = e.NewPageIndex;
            bind_gv();
        }
        public void bindData(string xlogin_name, string xmode)
        {
            hidLogin.Value = xlogin_name;
            hidMode.Value = xmode;

        }
        public DataTable getDataStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("No", typeof(string));
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
            for (int i = 1; i <= 1; i++)
            {
                var dr = dt.NewRow();
                dr["No"] = i.ToString();
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public void bind_gv()
        {
            switch (hidMode.Value)
            {
                case "myrequest":
                    {
                        bind_gv1(getMyRequest());
                    }; break;
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

            }
        }
        public DataTable getMyRequest()
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            //string sql = "Select process_id,subject,submit_by,updated_by,created_datetime,wf_status,updated_datetime, ( '" + host_url+ "' + link_url_format) as link_url_format from " +
            //    "wf_routing where process_id in (Select process_id from wf_routing where submit_by = '"+ hidLogin.Value + "' and wf_status in ('SAVE','WAITATCH')) and wf_status in ('SAVE','WAITATCH')";
            string sql = "Select assto_login,process_id,subject,submit_by,updated_by,created_datetime,wf_status,updated_datetime," +
                            "CASE " +
                                "WHEN step_name = 'Start' or wf_status in ('SAVE', 'WAITATCH') THEN ('" + host_url + "' + link_url_format) " +
                                "ELSE '" + host_url + "legalPortal/legalportal?m=myrequest#' " +
                            "END AS link_url_format " +
                            "from wf_routing where submit_by = '" + hidLogin.Value + "'" +
                            " and row_id in (select tb1.row_id from " +
                            "(SELECT process_id, " +
                            "MAX(row_id) as row_id " +
                            "FROM wf_routing where submit_by = '" + hidLogin.Value + "'" +
                            "GROUP BY  process_id)as tb1) and step_name not in ('End')";

            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstrbpm);

            return dt;
        }
        public DataTable getMyWorkList()
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            string sql = "Select assto_login,process_id,subject,submit_by,updated_by,created_datetime,wf_status,updated_datetime, ( '" + host_url + "' + link_url_format) as link_url_format from " +
                "wf_routing where process_id in (Select process_id from wf_routing where assto_login like '%" + hidLogin.Value + "%' and submit_answer = '') and submit_answer = ''";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstrbpm);

            return dt;
        }
        public DataTable getCompleteList()
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            string sql = "Select assto_login,process_id,subject,submit_by,updated_by,created_datetime,wf_status,updated_datetime, ( '" + host_url + "' + link_url_format) as link_url_format from " +
                "wf_routing where process_id in (Select process_id from wf_routing where submit_by = '" + hidLogin.Value + "' and step_name = 'End') and step_name = 'End'";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstrbpm);

            return dt;
        }
        public DataTable getPermitTrackingList()
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            string sql = @"Select req.document_no,wf.process_code,wf.assto_login,wf.process_id,wf.subject,wf.submit_by,wf.updated_by,wf.created_datetime,wf.updated_datetime,
                            CASE 
                                WHEN wf_status = '' THEN 'IN PROGRESS' 
                                ELSE wf_status
                            END AS wf_status 
                            ,'" + host_url + @"forms/permitapv.aspx?req='+wf.process_id+'&pc='+process_code+'&st='+step_name+'&mode=tracking' AS link_url_format
							,code.tof_permitreq_desc,req.tof_permitreq_code,req.license_code, license.license_desc_en + ': '+ license_desc as license_desc,req.bu_code,bu.bu_desc
                            from wf_routing as wf
							left outer join li_permit_request as req on req.process_id = wf.process_id
							left outer join li_type_of_permitrequest as code on code.tof_permitreq_code = req.tof_permitreq_code
                            left outer join li_permit_license as license on req.license_code = license.license_code
                            left outer join li_business_unit as bu on req.bu_code = bu.bu_code
							where wf.process_code in ('PMT_LIC', 'PMT_TAX', 'PMT_TM', 'PMT_UTIL', 'PMT_EMR')
                             and wf.row_id in (select tb1.row_id from
                            (SELECT process_id,
                            MAX(row_id) as row_id
                            FROM wf_routing where process_code in ('PMT_LIC', 'PMT_TAX', 'PMT_TM', 'PMT_UTIL', 'PMT_EMR')
                            GROUP BY process_id)as tb1) and wf.wf_status <> 'SAVE' order by document_no desc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstrbpm);

            return dt;
        }
        public DataTable getSearchPermitTracking(string kw, string code, string status, string license)
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            string sql = @"Select req.document_no,wf.process_code,wf.assto_login,wf.process_id,wf.subject,wf.submit_by,wf.updated_by,wf.created_datetime,wf.updated_datetime,
                            CASE 
                                WHEN wf_status = '' THEN 'IN PROGRESS' 
                                ELSE wf_status
                            END AS wf_status 
                            ,'" + host_url + @"forms/permitapv.aspx?req='+wf.process_id+'&pc='+process_code+'&st='+step_name+'&mode=tracking' AS link_url_format
							,code.tof_permitreq_desc,req.tof_permitreq_code,req.license_code, license.license_desc_en + ': '+ license_desc as license_desc,req.bu_code,bu.bu_desc
                            from wf_routing as wf
							left outer join li_permit_request as req on req.process_id = wf.process_id
							left outer join li_type_of_permitrequest as code on code.tof_permitreq_code = req.tof_permitreq_code
                            left outer join li_permit_license as license on req.license_code = license.license_code
                            left outer join li_business_unit as bu on req.bu_code = bu.bu_code
							where wf.process_code in ('PMT_LIC', 'PMT_TAX', 'PMT_TM', 'PMT_UTIL', 'PMT_EMR')
                             and wf.row_id in (select tb1.row_id from
                            (SELECT process_id,
                            MAX(row_id) as row_id
                            FROM wf_routing where process_code in ('PMT_LIC', 'PMT_TAX', 'PMT_TM', 'PMT_UTIL', 'PMT_EMR')
                            GROUP BY process_id)as tb1) and wf.wf_status <> 'SAVE'";

            if (!string.IsNullOrEmpty(kw)) 
            {
                //sql += " and wf.subject like '%" + kw + "%' or wf.submit_by like '%" + kw + "%' or wf.updated_by like '%" + kw + "%'";
                sql += " and wf.subject like '%" + kw + "%' ";
            }

            if(!string.IsNullOrEmpty(code)) 
            {
                if (code != "0") 
                {
                    sql += "and req.tof_permitreq_code like '%" + code + "%'";
                }
                
            }
            if (!string.IsNullOrEmpty(status))
            {
                if (status != "0")
                {
                    if (status == "IN PROGRESS")
                    {
                        sql += "and wf_status <> 'COMPLETED'";
                    }
                    else 
                    {
                        sql += "and wf_status like '%" + status + "%'";
                    }
                    
                }

            }
            if (!string.IsNullOrEmpty(license))
            {
                if (license != "0")
                {
                    sql += "and req.license_code like '%" + license + "%'";
                }

            }

            //order by 
            sql += " order by document_no desc";

            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstrbpm);

            return dt;
        }

        protected void btn_Export_Click(object sender, EventArgs e)
        {
            ExportGridToExcel();
        }
        protected void Reset(object sender, EventArgs e)
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            Response.Redirect(host_url + "frmpermit/permitworkassign.aspx", true);
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."
        }
        private void ExportGridToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "PermitTracking_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls";
            //string FileName = "PermitTracking_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gv1.GridLines = GridLines.Both;
            gv1.HeaderStyle.Font.Bold = true;

            gv1.AllowPaging = false;
            gv1.DataSource = getSearchPermitTracking(txtSearch.Text, ddlType_of_request.SelectedValue, ddl_status.SelectedValue, ddl_license.SelectedValue);
            gv1.DataBind();

            gv1.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            //Response.Flush();
            Response.End();

            //Response.Clear();
            //string FileName = "PermitTracking_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            //Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htmlWriter = new HtmlTextWriter(sw);

            //foreach (GridViewRow row in gv1.Rows)
            //{
            //    if (row.RowType == DataControlRowType.DataRow)
            //        for (int idxColumn = 0; idxColumn < row.Cells.Count; idxColumn++)
            //            row.Cells[idxColumn].Attributes.Add("class", "xlText");
            //}

            //gv1.RenderControl(htmlWriter);

            //string appendStyle = @"<style> .xlText { mso-number-format:\@; } </style> ";
            //Response.Write(appendStyle);

            //Response.Write(sw.ToString());
            //Response.End();
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