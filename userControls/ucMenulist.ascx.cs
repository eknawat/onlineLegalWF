using Microsoft.Office.Interop.Word;
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
            else 
            {
                if (Session["user_login"] != null)
                {
                    var xlogin_name = Session["user_login"].ToString();

                    if (!string.IsNullOrEmpty(xlogin_name))
                    {
                        setNotificationInbox(xlogin_name);
                    }

                }
            }
        }
        public void bind_menu(string userrole)
        {
            var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            string sql = @"select * from (
                            select distinct menu_code, menu_group_name, ( '" + host_url + @"' + menu_icon_filename) as menu_icon_filename , row_sort

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
                gvA.DataSource = ds.Tables[0];
                gvA.DataBind();

                //check menu permit tracking
                foreach (GridViewRow row in gvA.Rows)
                {
                    string menu_code = (row.FindControl("gvAlblMenuCode") as Label).Text;
                    if (menu_code == "permit_tracking")
                    {
                        if (Session["user_login"] != null)
                        {
                            var xlogin_name = Session["user_login"].ToString();

                            //pornsawan.s, naruemol.w, kanita.s, pattanis.r, suradach.k
                            if (xlogin_name == "pornsawan.s" || xlogin_name == "naruemol.w" || xlogin_name == "kanita.s" || xlogin_name == "pattanis.r" || xlogin_name == "suradach.k" || xlogin_name == "pichet.ku" || xlogin_name == "aunchisa.c" || xlogin_name == "chanetti.b" || xlogin_name == "kannika.pa" || xlogin_name == "mueanfan.m" || xlogin_name == "narisara.k" || xlogin_name == "zirint.w")
                            {
                                (row.FindControl("gvAlbtnMenu") as LinkButton).Visible = true;
                                (row.FindControl("gvAibtnMenuItemIcon") as ImageButton).Visible = true;
                            }
                            else 
                            {
                                (row.FindControl("gvAlbtnMenu") as LinkButton).Visible = false;
                                (row.FindControl("gvAibtnMenuItemIcon") as ImageButton).Visible = false;
                            }
                        }
                        
                    }
                    else if (menu_code == "instracking" || menu_code == "instracking_renew")
                    {
                        if (Session["user_login"] != null)
                        {
                            var xlogin_name = Session["user_login"].ToString();
                            if (xlogin_name == "jaroonsak.n" || xlogin_name == "warin.k")
                            {
                                (row.FindControl("gvAlbtnMenu") as LinkButton).Visible = true;
                                (row.FindControl("gvAibtnMenuItemIcon") as ImageButton).Visible = true;
                            }
                            else
                            {
                                (row.FindControl("gvAlbtnMenu") as LinkButton).Visible = false;
                                (row.FindControl("gvAibtnMenuItemIcon") as ImageButton).Visible = false;
                            }
                        }

                    }
                }

                if (Session["user_login"] != null)
                {
                    var xlogin_name = Session["user_login"].ToString();

                    if (!string.IsNullOrEmpty(xlogin_name))
                    {
                        setNotificationInbox(xlogin_name);
                    }

                }
            }
        }
        private void bind_gvA_menu(string menu_group_name, string index)
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
            gvA.DataSource = ds.Tables[0];
            gvA.DataBind();

            //check menu permit tracking
            foreach (GridViewRow row in gvA.Rows) 
            {
                string menu_code = (row.FindControl("gvAlblMenuCode") as Label).Text;
                if (menu_code == "permit_tracking")
                {
                    if (Session["user_login"] != null)
                    {
                        var xlogin_name = Session["user_login"].ToString();

                        //pornsawan.s, naruemol.w, kanita.s, pattanis.r, suradach.k
                        if (xlogin_name == "pornsawan.s" || xlogin_name == "naruemol.w" || xlogin_name == "kanita.s" || xlogin_name == "pattanis.r" || xlogin_name == "suradach.k" || xlogin_name == "pichet.ku" || xlogin_name == "aunchisa.c" || xlogin_name == "chanetti.b" || xlogin_name == "kannika.pa" || xlogin_name == "mueanfan.m" || xlogin_name == "narisara.k" || xlogin_name == "zirint.w")
                        {
                            (row.FindControl("gvAlbtnMenu") as LinkButton).Visible = true;
                            (row.FindControl("gvAibtnMenuItemIcon") as ImageButton).Visible = true;
                        }
                        else
                        {
                            (row.FindControl("gvAlbtnMenu") as LinkButton).Visible = false;
                            (row.FindControl("gvAibtnMenuItemIcon") as ImageButton).Visible = false;
                        }
                    }

                }
                else if (menu_code == "instracking" || menu_code == "instracking_renew")
                {
                    if (Session["user_login"] != null)
                    {
                        var xlogin_name = Session["user_login"].ToString();
                        if (xlogin_name == "jaroonsak.n" || xlogin_name == "warin.k")
                        {
                            (row.FindControl("gvAlbtnMenu") as LinkButton).Visible = true;
                            (row.FindControl("gvAibtnMenuItemIcon") as ImageButton).Visible = true;
                        }
                        else
                        {
                            (row.FindControl("gvAlbtnMenu") as LinkButton).Visible = false;
                            (row.FindControl("gvAibtnMenuItemIcon") as ImageButton).Visible = false;
                        }
                    }

                }
            }

            if (Session["user_login"] != null)
            {
                var xlogin_name = Session["user_login"].ToString();

                if (!string.IsNullOrEmpty(xlogin_name))
                {
                    setNotificationInbox(xlogin_name);
                }

            }


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
                var xurl = host_url + ((Label)grid.Rows[index].FindControl("gvAlblMenuUrl")).Text;
                Session["group_menu"] = ((Label)grid.Rows[index].FindControl("gvAlblMenuGroupName")).Text;
                Response.Redirect(xurl);

            }
        }

        public void setNotificationInbox(string login_name) 
        {
            string numnotify = "0";
            string sql = "Select count(submit_by) as total from wf_routing where process_id in (Select process_id from wf_routing where assto_login like '%" + login_name + "%' and submit_answer = '') and submit_answer = ''";
            System.Data.DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            if (dt.Rows.Count > 0) 
            {
                numnotify = dt.Rows[0]["total"].ToString();
            }
            string js = "$('#inbox_notify').html('" + numnotify+ "');";

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetNotify", js, true);
        }

        
    }
}