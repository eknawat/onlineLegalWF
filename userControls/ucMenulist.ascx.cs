﻿using Microsoft.Office.Interop.Word;
using onlineLegalWF.frmInsurance;
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
    public partial class ucMenulist : System.Web.UI.UserControl
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        //public string zconnstr = ConfigurationSettings.AppSettings["BPMDB"].ToString();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                bind_menu("");

                if (Session["group_menu"] != null && Session["group_menu_index"] != null)
                {
                    bind_gvA_menu(Session["group_menu"].ToString(), Session["group_menu_index"].ToString());
                }
            }
        }
        public void bind_menu(string userrole)
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            string sql = @"select * from (
                            select distinct menu_code, menu_group_name, ( '"+host_url+@"' + menu_icon_filename) as menu_icon_filename , row_sort

            from m_portal_menu where menu_url = '' ) as t1 
							order by t1.row_sort

            ";
           
            var ds = zdb.ExecSql_DataSet(sql, zconnstr);
            gv.DataSource = ds.Tables[0];
            gv.DataBind(); 

        }

        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            if (e.CommandName == "expandmenu") 
            {
                
                int i = System.Convert.ToInt32(e.CommandArgument);
                //gv.Rows[i].FindControl("gvlbtnMenu").Focus();
                Session["group_menu_index"] = i;
                var xmenu_group_name = ((Label)gv.Rows[i].FindControl("gvlblMenuGroupName")).Text;
                var gvA = ((GridView)gv.Rows[i].FindControl("gvA"));
                string sql = @"
           select  menu_code, menu_name, ( '" + host_url + @"' + menu_icon_filename) as menu_icon_filename, menu_url , row_sort ,menu_group_name

            from m_portal_menu 
            where menu_group_name = '" + xmenu_group_name + @"' and menu_url <> '' 
            order by row_sort";
                var ds = zdb.ExecSql_DataSet(sql, zconnstr);
                gvA.DataSource = ds;
                gvA.DataBind(); 
            }
        }
        private void bind_gvA_menu(string menu_group_name,string index)
        {
            int length = System.Convert.ToInt32(index);
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            var gvA = ((GridView)gv.Rows[length].FindControl("gvA"));
            string sql = @"
            select  menu_code, menu_name, ( '" + host_url + @"' + menu_icon_filename) as menu_icon_filename, menu_url , row_sort ,menu_group_name

             from m_portal_menu 
             where menu_group_name = '" + menu_group_name + @"' and menu_url <> '' 
             order by row_sort";
            var ds = zdb.ExecSql_DataSet(sql, zconnstr);
            gvA.DataSource = ds;
            gvA.DataBind();
        }
        //gvA_RowCommand
        protected void gvA_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            //string xurl = "";
            if (e.CommandName == "openprogram")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                GridView grid = sender as GridView;

                //var xmenu_code = ((Label)grid.Rows[index].FindControl("gvAlblMenuGroupName")).Text;
                var xurl = ((Label)grid.Rows[index].FindControl("gvAlblMenuUrl")).Text;
                Session["group_menu"] = ((Label)grid.Rows[index].FindControl("gvAlblMenuGroupName")).Text;
                Response.Redirect(xurl);

            }
        }
    }
}