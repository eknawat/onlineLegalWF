using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using onlineLegalWF.Class;

namespace onlineLegalWF.Class
{
    public class ReplaceInsNew
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        //public string zconnstr = ConfigurationSettings.AppSettings["BPMDB"].ToString();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        #endregion
        
        public DataTable genTagData(ReplaceInsNew_TagData data)
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
            dr0["tagname"] = "#company#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.company) ? data.company.Replace(",", "!comma") : ""); 
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#to#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.to) ? data.to.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#subject#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.subject) ? data.subject.Replace(",", "!comma") : "");
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
            dr0["tagname"] = "#name22#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.name22) ? data.name22.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#date22#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.date22) ? data.date22.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_name22#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_name22) ? data.sign_name22.Replace(",", "!comma") : "");
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

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name5#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.name5) ? data.name5.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#position5#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.position5) ? data.position5.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#date5#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.date5) ? data.date5.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_name5#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_name5) ? data.sign_name5.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#cb1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.cb1) ? data.cb1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#cb2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.cb2) ? data.cb2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#remark5#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.remark5) ? data.remark5.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            #endregion 
            return dtStr; 
        }
        public DataTable BindTagData(string xprocess_id, ReplaceInsNew_TagData data)
        {
            // Query wf_route
            // Query eform_data
            //Replace TAG STRING
            ReplaceInsNew_TagData res = new ReplaceInsNew_TagData();

            string xexternal_domain = "";
            string sql0 = @"select [row_id],[process_id],[req_no],[req_date],[toreq_code],ins.[company_name],[document_no],[subject],[dear],[objective]
                                  ,[reason],[approved_desc],[status],[updated_datetime], ins.[bu_code],bu.[external_domain],[property_insured_name] from li_insurance_request as ins
                              INNER JOIN li_business_unit as bu on ins.bu_code = bu.bu_code
                              where process_id = '" + xprocess_id + "'";
            var dt0 = zdb.ExecSql_DataTable(sql0, zconnstr);
            if (dt0.Rows.Count > 0)
            {
                var dr0 = dt0.Rows[0];
                res.docno = dr0["document_no"].ToString();
                res.company = dr0["company_name"].ToString();
                res.to = dr0["dear"].ToString();
                res.subject = dr0["subject"].ToString();
                res.reqdate = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr0["req_date"]), "en");
                res.objective = dr0["objective"].ToString();
                res.reason = dr0["reason"].ToString();
                res.approve = dr0["approved_desc"].ToString();
                xexternal_domain = dr0["external_domain"].ToString();
                res.name1 = data.name1;
                res.position1 = data.position1;
                res.date1 = data.date1;
                res.name2 = data.name2;
                res.position2 = data.position2;
                res.date2 = data.date2;
                res.name22 = data.name22;
                res.date22 = data.date22;
                res.name3 = data.name3;
                res.position3 = data.position3;
                res.date3 = data.date3;
                res.name4 = data.name4;
                res.position4 = data.position4;
                res.date4 = data.date4;
                res.name5 = data.name5;
                res.position5 = data.position5;
                res.date5 = data.date5;
                res.cb1 = data.cb1;
                res.cb2 = data.cb2;
            }

            string sql = "select * from wf_routing where process_id = '" + xprocess_id + "' ";
            var dt1 = zdb.ExecSql_DataTable(sql, zconnstr); 
            if ( dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    var dr = dt1.Rows[i];

                    if (dr["step_name"].ToString() == "GM Approve" && xexternal_domain == "N")
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
                    else if (dr["step_name"].ToString() == "Head AM Approve")
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
                    else if (dr["step_name"].ToString() == "Insurance Specialist Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name2 = "Approved by system";
                            res.date2 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "Head of Compliance and Insurance Approve")
                    {
                        
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name22 = "Approved by system";
                            res.date22 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "Head of Legal Approve")
                    {
                        
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name3 = "Approved by system";
                            res.date3 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "Head of Risk Management Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name4 = "Approved by system";
                            res.date4 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "CCO Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.cb1 = "X";
                            res.sign_name5 = "Approved by system";
                            res.date5 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "Edit Request")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name1 = "";
                            res.date1 = "";
                            res.sign_name2 = "";
                            res.date2 = "";
                            res.sign_name22 = "";
                            res.date22 = "";
                            res.sign_name3 = "";
                            res.date3 = "";
                            res.sign_name4 = "";
                            res.date4 = "";
                            res.cb1 = "";
                            res.sign_name5 = "";
                            res.date5 = "";
                        }
                    }

                }
            }

            var dtStr = genTagData(res); 
            return dtStr;
        }

        public DataTable genTagTableData(string xprocess_id) 
        {
            //#region Sample ReplaceTable

            //DataTable dt = new DataTable();
            //dt.Columns.Add("tagname", typeof(string));
            //dt.Columns.Add("No", typeof(string));
            //dt.Columns.Add("Property Insured", typeof(string));
            //dt.Columns.Add("Indemnity Period", typeof(string));
            //dt.Columns.Add("Sum Insured", typeof(string));
            //dt.Columns.Add("Start Date", typeof(string));
            //dt.Columns.Add("End Date", typeof(string));

            ////DataTable for #table1#
            //string sqlins = @"select * from li_insurance_request
            //                  where process_id = '" + xprocess_id + "'";
            //var dtins = zdb.ExecSql_DataTable(sqlins, zconnstr);
            //if (dtins.Rows.Count > 0) 
            //{
            //    var drins = dtins.Rows[0];
            //    string id = drins["req_no"].ToString();

            //    string sqlinsprop = @"SELECT [row_id],[req_no],tb1.[top_ins_code],[indemnityperiod],[suminsured],[startdate]
            //                      ,[enddate] ,[created_datetime],[updated_datetime],tb2.[top_ins_desc]
            //                  FROM [li_insurance_req_property_insured] as tb1
            //                  INNER JOIN [li_type_of_property_insured] as tb2 on tb1.top_ins_code = tb2.top_ins_code
            //                  where req_no = '" + id + "'";
            //    var dtlinsprop = zdb.ExecSql_DataTable(sqlinsprop, zconnstr);

            //    if(dtlinsprop.Rows.Count > 0) 
            //    {
            //        var drlinsprop = dtlinsprop.Rows[0];
            //        //Assign DataTable for #table#
            //        DataRow dr1 = dt.NewRow();
            //        dr1["tagname"] = "#table1#";
            //        dr1["No"] = "1";
            //        dr1["Property Insured"] = drlinsprop["top_ins_desc"].ToString().Replace(",", "!comma");  // "xxxxx";//.Text.Replace(",", "!comma");
            //        dr1["Indemnity Period"] = drlinsprop["indemnityperiod"].ToString().Replace(",", "!comma"); // "1,000,000".Replace(",", "!comma"); ;
            //        dr1["Sum Insured"] = drlinsprop["suminsured"].ToString().Replace(",", "!comma");  // "15,000".Replace(",", "!comma"); ;
            //        dr1["Start Date"] = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(drlinsprop["startdate"]), "en");
            //        dr1["End Date"] = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(drlinsprop["enddate"]), "en");
            //        dt.Rows.Add(dr1);
            //    }
            //}

            //#endregion

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
    }
   
    public class ReplaceInsNew_TagData
    {
        public string docno { get; set;  }
        public string company { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string reqdate { get; set; }
        public string objective { get; set; }
        public string reason { get; set; }

        public string sign_name1 { get; set; }
        public string name1 { get; set; }
        public string position1 { get; set; }
        public string date1 { get; set; }
       
        public string sign_name2 { get; set; }
        public string name2 { get; set; }
        public string position2 { get; set; }
        public string date2 { get; set; }

        public string sign_name22 { get; set; }
        public string name22 { get; set; }
        public string date22 { get; set; }

        public string sign_name3 { get; set; }
        public string name3 { get; set; }
        public string position3 { get; set; }
        public string date3 { get; set; }

        public string sign_name4 { get; set; }
        public string name4{ get; set; }
        public string position4 { get; set; }
        public string date4 { get; set; }

        public string approve { get; set; }
        public string sign_name5 { get; set; }
        public string name5 { get; set; }
        public string position5 { get; set; }
        public string date5 { get; set; }
        public string cb1 { get; set; }
        public string cb2 { get; set; }
        public string remark5 { get; set; }


    }
}