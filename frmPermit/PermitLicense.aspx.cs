﻿using DocumentFormat.OpenXml.ExtendedProperties;
using iTextSharp.text.pdf;
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
using static onlineLegalWF.Class.ReplacePermit;

namespace onlineLegalWF.frmPermit
{
    public partial class PermitLicense : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        public ReplacePermit zreplacepermit = new ReplacePermit();
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
            ucHeader1.setHeader("License Request");
            string xreq_no = System.DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            req_no.Text = xreq_no;

            string pid = zwf.iniPID("LEGALWF");
            lblPID.Text = pid;
            hid_PID.Value = pid;
            ucAttachment1.ini_object(pid);
            ucCommentlog1.ini_object(pid);

            type_requester.DataSource = GetTypeOfRequester();
            type_requester.DataBind();
            type_requester.DataTextField = "tof_requester_desc";
            type_requester.DataValueField = "tof_requester_code";
            type_requester.DataBind();

            //type_project.DataSource = GetBusinessUnit();
            type_project.DataSource = GetListBuByTypeReq("01");
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
            try
            {
                // Validate
                if (string.IsNullOrEmpty(responsible_phone.Text))
                {
                    showAlertError("alertTitleErr", "Warning! Please input responsible_phone");
                    return;
                }
                if (string.IsNullOrEmpty(permit_subject.Text))
                {
                    showAlertError("alertTitleErr", "Warning! Please input permit_subject");
                    return;
                }
                if (string.IsNullOrEmpty(permit_desc.Text))
                {
                    showAlertError("alertTitleErr", "Warning! Please input permit_desc");
                    return;
                }
                if (string.IsNullOrEmpty(contact_agency.Text))
                {
                    showAlertError("alertTitleErr", "Warning! Please input contact_agency");
                    return;
                }
                if (string.IsNullOrEmpty(attorney_name.Text))
                {
                    showAlertError("alertTitleErr", "Warning! Please input attorney_name");
                    return;
                }
                if (cb_urgent.Checked) 
                {
                    if (string.IsNullOrEmpty(urgent_remark.Text))
                    {
                        showAlertError("alertTitleErr", "Warning! Please input Urgent Remark");
                        return;
                    }
                }

                int res = SaveRequest();

                if (res > 0)
                {
                    // wf save draft
                    string process_code = "PMT_LIC";
                    int version_no = 1;

                    // getCurrentStep
                    var wfAttr = zwf.getCurrentStep(lblPID.Text, process_code, version_no);
                    var xbu_code = type_project.SelectedValue.Trim();

                    // check session_user
                    if (Session["user_login"] != null)
                    {
                        var xlogin_name = Session["user_login"].ToString();
                        var empFunc = new EmpInfo();

                        //get data user
                        var emp = empFunc.getEmpInfo(xlogin_name);

                        // set WF Attributes
                        wfAttr.subject = "เรื่อง " + permit_subject.Text.Trim();
                        wfAttr.wf_status = "SAVE";
                        wfAttr.submit_answer = "SAVE";
                        wfAttr.submit_by = emp.user_login;
                        wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, emp.user_login, lblPID.Text, xbu_code);
                        // wf.updateProcess
                        var wfA_NextStep = zwf.updateProcess(wfAttr);

                    }

                    //Response.Write("<script>alert('Successfully added');</script>");
                    showAlertSuccess("alertSuccess", "Successfully added");
                    var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                    Response.Redirect(host_url + "frmPermit/PermitLicenseEdit.aspx?id=" + req_no.Text.Trim());
                }
                else
                {
                    showAlertError("alertErr", "Error !!!");
                }
            }
            catch (Exception ex)
            {
                showAlertError("alertErr", ex.Message);
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
                            where license.[license_code] = '"+xlicense_code+"' order by license.[row_sort] asc";
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
        private int SaveRequest()
        {
            int ret = 0;

            if (doc_no.Text.Trim() == "")
            {
                doc_no.Text = zwf.genDocNo("DCP-" + System.DateTime.Now.ToString("yyyy", new CultureInfo("en-US")) + "-", 6);
            }
            var xpermit_no = req_no.Text.Trim();
            var xprocess_id = hid_PID.Value.ToString();
            var xdoc_no = doc_no.Text.Trim();
            var xtof_requester_code = type_requester.SelectedValue;
            var xtof_requester_other_desc = "";
            var xpermit_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var xproject_code = type_project.SelectedValue;
            var xtof_permitreq_code = type_req_license.SelectedValue;
            var xtof_permitreq_other_desc = "";
            var xlicense_code = license_code.SelectedValue;
            var xsublicense_code = ddl_sublicense.SelectedValue;
            var xpermit_desc = permit_desc.Text.Trim();
            var xcontact_agency = contact_agency.Text.Trim();
            var xattorney_name = attorney_name.Text.Trim();
            var xstatus = "verify";
            var xsubject = permit_subject.Text.Trim();
            var xresponsible_phone = responsible_phone.Text.Trim();
            var xnumber_of_licenses = number_of_licenses.Text.Trim();
            var xcb_urgent = cb_urgent.Checked;
            var xurgent_remark = urgent_remark.Text.Trim();

            string sql = "";

            if (type_requester.SelectedValue == "03") 
            {
                xtof_requester_other_desc = tof_requester_other_desc.Text.Trim();
            }

            if (type_req_license.SelectedValue == "04")
            {
                xtof_permitreq_other_desc = tof_permitreq_other_desc.Text.Trim();
            }

            if (license_code.SelectedValue == "11" || license_code.SelectedValue == "13")
            {
                sql = @"INSERT INTO [dbo].[li_permit_request]
                                   ([process_id],[permit_no],[document_no],[permit_date],[permit_subject],[permit_desc],[tof_requester_code],[tof_requester_other_desc],[bu_code],[tof_permitreq_code],[tof_permitreq_other_desc],[license_code],[contact_agency],[attorney_name],[responsible_phone],[number_of_licenses],[isurgent],[urgent_remark],[status])
                             VALUES
                                   ('" + xprocess_id + @"'
                                   ,'" + xpermit_no + @"'
                                   ,'" + xdoc_no + @"'
                                   ,'" + xpermit_date + @"'
                                   ,'" + xsubject + @"'
                                   ,'" + xpermit_desc + @"'
                                   ,'" + xtof_requester_code + @"'
                                   ,'" + xtof_requester_other_desc + @"'
                                   ,'" + xproject_code + @"'
                                   ,'" + xtof_permitreq_code + @"'
                                   ,'" + xtof_permitreq_other_desc + @"'
                                   ,'" + xlicense_code + @"'
                                   ,'" + xcontact_agency + @"'
                                   ,'" + xattorney_name + @"'
                                   ,'" + xresponsible_phone + @"'
                                   ,'" + xnumber_of_licenses + @"'
                                   ,'" + xcb_urgent + @"'
                                   ,'" + xurgent_remark + @"'
                                   ,'" + xstatus + @"')";
            }
            else 
            {
                sql = @"INSERT INTO [dbo].[li_permit_request]
                                   ([process_id],[permit_no],[document_no],[permit_date],[permit_subject],[permit_desc],[tof_requester_code],[tof_requester_other_desc],[bu_code],[tof_permitreq_code],[tof_permitreq_other_desc],[license_code],[sublicense_code],[contact_agency],[attorney_name],[responsible_phone],[number_of_licenses],[isurgent],[urgent_remark],[status])
                             VALUES
                                   ('" + xprocess_id + @"'
                                   ,'" + xpermit_no + @"'
                                   ,'" + xdoc_no + @"'
                                   ,'" + xpermit_date + @"'
                                   ,'" + xsubject + @"'
                                   ,'" + xpermit_desc + @"'
                                   ,'" + xtof_requester_code + @"'
                                   ,'" + xtof_requester_other_desc + @"'
                                   ,'" + xproject_code + @"'
                                   ,'" + xtof_permitreq_code + @"'
                                   ,'" + xtof_permitreq_other_desc + @"'
                                   ,'" + xlicense_code + @"'
                                   ,'" + xsublicense_code + @"'
                                   ,'" + xcontact_agency + @"'
                                   ,'" + xattorney_name + @"'
                                   ,'" + xresponsible_phone + @"'
                                   ,'" + xnumber_of_licenses + @"'
                                   ,'" + xcb_urgent + @"'
                                   ,'" + xurgent_remark + @"'
                                   ,'" + xstatus + @"')";
            }

            

            ret = zdb.ExecNonQueryReturnID(sql, zconnstr);


            return ret;
        }

        private void GenDocumnet()
        {
            // Replace Doc
            var xdoc_no = doc_no.Text.Trim();
            //var xprocess_id = hid_PID.Value.ToString();
            var xreq_date = System.DateTime.Now;

            var path_template = ConfigurationManager.AppSettings["WT_Template_permit"].ToString();
            string templatefile = path_template + @"\PermitTemplate.docx";
            string outputfolder = path_template + @"\Output";
            string outputfn = outputfolder + @"\permit_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();

            #region gentagstr data form
            ReplacePermit_TagData data = new ReplacePermit_TagData();

            data.docno = xdoc_no.Replace(",", "!comma");
            data.reqdate = Utillity.ConvertDateToLongDateTime(xreq_date, "th");
            var xrequester_code = type_requester.SelectedValue;
            data.req_other = "";
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
                data.req_other = tof_requester_other_desc.Text.Trim();
            }
            
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
            data.license_other = "";
            data.tax_other = "";
            data.trademarks_other = "";

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
                data.license_other = tof_permitreq_other_desc.Text.Trim();
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
                data.tax_other = tof_permitreq_other_desc.Text.Trim();
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
                data.trademarks_other = tof_permitreq_other_desc.Text.Trim();
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
            }

            data.desc_req = permit_desc.Text.Trim();
            data.contact_agency = contact_agency.Text.Trim();
            data.attorney_name = attorney_name.Text.Trim();
            data.list_doc_attach = "ตรวจสอบเอกสารแนบได้ที่ระบบ";


            DataTable dtStr = zreplacepermit.genTagData(data);
            #endregion


            // Convert to JSONString
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
            pdf_render.Attributes["src"] = host_url + "render/pdf?id=" + filePath;
        }
        public void showAlertSuccess(string key, string msg)
        {
            ClientScript.RegisterStartupScript(GetType(), key, "showAlertSuccess('" + msg + "');", true);
        }

        public void showAlertError(string key, string msg)
        {
            ClientScript.RegisterStartupScript(GetType(), key, "showAlertError('" + msg + "');", true);
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
    }
}