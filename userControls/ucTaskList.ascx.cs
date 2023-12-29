﻿using System;
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
                        getMyWorkList(); 
                    }; break;
                case "completelist":
                    {
                        getCompleteList(); 
                    }; break;
               
            }
        }
        public DataTable getMyRequest()
        {
            var dt = ini_data();
            //string sql = "Select * from wf_routing where process_id = 'PID_LEGALWF_2023_000189'and submit_by = 'eknawat.c'";
            //DataTable dt = zdb.ExecSql_DataTable(sql, zconnstrbpm);

            return dt; 
        }
        public DataTable getMyWorkList()
        {
            var dt = ini_data();
            return dt;
        }
        public DataTable getCompleteList()
        {
            var dt = ini_data();
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