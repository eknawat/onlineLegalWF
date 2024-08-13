using iTextSharp.text.pdf;
using onlineLegalWF.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static onlineLegalWF.Class.ReplacePermit;

namespace onlineLegalWF.frmPermit
{
    public partial class PermitLicenseEdit : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        public ReplacePermit zreplacepermit = new ReplacePermit();
        public SendMail zsendmail = new SendMail();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(id))
                {
                    setData(id);
                }
                
            }
        }

        private void setData(string id)
        {
            ucHeader1.setHeader("License Request Edit");

            //type_requester.DataSource = GetTypeOfRequester();
            //type_requester.DataBind();
            //type_requester.DataTextField = "tof_requester_desc";
            //type_requester.DataValueField = "tof_requester_code";
            //type_requester.DataBind();

            ////type_project.DataSource = GetBusinessUnit();
            //type_project.DataSource = GetListBuByTypeReq("01");
            //type_project.DataBind();
            //type_project.DataTextField = "bu_desc";
            //type_project.DataValueField = "bu_code";
            //type_project.DataBind();

            //license_code.DataSource = GetTypeOfPermitLicense();
            //license_code.DataBind();
            //license_code.DataTextField = "license_desc_all";
            //license_code.DataValueField = "license_code";
            //license_code.DataBind();

            //var dtSublicense = GetSubPermitLicense(license_code.SelectedValue);

            //if (dtSublicense.Rows.Count > 0)
            //{
            //    ddl_sublicense.Visible = true;
            //    ddl_sublicense.DataSource = dtSublicense;
            //    ddl_sublicense.DataBind();
            //    ddl_sublicense.DataTextField = "sublicense_desc";
            //    ddl_sublicense.DataValueField = "sublicense_code";
            //    ddl_sublicense.DataBind();
            //}

            //var dtSublicenseRefdoc = GetSubPermitLicenseRefDoc(ddl_sublicense.SelectedValue);

            //if (dtSublicenseRefdoc.Rows.Count > 0)
            //{
            //    refdoc.Visible = true;
            //    ddl_refdoc.Visible = true;
            //    ddl_refdoc.DataSource = dtSublicenseRefdoc;
            //    ddl_refdoc.DataBind();
            //    ddl_refdoc.DataTextField = "sublicense_refdoc_desc";
            //    ddl_refdoc.DataValueField = "sublicense_code";
            //    ddl_refdoc.DataBind();
            //}

            string sql = "select * from li_permit_request where permit_no='" + id + "'";

            var res = zdb.ExecSql_DataTable(sql, zconnstr);

            if (res.Rows.Count > 0) 
            {
                type_requester.DataSource = GetTypeOfRequester();
                type_requester.DataBind();
                type_requester.DataTextField = "tof_requester_desc";
                type_requester.DataValueField = "tof_requester_code";
                type_requester.DataBind();

                //type_project.DataSource = GetBusinessUnit();
                type_project.DataSource = GetListBuByTypeReq(res.Rows[0]["tof_requester_code"].ToString());
                type_project.DataBind();
                type_project.DataTextField = "bu_desc";
                type_project.DataValueField = "bu_code";
                type_project.DataBind();

                license_code.DataSource = GetTypeOfPermitLicense();
                license_code.DataBind();
                license_code.DataTextField = "license_desc_all";
                license_code.DataValueField = "license_code";
                license_code.DataBind();

                var dtSublicense = GetSubPermitLicense(license_code.SelectedValue);

                if (dtSublicense.Rows.Count > 0)
                {
                    ddl_sublicense.Visible = true;
                    ddl_sublicense.DataSource = dtSublicense;
                    ddl_sublicense.DataBind();
                    ddl_sublicense.DataTextField = "sublicense_desc";
                    ddl_sublicense.DataValueField = "sublicense_code";
                    ddl_sublicense.DataBind();
                }

                var dtSublicenseRefdoc = GetSubPermitLicenseRefDoc(ddl_sublicense.SelectedValue);

                if (dtSublicenseRefdoc.Rows.Count > 0)
                {
                    refdoc.Visible = true;
                    ddl_refdoc.Visible = true;
                    ddl_refdoc.DataSource = dtSublicenseRefdoc;
                    ddl_refdoc.DataBind();
                    ddl_refdoc.DataTextField = "sublicense_refdoc_desc";
                    ddl_refdoc.DataValueField = "sublicense_code";
                    ddl_refdoc.DataBind();
                }


                req_date.Value = Convert.ToDateTime(res.Rows[0]["permit_date"]).ToString("yyyy-MM-dd");
                lblPID.Text = res.Rows[0]["process_id"].ToString();
                hid_PID.Value = res.Rows[0]["process_id"].ToString();
                ucAttachment1.ini_object(res.Rows[0]["process_id"].ToString());
                ucCommentlog1.ini_object(res.Rows[0]["process_id"].ToString());
                req_no.Text = res.Rows[0]["permit_no"].ToString();
                doc_no.Text = res.Rows[0]["document_no"].ToString();
                permit_subject.Text = res.Rows[0]["permit_subject"].ToString();
                permit_desc.Text = res.Rows[0]["permit_desc"].ToString();
                type_requester.SelectedValue = res.Rows[0]["tof_requester_code"].ToString();
                tof_requester_other_desc.Text = res.Rows[0]["tof_requester_other_desc"].ToString();
                responsible_phone.Text = res.Rows[0]["responsible_phone"].ToString();
                number_of_licenses.Text = res.Rows[0]["number_of_licenses"].ToString();
                if (res.Rows[0]["tof_requester_code"].ToString() == "03")
                {
                    tof_requester_other_desc.Enabled = true;
                }
                else 
                {
                    tof_requester_other_desc.Enabled = false;
                }
                type_project.SelectedValue = res.Rows[0]["bu_code"].ToString();
                type_req_license.SelectedValue = res.Rows[0]["tof_permitreq_code"].ToString();
                tof_permitreq_other_desc.Text = res.Rows[0]["tof_permitreq_other_desc"].ToString();
                if (type_req_license.SelectedValue == "04" || type_req_license.SelectedValue == "03")
                {
                    tof_permitreq_other_desc.Enabled = true;
                    license_code.Enabled = false;
                    ddl_sublicense.Visible = false;
                    refdoc.Visible = false;
                    ddl_refdoc.Visible = false;
                }
                else
                {
                    license_code.Enabled = true;
                    ddl_sublicense.Visible = true;
                    refdoc.Visible = true;
                    ddl_refdoc.Visible = true;
                    tof_permitreq_other_desc.Enabled = false;
                }
                license_code.SelectedValue = res.Rows[0]["license_code"].ToString();
                if (!string.IsNullOrEmpty(res.Rows[0]["sublicense_code"].ToString())) 
                {
                    ddl_sublicense.SelectedValue = res.Rows[0]["sublicense_code"].ToString();

                    dtSublicense = GetSubPermitLicense(license_code.SelectedValue);

                    if (dtSublicense.Rows.Count > 0)
                    {
                        ddl_sublicense.Visible = true;
                    }

                   dtSublicenseRefdoc = GetSubPermitLicenseRefDoc(ddl_sublicense.SelectedValue);

                    if (dtSublicenseRefdoc.Rows.Count > 0)
                    {
                        refdoc.Visible = true;
                        ddl_refdoc.Visible = true;
                        ddl_refdoc.DataSource = dtSublicenseRefdoc;
                        ddl_refdoc.DataBind();
                        ddl_refdoc.DataTextField = "sublicense_refdoc_desc";
                        ddl_refdoc.DataValueField = "sublicense_code";
                        ddl_refdoc.DataBind();
                    }
                }
                contact_agency.Text = res.Rows[0]["contact_agency"].ToString();
                attorney_name.Text = res.Rows[0]["attorney_name"].ToString();
                company.Text = GetCompanyNameByBuCode(type_project.SelectedValue);
                cb_urgent.Checked = Convert.ToBoolean(res.Rows[0]["isurgent"].ToString());
                urgent_remark.Text = res.Rows[0]["urgent_remark"].ToString();
                if (cb_urgent.Checked)
                {
                    urgent_remark.Enabled = true;
                }
                else 
                {
                    urgent_remark.Enabled = false;
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
        protected void type_project_Changed(object sender, EventArgs e)
        {
            company.Text = GetCompanyNameByBuCode(type_project.SelectedValue.ToString());
        }

        protected void ddl_type_requester_Changed(object sender, EventArgs e)
        {
            if (type_requester.SelectedValue == "03")
            {
                tof_requester_other_desc.Enabled = true;
            }
            else
            {
                tof_requester_other_desc.Text = string.Empty;
                tof_requester_other_desc.Enabled = false;
            }

            type_project.DataSource = GetListBuByTypeReq(type_requester.SelectedValue);
            type_project.DataBind();
            type_project.DataTextField = "bu_desc";
            type_project.DataValueField = "bu_code";
            type_project.DataBind();

            company.Text = GetCompanyNameByBuCode(type_project.SelectedValue.ToString());

        }
        public DataTable GetListBuByTypeReq(string tof_reqid)
        {
            string sql = "";
            if (tof_reqid == "01")
            {
                sql = "select * from li_business_unit where bu_type in ('C','RW&WH') and isactive=1 order by row_sort asc";
            }
            else if (tof_reqid == "02")
            {
                sql = "select * from li_business_unit where bu_type in ('H') and isactive=1 order by row_sort asc";
            }
            else
            {
                sql = "select * from li_business_unit where isactive=1 order by row_sort asc";
            }
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);

            return dt;
        }

        protected void type_req_license_Changed(object sender, EventArgs e)
        {
            if (type_req_license.SelectedValue == "04" || type_req_license.SelectedValue == "03")
            {
                tof_permitreq_other_desc.Enabled = true;
                license_code.Enabled = false;
                ddl_sublicense.Visible = false;
                refdoc.Visible = false;
                ddl_refdoc.Visible = false;
            }
            else
            {
                license_code.Enabled = true;
                ddl_sublicense.Visible = true;
                refdoc.Visible = true;
                ddl_refdoc.Visible = true;
                tof_permitreq_other_desc.Enabled = false;
                tof_permitreq_other_desc.Text = string.Empty;
            }
        }

        protected void ddl_license_Changed(object sender, EventArgs e)
        {
            var dtSublicense = GetSubPermitLicense(license_code.SelectedValue);

            if (dtSublicense.Rows.Count > 0)
            {
                ddl_sublicense.Visible = true;
                ddl_sublicense.DataSource = dtSublicense;
                ddl_sublicense.DataBind();
                ddl_sublicense.DataTextField = "sublicense_desc";
                ddl_sublicense.DataValueField = "sublicense_code";
                ddl_sublicense.DataBind();

                var dtSublicenseRefdoc = GetSubPermitLicenseRefDoc(ddl_sublicense.SelectedValue);

                if (dtSublicenseRefdoc.Rows.Count > 0)
                {
                    refdoc.Visible = true;
                    ddl_refdoc.Visible = true;
                    ddl_refdoc.DataSource = dtSublicenseRefdoc;
                    ddl_refdoc.DataBind();
                    ddl_refdoc.DataTextField = "sublicense_refdoc_desc";
                    ddl_refdoc.DataValueField = "sublicense_code";
                    ddl_refdoc.DataBind();
                }
                else
                {
                    refdoc.Visible = false;
                    ddl_refdoc.Visible = false;
                }
            }
            else
            {
                ddl_sublicense.Visible = false;
                refdoc.Visible = false;
                ddl_refdoc.Visible = false;
            }
        }

        protected void ddl_sublicense_Changed(object sender, EventArgs e)
        {
            var dtSublicenseRefdoc = GetSubPermitLicenseRefDoc(ddl_sublicense.SelectedValue);

            if (dtSublicenseRefdoc.Rows.Count > 0)
            {
                refdoc.Visible = true;
                ddl_refdoc.Visible = true;
                ddl_refdoc.DataSource = dtSublicenseRefdoc;
                ddl_refdoc.DataBind();
                ddl_refdoc.DataTextField = "sublicense_refdoc_desc";
                ddl_refdoc.DataValueField = "sublicense_code";
                ddl_refdoc.DataBind();
            }
            else
            {
                refdoc.Visible = false;
                ddl_refdoc.Visible = false;
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            int res = UpdateRequest();

            if (res > 0)
            {
                Response.Write("<script>alert('Successfully Udated');</script>");
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
        public DataTable GetTypeOfRequester()
        {
            string sql = "select * from li_type_of_requester order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }
        public DataTable GetBusinessUnit()
        {
            string sql = "select * from li_business_unit where isactive=1 order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }

        public DataTable GetTypeOfPermitLicense()
        {
            string sql = "select [license_code],[license_desc],[license_desc_en],concat(license_desc_en, ' : ', license_desc) as [license_desc_all],[row_sort] from [li_permit_license] order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }

        public DataTable GetSubPermitLicense(string xlicense_code)
        {
            string sql = @"select license.[license_code]
                                  ,license.[sublicense_code]
	                              ,sub.[sublicense_desc]
                                  ,license.[row_sort]
                              from [li_permit_group_license] as license
                            inner join li_permit_sublicense as sub on sub.sublicense_code = license.sublicense_code
                            where license.[license_code] = '" + xlicense_code + "' order by license.[row_sort] asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }
        public DataTable GetSubPermitLicenseRefDoc(string xsublicense_code)
        {
            string sql = @"select * from li_permit_sublicense_refdoc
                            where sublicense_code = '" + xsublicense_code + "' order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }
        private int UpdateRequest()
        {
            int ret = 0;

            var xpermit_no = req_no.Text.Trim();
            var xtof_requester_code = type_requester.SelectedValue;
            var xtof_requester_other_desc = tof_requester_other_desc.Text.Trim();
            var xpermit_updatedate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var xproject_code = type_project.SelectedValue;
            var xtof_permitreq_code = type_req_license.SelectedValue;
            var xtof_permitreq_other_desc = tof_permitreq_other_desc.Text.Trim();
            var xlicense_code = license_code.SelectedValue;
            var xsublicense_code = "";
            var xresponsible_phone = responsible_phone.Text.Trim();
            if (license_code.SelectedValue != "11" || license_code.SelectedValue != "13") 
            {
                xsublicense_code = ddl_sublicense.SelectedValue;
            }
            var xpermit_subject = permit_subject.Text.Trim();
            var xpermit_desc = permit_desc.Text.Trim();
            var xcontact_agency = contact_agency.Text.Trim();
            var xattorney_name = attorney_name.Text.Trim();
            var xnumber_of_licenses = number_of_licenses.Text.Trim();
            var xcb_urgent = cb_urgent.Checked;
            var xurgent_remark = urgent_remark.Text.Trim();

            string sql = @"UPDATE [dbo].[li_permit_request]
                           SET [permit_subject] = '" + xpermit_subject + @"'
                              ,[permit_desc] = '" +xpermit_desc+ @"'
                              ,[tof_requester_code] = '"+xtof_requester_code+@"'
                              ,[tof_requester_other_desc] = '"+xtof_requester_other_desc+@"'
                              ,[tof_permitreq_code] = '"+xtof_permitreq_code+@"'
                              ,[tof_permitreq_other_desc] = '"+xtof_permitreq_other_desc+@"'
                              ,[license_code] = '"+xlicense_code+@"'
                              ,[sublicense_code] = '"+xsublicense_code+@"'
                              ,[contact_agency] = '"+xcontact_agency+@"'
                              ,[attorney_name] = '"+xattorney_name+@"'
                              ,[bu_code] = '"+xproject_code+@"'
                              ,[updated_datetime] = '"+xpermit_updatedate+ @"'
                              ,[responsible_phone] = '" + xresponsible_phone+ @"'
                              ,[number_of_licenses] = '" + xnumber_of_licenses + @"'
                              ,[isurgent] = '" + xcb_urgent + @"'
                              ,[urgent_remark] = '" + xurgent_remark + @"'
                         WHERE [permit_no] = '"+ xpermit_no + "'";

            ret = zdb.ExecNonQueryReturnID(sql, zconnstr);


            return ret;
        }

        private void GenDocumnet()
        {
            // Replace Doc
            var xdoc_no = doc_no.Text.Trim();
            //var xprocess_id = hid_PID.Value.ToString();
            var xreq_date = Utillity.ConvertStringToDate(req_date.Value);

            var path_template = ConfigurationManager.AppSettings["WT_Template_permit"].ToString();
            string templatefile = path_template + @"\PermitTemplate.docx";
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\permit_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            #region gentagstr data form
            ReplacePermit_TagData data = new ReplacePermit_TagData();

            data.docno = xdoc_no.Replace(",", "!comma");
            data.reqdate = Utillity.ConvertDateToLongDateTime(xreq_date, "th");
            data.responsible_phone = responsible_phone.Text.Trim();
            if (cb_urgent.Checked)
            {
                data.cb_urgent = "☑";
            }
            else 
            {
                data.cb_urgent = "☐";
            }
            data.urgent_remark = urgent_remark.Text.Trim();
            var xrequester_code = type_requester.SelectedValue;
            if (xrequester_code == "01")
            {
                data.r1 = "☑";
                data.r2 = "☐";
                data.r3 = "☐";
            }
            else if (xrequester_code == "02")
            {
                data.r1 = "☐";
                data.r2 = "☑";
                data.r3 = "☐";
            }
            else if (xrequester_code == "03")
            {
                data.r1 = "☐";
                data.r2 = "☐";
                data.r3 = "☑";
            }
            data.req_other = tof_requester_other_desc.Text.Trim();
            var proceed_by = "";
            var approved_by = "";
            ///get gm am heam_am
            string sqlbu = @"select * from li_business_unit where bu_code = '" + type_project.SelectedValue + "'";
            var resbu = zdb.ExecSql_DataTable(sqlbu, zconnstr);
            if (resbu.Rows.Count > 0)
            {
                string xexternal_domain = resbu.Rows[0]["external_domain"].ToString();
                string xgm = resbu.Rows[0]["gm"].ToString();
                string xam = resbu.Rows[0]["am"].ToString();
                string xhead_am = resbu.Rows[0]["head_am"].ToString();

                if (Session["user_login"] != null)
                {
                    var xlogin_name = Session["user_login"].ToString();
                    var empFunc = new EmpInfo();

                    //get data user
                    if (xexternal_domain == "Y")
                    {
                        //Hotel get am
                        var empam = empFunc.getEmpInfo(xam);
                        if (!string.IsNullOrEmpty(empam.full_name_en))
                        {
                            proceed_by = empam.full_name_en;
                        }

                        //Hotel get head am
                        var empheam_am = empFunc.getEmpInfo(xhead_am);
                        if (!string.IsNullOrEmpty(empheam_am.full_name_en))
                        {
                            approved_by = empheam_am.full_name_en;
                        }
                    }
                    else
                    {
                        //get requester
                        var emp = empFunc.getEmpInfo(xlogin_name);
                        if (!string.IsNullOrEmpty(emp.full_name_en))
                        {
                            proceed_by = emp.full_name_en;
                        }

                        //get gm
                        var empgm = empFunc.getEmpInfo(xgm);
                        if (!string.IsNullOrEmpty(empgm.full_name_en))
                        {
                            approved_by = empgm.full_name_en;
                        }
                    }

                }

            }

            data.name1 = proceed_by;
            data.signdate1 = "";
            data.name2 = approved_by;
            data.signdate2 = "";

            data.subject = permit_subject.Text.Trim();
            data.bu_name = type_project.SelectedItem.Text.Trim();

            var xtof_permitreq_code = type_req_license.SelectedValue;
            if (xtof_permitreq_code == "01")
            {
                data.t1 = "☑";
                data.t2 = "☐";
                data.t3 = "☐";
                data.t4 = "☐";
                data.t5 = "☐";
                data.t6 = "☐";
                data.t7 = "☐";
                data.t8 = "☐";
                data.t9 = "☐";
                data.t10 = "☐";
                data.t11 = "☐";
                data.t12 = "☐";
                data.t13 = "☐";
            }
            else if (xtof_permitreq_code == "02")
            {
                data.t1 = "☐";
                data.t2 = "☑";
                data.t3 = "☐";
                data.t4 = "☐";
                data.t5 = "☐";
                data.t6 = "☐";
                data.t7 = "☐";
                data.t8 = "☐";
                data.t9 = "☐";
                data.t10 = "☐";
                data.t11 = "☐";
                data.t12 = "☐";
                data.t13 = "☐";
            }
            else if (xtof_permitreq_code == "03")
            {
                data.t1 = "☐";
                data.t2 = "☐";
                data.t3 = "☑";
                data.t4 = "☐";
                data.t5 = "☐";
                data.t6 = "☐";
                data.t7 = "☐";
                data.t8 = "☐";
                data.t9 = "☐";
                data.t10 = "☐";
                data.t11 = "☐";
                data.t12 = "☐";
                data.t13 = "☐";
            }
            else if (xtof_permitreq_code == "04")
            {
                data.t1 = "☐";
                data.t2 = "☐";
                data.t3 = "☐";
                data.t4 = "☑";
                data.t5 = "☐";
                data.t6 = "☐";
                data.t7 = "☐";
                data.t8 = "☐";
                data.t9 = "☐";
                data.t10 = "☐";
                data.t11 = "☐";
                data.t12 = "☐";
                data.t13 = "☐";
                //data.license_other = tof_permitreq_other_desc.Text.Trim();
            }
            else if (xtof_permitreq_code == "05")
            {
                data.t1 = "☐";
                data.t2 = "☐";
                data.t3 = "☐";
                data.t4 = "☐";
                data.t5 = "☑";
                data.t6 = "☐";
                data.t7 = "☐";
                data.t8 = "☐";
                data.t9 = "☐";
                data.t10 = "☐";
                data.t11 = "☐";
                data.t12 = "☐";
                data.t13 = "☐";
            }
            else if (xtof_permitreq_code == "06")
            {
                data.t1 = "☐";
                data.t2 = "☐";
                data.t3 = "☐";
                data.t4 = "☐";
                data.t5 = "☐";
                data.t6 = "☑";
                data.t7 = "☐";
                data.t8 = "☐";
                data.t9 = "☐";
                data.t10 = "☐";
                data.t11 = "☐";
                data.t12 = "☐";
                data.t13 = "☐";
            }
            else if (xtof_permitreq_code == "07")
            {
                data.t1 = "☐";
                data.t2 = "☐";
                data.t3 = "☐";
                data.t4 = "☐";
                data.t5 = "☐";
                data.t6 = "☐";
                data.t7 = "☑";
                data.t8 = "☐";
                data.t9 = "☐";
                data.t10 = "☐";
                data.t11 = "☐";
                data.t12 = "☐";
                data.t13 = "☐";
                //data.tax_other = tof_permitreq_other_desc.Text.Trim();
            }
            else if (xtof_permitreq_code == "08")
            {
                data.t1 = "☐";
                data.t2 = "☐";
                data.t3 = "☐";
                data.t4 = "☐";
                data.t5 = "☐";
                data.t6 = "☐";
                data.t7 = "☐";
                data.t8 = "☑";
                data.t9 = "☐";
                data.t10 = "☐";
                data.t11 = "☐";
                data.t12 = "☐";
                data.t13 = "☐";
            }
            else if (xtof_permitreq_code == "09")
            {
                data.t1 = "☐";
                data.t2 = "☐";
                data.t3 = "☐";
                data.t4 = "☐";
                data.t5 = "☐";
                data.t6 = "☐";
                data.t7 = "☐";
                data.t8 = "☐";
                data.t9 = "☑";
                data.t10 = "☐";
                data.t11 = "☐";
                data.t12 = "☐";
                data.t13 = "☐";
                //data.trademarks_other = tof_permitreq_other_desc.Text.Trim();
            }
            else if (xtof_permitreq_code == "10")
            {
                data.t1 = "☐";
                data.t2 = "☐";
                data.t3 = "☐";
                data.t4 = "☐";
                data.t5 = "☐";
                data.t6 = "☐";
                data.t7 = "☐";
                data.t8 = "☐";
                data.t9 = "☐";
                data.t10 = "☑";
                data.t11 = "☐";
                data.t12 = "☐";
                data.t13 = "☐";
            }
            else if (xtof_permitreq_code == "11")
            {
                data.t1 = "☐";
                data.t2 = "☐";
                data.t3 = "☐";
                data.t4 = "☐";
                data.t5 = "☐";
                data.t6 = "☐";
                data.t7 = "☐";
                data.t8 = "☐";
                data.t9 = "☐";
                data.t10 = "☐";
                data.t11 = "☑";
                data.t12 = "☐";
                data.t13 = "☐";
            }
            else if (xtof_permitreq_code == "12")
            {
                data.t1 = "☐";
                data.t2 = "☐";
                data.t3 = "☐";
                data.t4 = "☐";
                data.t5 = "☐";
                data.t6 = "☐";
                data.t7 = "☐";
                data.t8 = "☐";
                data.t9 = "☐";
                data.t10 = "☐";
                data.t11 = "☐";
                data.t12 = "☑";
                data.t13 = "☐";
            }
            else if (xtof_permitreq_code == "13")
            {
                data.t1 = "☐";
                data.t2 = "☐";
                data.t3 = "☐";
                data.t4 = "☐";
                data.t5 = "☐";
                data.t6 = "☐";
                data.t7 = "☐";
                data.t8 = "☐";
                data.t9 = "☐";
                data.t10 = "☐";
                data.t11 = "☐";
                data.t12 = "☐";
                data.t13 = "☑";
            }

            data.desc_req = permit_desc.Text.Trim();
            data.contact_agency = contact_agency.Text.Trim();
            data.attorney_name = attorney_name.Text.Trim();
            data.list_doc_attach = "ตรวจสอบเอกสารแนบได้ที่ระบบ";


            DataTable dtStr = zreplacepermit.genTagData(data);
            #endregion


            // Convert to JSONString
            //DataTable dtTagPropsTable = new DataTable();
            //dtTagPropsTable.Columns.Add("tagname", typeof(string));
            //dtTagPropsTable.Columns.Add("jsonstring", typeof(string));

            //DataTable dtTagDataTable = new DataTable();
            //dtTagDataTable.Columns.Add("tagname", typeof(string));
            //dtTagDataTable.Columns.Add("jsonstring", typeof(string));
            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
            //var jsonDTProperties1 = repl.DataTableToJSONWithStringBuilder(dtProperties1);
            //var jsonDTdata = repl.DataTableToJSONWithStringBuilder(dt);
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
            pdf_render.Attributes["src"] = host_url + "render/pdf?id=" + filePath;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            // Sample Submit
            string process_code = "PMT_LIC";
            int version_no = 1;
            string xbu_code = type_project.SelectedValue.Trim();

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
                wfAttr.subject = "เรื่อง " + permit_subject.Text.Trim();
                wfAttr.assto_login = emp.next_line_mgr_login;
                wfAttr.wf_status = "SUBMITTED";
                wfAttr.submit_answer = "SUBMITTED";
                wfAttr.submit_by = emp.user_login;

                wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, lblPID.Text, xbu_code);
                wfAttr.updated_by = emp.user_login;
                wfAttr.division = emp.division;
                // wf.updateProcess
                var wfA_NextStep = zwf.updateProcess(wfAttr);
                //wfA_NextStep.next_assto_login = emp.next_line_mgr_login;
                wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by, lblPID.Text, xbu_code);
                string status = zwf.Insert_NextStep(wfA_NextStep);

                if (status == "Success")
                {
                    GenDocumnetPermit(lblPID.Text);
                    //send mail
                    string subject = "";
                    string body = "";
                    string sqlmail = @"select * from li_permit_request where process_id = '" + wfAttr.process_id + "'";
                    var dt = zdb.ExecSql_DataTable(sqlmail, zconnstr);
                    if (dt.Rows.Count > 0)
                    {
                        var dr = dt.Rows[0];
                        string id = dr["permit_no"].ToString();
                        subject = wfAttr.subject;
                        var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                        body = "คุณได้รับมอบหมายให้ตรวจสอบเอกสารเลขที่ " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a> <br/>" +
                                "You have been assigned to check document no " + dr["document_no"].ToString() + " Please check and proceed through the system <a target='_blank' href='" + host_url_sendmail + "legalportal/legalportal?m=myworklist'>Click</a>";

                        string pathfileins = "";

                        string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                        var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                        if (resfile.Rows.Count > 0)
                        {
                            pathfileins = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");

                            string email = "";
                            string[] emails;
                            string[] ccemails;

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
                                ccemails = new string[] { "pornsawan.s@assetworldcorp-th.com", "naruemol.w@assetworldcorp-th.com", "kanita.s@assetworldcorp-th.com", "pattanis.r@assetworldcorp-th.com", "suradach.k@assetworldcorp-th.com", "pichet.ku@assetworldcorp-th.com" };

                            }
                            else
                            {
                                ////fix mail test
                                email = "legalwfuat2024@gmail.com";
                                ccemails = new string[] { "worawut.m@assetworldcorp-th.com", "manit.ch@assetworldcorp-th.com" };
                            }

                            if (!string.IsNullOrEmpty(email))
                            {
                                //_ = zsendmail.sendEmail(subject + " Mail To Next Appove", email, body, pathfileins);
                                emails = new string[] { email };
                                _ = zsendmail.sendEmailsCCs(subject + " Mail To Next Appove", emails, ccemails, body, pathfileins);

                                if (cb_urgent.Checked)
                                {
                                    sendMailUrgentToPermit(lblPID.Text, wfAttr.subject);
                                }
                            }

                        }

                    }

                    var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                    Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);

                    
                }

            }
        }

        private void GenDocumnetPermit(string pid)
        {
            string xreq_no = "";
            var path_template = ConfigurationManager.AppSettings["WT_Template_permit"].ToString();
            string templatefile = path_template + @"\PermitTemplate.docx";
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\permit_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            string sqlpermit = "select * from li_permit_request where process_id='" + pid + "'";
            var respermit= zdb.ExecSql_DataTable(sqlpermit, zconnstr);

            #region gentagstr data form
            ReplacePermit_TagData data = new ReplacePermit_TagData();

            if (respermit.Rows.Count > 0)
            {
                xreq_no = respermit.Rows[0]["permit_no"].ToString();
                string xbu_code = respermit.Rows[0]["bu_code"].ToString();
                var proceed_by = "";
                var approved_by = "";
                ///get gm am heam_am
                string sqlbu = @"select * from li_business_unit where bu_code = '" + xbu_code + "'";
                var resbu = zdb.ExecSql_DataTable(sqlbu, zconnstr);
                if (resbu.Rows.Count > 0)
                {
                    string xexternal_domain = resbu.Rows[0]["external_domain"].ToString();
                    string xgm = resbu.Rows[0]["gm"].ToString();
                    string xam = resbu.Rows[0]["am"].ToString();
                    string xhead_am = resbu.Rows[0]["head_am"].ToString();

                    if (Session["user_login"] != null)
                    {
                        var xlogin_name = Session["user_login"].ToString();
                        var empFunc = new EmpInfo();

                        //get data user
                        if (xexternal_domain == "Y")
                        {
                            //Hotel get am
                            var empam = empFunc.getEmpInfo(xam);
                            if (!string.IsNullOrEmpty(empam.full_name_en))
                            {
                                proceed_by = empam.full_name_en;
                            }

                            //Hotel get head am
                            var empheam_am = empFunc.getEmpInfo(xhead_am);
                            if (!string.IsNullOrEmpty(empheam_am.full_name_en))
                            {
                                approved_by = empheam_am.full_name_en;
                            }
                        }
                        else
                        {
                            //get requester
                            var emp = empFunc.getEmpInfo(xlogin_name);
                            if (!string.IsNullOrEmpty(emp.full_name_en))
                            {
                                proceed_by = emp.full_name_en;
                            }

                            //get gm
                            var empgm = empFunc.getEmpInfo(xgm);
                            if (!string.IsNullOrEmpty(empgm.full_name_en))
                            {
                                approved_by = empgm.full_name_en;
                            }
                        }

                    }

                }

                data.name1 = proceed_by;
                data.signdate1 = "";
                data.name2 = approved_by;
                data.signdate2 = "";
            }

            DataTable dtStr = zreplacepermit.BindTagData(pid, data);
            #endregion

            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
            var jsonDTProperties1 = "";
            var jsonDTdata = "";

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

        }

        protected void cb_urgent_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_urgent.Checked)
            {
                urgent_remark.Enabled = true;
            }
            else
            {
                urgent_remark.Enabled = false;
                urgent_remark.Text = string.Empty;
            }
        }

        private void sendMailUrgentToPermit(string pid, string xsubject)
        {
            string subject = "";
            string body = "";
            string sql = @"select * from li_permit_request where process_id = '" + pid + "'";
            var dt = zdb.ExecSql_DataTable(sql, zconnstr);
            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                string id = dr["permit_no"].ToString();
                subject = xsubject;
                var host_url_sendmail = ConfigurationManager.AppSettings["host_url"].ToString();
                body = "!!!Urgent คำขอเลขที่ " + dr["process_id"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url_sendmail + "frmpermit/permitworkassign'>Click</a><br/>" +
                    "!!!Urgent Request no" + dr["process_id"].ToString() + " Please check and proceed through the system. <a target='_blank' href='" + host_url_sendmail + "frmpermit/permitworkassign'>Click</a>";

                string pathfile = "";

                string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                if (resfile.Rows.Count > 0)
                {
                    pathfile = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");

                    string[] email;

                    var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                    ////get mail from db
                    if (isdev != "true")
                    {
                        email = new string[] { "pornsawan.s@assetworldcorp-th.com", "naruemol.w@assetworldcorp-th.com", "kanita.s@assetworldcorp-th.com", "pattanis.r@assetworldcorp-th.com", "suradach.k@assetworldcorp-th.com", "pichet.ku@assetworldcorp-th.com" };
                    }
                    else
                    {
                        ////fix mail test
                        email= new string[] { "legalwfuat2024@gmail.com", "manit.ch@assetworldcorp-th.com" };
                    }

                    if (email.Length > 0)
                    {
                        _ = zsendmail.sendEmails(subject + " Mail To Permit", email, body, pathfile);
                    }
                }

            }
        }
    }
}