using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using onlineLegalWF.Class;
using System.Configuration;
using System.Globalization;

namespace onlineLegalWF.frmLitigation
{
    public partial class LitigationRequestEditByAdmin : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public WFFunctions zwf = new WFFunctions();
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
            ucHeader1.setHeader("Litigation Edit Request");

            type_req.DataSource = GetTypeOfRequest();
            type_req.DataBind();
            type_req.DataTextField = "tof_litigationreq_desc";
            type_req.DataValueField = "tof_litigationreq_code";
            type_req.DataBind();

            ddl_bu.DataSource = GetBusinessUnit();
            ddl_bu.DataBind();
            ddl_bu.DataTextField = "bu_desc";
            ddl_bu.DataValueField = "bu_code";
            ddl_bu.DataBind();

            string sql = "select * from li_litigation_request where process_id='" + id + "'";

            var res = zdb.ExecSql_DataTable(sql, zconnstr);
            if (res.Rows.Count > 0)
            {

                req_no.Text = res.Rows[0]["req_no"].ToString();
                req_date.Value = Convert.ToDateTime(res.Rows[0]["req_date"]).ToString("yyyy-MM-dd");
                doc_no.Text = res.Rows[0]["document_no"].ToString();
                type_req.SelectedValue = res.Rows[0]["tof_litigationreq_code"].ToString();
                if (type_req.SelectedValue == "01")
                {
                    pro_occ_section.Visible = false;
                    section_bu.Visible = false;
                    section_company.Visible = false;
                }
                else
                {
                    pro_occ_section.Visible = true;
                    section_bu.Visible = true;
                    section_company.Visible = true;
                    if (!string.IsNullOrEmpty(res.Rows[0]["bu_code"].ToString()))
                    {
                        ddl_bu.SelectedValue = res.Rows[0]["bu_code"].ToString();
                    }
                }

                subject.Text = res.Rows[0]["lit_subject"].ToString();
                desc.Text = res.Rows[0]["lit_desc"].ToString();
                company.Text = res.Rows[0]["company_name"].ToString();
                
                pro_occ_desc.Text = res.Rows[0]["pro_occ_desc"].ToString();

                string sqlcase = "select * from li_litigation_req_case where req_no='" + res.Rows[0]["req_no"].ToString() + "'";
                var rescase = zdb.ExecSql_DataTable(sqlcase, zconnstr);

                if (rescase.Rows.Count > 0)
                {
                    row_gv_data.Visible = true;
                    List<LitigationCivilCaseData> listCivilCaseData = new List<LitigationCivilCaseData>();
                    var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                    foreach (DataRow item in rescase.Rows)
                    {
                        string xurl_detail = host_url + "frmlitigation/litigationdetail.aspx?id=" + item["case_no"].ToString();
                        LitigationCivilCaseData civilCaseData = new LitigationCivilCaseData();
                        civilCaseData.req_no = item["req_no"].ToString();
                        civilCaseData.case_no = item["case_no"].ToString();
                        civilCaseData.no = item["no"].ToString();
                        civilCaseData.contract_no = item["contract_no"].ToString();
                        civilCaseData.bu_name = item["bu_name"].ToString();
                        civilCaseData.customer_no = item["customer_no"].ToString();
                        civilCaseData.customer_name = item["customer_name"].ToString();
                        civilCaseData.customer_room = item["customer_room"].ToString();
                        civilCaseData.overdue_desc = item["overdue_desc"].ToString();
                        civilCaseData.outstanding_debt = item["outstanding_debt"].ToString();
                        civilCaseData.outstanding_debt_ack_of_debt = item["outstanding_debt_ack_of_debt"].ToString();
                        civilCaseData.fine_debt = item["fine_debt"].ToString();
                        civilCaseData.total_net = item["total_net"].ToString();
                        civilCaseData.retention_money = item["retention_money"].ToString();
                        civilCaseData.total_after_retention_money = item["total_after_retention_money"].ToString();
                        civilCaseData.remark = item["remark"].ToString();
                        civilCaseData.status = item["status"].ToString();
                        civilCaseData.assto_login = item["assto_login"].ToString();
                        civilCaseData.url_detail = xurl_detail;
                        listCivilCaseData.Add(civilCaseData);
                    }
                    gvExcelFile.DataSource = listCivilCaseData;
                    gvExcelFile.DataBind();
                }

                string pid = res.Rows[0]["process_id"].ToString();
                lblPID.Text = pid;
                hid_PID.Value = pid;
                ucAttachment1.ini_object(pid);
                ucCommentlog1.ini_object(pid);
            }
        }

        public DataTable GetBusinessUnit()
        {
            string sql = "select * from li_business_unit where isactive=1 order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
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
        public DataTable GetTypeOfRequest()
        {
            string sql = "select * from li_type_of_litigationrequest order by row_sort asc";
            DataTable dt = zdb.ExecSql_DataTable(sql, zconnstr);
            return dt;
        }

        public class LitigationCivilCaseData
        {
            public string req_no { get; set; }
            public string case_no { get; set; }
            public string no { get; set; }
            public string contract_no { get; set; }
            public string bu_name { get; set; }
            public string customer_no { get; set; }
            public string customer_name { get; set; }
            public string customer_room { get; set; }
            public string overdue_desc { get; set; }
            public string outstanding_debt { get; set; }
            public string outstanding_debt_ack_of_debt { get; set; }
            public string fine_debt { get; set; }
            public string total_net { get; set; }
            public string retention_money { get; set; }
            public string total_after_retention_money { get; set; }
            public string remark { get; set; }
            public string status { get; set; }
            public string assto_login { get; set; }
            public string url_detail { get; set; }
        }

        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "openModal")
            {
                int i = System.Convert.ToInt32(e.CommandArgument);
                var xcase_no = ((HiddenField)gvExcelFile.Rows[i].FindControl("gv_case_no")).Value.ToString();
                var xcontract_no = ((Label)gvExcelFile.Rows[i].FindControl("gv_contract_no")).Text.ToString();
                var xcustomer_no = ((Label)gvExcelFile.Rows[i].FindControl("gv_customer_no")).Text.ToString();
                var xcustomer_name = ((Label)gvExcelFile.Rows[i].FindControl("gv_customer_name")).Text.ToString();

                ucLitigationCaseAttachment1.ini_object(xcase_no, xcontract_no, xcustomer_no, xcustomer_name);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalEditData();", true);
            }
            else if(e.CommandName == "openModalAssign")
            {
                int i = System.Convert.ToInt32(e.CommandArgument);
                var xcase_no = ((HiddenField)gvExcelFile.Rows[i].FindControl("gv_case_no")).Value.ToString();
                var xassto_login = ((Label)gvExcelFile.Rows[i].FindControl("gv_assto_login")).Text.ToString();
                var xstatus = ((Label)gvExcelFile.Rows[i].FindControl("gv_status")).Text.ToString();
                hid_case_no.Value = xcase_no;
                if (!string.IsNullOrEmpty(xassto_login))
                {
                    ddlNameList.SelectedValue = xassto_login;
                }

                rdlAction.SelectedValue = xstatus;

                btn_update_modal.Visible = true;
                btn_update_all_modal.Visible = false;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalAssign();", true);
            }
            else if (e.CommandName == "openNewTab")
            {
                int i = System.Convert.ToInt32(e.CommandArgument);
                var xurl_detail = ((HiddenField)gvExcelFile.Rows[i].FindControl("hid_url_detail")).Value.ToString();

                ScriptManager.RegisterStartupScript(this ,this.GetType(), "OpenWindow", "window.open('"+ xurl_detail + "','_blank');", true);
            }
        }

        protected void Assign_Update_Click(object sender, EventArgs e)
        {
            var xcase_no = hid_case_no.Value.Trim();
            var xassto_login = ddlNameList.SelectedValue.Trim();
            var xstatus = rdlAction.SelectedValue.Trim();
            var xupdate_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            int ret = 0;
            string sqlupdate = @"UPDATE [li_litigation_req_case]
                                   SET [status] = '"+xstatus+@"'
                                      ,[assto_login] = '"+xassto_login+@"'
	                                  ,[updated_datetime] = '"+xupdate_date+@"'
                                 WHERE [case_no] = '"+xcase_no+"'";

            ret = zdb.ExecNonQueryReturnID(sqlupdate, zconnstr);
            if (ret > 0)
            {
                Response.Write("<script>alert('Successfully Updated');</script>");
                setData(hid_PID.Value);

                if (xstatus == "In Progress")
                {
                    //send mail
                    string subject = "";
                    string body = "";
                    string sqlmail = @"select * from li_litigation_request where process_id = '" + hid_PID.Value + "'";
                    var dt = zdb.ExecSql_DataTable(sqlmail, zconnstr);
                    if (dt.Rows.Count > 0)
                    {
                        var dr = dt.Rows[0];
                        string id = dr["req_no"].ToString();
                        subject = "เรื่อง " + dr["lit_subject"].ToString().Trim();
                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        body = "คุณได้รับมอบหมายงาน " + dr["document_no"].ToString() + " กรุณาตรวจสอบและดำเนินการผ่านระบบ <a target='_blank' href='" + host_url + "frmlitigation/litigationrequesteditbyadmin.aspx?id=" + hid_PID.Value + "'>Click</a>";

                        string pathfile = "";

                        string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                        var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                        if (resfile.Rows.Count > 0)
                        {
                            pathfile = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");

                            string email = "";

                            var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                            ////get mail from db
                            /////send mail to next_approve
                            if (isdev != "true")
                            {
                                string sqlbpm = "select * from li_user where user_login = '" + xassto_login + "' ";
                                System.Data.DataTable dtbpm = zdb.ExecSql_DataTable(sqlbpm, zconnstr);

                                if (dtbpm.Rows.Count > 0)
                                {
                                    email = dtbpm.Rows[0]["email"].ToString();

                                }
                                else
                                {
                                    string sqlpra = "select * from Rpa_Mst_HrNameList where Login = 'ASSETWORLDCORP-\\" + xassto_login + "' ";
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
                                _ = zsendmail.sendEmail(subject, email, body, pathfile);
                            }

                        }

                    }
                }
            }
            else
            {
                Response.Write("<script>alert('Error !!!');</script>");
            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string process_code = "";
            var xtype_req = type_req.SelectedValue;
            if (xtype_req == "01")
            {
                process_code = "LIT";
            }
            else
            {
                process_code = "LIT_2";
            }
            int version_no = 1;

            if (!string.IsNullOrEmpty(process_code))
            {
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
                    wfAttr.subject = "เรื่อง " + subject.Text.Trim();
                    wfAttr.assto_login = emp.next_line_mgr_login;
                    wfAttr.wf_status = "COMPLETED";
                    wfAttr.submit_answer = "COMPLETED";
                    wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, "");
                    wfAttr.updated_by = emp.user_login;
                    wfAttr.submit_by = wfAttr.submit_by;
                    // wf.updateProcess
                    var wfA_NextStep = zwf.updateProcess(wfAttr);
                    wfA_NextStep.submit_by = wfAttr.submit_by;
                    wfA_NextStep.wf_status = "COMPLETED";
                    string status = zwf.Insert_NextStep(wfA_NextStep);

                    if (status == "Success")
                    {
                        string sqlupdate = @"update li_litigation_request set status='completed',updated_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where process_id = '" + wfAttr.process_id + "'";
                        zdb.ExecNonQuery(sqlupdate, zconnstr);

                        string subject = "";
                        string body = "";
                        string sql = @"select * from li_litigation_request
                                                where process_id = '" + wfAttr.process_id + "'";
                        var dt = zdb.ExecSql_DataTable(sql, zconnstr);
                        if (dt.Rows.Count > 0)
                        {
                            var dr = dt.Rows[0];
                            string id = dr["req_no"].ToString();
                            subject = wfAttr.subject;
                            body = "เอกสารเลขที่ " + dr["document_no"].ToString() + " ได้รับการดำเนินการเสร็จสิ้นแล้ว";

                            string pathfile = "";

                            string sqlfile = "select top 1 * from z_replacedocx_log where replacedocx_reqno='" + id + "' order by row_id desc";

                            var resfile = zdb.ExecSql_DataTable(sqlfile, zconnstr);

                            if (resfile.Rows.Count > 0)
                            {
                                pathfile = resfile.Rows[0]["output_filepath"].ToString().Replace(".docx", ".pdf");

                                string email = "";

                                var isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                                ////get mail from db
                                if (isdev != "true")
                                {
                                    string sqlbpm = "select * from li_user where user_login = '" + wfA_NextStep.submit_by + "' ";
                                    System.Data.DataTable dtbpm = zdb.ExecSql_DataTable(sqlbpm, zconnstr);

                                    if (dtbpm.Rows.Count > 0)
                                    {
                                        email = dtbpm.Rows[0]["email"].ToString();

                                    }
                                    else
                                    {
                                        string sqlpra = "select * from Rpa_Mst_HrNameList where Login = 'ASSETWORLDCORP-\\" + wfA_NextStep.submit_by + "' ";
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
                                    //send mail to requester
                                    _ = zsendmail.sendEmail(subject + " Mail To Requester", email, body, pathfile);
                                }

                            }

                        }

                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                    }

                }
            }
        }

        protected void btn_Reject_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalReject();", true);
        }
        protected void btn_reject_submit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comment.Text))
            {
                // comment not found 
                Response.Write("<script> alert('Warning! Please input comment');</script>");

            }
            else
            {
                //insert comment
                string xpid = hid_PID.Value;
                string xcomment = comment.Text.Trim();
                string xcreate_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", new CultureInfo("en-US"));
                string xby_login = Session["user_login"].ToString();

                // insert into db
                string sql = @"INSERT INTO [dbo].[wf_comment_log] 
                                        ([pid],[comment],[by_login],[created_datetime])
                                         VALUES
                                               ('" + xpid + @"'
                                               ,'" + xcomment + @"'
                                               ,'" + xby_login + @"'
                                               ,'" + xcreate_date + @"')";
                zdb.ExecNonQuery(sql, zconnstr);

                //submit workflow
                string process_code = Request.QueryString["pc"];
                int version_no = 1;
                string xbu_code = "";

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
                    wfAttr.subject = "เรื่อง " + subject.Text.Trim();
                    wfAttr.assto_login = emp.next_line_mgr_login;
                    wfAttr.wf_status = "REJECT";
                    wfAttr.submit_answer = "REJECT";
                    wfAttr.submit_by = wfAttr.submit_by;
                    wfAttr.next_assto_login = zwf.findNextStep_Assignee(wfAttr.process_code, wfAttr.step_name, emp.user_login, wfAttr.submit_by, lblPID.Text, xbu_code);
                    wfAttr.updated_by = emp.user_login;

                    // wf.updateProcess
                    var wfA_NextStep = zwf.updateProcess(wfAttr);
                    wfA_NextStep.wf_status = "SAVE";
                    wfA_NextStep.next_assto_login = zwf.findNextStep_Assignee(wfA_NextStep.process_code, wfA_NextStep.step_name, emp.user_login, wfAttr.submit_by, lblPID.Text, xbu_code);
                    string status = zwf.Insert_NextStep(wfA_NextStep);

                    if (status == "Success")
                    {
                        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                        Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myworklist", false);
                    }

                }
            }

        }

    }
}