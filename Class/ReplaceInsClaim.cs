using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace onlineLegalWF.Class
{
    public class ReplaceInsClaim
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        //public string zconnstr = ConfigurationSettings.AppSettings["BPMDB"].ToString();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        #endregion

        public DataTable genTagData(ReplaceInsClaim_TagData data)
        {
            #region prepare data
            //Replace TAG STRING
            DataTable dtStr = new DataTable();
            dtStr.Columns.Add("tagname", typeof(string));
            dtStr.Columns.Add("tagvalue", typeof(string));

            DataRow dr0 = dtStr.NewRow();
            dr0["tagname"] = "#company#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.company) ? data.company.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#propertyname#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.propertyname) ? data.propertyname.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#incident#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.incident) ? data.incident.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#occurreddate#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.occurreddate) ? data.occurreddate.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#submidate#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.submidate) ? data.submidate.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#submiday#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.submiday) ? data.submiday.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#incidentsummary#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.incidentsummary) ? data.incidentsummary.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#docref#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.docref) ? data.docref.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#surveyor#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.surveyor) ? data.surveyor.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#companysurveyor#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.companysurveyor) ? data.companysurveyor.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#stmdate#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.stmdate) ? data.stmdate.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#stmday#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.stmday) ? data.stmday.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#remark#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.remark) ? data.remark.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_propname1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_propname1) ? data.sign_propname1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#propname1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.propname1) ? data.propname1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#propposition1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.propposition1) ? data.propposition1.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#propdate1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.propdate1) ? data.propdate1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_propname2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_propname2) ? data.sign_propname2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#propname2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.propname2) ? data.propname2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#propposition2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.propposition2) ? data.propposition2.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#propdate2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.propdate2) ? data.propdate2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_propname3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_propname3) ? data.sign_propname3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#propname3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.propname3) ? data.propname3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#propposition3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.propposition3) ? data.propposition3.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#propdate3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.propdate3) ? data.propdate3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_propname3_1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_propname3_1) ? data.sign_propname3_1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#propname3_1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.propname3_1) ? data.propname3_1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#propposition3_1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.propposition3_1) ? data.propposition3_1.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#propdate3_1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.propdate3_1) ? data.propdate3_1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_awcname1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_awcname1) ? data.sign_awcname1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcname1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcname1) ? data.awcname1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcposition1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcposition1) ? data.awcposition1.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcdate1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcdate1) ? data.awcdate1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_awcname1_2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_awcname1_2) ? data.sign_awcname1_2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcname1_2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcname1_2) ? data.awcname1_2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcposition1_2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcposition1_2) ? data.awcposition1_2.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcdate1_2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcdate1_2) ? data.awcdate1_2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_awcname1_3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_awcname1_3) ? data.sign_awcname1_3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcname1_3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcname1_3) ? data.awcname1_3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcposition1_3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcposition1_3) ? data.awcposition1_3.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcdate1_3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcdate1_3) ? data.awcdate1_3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_awcname2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_awcname2) ? data.sign_awcname2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcname2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcname2) ? data.awcname2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcposition2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcposition2) ? data.awcposition2.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcdate2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcdate2) ? data.awcdate2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_awcname3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_awcname3) ? data.sign_awcname3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcname3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcname3) ? data.awcname3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcposition3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcposition3) ? data.awcposition3.Replace(",", "!comma").Replace("'", "’") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#awcdate3#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.awcdate3) ? data.awcdate3.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            #endregion

            return dtStr;
        }

        public DataTable bindTagData(string xprocess_id, ReplaceInsClaim_TagData data) 
        {
            // Query wf_route
            // Query eform_data
            //Replace TAG STRING
            ReplaceInsClaim_TagData res = new ReplaceInsClaim_TagData();

            string xexternal_domain = "";
            string sql0 = @"SELECT [row_id],[process_id],claim.[company_name],[claim_no],[claim_date],[document_no],claim.[bu_code],[incident],[occurred_date]
                              ,[submission_date],[incident_summary],[surveyor_name],[surveyor_company],[settlement_date],[settlement_day],[iar_atc],[iar_ded]
                              ,[iar_pfc],[iar_uatc],[bi_atc],[bi_ded],[bi_pfc],[bi_uatc],[pl_cgl_atc],[pl_cgl_ded],[pl_cgl_pfc],[pl_cgl_uatc],[pv_atc]
                              ,[pv_ded],[pv_pfc],[pv_uatc],[ttl_atc],[ttl_ded],[ttl_pfc],[ttl_uatc],[remark] ,[status],[updated_datetime],bu.[external_domain],bu.[bu_desc]
                          FROM li_insurance_claim as claim
                          INNER JOIN li_business_unit as bu on claim.bu_code = bu.bu_code
                          where process_id = '" + xprocess_id +"'";
            var dt0 = zdb.ExecSql_DataTable(sql0, zconnstr);
            if (dt0.Rows.Count > 0)
            {
                var dr0 = dt0.Rows[0];
                res.company = dr0["company_name"].ToString();
                res.propertyname = dr0["bu_desc"].ToString();
                res.incident = dr0["incident"].ToString();
                res.occurreddate = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr0["occurred_date"]), "en");
                res.submidate = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr0["submission_date"]), "en");
                res.submiday = ((Convert.ToDateTime(dr0["submission_date"]) - Convert.ToDateTime(dr0["occurred_date"])).TotalDays).ToString() + " days";
                res.incidentsummary = dr0["incident_summary"].ToString();
                res.surveyor = dr0["surveyor_name"].ToString();
                res.companysurveyor = dr0["surveyor_company"].ToString();
                res.stmdate = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr0["settlement_date"]), "en");
                res.stmday = dr0["settlement_day"].ToString() + " days";
                res.remark = dr0["remark"].ToString();
                xexternal_domain = dr0["external_domain"].ToString();
                res.propname1 = data.propname1;
                res.propposition1 = data.propposition1;
                res.propdate1 = data.propdate1;
                res.propname2 = data.propname2;
                res.propposition2 = data.propposition2;
                res.propdate2 = data.propdate2;
                res.propname3 = data.propname3;
                res.propposition3 = data.propposition3;
                res.propdate3 = data.propdate3;
                res.propname3_1 = data.propname3_1;
                res.propposition3_1 = data.propposition3_1;
                res.propdate3_1 = data.propdate3_1;
                res.awcname1 = data.awcname1;
                res.awcposition1 = data.awcposition1;
                res.awcdate1 = data.awcdate1;
                res.awcname1_2 = data.awcname1_2;
                res.awcposition1_2 = data.awcposition1_2;
                res.awcdate1_2 = data.awcdate1_2;
                res.awcname1_3 = data.awcname1_3;
                res.awcposition1_3 = data.awcposition1_3;
                res.awcdate1_3 = data.awcdate1_3;
                res.awcname2 = data.awcname2;
                res.awcposition2 = data.awcposition2;
                res.awcdate2 = data.awcdate2;
                res.awcname3 = data.awcname3;
                res.awcposition3 = data.awcposition3;
                res.awcdate3 = data.awcdate3;
                res.docref = data.docref;
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
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_propname1 = "Approved by system";
                            res.propdate1 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "GM Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_propname2 = "Approved by system";
                            res.propdate2 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "AM Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_propname3 = "Approved by system";
                            res.propdate3 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "Head AM Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_propname3_1 = "Approved by system";
                            res.propdate3_1 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "AWC Validate Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_awcname1 = "Approved by system";
                            res.awcdate1 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "AWC Validate2 Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_awcname1_2 = "Approved by system";
                            res.awcdate1_2 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "AWC Validate3 Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_awcname1_3 = "Approved by system";
                            res.awcdate1_3 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "AWC Reviewer Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_awcname2 = "Approved by system";
                            res.awcdate2 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "AWC Approval Approve")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_awcname3 = "Approved by system";
                            res.awcdate3 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                        }
                    }
                    else if (dr["step_name"].ToString() == "Edit Request")
                    {
                        if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
                        {
                            res.sign_propname1 = "Approved by system";
                            res.propdate1 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
                            res.sign_propname2 = "";
                            res.propdate2 = "";
                            res.sign_propname3 = "";
                            res.propdate3 = "";
                            res.sign_propname3_1 = "";
                            res.propdate3_1 = "";
                            res.sign_awcname1 = "";
                            res.awcdate1 = "";
                            res.sign_awcname1_2 = "";
                            res.awcdate1_2 = "";
                            res.sign_awcname1_3 = "";
                            res.awcdate1_3 = "";
                            res.sign_awcname2 = "";
                            res.awcdate2 = "";
                            res.sign_awcname3 = "";
                            res.awcdate3 = "";
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
            dt.Columns.Add("Estimated losses to claim", typeof(string));
            dt.Columns.Add("IAR", typeof(string));
            dt.Columns.Add("BI", typeof(string));
            dt.Columns.Add("PL/CGL", typeof(string));
            dt.Columns.Add("PV", typeof(string));
            dt.Columns.Add("Total", typeof(string));

            string sqliclaim = @"select * from li_insurance_claim
                              where process_id = '" + xprocess_id + "'";
            var dtclaim = zdb.ExecSql_DataTable(sqliclaim, zconnstr);
            if (dtclaim.Rows.Count > 0)
            {
                var drclaim = dtclaim.Rows[0];

                //Assign DataTable for #tableeltc#
                DataRow dr1 = dt.NewRow();
                dr1["tagname"] = "#tableeltc#";
                dr1["Estimated losses to claim"] = "Amount to claim มูลค่าเรียกร้อง (a)";
                dr1["IAR"] = (!string.IsNullOrEmpty(drclaim["iar_atc"].ToString()) ? drclaim["iar_atc"].ToString().Replace(",", "!comma") : "");
                dr1["BI"] = (!string.IsNullOrEmpty(drclaim["bi_atc"].ToString()) ? drclaim["bi_atc"].ToString().Replace(",", "!comma") : "");
                dr1["PL/CGL"] = (!string.IsNullOrEmpty(drclaim["pl_cgl_atc"].ToString()) ? drclaim["pl_cgl_atc"].ToString().Replace(",", "!comma") : "");
                dr1["PV"] = (!string.IsNullOrEmpty(drclaim["pv_atc"].ToString()) ? drclaim["pv_atc"].ToString().Replace(",", "!comma") : "");
                dr1["Total"] = (!string.IsNullOrEmpty(drclaim["ttl_atc"].ToString()) ? drclaim["ttl_atc"].ToString().Replace(",", "!comma") : "");
                dt.Rows.Add(dr1);

                dr1 = dt.NewRow();
                dr1["tagname"] = "#tableeltc#";
                dr1["Estimated losses to claim"] = "Deductible ค่ารับผิดส่วนแรก (b)";
                dr1["IAR"] = (!string.IsNullOrEmpty(drclaim["iar_ded"].ToString()) ? drclaim["iar_ded"].ToString().Replace(",", "!comma") : "");
                dr1["BI"] = (!string.IsNullOrEmpty(drclaim["bi_ded"].ToString()) ? drclaim["bi_ded"].ToString().Replace(",", "!comma") : "");
                dr1["PL/CGL"] = (!string.IsNullOrEmpty(drclaim["pl_cgl_ded"].ToString()) ? drclaim["pl_cgl_ded"].ToString().Replace(",", "!comma") : "");
                dr1["PV"] = (!string.IsNullOrEmpty(drclaim["pv_ded"].ToString()) ? drclaim["pv_ded"].ToString().Replace(",", "!comma") : "");
                dr1["Total"] = (!string.IsNullOrEmpty(drclaim["ttl_ded"].ToString()) ? drclaim["ttl_ded"].ToString().Replace(",", "!comma") : "");
                dt.Rows.Add(dr1);

                dr1 = dt.NewRow();
                dr1["tagname"] = "#tableeltc#";
                dr1["Estimated losses to claim"] = "Proceeds from claim มูลค่าสินไหม (C)";
                dr1["IAR"] = (!string.IsNullOrEmpty(drclaim["iar_pfc"].ToString()) ? drclaim["iar_pfc"].ToString().Replace(",", "!comma") : "");
                dr1["BI"] = (!string.IsNullOrEmpty(drclaim["bi_pfc"].ToString()) ? drclaim["bi_pfc"].ToString().Replace(",", "!comma") : "");
                dr1["PL/CGL"] = (!string.IsNullOrEmpty(drclaim["pl_cgl_pfc"].ToString()) ? drclaim["pl_cgl_pfc"].ToString().Replace(",", "!comma") : "");
                dr1["PV"] = (!string.IsNullOrEmpty(drclaim["pv_pfc"].ToString()) ? drclaim["pv_pfc"].ToString().Replace(",", "!comma") : "");
                dr1["Total"] = (!string.IsNullOrEmpty(drclaim["ttl_pfc"].ToString()) ? drclaim["ttl_pfc"].ToString().Replace(",", "!comma") : "");
                dt.Rows.Add(dr1);

                dr1 = dt.NewRow();
                dr1["tagname"] = "#tableeltc#";
                dr1["Estimated losses to claim"] = "Under amount to claim ส่วนต่างมูลค่าการเคลม (a-b-c)";
                dr1["IAR"] = (!string.IsNullOrEmpty(drclaim["iar_uatc"].ToString()) ? drclaim["iar_uatc"].ToString().Replace(",", "!comma") : "");
                dr1["BI"] = (!string.IsNullOrEmpty(drclaim["bi_uatc"].ToString()) ? drclaim["bi_uatc"].ToString().Replace(",", "!comma") : "");
                dr1["PL/CGL"] = (!string.IsNullOrEmpty(drclaim["pl_cgl_uatc"].ToString()) ? drclaim["pl_cgl_uatc"].ToString().Replace(",", "!comma") : "");
                dr1["PV"] = (!string.IsNullOrEmpty(drclaim["pv_uatc"].ToString()) ? drclaim["pv_uatc"].ToString().Replace(",", "!comma") : "");
                dr1["Total"] = (!string.IsNullOrEmpty(drclaim["ttl_uatc"].ToString()) ? drclaim["ttl_uatc"].ToString().Replace(",", "!comma") : "");
                dt.Rows.Add(dr1);

            }

            #endregion

            return dt;
        }

        public class ReplaceInsClaim_TagData
        {
            public string company { get; set; }
            public string propertyname { get; set; }
            public string incident { get; set; }
            public string occurreddate { get; set; }
            public string submidate { get; set; }
            public string submiday { get; set; }
            public string incidentsummary { get; set; }
            public string docref { get; set; }
            public string surveyor { get; set; }
            public string companysurveyor { get; set; }
            public string stmdate { get; set; }
            public string stmday { get; set; }
            public string remark { get; set; }

            public string sign_propname1 { get; set; }
            public string propname1 { get; set; }
            public string propposition1 { get; set; }
            public string propdate1 { get; set; }

            public string sign_propname2 { get; set; }
            public string propname2 { get; set; }
            public string propposition2 { get; set; }
            public string propdate2 { get; set; }

            public string sign_propname3 { get; set; }
            public string propname3 { get; set; }
            public string propposition3 { get; set; }
            public string propdate3 { get; set; }

            public string sign_propname3_1 { get; set; }
            public string propname3_1 { get; set; }
            public string propposition3_1 { get; set; }
            public string propdate3_1 { get; set; }

            public string sign_awcname1 { get; set; }
            public string awcname1 { get; set; }
            public string awcposition1 { get; set; }
            public string awcdate1 { get; set; }

            public string sign_awcname1_2 { get; set; }
            public string awcname1_2 { get; set; }
            public string awcposition1_2 { get; set; }
            public string awcdate1_2 { get; set; }

            public string sign_awcname1_3 { get; set; }
            public string awcname1_3 { get; set; }
            public string awcposition1_3 { get; set; }
            public string awcdate1_3 { get; set; }

            public string sign_awcname2 { get; set; }
            public string awcname2 { get; set; }
            public string awcposition2 { get; set; }
            public string awcdate2 { get; set; }

            public string sign_awcname3 { get; set; }
            public string awcname3 { get; set; }
            public string awcposition3 { get; set; }
            public string awcdate3 { get; set; }


        }
    }
}