using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace onlineLegalWF.Class
{
    public class ReplaceInsRenew
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        #endregion

        public DataTable genTagData(ReplaceInsReNew_TagData data)
        {
            #region prepare data
            //Replace TAG STRING
            DataTable dtStr = new DataTable();
            dtStr.Columns.Add("tagname", typeof(string));
            dtStr.Columns.Add("tagvalue", typeof(string));

            DataRow dr0 = dtStr.NewRow();
            dr0["tagname"] = "#docno#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.docno) ? data.docno.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#from#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.from) ? data.from.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#attn#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.attn) ? data.attn.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#re#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.re) ? data.re.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#reqdate#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.reqdate) ? data.reqdate.Replace(",", "!comma") : ""); // Utillity.ConvertDateToLongDateTime(xreq_date, "en");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#objective#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.objective) ? data.objective.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#reason#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.reason) ? data.reason.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#approve#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.approve) ? data.approve.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            #endregion
            #region DOA 


            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.name1) ? data.name1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#position1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.position1) ? data.position1.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#date1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.date1) ? data.date1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_name1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_name1) ? data.sign_name1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.name2) ? data.name2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#position2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.position2) ? data.position2.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#date2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.date2) ? data.date2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_name2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_name2) ? data.sign_name2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.name3) ? data.name3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#position3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.position3) ? data.position3.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#date3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.date3) ? data.date3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_name3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_name3) ? data.sign_name3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name3_1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.name3_1) ? data.name3_1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#position3_1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.position3_1) ? data.position3_1.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#date3_1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.date3_1) ? data.date3_1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_name3_1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_name3_1) ? data.sign_name3_1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);


            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name4#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.name4) ? data.name4.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#position4#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.position4) ? data.position4.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#date4#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.date4) ? data.date4.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_name4#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_name4) ? data.sign_name4.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            #endregion 
            return dtStr;
        }

        public DataTable BindTagData(string xprocess_id, ReplaceInsReNew_TagData data)
        {
            // Query wf_route
            // Query eform_data
            //Replace TAG STRING
            ReplaceInsReNew_TagData res = new ReplaceInsReNew_TagData();

            //string xexternal_domain = "";
            string sql0 = @"select [row_id],[process_id],[req_no],[req_date],[toreq_code],ins.[company_name],[document_no],[subject],[dear],[objective]
                                  ,[reason],[approved_desc],[status],[updated_datetime], ins.[bu_code],bu.[external_domain],[property_insured_name] from li_insurance_request as ins
                              INNER JOIN li_business_unit as bu on ins.bu_code = bu.bu_code
                              where process_id = '" + xprocess_id + "'";
            var dt0 = zdb.ExecSql_DataTable(sql0, zconnstr);
            if (dt0.Rows.Count > 0)
            {
                var dr0 = dt0.Rows[0];
                res.docno = dr0["document_no"].ToString();
                res.from = dr0["company_name"].ToString();
                res.attn = dr0["dear"].ToString();
                res.re = dr0["subject"].ToString();
                res.reqdate = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr0["req_date"]), "en");
                res.objective = dr0["objective"].ToString();
                res.reason = dr0["reason"].ToString();
                res.approve = data.approve;
                //xexternal_domain = dr0["external_domain"].ToString();
                res.name1 = data.name1;
                res.position1 = data.position1;
                res.date1 = data.date1;
                res.name2 = data.name2;
                res.position2 = data.position2;
                res.date2 = data.date2;
                res.name3 = data.name3;
                res.position3 = data.position3;
                res.date3 = data.date3;
                res.name3_1 = data.name3_1;
                res.position3_1 = data.position3_1;
                res.date3_1 = data.date3_1;
                res.name4 = data.name4;
                res.position4 = data.position4;
                res.date4 = data.date4;
            }

            string sql = "select * from wf_routing where process_id = '" + xprocess_id + "' ";
            var dt1 = zdb.ExecSql_DataTable(sql, zconnstr);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    var dr = dt1.Rows[i];

                    if (dr["step_name"].ToString() == "Start")
                    {
                        res.name1 = data.name1;
                        res.position1 = data.position1;
                        res.date1 = data.date1;
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name1 = "Approved by system";
                            res.date1 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "GM Approve")
                    {
                        res.name2 = data.name2;
                        res.position2 = data.position2;
                        res.date2 = data.date2;
                        if (dr["wf_status"].ToString() != "")
                        {
                            res.sign_name2 = "Approved by system";
                            res.date2 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "AM Approve")
                    {
                        res.name3 = data.name3;
                        res.position3 = data.position3;
                        res.date3 = data.date3;
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name3 = "Approved by system";
                            res.date3 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "Head AM Approve")
                    {
                        res.name3_1 = data.name3_1;
                        res.position3_1 = data.position3_1;
                        res.date3_1 = data.date3_1;
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name3_1 = "Approved by system";
                            res.date3_1 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "BU Approve")
                    {
                        res.name4 = data.name4;
                        res.position4 = data.position4;
                        res.date4 = data.date4;
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name4 = "Approved by system";
                            res.date4 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "Edit Request")
                    {
                        res.name1 = data.name1;
                        res.position1 = data.position1;
                        res.date1 = data.date1;
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name1 = "Approved by system";
                            res.date1 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");

                            res.sign_name2 = "";
                            res.date2 = "";
                            res.sign_name3 = "";
                            res.date3 = "";
                            res.sign_name3_1 = "";
                            res.date3_1 = "";
                            res.sign_name4 = "";
                            res.date4 = "";
                        }
                    }

                }
            }

            var dtStr = genTagData(res);
            return dtStr;
        }

        public DataTable genTagTableData(string xprocess_id)
        {
            #region Sample ReplaceTable

            DataTable dt = new DataTable();
            dt.Columns.Add("tagname", typeof(string));
            dt.Columns.Add("No", typeof(string));
            dt.Columns.Add("Property Insured", typeof(string));
            dt.Columns.Add("Indemnity Period", typeof(string));
            dt.Columns.Add("Sum Insured", typeof(string));
            dt.Columns.Add("Start Date", typeof(string));
            dt.Columns.Add("End Date", typeof(string));

            //DataTable for #table1#
            string sqlins = @"select * from li_insurance_request
                              where process_id = '" + xprocess_id + "'";
            var dtins = zdb.ExecSql_DataTable(sqlins, zconnstr);
            if (dtins.Rows.Count > 0)
            {
                var drins = dtins.Rows[0];
                string id = drins["req_no"].ToString();

                string sqlinsprop = @"SELECT [row_id],[req_no],tb1.[top_ins_code],[indemnityperiod],[suminsured],[startdate]
                                  ,[enddate] ,[created_datetime],[updated_datetime],tb2.[top_ins_desc]
                              FROM [li_insurance_req_property_insured] as tb1
                              INNER JOIN [li_type_of_property_insured] as tb2 on tb1.top_ins_code = tb2.top_ins_code
                              where req_no = '" + id + "'";
                var dtlinsprop = zdb.ExecSql_DataTable(sqlinsprop, zconnstr);

                if (dtlinsprop.Rows.Count > 0)
                {
                    //Assign Data From gv1
                    var drGV = dt.NewRow();
                    int no = 0;
                    foreach (DataRow item in dtlinsprop.Rows)
                    {
                        drGV = dt.NewRow();
                        drGV["tagname"] = "#table1#";
                        drGV["No"] = (no + 1);
                        drGV["Property Insured"] = item["top_ins_desc"].ToString().Replace(",", "!comma");
                        drGV["Indemnity Period"] = item["indemnityperiod"].ToString().Replace(",", "!comma");
                        drGV["Sum Insured"] = item["suminsured"].ToString().Replace(",", "!comma");
                        drGV["Start Date"] = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(item["startdate"]), "en");
                        drGV["End Date"] = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(item["enddate"]), "en");
                        dt.Rows.Add(drGV);

                        no++;
                    }
                }
            }

            #endregion

            return dt;
        }

        public class ReplaceInsReNew_TagData
        {
            public string docno { get; set; }
            public string attn { get; set; }
            public string from { get; set; }
            public string re { get; set; }
            public string reqdate { get; set; }
            public string objective { get; set; }
            public string reason { get; set; }
            public string approve { get; set; }

            public string sign_name1 { get; set; }
            public string name1 { get; set; }
            public string position1 { get; set; }
            public string date1 { get; set; }

            public string sign_name2 { get; set; }
            public string name2 { get; set; }
            public string position2 { get; set; }
            public string date2 { get; set; }

            public string sign_name3 { get; set; }
            public string name3 { get; set; }
            public string position3 { get; set; }
            public string date3 { get; set; }

            public string sign_name3_1 { get; set; }
            public string name3_1 { get; set; }
            public string position3_1 { get; set; }
            public string date3_1 { get; set; }

            public string sign_name4 { get; set; }
            public string name4 { get; set; }
            public string position4 { get; set; }
            public string date4 { get; set; }

        }
    }
}