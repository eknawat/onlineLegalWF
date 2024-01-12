﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using static ReplaceDocx.Class.ReplaceDocx;
using onlineLegalWF.Class;
using System.Globalization;
using Spire.Doc;

namespace onlineLegalWF.test
{
    public partial class testReplaceDoc1 : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationSettings.AppSettings["BPMDB"].ToString();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            var tag1 = int.Parse("1,500,500", NumberStyles.AllowThousands);
            var tag2 = int.Parse("1500500", NumberStyles.AllowThousands);

            tagname1.Text = tag1.ToString();

            tagname2.Text = tag2.ToString();

            string file1 = @"C__WordTemplate_Insurance_Output_inreq_20240108_133407.docx";
            string file2 = @"C__WordTemplate_Insurance_Output_inreq_20240108_114222.docx";
            string folder = @"D:\Users\worawut.m\Downloads\";
            Document document = new Document();
            document.LoadFromFile(folder+file1, FileFormat.Docx);
            document.InsertTextFromFile(folder+file2, FileFormat.Docx);
            document.SaveToFile(folder+"MergedFile.docx", FileFormat.Docx);
            System.Diagnostics.Process.Start(folder+"MergedFile.docx");

        }

        protected void btnTestRun_Click(object sender, EventArgs e)
        {
            // Replace Doc

            //var tag1 = tagname1.Text.Trim();
            //var tag2 = tagname2.Text.Trim();

            var xsign_upload = sign_upload;

            string templatefile = @"C:\WordTemplate\test\testReplaceSign.docx";
            string outputfoler = @"C:\WordTemplate\test\Output";
            string outputfn = outputfoler + @"\inreq_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

            var rdoc = new ReplaceDocx.Class.ReplaceDocx();


            #region prepare data
            ////Replace TAG STRING
            //DataTable dtStr = new DataTable();
            //dtStr.Columns.Add("tagname", typeof(string));
            //dtStr.Columns.Add("tagvalue", typeof(string));

            //DataRow dr0 = dtStr.NewRow();
            //dr0["tagname"] = "#1#";
            //dr0["tagvalue"] = tag1.Replace(",", "!comma");
            //dtStr.Rows.Add(dr0);
            //dr0 = dtStr.NewRow();
            //dr0["tagname"] = "#2#";
            //dr0["tagvalue"] = tag2.Replace(",", "!comma");
            //dtStr.Rows.Add(dr0);

            List<TagData> listTagData = new List<TagData>();


            if (xsign_upload.HasFile)
            {
                

                var fileByte = xsign_upload.FileBytes;

                //DOA
                #region DOA 
                var requestor = "คุณรุ่งเรือง วิโรจน์ชีวัน";
                var requestorpos = "Head of Operations";
                var requestordate = Utillity.ConvertDateToLongDateTime(System.DateTime.Now, "en");

                var apv1 = "คุณจรูณศักดิ์ นามะฮง";
                var apv1pos = "Insurance Specialist";
                var apv1date = "";
                var apv2 = "คุณชโลทร ศรีสมวงษ์";
                var apv2pos = "Head of Legal";
                var apv2date = "";
                var apv3 = "คุณชยุต อมตวนิช";
                var apv3pos = "Head of Risk Management";
                var apv3date = "";

                var apv4 = "ดร.สิเวศ โรจนสุนทร";
                var apv4pos = "CCO";
                var apv4date = "";
                var apv4cb1 = "";
                var apv4cb2 = "";
                var apv4remark = "";

                TagData tagData = new TagData();
                tagData.tagname = "#name1#";
                tagData.string_tag = true;
                tagData.tagvalue = requestor.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#position1#";
                tagData.string_tag = true;
                tagData.tagvalue = requestorpos.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#date1#";
                tagData.string_tag = true;
                tagData.tagvalue = requestordate.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#name2#";
                tagData.string_tag = true;
                tagData.tagvalue = apv1.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#position2#";
                tagData.string_tag = true;
                tagData.tagvalue = apv1pos.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#date2#";
                tagData.string_tag = true;
                tagData.tagvalue = apv1date.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#name3#";
                tagData.string_tag = true;
                tagData.tagvalue = apv2.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#position3#";
                tagData.string_tag = true;
                tagData.tagvalue = apv2pos.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#date3#";
                tagData.string_tag = true;
                tagData.tagvalue = apv2date.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#name4#";
                tagData.string_tag = true;
                tagData.tagvalue = apv3.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#position4#";
                tagData.string_tag = true;
                tagData.tagvalue = apv3pos.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#date4#";
                tagData.string_tag = true;
                tagData.tagvalue = apv3date.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#name5#";
                tagData.string_tag = true;
                tagData.tagvalue = apv4.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#position5#";
                tagData.string_tag = true;
                tagData.tagvalue = apv4pos.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#date5#";
                tagData.string_tag = true;
                tagData.tagvalue = apv4date.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#cb1#";
                tagData.string_tag = true;
                tagData.tagvalue = apv4cb1.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#cb2#";
                tagData.string_tag = true;
                tagData.tagvalue = apv4cb2.Replace(",", "!comma");
                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#remark5#";
                tagData.string_tag = true;
                tagData.tagvalue = apv4remark.Replace(",", "!comma");
                listTagData.Add(tagData);
                #endregion


                //Replace Sign Form FileUpload
                tagData = new TagData();
                tagData.tagname = "#sign_name1#";
                tagData.image_tag = true;
                tagData.imagecontent = fileByte;

                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#sign_name2#";
                tagData.image_tag = true;
                tagData.imagecontent = fileByte;

                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#sign_name3#";
                tagData.image_tag = true;
                tagData.imagecontent = fileByte;

                listTagData.Add(tagData);

                tagData = new TagData();
                tagData.tagname = "#sign_name4#";
                tagData.image_tag = true;
                tagData.imagecontent = fileByte;

                listTagData.Add(tagData);
                //string extension = System.IO.Path.GetExtension(xsign_upload.FileName);

                //if (extension == ".jpg")
                //{
                //    xsign_upload.SaveAs(outputfoler + xsign_upload.FileName);

                //}
                //else
                //{
                //    Response.Write("Only .Jpg allowed");
                //}
            }
            #endregion


            #region Sample ReplaceTable

            ////DataTable Column Properties
            ////col_name, col_width, col_align, col_valign,
            //DataTable dtProperties1 = new DataTable();
            //dtProperties1.Columns.Add("tagname", typeof(string));
            //dtProperties1.Columns.Add("col_name", typeof(string));
            //dtProperties1.Columns.Add("col_width", typeof(string));
            //dtProperties1.Columns.Add("col_align", typeof(string)); //Left, Right, Center
            //dtProperties1.Columns.Add("col_valign", typeof(string)); //Top, Middle, Bottom
            //dtProperties1.Columns.Add("col_font", typeof(string));
            //dtProperties1.Columns.Add("col_fontsize", typeof(string));
            //dtProperties1.Columns.Add("col_fontcolor", typeof(string));
            //dtProperties1.Columns.Add("col_color", typeof(string));
            //dtProperties1.Columns.Add("header_height", typeof(string));
            //dtProperties1.Columns.Add("header_color", typeof(string));
            //dtProperties1.Columns.Add("header_font", typeof(string));
            //dtProperties1.Columns.Add("header_fontsize", typeof(string));
            //dtProperties1.Columns.Add("header_fontbold", typeof(string));
            //dtProperties1.Columns.Add("header_align", typeof(string)); //Left, Right, Center
            //dtProperties1.Columns.Add("header_valign", typeof(string)); //Top, Middle, Bottom
            //dtProperties1.Columns.Add("header_fontcolor", typeof(string));
            //dtProperties1.Columns.Add("row_height", typeof(string));
            //// Replace #table1# ------------------------------------------------------
            //DataRow dr = dtProperties1.NewRow();
            //dr["tagname"] = "#table1#";
            //dr["col_name"] = "No";
            //dr["col_width"] = "100";
            //dr["col_align"] = "Center";
            //dr["col_valign"] = "Top";
            //dr["col_font"] = "Tahoma";
            //dr["col_fontsize"] = "9";
            //dr["col_fontcolor"] = "Black";
            //dr["col_color"] = "Transparent";
            //dr["header_height"] = "20";
            //dr["header_color"] = "Gray";
            //dr["header_font"] = "Tahoma";
            //dr["header_fontsize"] = "9";
            //dr["header_fontbold"] = "true";
            //dr["header_align"] = "Middle";
            //dr["header_valign"] = "Center";
            //dr["header_fontcolor"] = "White";
            //dr["row_height"] = "16";
            //dtProperties1.Rows.Add(dr);

            //dr = dtProperties1.NewRow();
            //dr["tagname"] = "#table1#";
            //dr["col_name"] = "Property Insured";
            //dr["col_width"] = "200";
            //dr["col_align"] = "Left";
            //dr["col_valign"] = "Top";
            //dr["col_font"] = "Tahoma";
            //dr["col_fontsize"] = "9";
            //dr["col_fontcolor"] = "Black";
            //dr["col_color"] = "Transparent";
            //dr["header_height"] = "20";
            //dr["header_color"] = "Gray";
            //dr["header_font"] = "Tahoma";
            //dr["header_fontsize"] = "9";
            //dr["header_fontbold"] = "true";
            //dr["header_align"] = "Center";
            //dr["header_valign"] = "Middle";
            //dr["header_fontcolor"] = "White";
            //dr["row_height"] = "16";
            //dtProperties1.Rows.Add(dr);

            //dr = dtProperties1.NewRow();
            //dr["tagname"] = "#table1#";
            //dr["col_name"] = "Indemnity Period";
            //dr["col_width"] = "200";
            //dr["col_align"] = "Left";
            //dr["col_valign"] = "Top";
            //dr["col_font"] = "Tahoma";
            //dr["col_fontsize"] = "9";
            //dr["col_fontcolor"] = "Black";
            //dr["col_color"] = "Transparent";
            //dr["header_height"] = "20";
            //dr["header_color"] = "Gray";
            //dr["header_font"] = "Tahoma";
            //dr["header_fontsize"] = "9";
            //dr["header_fontbold"] = "true";
            //dr["header_align"] = "Center";
            //dr["header_valign"] = "Middle";
            //dr["header_fontcolor"] = "White";
            //dr["row_height"] = "16";
            //dtProperties1.Rows.Add(dr);

            //dr = dtProperties1.NewRow();
            //dr["tagname"] = "#table1#";
            //dr["col_name"] = "Sum Insured";
            //dr["col_width"] = "200";
            //dr["col_align"] = "left";
            //dr["col_valign"] = "top";
            //dr["col_font"] = "Tahoma";
            //dr["col_fontsize"] = "9";
            //dr["col_fontcolor"] = "Black";
            //dr["col_color"] = "Transparent";
            //dr["header_height"] = "20";
            //dr["header_color"] = "Gray";
            //dr["header_font"] = "Tahoma";
            //dr["header_fontsize"] = "9";
            //dr["header_fontbold"] = "true";
            //dr["header_align"] = "Center";
            //dr["header_valign"] = "Middle";
            //dr["header_fontcolor"] = "White";
            //dr["row_height"] = "16";
            //dtProperties1.Rows.Add(dr);

            //dr = dtProperties1.NewRow();
            //dr["tagname"] = "#table1#";
            //dr["col_name"] = "Start Date";
            //dr["col_width"] = "100";
            //dr["col_align"] = "left";
            //dr["col_valign"] = "top";
            //dr["col_font"] = "Tahoma";
            //dr["col_fontsize"] = "9";
            //dr["col_fontcolor"] = "Black";
            //dr["col_color"] = "Transparent";
            //dr["header_height"] = "20";
            //dr["header_color"] = "Gray";
            //dr["header_font"] = "Tahoma";
            //dr["header_fontsize"] = "9";
            //dr["header_fontbold"] = "true";
            //dr["header_align"] = "Center";
            //dr["header_valign"] = "Middle";
            //dr["header_fontcolor"] = "White";
            //dr["row_height"] = "16";
            //dtProperties1.Rows.Add(dr);

            //dr = dtProperties1.NewRow();
            //dr["tagname"] = "#table1#";
            //dr["col_name"] = "End Date";
            //dr["col_width"] = "100";
            //dr["col_align"] = "left";
            //dr["col_valign"] = "top";
            //dr["col_font"] = "Tahoma";
            //dr["col_fontsize"] = "9";
            //dr["col_fontcolor"] = "Black";
            //dr["col_color"] = "Transparent";
            //dr["header_height"] = "20";
            //dr["header_color"] = "Gray";
            //dr["header_font"] = "Tahoma";
            //dr["header_fontsize"] = "9";
            //dr["header_fontbold"] = "true";
            //dr["header_align"] = "Center";
            //dr["header_valign"] = "Middle";
            //dr["header_fontcolor"] = "White";
            //dr["row_height"] = "16";
            //dtProperties1.Rows.Add(dr);

            ////  DataTable dt = new DataTable();
            //DataTable dt = new DataTable();
            //dt.Columns.Add("tagname", typeof(string));
            //dt.Columns.Add("No", typeof(string));
            //dt.Columns.Add("Property Insured", typeof(string));
            //dt.Columns.Add("Indemnity Period", typeof(string));
            //dt.Columns.Add("Sum Insured", typeof(string));
            //dt.Columns.Add("Start Date", typeof(string));
            //dt.Columns.Add("End Date", typeof(string));

            ////DataTable for #table1#
            //var dataGV = iniDataTable();

            //for (int i = 0; i < dataGV.Rows.Count; i++)
            //{
            //    var drGV = dataGV.Rows[i];

            //    DataRow dr1 = dt.NewRow();
            //    dr1["tagname"] = "#table1#";
            //    dr1["No"] = drGV["No"].ToString();
            //    dr1["Property Insured"] = drGV["PropertyInsured"].ToString();  // "xxxxx";//.Text.Replace(",", "!comma");
            //    dr1["Indemnity Period"] = drGV["IndemnityPeriod"].ToString(); // "1,000,000".Replace(",", "!comma"); ;
            //    dr1["Sum Insured"] = drGV["SumInsured"].ToString();  // "15,000".Replace(",", "!comma"); ;
            //    dr1["Start Date"] = drGV["StartDate"].ToString();
            //    dr1["End Date"] = drGV["EndDate"].ToString();
            //    dt.Rows.Add(dr1);
            //}
            #endregion


            //// Convert to JSONString
            //DataTable dtTagPropsTable = new DataTable();
            //dtTagPropsTable.Columns.Add("tagname", typeof(string));
            //dtTagPropsTable.Columns.Add("jsonstring", typeof(string));

            //DataTable dtTagDataTable = new DataTable();
            //dtTagDataTable.Columns.Add("tagname", typeof(string));
            //dtTagDataTable.Columns.Add("jsonstring", typeof(string));
            //ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            //var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
            //var jsonDTProperties1 = repl.DataTableToJSONWithStringBuilder(dtProperties1);
            ////var jsonDTProperties2 = repl.DataTableToJSONWithStringBuilder(dtProperties2);
            ////var jsonDTdata = repl.DataTableToJSONWithStringBuilder(dt);
            ////var jsonDTdata2 = repl.DataTableToJSONWithStringBuilder(dt2);
            ////end prepare data

            //// Save to Database z_replacedocx_log
            ////string xreq_no = System.DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            ////string sql = @"insert into z_replacedocx_log (replacedocx_reqno,jsonTagString, jsonTableProp, jsonTableData,template_filepath , output_directory,output_filepath, delete_output ) 
            ////            values('"+xreq_no+ @"',
            ////                   '" + jsonDTStr + @"', 
            ////                    '" + jsonDTProperties1 + @"', 
            ////                    '" + jsonDTdata + @"', 
            ////                    '" + templatefile + @"', 
            ////                    '" + outputfoler + @"', 
            ////                    '" + outputfn + @"',  
            ////                    '" + "0" + @"',
            ////                ) ";

            ////zdb.ExecNonQuery(sql, zconnstr); 

            //var outputbyte = rdoc.ReplaceData2(jsonDTStr, jsonDTProperties1, jsonDTdata, templatefile, outputfoler, outputfn, false);

            var outputbyte = rdoc.ReplaceData(listTagData, templatefile, outputfoler, outputfn, false);

            ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
            repl.convertDOCtoPDF(outputfn, outputfn.Replace(".docx", ".pdf"), false);
            // Dowload Word 
            Response.Clear();
            Response.ContentType = "text/xml";
            Response.AddHeader("content-disposition", $"attachment; filename={outputfn}");
            Response.BinaryWrite(outputbyte);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.End();


        }
        //protected void btnTestRun_Click(object sender, EventArgs e)
        //{
        //    // Replace Doc

        //    var tag1 = tagname1.Text.Trim();
        //    var tag2 = tagname2.Text.Trim();

        //    string templatefile = @"C:\WordTemplate\test\testReplaceSign.docx";
        //    string outputfoler = @"C:\WordTemplate\test\Output";
        //    string outputfn = outputfoler + @"\inreq_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".docx";

        //    var rdoc = new ReplaceDocx.Class.ReplaceDocx();

        //    #region prepare data
        //    //Replace TAG STRING
        //    DataTable dtStr = new DataTable();
        //    dtStr.Columns.Add("tagname", typeof(string));
        //    dtStr.Columns.Add("tagvalue", typeof(string));

        //    DataRow dr0 = dtStr.NewRow();
        //    dr0["tagname"] = "#1#";
        //    dr0["tagvalue"] = tag1.Replace(",", "!comma");
        //    dtStr.Rows.Add(dr0);
        //    dr0 = dtStr.NewRow();
        //    dr0["tagname"] = "#2#";
        //    dr0["tagvalue"] = tag2.Replace(",", "!comma");
        //    dtStr.Rows.Add(dr0);
        //    #endregion

            
        //    #region Sample ReplaceTable

        //    //DataTable Column Properties
        //    //col_name, col_width, col_align, col_valign,
        //    DataTable dtProperties1 = new DataTable();
        //    dtProperties1.Columns.Add("tagname", typeof(string));
        //    dtProperties1.Columns.Add("col_name", typeof(string));
        //    dtProperties1.Columns.Add("col_width", typeof(string));
        //    dtProperties1.Columns.Add("col_align", typeof(string)); //Left, Right, Center
        //    dtProperties1.Columns.Add("col_valign", typeof(string)); //Top, Middle, Bottom
        //    dtProperties1.Columns.Add("col_font", typeof(string));
        //    dtProperties1.Columns.Add("col_fontsize", typeof(string));
        //    dtProperties1.Columns.Add("col_fontcolor", typeof(string));
        //    dtProperties1.Columns.Add("col_color", typeof(string));
        //    dtProperties1.Columns.Add("header_height", typeof(string));
        //    dtProperties1.Columns.Add("header_color", typeof(string));
        //    dtProperties1.Columns.Add("header_font", typeof(string));
        //    dtProperties1.Columns.Add("header_fontsize", typeof(string));
        //    dtProperties1.Columns.Add("header_fontbold", typeof(string));
        //    dtProperties1.Columns.Add("header_align", typeof(string)); //Left, Right, Center
        //    dtProperties1.Columns.Add("header_valign", typeof(string)); //Top, Middle, Bottom
        //    dtProperties1.Columns.Add("header_fontcolor", typeof(string));
        //    dtProperties1.Columns.Add("row_height", typeof(string));
        //    // Replace #table1# ------------------------------------------------------
        //    DataRow dr = dtProperties1.NewRow();
        //    dr["tagname"] = "#table1#";
        //    dr["col_name"] = "No";
        //    dr["col_width"] = "100";
        //    dr["col_align"] = "Center";
        //    dr["col_valign"] = "Top";
        //    dr["col_font"] = "Tahoma";
        //    dr["col_fontsize"] = "9";
        //    dr["col_fontcolor"] = "Black";
        //    dr["col_color"] = "Transparent";
        //    dr["header_height"] = "20";
        //    dr["header_color"] = "Gray";
        //    dr["header_font"] = "Tahoma";
        //    dr["header_fontsize"] = "9";
        //    dr["header_fontbold"] = "true";
        //    dr["header_align"] = "Middle";
        //    dr["header_valign"] = "Center";
        //    dr["header_fontcolor"] = "White";
        //    dr["row_height"] = "16";
        //    dtProperties1.Rows.Add(dr);

        //    dr = dtProperties1.NewRow();
        //    dr["tagname"] = "#table1#";
        //    dr["col_name"] = "Property Insured";
        //    dr["col_width"] = "200";
        //    dr["col_align"] = "Left";
        //    dr["col_valign"] = "Top";
        //    dr["col_font"] = "Tahoma";
        //    dr["col_fontsize"] = "9";
        //    dr["col_fontcolor"] = "Black";
        //    dr["col_color"] = "Transparent";
        //    dr["header_height"] = "20";
        //    dr["header_color"] = "Gray";
        //    dr["header_font"] = "Tahoma";
        //    dr["header_fontsize"] = "9";
        //    dr["header_fontbold"] = "true";
        //    dr["header_align"] = "Center";
        //    dr["header_valign"] = "Middle";
        //    dr["header_fontcolor"] = "White";
        //    dr["row_height"] = "16";
        //    dtProperties1.Rows.Add(dr);

        //    dr = dtProperties1.NewRow();
        //    dr["tagname"] = "#table1#";
        //    dr["col_name"] = "Indemnity Period";
        //    dr["col_width"] = "200";
        //    dr["col_align"] = "Left";
        //    dr["col_valign"] = "Top";
        //    dr["col_font"] = "Tahoma";
        //    dr["col_fontsize"] = "9";
        //    dr["col_fontcolor"] = "Black";
        //    dr["col_color"] = "Transparent";
        //    dr["header_height"] = "20";
        //    dr["header_color"] = "Gray";
        //    dr["header_font"] = "Tahoma";
        //    dr["header_fontsize"] = "9";
        //    dr["header_fontbold"] = "true";
        //    dr["header_align"] = "Center";
        //    dr["header_valign"] = "Middle";
        //    dr["header_fontcolor"] = "White";
        //    dr["row_height"] = "16";
        //    dtProperties1.Rows.Add(dr);

        //    dr = dtProperties1.NewRow();
        //    dr["tagname"] = "#table1#";
        //    dr["col_name"] = "Sum Insured";
        //    dr["col_width"] = "200";
        //    dr["col_align"] = "left";
        //    dr["col_valign"] = "top";
        //    dr["col_font"] = "Tahoma";
        //    dr["col_fontsize"] = "9";
        //    dr["col_fontcolor"] = "Black";
        //    dr["col_color"] = "Transparent";
        //    dr["header_height"] = "20";
        //    dr["header_color"] = "Gray";
        //    dr["header_font"] = "Tahoma";
        //    dr["header_fontsize"] = "9";
        //    dr["header_fontbold"] = "true";
        //    dr["header_align"] = "Center";
        //    dr["header_valign"] = "Middle";
        //    dr["header_fontcolor"] = "White";
        //    dr["row_height"] = "16";
        //    dtProperties1.Rows.Add(dr);

        //    dr = dtProperties1.NewRow();
        //    dr["tagname"] = "#table1#";
        //    dr["col_name"] = "Start Date";
        //    dr["col_width"] = "100";
        //    dr["col_align"] = "left";
        //    dr["col_valign"] = "top";
        //    dr["col_font"] = "Tahoma";
        //    dr["col_fontsize"] = "9";
        //    dr["col_fontcolor"] = "Black";
        //    dr["col_color"] = "Transparent";
        //    dr["header_height"] = "20";
        //    dr["header_color"] = "Gray";
        //    dr["header_font"] = "Tahoma";
        //    dr["header_fontsize"] = "9";
        //    dr["header_fontbold"] = "true";
        //    dr["header_align"] = "Center";
        //    dr["header_valign"] = "Middle";
        //    dr["header_fontcolor"] = "White";
        //    dr["row_height"] = "16";
        //    dtProperties1.Rows.Add(dr);

        //    dr = dtProperties1.NewRow();
        //    dr["tagname"] = "#table1#";
        //    dr["col_name"] = "End Date";
        //    dr["col_width"] = "100";
        //    dr["col_align"] = "left";
        //    dr["col_valign"] = "top";
        //    dr["col_font"] = "Tahoma";
        //    dr["col_fontsize"] = "9";
        //    dr["col_fontcolor"] = "Black";
        //    dr["col_color"] = "Transparent";
        //    dr["header_height"] = "20";
        //    dr["header_color"] = "Gray";
        //    dr["header_font"] = "Tahoma";
        //    dr["header_fontsize"] = "9";
        //    dr["header_fontbold"] = "true";
        //    dr["header_align"] = "Center";
        //    dr["header_valign"] = "Middle";
        //    dr["header_fontcolor"] = "White";
        //    dr["row_height"] = "16";
        //    dtProperties1.Rows.Add(dr);

        //    //  DataTable dt = new DataTable();
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("tagname", typeof(string));
        //    dt.Columns.Add("No", typeof(string));
        //    dt.Columns.Add("Property Insured", typeof(string));
        //    dt.Columns.Add("Indemnity Period", typeof(string));
        //    dt.Columns.Add("Sum Insured", typeof(string));
        //    dt.Columns.Add("Start Date", typeof(string));
        //    dt.Columns.Add("End Date", typeof(string));

        //    //DataTable for #table1#
        //    var dataGV = iniDataTable();

        //    for (int i = 0; i < dataGV.Rows.Count; i++)
        //    {
        //        var drGV = dataGV.Rows[i];

        //        DataRow dr1 = dt.NewRow();
        //        dr1["tagname"] = "#table1#";
        //        dr1["No"] = drGV["No"].ToString();
        //        dr1["Property Insured"] = drGV["PropertyInsured"].ToString();  // "xxxxx";//.Text.Replace(",", "!comma");
        //        dr1["Indemnity Period"] = drGV["IndemnityPeriod"].ToString(); // "1,000,000".Replace(",", "!comma"); ;
        //        dr1["Sum Insured"] = drGV["SumInsured"].ToString();  // "15,000".Replace(",", "!comma"); ;
        //        dr1["Start Date"] = drGV["StartDate"].ToString();
        //        dr1["End Date"] = drGV["EndDate"].ToString();
        //        dt.Rows.Add(dr1);
        //    }
        //    #endregion


        //    // Convert to JSONString
        //    DataTable dtTagPropsTable = new DataTable();
        //    dtTagPropsTable.Columns.Add("tagname", typeof(string));
        //    dtTagPropsTable.Columns.Add("jsonstring", typeof(string));

        //    DataTable dtTagDataTable = new DataTable();
        //    dtTagDataTable.Columns.Add("tagname", typeof(string));
        //    dtTagDataTable.Columns.Add("jsonstring", typeof(string));
        //    ReplaceDocx.Class.ReplaceDocx repl = new ReplaceDocx.Class.ReplaceDocx();
        //    var jsonDTStr = repl.DataTableToJSONWithStringBuilder(dtStr);
        //    var jsonDTProperties1 = repl.DataTableToJSONWithStringBuilder(dtProperties1);
        //    //var jsonDTProperties2 = repl.DataTableToJSONWithStringBuilder(dtProperties2);
        //    var jsonDTdata = repl.DataTableToJSONWithStringBuilder(dt);
        //    //var jsonDTdata2 = repl.DataTableToJSONWithStringBuilder(dt2);
        //    //end prepare data

        //    // Save to Database z_replacedocx_log
        //    //string xreq_no = System.DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
        //    //string sql = @"insert into z_replacedocx_log (replacedocx_reqno,jsonTagString, jsonTableProp, jsonTableData,template_filepath , output_directory,output_filepath, delete_output ) 
        //    //            values('"+xreq_no+ @"',
        //    //                   '" + jsonDTStr + @"', 
        //    //                    '" + jsonDTProperties1 + @"', 
        //    //                    '" + jsonDTdata + @"', 
        //    //                    '" + templatefile + @"', 
        //    //                    '" + outputfoler + @"', 
        //    //                    '" + outputfn + @"',  
        //    //                    '" + "0" + @"',
        //    //                ) ";

        //    //zdb.ExecNonQuery(sql, zconnstr); 

        //    var outputbyte = rdoc.ReplaceData2(jsonDTStr, jsonDTProperties1, jsonDTdata, templatefile, outputfoler, outputfn, false);

        //    repl.convertDOCtoPDF(outputfn, outputfn.Replace(".docx", ".pdf"), false);
        //    // Dowload Word 
        //    Response.Clear();
        //    Response.ContentType = "text/xml";
        //    Response.AddHeader("content-disposition", $"attachment; filename={outputfn}");
        //    Response.BinaryWrite(outputbyte);
        //    Response.ContentEncoding = System.Text.Encoding.UTF8;
        //    Response.End();


        //}

        public DataTable iniDataTable()
        {
            //getData
            var dt = iniDTStructure();
            var dr = dt.NewRow();
            dr["No"] = "1";
            dr["PropertyInsured"] = "IAR";
            dr["IndemnityPeriod"] = "";
            dr["SumInsured"] = "";
            dr["StartDate"] = "";
            dr["EndDate"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["No"] = "2";
            dr["PropertyInsured"] = "BI";
            dr["IndemnityPeriod"] = "";
            dr["SumInsured"] = "";
            dr["StartDate"] = "";
            dr["EndDate"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["No"] = "3";
            dr["PropertyInsured"] = "CGL";
            dr["IndemnityPeriod"] = "";
            dr["SumInsured"] = "";
            dr["StartDate"] = "";
            dr["EndDate"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["No"] = "4";
            dr["PropertyInsured"] = "PL";
            dr["IndemnityPeriod"] = "";
            dr["SumInsured"] = "";
            dr["StartDate"] = "";
            dr["EndDate"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["No"] = "5";
            dr["PropertyInsured"] = "PV";
            dr["IndemnityPeriod"] = "";
            dr["SumInsured"] = "";
            dr["StartDate"] = "";
            dr["EndDate"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["No"] = "6";
            dr["PropertyInsured"] = "LPG";
            dr["IndemnityPeriod"] = "";
            dr["SumInsured"] = "";
            dr["StartDate"] = "";
            dr["EndDate"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["No"] = "7";
            dr["PropertyInsured"] = "D&O";
            dr["IndemnityPeriod"] = "";
            dr["SumInsured"] = "";
            dr["StartDate"] = "";
            dr["EndDate"] = "";
            dt.Rows.Add(dr);

            return dt;
        }
        public DataTable iniDTStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("No", typeof(string));
            dt.Columns.Add("PropertyInsured", typeof(string));
            dt.Columns.Add("IndemnityPeriod", typeof(string));
            dt.Columns.Add("SumInsured", typeof(string));
            dt.Columns.Add("StartDate", typeof(string));
            dt.Columns.Add("EndDate", typeof(string));
            return dt;
        }
    } 
}