﻿using iTextSharp.text.pdf;
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
        public WFFunctions zwf = new WFFunctions();
        public ReplacePermit zreplacepermit = new ReplacePermit();
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

            type_requester.DataSource = GetTypeOfRequester();
            type_requester.DataBind();
            type_requester.DataTextField = "tof_requester_desc";
            type_requester.DataValueField = "tof_requester_code";
            type_requester.DataBind();

            type_project.DataSource = GetBusinessUnit();
            type_project.DataBind();
            type_project.DataTextField = "bu_desc";
            type_project.DataValueField = "bu_code";
            type_project.DataBind();

            license_code.DataSource = GetTypeOfPermitLicense();
            license_code.DataBind();
            license_code.DataTextField = "license_desc";
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

            string sql = "select * from li_permit_request where permit_no='" + id + "'";

            var res = zdb.ExecSql_DataTable(sql, zconnstr);

            if (res.Rows.Count > 0) 
            {
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
            }


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
            string sql = "select * from li_permit_license order by row_sort asc";
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
            if (license_code.SelectedValue != "11" || license_code.SelectedValue != "13") 
            {
                xsublicense_code = ddl_sublicense.SelectedValue;
            }
            var xpermit_subject = permit_subject.Text.Trim();
            var xpermit_desc = permit_desc.Text.Trim();
            var xcontact_agency = contact_agency.Text.Trim();
            var xattorney_name = attorney_name.Text.Trim();

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
                              ,[updated_datetime] = '"+xpermit_updatedate+@"'
                         WHERE [permit_no] = '"+ xpermit_no + "'";

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
    }
}