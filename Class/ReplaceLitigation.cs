﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using static onlineLegalWF.Class.ReplaceCommRegis;

namespace onlineLegalWF.Class
{
    public class ReplaceLitigation
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        public string zconnstrrpa = ConfigurationManager.AppSettings["RPADB"].ToString();
        public WFFunctions zwf = new WFFunctions();
        #endregion

        public DataTable genTagData(ReplaceLitigation_TagData data)
        {
            DataTable dtStr = new DataTable();
            dtStr.Columns.Add("tagname", typeof(string));
            dtStr.Columns.Add("tagvalue", typeof(string));

            DataRow dr0 = dtStr.NewRow();
            dr0["tagname"] = "#docno#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.docno) ? data.docno.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#subject#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.subject) ? data.subject.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#to#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.to) ? data.to.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#reqdate#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.reqdate) ? data.reqdate.Replace(",", "!comma") : ""); // Utillity.ConvertDateToLongDateTime(xreq_date, "en");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#desc#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.desc) ? data.desc.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);

            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#name1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.name1) ? data.name1.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#position1#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.position1) ? data.position1.Replace(",", "!comma") : "");
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
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.position2) ? data.position2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#date2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.date2) ? data.date2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            dr0 = dtStr.NewRow();
            dr0["tagname"] = "#sign_name2#";
            dr0["tagvalue"] = (!string.IsNullOrEmpty(data.sign_name2) ? data.sign_name2.Replace(",", "!comma") : "");
            dtStr.Rows.Add(dr0);
            return dtStr;
        }

        public DataTable BindTagData(string xprocess_id, ReplaceLitigation_TagData data)
        {
            ReplaceLitigation_TagData res = new ReplaceLitigation_TagData();

            string sql0 = @"select * from li_litigation_request where process_id = '" + xprocess_id + "'";
            var dt0 = zdb.ExecSql_DataTable(sql0, zconnstr);
            if (dt0.Rows.Count > 0)
            {
                var dr0 = dt0.Rows[0];
                res.docno = dr0["document_no"].ToString().Trim();
                res.subject = dr0["lit_subject"].ToString().Trim();
                res.reqdate = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr0["req_date"]), "th").Trim();
                res.desc = dr0["lit_desc"].ToString().Trim();
                res.to = "คุณอร่าม รัตนโชติ Head of Litigation and Registration";
                res.name1 = data.name1;
                res.position1 = data.position1;
                res.date1 = data.date1;
                res.name2 = data.name2;
                res.position2 = data.position2;
                res.date2 = data.date2;
            }

            //string sql = "select * from wf_routing where process_id = '" + xprocess_id + "' ";
            //var dt1 = zdb.ExecSql_DataTable(sql, zconnstr);
            //if (dt1.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt1.Rows.Count; i++)
            //    {
            //        var dr = dt1.Rows[i];

            //        if (dr["step_name"].ToString() == "Start")
            //        {
            //            if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
            //            {
            //                res.sign_name1 = "Approved by system";
            //                res.date1 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
            //            }
            //        }
            //        else if (dr["step_name"].ToString() == "Supervisor Approve")
            //        {
            //            if (dr["wf_status"].ToString() != "" && dr["updated_datetime"].ToString() != "")
            //            {
            //                res.sign_name2 = "Approved by system";
            //                res.date2 = Utillity.ConvertDateToLongDateTime(Convert.ToDateTime(dr["updated_datetime"]), "en");
            //            }
            //        }

            //    }
            //}

            var dtStr = genTagData(res);
            return dtStr;
        }

        public class ReplaceLitigation_TagData
        {
            public string docno { get; set; }
            public string subject { get; set; }
            public string to { get; set; }
            public string reqdate { get; set; }
            public string desc { get; set; }

            public string sign_name1 { get; set; }
            public string name1 { get; set; }
            public string position1 { get; set; }
            public string date1 { get; set; }

            public string sign_name2 { get; set; }
            public string name2 { get; set; }
            public string position2 { get; set; }
            public string date2 { get; set; }


        }
    }
}