using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using static iTextSharp.text.pdf.AcroFields;

namespace onlineLegalWF.Class
{
    public class ReplaceInsRenewAWC
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        
        #endregion
        public DataTable genTagData(ReplaceInsReNewAWC_TagData data)
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
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.company) ? data.company.Replace(",", "!comma").Replace("'", "’") : "");
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
            dr0["tagname"] = "#description#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.description) ? data.description.Replace(",", "!comma") : "");
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

        public DataTable BindTagData(string xprocess_id, ReplaceInsReNewAWC_TagData data)
        {
            // Query wf_route
            // Query eform_data
            //Replace TAG STRING
            ReplaceInsReNewAWC_TagData res = new ReplaceInsReNewAWC_TagData();

            string sql0 = @"select * from li_insurance_renew_awc_memo
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
                res.description = dr0["description"].ToString();
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
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    var dr = dt1.Rows[i];

                    if (dr["step_name"].ToString() == "Insurance Specialist Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name2 = "อนุมัติผ่านระบบ";
                            res.date2 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "Head of Compliance and Insurance Approve")
                    {

                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name22 = "อนุมัติผ่านระบบ";
                            res.date22 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "Head of Legal Approve")
                    {

                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name3 = "อนุมัติผ่านระบบ";
                            res.date3 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "Head of Risk Management Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_name4 = "อนุมัติผ่านระบบ";
                            res.date4 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "CCO Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.cb1 = "X";
                            res.sign_name5 = "อนุมัติผ่านระบบ";
                            res.date5 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
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
            dt.Columns.Add("TYPE_PROP", typeof(string));
            dt.Columns.Add("IAR", typeof(string));
            dt.Columns.Add("BI", typeof(string));
            dt.Columns.Add("CGL", typeof(string));
            dt.Columns.Add("PL", typeof(string));
            dt.Columns.Add("PV", typeof(string));
            dt.Columns.Add("LPG", typeof(string));
            dt.Columns.Add("D_O", typeof(string));
            dt.Columns.Add("Row_Sort", typeof(string));

            //DataTable for #tablesum#
            string sqlinssum = @"select row_sort,type_prop
                                  ,format(isnull(cast(sum_iar as int),0), '##,##0') as sum_iar
                                  ,format(isnull(cast(sum_bi as int),0), '##,##0') as sum_bi
                                  ,format(isnull(cast(sum_cgl as int),0), '##,##0') as sum_cgl
                                  ,format(isnull(cast(sum_pl as int),0), '##,##0') as sum_pl
                                  ,format(isnull(cast(sum_pv as int),0), '##,##0') as sum_pv
                                  ,format(isnull(cast(sum_lpg as int),0), '##,##0') as sum_lpg
                                  ,format(isnull(cast(sum_d_o as int),0), '##,##0') as sum_d_o
                            from li_insurance_renew_awc_memo_sumins where process_id = '" + xprocess_id + "'";
            var dtlinssum = zdb.ExecSql_DataTable(sqlinssum, zconnstr);
            var drGV = dt.NewRow();

            if (dtlinssum.Rows.Count > 0)
            {
                //Assign DataTable for #tablesum#
                foreach (DataRow dritem in dtlinssum.Rows) 
                {
                    drGV = dt.NewRow();
                    drGV["tagname"] = "#tablesum#";
                    drGV["TYPE_PROP"] = dritem["type_prop"].ToString().Replace(",", "!comma");
                    drGV["IAR"] = dritem["sum_iar"].ToString().Replace(",", "!comma");
                    drGV["BI"] = dritem["sum_bi"].ToString().Replace(",", "!comma");
                    drGV["CGL"] = dritem["sum_cgl"].ToString().Replace(",", "!comma");
                    drGV["PL"] = dritem["sum_pl"].ToString().Replace(",", "!comma");
                    drGV["PV"] = dritem["sum_pv"].ToString().Replace(",", "!comma");
                    drGV["LPG"] = dritem["sum_lpg"].ToString().Replace(",", "!comma");
                    drGV["D_O"] = dritem["sum_d_o"].ToString().Replace(",", "!comma");
                    drGV["Row_Sort"] = dritem["row_sort"].ToString().Replace(",", "!comma");
                    dt.Rows.Add(drGV);
                }
                
            }

            #endregion

            return dt;
        }
        public class ReplaceInsReNewAWC_TagData
        {
            public string docno { get; set; }
            public string to { get; set; }
            public string company { get; set; }
            public string subject { get; set; }
            public string reqdate { get; set; }
            public string description { get; set; }

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
            public string name4 { get; set; }
            public string position4 { get; set; }
            public string date4 { get; set; }

            public string sign_name5 { get; set; }
            public string name5 { get; set; }
            public string position5 { get; set; }
            public string date5 { get; set; }
            public string cb1 { get; set; }
            public string cb2 { get; set; }
            public string remark5 { get; set; }
        }
    }
}