using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace onlineLegalWF.Class
{
    public class ReplacePermit
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        #endregion

        public DataTable genTagData(ReplacePermit_TagData data)
        {
            DataTable dtStr = new DataTable();
            dtStr.Columns.Add("tagname", typeof(string));
            dtStr.Columns.Add("tagvalue", typeof(string));

            DataRow dr0 = dtStr.NewRow();
            dr0["tagname"] = "#doc_no#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.docno) ? data.docno.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#req_date#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.reqdate) ? data.reqdate.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#r1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.r1) ? data.r1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#r2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.r2) ? data.r2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#r3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.r3) ? data.r3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#req_other#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.req_other) ? data.req_other.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#license_other#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.license_other) ? data.license_other.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#tax_other#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.tax_other) ? data.tax_other.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#trademarks_other#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.trademarks_other) ? data.trademarks_other.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.name1) ? data.name1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#signname1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.signname1) ? data.signname1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#signdate1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.signdate1) ? data.signdate1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.name2) ? data.name2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#signname2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.signname2) ? data.signname2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#signdate2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.signdate2) ? data.signdate2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#subject#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.subject) ? data.subject.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#bu_name#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.bu_name) ? data.bu_name.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#t1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.t1) ? data.t1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#t2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.t2) ? data.t2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#t3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.t3) ? data.t3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#t4#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.t4) ? data.t4.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#t5#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.t5) ? data.t5.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#t6#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.t6) ? data.t6.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#t7#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.t7) ? data.t7.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#t8#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.t8) ? data.t8.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#t9#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.t9) ? data.t9.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#t10#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.t10) ? data.t10.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#t11#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.t11) ? data.t11.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#t12#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.t12) ? data.t12.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#desc_req#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.desc_req) ? data.desc_req.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#contact_agency#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.contact_agency) ? data.contact_agency.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#attorney_name#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.attorney_name) ? data.attorney_name.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#list_doc_attach#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.list_doc_attach) ? data.list_doc_attach.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#responsible_phone#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.responsible_phone) ? data.responsible_phone.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#cb_urgent#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.cb_urgent) ? data.cb_urgent.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#urgent_remark#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.urgent_remark) ? data.urgent_remark.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);


            return dtStr;
        }

        public DataTable BindTagData(string xprocess_id, ReplacePermit_TagData data)
        {
            ReplacePermit_TagData res = new ReplacePermit_TagData();

            string xexternal_domain = "";
            string sql0 = @"SELECT permit.[row_id],permit.[process_id],permit.[permit_no],permit.[document_no],permit.[permit_date],permit.[permit_subject],permit.[permit_desc],permit.[isurgent],permit.[urgent_remark]
                                  ,permit.[tof_requester_code],permit.[tof_requester_other_desc],permit.[tof_permitreq_code],permit.[tof_permitreq_other_desc],permit.[license_code],permit.[sublicense_code]
                                  ,permit.[contact_agency],permit.[attorney_name],permit.[email_accounting],permit.[bu_code],bu.[bu_desc],permit.[status],permit.[updated_datetime],permit.[responsible_phone],bu.[external_domain]
                              FROM [li_permit_request] as permit
                              INNER JOIN [li_business_unit] as bu on permit.bu_code = bu.bu_code
                              where process_id = '" + xprocess_id + "'";
            var dt0 = zdb.ExecSql_DataTable(sql0, zconnstr);
            if (dt0.Rows.Count > 0)
            {
                var dr0 = dt0.Rows[0];
                res.docno = dr0["document_no"].ToString().Trim();
                res.reqdate = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr0["permit_date"]), "th").Trim();
                var xrequester_code = dr0["tof_requester_code"].ToString().Trim();
                xexternal_domain = dr0["external_domain"].ToString();
                res.req_other = "";
                res.responsible_phone = dr0["responsible_phone"].ToString().Trim();
                var xcb_urgent = dr0["isurgent"].ToString();
                if (xcb_urgent == "True") 
                {
                    res.cb_urgent = "☑";
                }
                else 
                { 
                    res.cb_urgent = "☐"; 
                }
                res.urgent_remark = dr0["urgent_remark"].ToString();
                if (xrequester_code == "01")
                {
                    res.r1 = "☑";
                    res.r2 = "☐";
                    res.r3 = "☐";
                }
                else if (xrequester_code == "02")
                {
                    res.r1 = "☐";
                    res.r2 = "☑";
                    res.r3 = "☐";
                }
                else if (xrequester_code == "03")
                {
                    res.r1 = "☐";
                    res.r2 = "☐";
                    res.r3 = "☑";
                    res.req_other = dr0["tof_requester_other_desc"].ToString().Trim();
                }
                res.name1 = data.name1;
                res.signdate1 = data.signdate1;
                res.name2 = data.name2;
                res.signdate2 = data.signdate2;

                res.subject = dr0["permit_subject"].ToString().Trim();
                res.bu_name = dr0["bu_desc"].ToString().Trim();
                res.license_other = "";
                res.tax_other = "";
                res.trademarks_other = "";

                var xtof_permitreq_code = dr0["tof_permitreq_code"].ToString().Trim();
                if (xtof_permitreq_code == "01") 
                {
                    res.t1 = "☑";
                    res.t2 = "☐";
                    res.t3 = "☐";
                    res.t4 = "☐";
                    res.t5 = "☐";
                    res.t6 = "☐";
                    res.t7 = "☐";
                    res.t8 = "☐";
                    res.t9 = "☐";
                    res.t10 = "☐";
                    res.t11 = "☐";
                    res.t12 = "☐";
                }
                else if (xtof_permitreq_code == "02")
                {
                    res.t1 = "☐";
                    res.t2 = "☑";
                    res.t3 = "☐";
                    res.t4 = "☐";
                    res.t5 = "☐";
                    res.t6 = "☐";
                    res.t7 = "☐";
                    res.t8 = "☐";
                    res.t9 = "☐";
                    res.t10 = "☐";
                    res.t11 = "☐";
                    res.t12 = "☐";
                }
                else if (xtof_permitreq_code == "03")
                {
                    res.t1 = "☐";
                    res.t2 = "☐";
                    res.t3 = "☑";
                    res.t4 = "☐";
                    res.t5 = "☐";
                    res.t6 = "☐";
                    res.t7 = "☐";
                    res.t8 = "☐";
                    res.t9 = "☐";
                    res.t10 = "☐";
                    res.t11 = "☐";
                    res.t12 = "☐";
                }
                else if (xtof_permitreq_code == "04")
                {
                    res.t1 = "☐";
                    res.t2 = "☐";
                    res.t3 = "☐";
                    res.t4 = "☑";
                    res.t5 = "☐";
                    res.t6 = "☐";
                    res.t7 = "☐";
                    res.t8 = "☐";
                    res.t9 = "☐";
                    res.t10 = "☐";
                    res.t11 = "☐";
                    res.t12 = "☐";
                    res.license_other = dr0["tof_permitreq_other_desc"].ToString().Trim();
                }
                else if (xtof_permitreq_code == "05")
                {
                    res.t1 = "☐";
                    res.t2 = "☐";
                    res.t3 = "☐";
                    res.t4 = "☐";
                    res.t5 = "☑";
                    res.t6 = "☐";
                    res.t7 = "☐";
                    res.t8 = "☐";
                    res.t9 = "☐";
                    res.t10 = "☐";
                    res.t11 = "☐";
                    res.t12 = "☐";
                }
                else if (xtof_permitreq_code == "06")
                {
                    res.t1 = "☐";
                    res.t2 = "☐";
                    res.t3 = "☐";
                    res.t4 = "☐";
                    res.t5 = "☐";
                    res.t6 = "☑";
                    res.t7 = "☐";
                    res.t8 = "☐";
                    res.t9 = "☐";
                    res.t10 = "☐";
                    res.t11 = "☐";
                    res.t12 = "☐";
                }
                else if (xtof_permitreq_code == "07")
                {
                    res.t1 = "☐";
                    res.t2 = "☐";
                    res.t3 = "☐";
                    res.t4 = "☐";
                    res.t5 = "☐";
                    res.t6 = "☐";
                    res.t7 = "☑";
                    res.t8 = "☐";
                    res.t9 = "☐";
                    res.t10 = "☐";
                    res.t11 = "☐";
                    res.t12 = "☐";
                    res.tax_other = dr0["tof_permitreq_other_desc"].ToString().Trim();
                }
                else if (xtof_permitreq_code == "08")
                {
                    res.t1 = "☐";
                    res.t2 = "☐";
                    res.t3 = "☐";
                    res.t4 = "☐";
                    res.t5 = "☐";
                    res.t6 = "☐";
                    res.t7 = "☐";
                    res.t8 = "☑";
                    res.t9 = "☐";
                    res.t10 = "☐";
                    res.t11 = "☐";
                    res.t12 = "☐";
                }
                else if (xtof_permitreq_code == "09")
                {
                    res.t1 = "☐";
                    res.t2 = "☐";
                    res.t3 = "☐";
                    res.t4 = "☐";
                    res.t5 = "☐";
                    res.t6 = "☐";
                    res.t7 = "☐";
                    res.t8 = "☐";
                    res.t9 = "☑";
                    res.t10 = "☐";
                    res.t11 = "☐";
                    res.t12 = "☐";
                    res.trademarks_other = dr0["tof_permitreq_other_desc"].ToString().Trim();
                }
                else if (xtof_permitreq_code == "10")
                {
                    res.t1 = "☐";
                    res.t2 = "☐";
                    res.t3 = "☐";
                    res.t4 = "☐";
                    res.t5 = "☐";
                    res.t6 = "☐";
                    res.t7 = "☐";
                    res.t8 = "☐";
                    res.t9 = "☐";
                    res.t10 = "☑";
                    res.t11 = "☐";
                    res.t12 = "☐";
                }
                else if (xtof_permitreq_code == "11")
                {
                    res.t1 = "☐";
                    res.t2 = "☐";
                    res.t3 = "☐";
                    res.t4 = "☐";
                    res.t5 = "☐";
                    res.t6 = "☐";
                    res.t7 = "☐";
                    res.t8 = "☐";
                    res.t9 = "☐";
                    res.t10 = "☐";
                    res.t11 = "☑";
                    res.t12 = "☐";
                }
                else if (xtof_permitreq_code == "12")
                {
                    res.t1 = "☐";
                    res.t2 = "☐";
                    res.t3 = "☐";
                    res.t4 = "☐";
                    res.t5 = "☐";
                    res.t6 = "☐";
                    res.t7 = "☐";
                    res.t8 = "☐";
                    res.t9 = "☐";
                    res.t10 = "☐";
                    res.t11 = "☐";
                    res.t12 = "☑";
                }

                res.desc_req = dr0["permit_desc"].ToString().Trim();
                res.contact_agency = dr0["contact_agency"].ToString().Trim();
                res.attorney_name = dr0["attorney_name"].ToString().Trim();
                res.list_doc_attach = "ตรวจสอบเอกสารแนบได้ที่ระบบ";

            }

            string sql = "select * from wf_routing where process_id = '" + xprocess_id + "' ";
            var dt1 = zdb.ExecSql_DataTable(sql, zconnstr);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    var dr = dt1.Rows[i];

                    if (dr["step_name"].ToString() == "Start" && xexternal_domain == "N")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.signname1 = "Approved by system";
                            res.signdate1 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    if (dr["step_name"].ToString() == "GM Approve" && xexternal_domain == "N")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.signname2 = "Approved by system";
                            res.signdate2 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "AM Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.signname1 = "Approved by system";
                            res.signdate1 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "Head AM Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.signname2 = "Approved by system";
                            res.signdate2 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }

                }
            }

            var dtStr = genTagData(res);
            return dtStr;
        }

        public class ReplacePermit_TagData
        {
            public string docno { get; set; }
            public string reqdate { get; set; }
            public string r1 { get; set; }
            public string r2 { get; set; }
            public string r3 { get; set; }
            public string req_other { get; set; }
            public string license_other { get; set; }
            public string tax_other { get; set; }
            public string trademarks_other { get; set; }
            public string signname1 { get; set; }
            public string name1 { get; set; }
            public string signdate1 { get; set; }

            public string signname2 { get; set; }
            public string name2 { get; set; }
            public string signdate2 { get; set; }

            public string subject { get; set; }
            public string bu_name { get; set; }
            public string t1 { get; set; }
            public string t2 { get; set; }
            public string t3 { get; set; }
            public string t4 { get; set; }
            public string t5 { get; set; }
            public string t6 { get; set; }
            public string t7 { get; set; }
            public string t8 { get; set; }
            public string t9 { get; set; }
            public string t10 { get; set; }
            public string t11 { get; set; }
            public string t12 { get; set; }
            public string desc_req { get; set; }
            public string contact_agency { get; set; }
            public string attorney_name { get; set; }
            public string list_doc_attach { get; set; }
            public string responsible_phone { get; set; }
            public string cb_urgent { get; set; }
            public string urgent_remark { get; set; }
        }
    }
}