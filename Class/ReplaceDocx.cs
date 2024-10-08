using onlineLegalWF;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ReplaceDocx.Class
{
    public class ReplaceDocx
    {
        #region Public
        private string zBPMDB = ConfigurationManager.AppSettings["BPMDB"].ToString();
       
        #endregion
        public byte[] ReplaceData(List<TagData> tagdata, string template_filepath,string output_directory, string output_filepath, bool delete_output)
        {
            var xtemp = template_filepath.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string template_fn = xtemp[xtemp.Length - 1];
            string success_fn = "out_"+System.DateTime.Now.ToString("yyyyMMdd_HHmmss")+"_" + template_fn; 
            string temp_processDirectory = output_directory + "\\process_"+ System.DateTime.Now.ToString("yyyyMMdd_HHmmss") ;
            string template_filepath2 = temp_processDirectory + "\\" + template_fn;
            System.IO.Directory.CreateDirectory(temp_processDirectory);
            System.IO.File.Copy(template_filepath, template_filepath2);

            byte[] xoutput_content = new byte[0];
            var templateFile = template_filepath2; // "Template_letter.docx";
            var outPutFile = output_filepath; // @"C:\workspace13\wordtemplate\"+hidDocxName.Value  ;
            Document myDoc = new Document();
            myDoc.LoadFromFile(templateFile);

            for (int i = 0; i < tagdata.Count; i++)
            {
                if (tagdata[i].string_tag == true)  // Replace String
                {
                    myDoc.Replace(tagdata[i].tagname, tagdata[i].tagvalue, true, true);
                }
              
            }
            success_fn = temp_processDirectory + "\\" + success_fn;
            myDoc.SaveToFile(success_fn, FileFormat.Docx);
            myDoc = new Document();
            myDoc.LoadFromFile(success_fn);
           
            for (int i = 0; i < tagdata.Count; i++)
            {
                //if (tagdata[i].string_tag == true)  // Replace String
                //{
                //   // myDoc.Replace(tagdata[i].tagname, tagdata[i].tagvalue, true, true);
                //}
                //else if (tagdata[i].image_tag == true) // Replace Image
                if (tagdata[i].image_tag == true) // Replace Image
                {
                    Section section = myDoc.Sections[0];
                    TextSelection[] selections = myDoc.FindAllString(tagdata[i].tagname, true, true);

                    int index = 0;
                    TextRange range = null;
                    foreach (TextSelection selection in selections)
                    {
                        DocPicture pic = new DocPicture(myDoc);
                        pic.LoadImage(tagdata[i].imagecontent);
                        range = selection.GetAsOneRange();
                        index = range.OwnerParagraph.ChildObjects.IndexOf(range);
                        range.OwnerParagraph.ChildObjects.Insert(index, pic);
                        range.OwnerParagraph.ChildObjects.Remove(range);
                        
                    }

                }
                else if (tagdata[i].table_tag == true) // Replace TABLE
                {
                    string vertical_alignment = "";
                    string text_alignment = "";
                    Section section = myDoc.Sections[0];
                    TextSelection selection = myDoc.FindString(tagdata[i].tagname, true, true);
                    TextRange range = selection.GetAsOneRange();
                    Paragraph paragraph = range.OwnerParagraph;
                    Body body = paragraph.OwnerTextBody;
                    int index = body.ChildObjects.IndexOf(paragraph);

                    Table table = section.AddTable(true);
                    var dtData = tagdata[i].datatable;
                    string[] Header = new string[dtData.Columns.Count];
                    string xheader_value = "";
                    for (int acol = 0; acol < dtData.Columns.Count; acol++)
                    {
                        xheader_value = dtData.Rows[0][acol].ToString();
                        Header.SetValue(xheader_value, acol);
                    }
                    
                    table.ResetCells(dtData.Rows.Count, dtData.Columns.Count);
                    //Data Headeer
                    TableRow FRow = table.Rows[0];
                    FRow.IsHeader = true;
                    if (tagdata[i].datatable_header_rowheight > 0)
                    { FRow.Height = tagdata[i].datatable_header_rowheight; }
                    else { FRow.Height = 23; }
                    if (tagdata[i].datatable_header_backcolor != null) { FRow.RowFormat.BackColor = tagdata[i].datatable_header_backcolor; }
                    else { FRow.RowFormat.BackColor = Color.Blue; }
                 
                    for (int a = 0; a < Header.Length; a++)
                    {
                        //Cell Alignment
                        Paragraph p = FRow.Cells[a].AddParagraph();
                        try { FRow.Cells[a].Width = tagdata[i].datatable_header_colwidths[a]; } catch { }
                        try { vertical_alignment = tagdata[i].datatable_header_valign_TopMiddleBottom[a]; }
                        catch { vertical_alignment = "Middle"; }
                        try { text_alignment= tagdata[i].datatable_header_textalign_LeftCenterRight[a];  }
                        catch { text_alignment = "Center"; }
                        if (vertical_alignment.Contains("Middle"))
                        {
                            FRow.Cells[a].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        }else if (vertical_alignment.Contains("Top"))
                        {
                            FRow.Cells[a].CellFormat.VerticalAlignment = VerticalAlignment.Top;
                        }
                        else if (vertical_alignment.Contains("Bottom"))
                        {
                            FRow.Cells[a].CellFormat.VerticalAlignment = VerticalAlignment.Bottom;
                        }
                        if (text_alignment.Contains("Center"))
                        {
                            p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        }
                        else if (text_alignment.Contains("Left"))
                        {
                            p.Format.HorizontalAlignment = HorizontalAlignment.Left;
                        }
                        else if (text_alignment.Contains("Right"))
                        {
                            p.Format.HorizontalAlignment = HorizontalAlignment.Right;
                        }
                        TextRange TR = p.AppendText(Header[a]);
                        if (tagdata[i].datatable_header_font_name != "")
                        {
                            TR.CharacterFormat.FontName = tagdata[i].datatable_header_font_name;
                        }
                        else
                        {
                            TR.CharacterFormat.FontName = "Tahoma";
                        }
                        if (tagdata[i].datatable_header_font_size > 0)
                        {
                            TR.CharacterFormat.FontSize = tagdata[i].datatable_header_font_size;
                        }
                        else
                        {
                            TR.CharacterFormat.FontSize = 11;
                        }
                        if (tagdata[i].datatable_header_font_color != null)
                        {
                            TR.CharacterFormat.TextColor = tagdata[i].datatable_header_font_color;
                        }
                        else { TR.CharacterFormat.TextColor = Color.White; }
                        if (tagdata[i].datatable_header_font_bold == true) { TR.CharacterFormat.Bold = true; }
                        else { TR.CharacterFormat.Bold = false; }


                    }
                    //Data Row

                    for (int r = 1; r < tagdata[i].datatable.Rows.Count; r++)
                    {
                        
                        string[] dataCol = new string[Header.Length];

                        for (int b = 0; b < Header.Length; b++)
                        {
                           
                            dataCol.SetValue(tagdata[i].datatable.Rows[r][b].ToString(), b);
                            TableRow DataRow = table.Rows[r ];
                            if (tagdata[i].datatable_row_height > 0) { DataRow.Height = tagdata[i].datatable_row_height; } 
                            else { DataRow.Height = 20; }

                            //Cell Alignment
                            try { vertical_alignment = tagdata[i].datatable_col_valign_TopMiddleBottom[b]; }
                            catch { vertical_alignment = "Middle"; }
                            try { text_alignment = tagdata[i].datatable_col_textalign_LeftCenterRight[b]; }
                            catch { text_alignment = "Center"; }
                            if (vertical_alignment.Contains("Middle"))
                            {
                                DataRow.Cells[b].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                            }
                            else if (vertical_alignment.Contains("Top"))
                            {
                                DataRow.Cells[b].CellFormat.VerticalAlignment = VerticalAlignment.Top;
                            }
                            else if (vertical_alignment.Contains("Bottom"))
                            {
                                DataRow.Cells[b].CellFormat.VerticalAlignment = VerticalAlignment.Bottom;
                            }
                            //Fill Data in Rows
                             Paragraph p2 = DataRow.Cells[b].AddParagraph();
                             TextRange TR2 = p2.AppendText(dataCol[b]);

                            //Format Cells
                            if (text_alignment.Contains("Left"))
                            {
                                p2.Format.HorizontalAlignment = HorizontalAlignment.Left;
                            }
                            else if (text_alignment.Contains("Center"))
                            {
                                p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                            }
                            else if (text_alignment.Contains("Right"))
                            {
                                p2.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            }
                            else { p2.Format.HorizontalAlignment = HorizontalAlignment.Center; }
                            if (tagdata[i].datatable_col_font_name != "")
                            {
                                TR2.CharacterFormat.FontName = "Tahoma";
                            } else { TR2.CharacterFormat.FontName = "Tahoma"; }
                            if (tagdata[i].datatable_col_font_size > 0)
                            {
                                TR2.CharacterFormat.FontSize = tagdata[i].datatable_col_font_size;
                            }
                            else { TR2.CharacterFormat.FontSize = 10; }
                            if (tagdata[i].datatable_col_font_color != null)
                            {
                                TR2.CharacterFormat.TextColor = tagdata[i].datatable_col_font_color;
                            }
                            else
                            {
                                TR2.CharacterFormat.TextColor = Color.Black;
                            }
                        }
                        

                    }
                    body.ChildObjects.Remove(paragraph);
                    body.ChildObjects.Insert(index, table);
                }
            }
            success_fn = success_fn.Replace("out_", "out2_");
          //  success_fn = temp_processDirectory + "\\" + success_fn; 
            myDoc.SaveToFile( success_fn, FileFormat.Docx);
            xoutput_content = System.IO.File.ReadAllBytes(success_fn);
            System.IO.File.Copy(success_fn, outPutFile,true);
           
            System.IO.Directory.Delete(temp_processDirectory,true);
            if (delete_output == true) {System.IO.File.Delete(outPutFile); }
            return xoutput_content;
        }
        public byte[] ReplaceData2(string jsonTagString, string jsonTableProp, string jsonTableData,
            string template_filepath, string output_directory, string output_filepath, bool delete_output)
        {
            ReplaceDocxResult result = new ReplaceDocxResult();
            var xtemp = template_filepath.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string template_fn = xtemp[xtemp.Length - 1];
            string success_fn = "out_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + template_fn;
            string temp_processDirectory = output_directory + "\\process_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string template_filepath2 = temp_processDirectory + "\\" + template_fn;
            System.IO.Directory.CreateDirectory(temp_processDirectory);
            System.IO.File.Copy(template_filepath, template_filepath2);

            byte[] xoutput_content = new byte[0];
            var templateFile = template_filepath2; // "Template_letter.docx";
            var outPutFile = output_filepath; // @"C:\workspace13\wordtemplate\"+hidDocxName.Value  ;
            Document myDoc = new Document();
            myDoc.LoadFromFile(templateFile);

            // Replace Tag String 
            if (!string.IsNullOrEmpty(jsonTagString)) 
            {
                var dtString = JsonStringToDataTable(jsonTagString);
                for (int i =0; i < dtString.Rows.Count; i++)
                {
                    var dr = dtString.Rows[i];
                    myDoc.Replace(dr["tagname"].ToString(), dr["tagvalue"].ToString().Replace("!comma", ","), true, true);
                    
                }
            }
          
            success_fn = temp_processDirectory + "\\" + success_fn;
            myDoc.SaveToFile(success_fn, FileFormat.Docx);
            myDoc = new Document();
            myDoc.LoadFromFile(success_fn);

            // Replace Tag Table
            if (!string.IsNullOrEmpty(jsonTableData))
            {
                string[] dtProp0String = jsonTableProp.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] dtData0String = jsonTableData.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
             
                for (int arr = 0; arr < dtProp0String.Length; arr++)
                {
                    var dtProp0 = JsonStringToDataTable(dtProp0String[arr]);
                    var dtData0 = JsonStringToDataTable(dtData0String[arr]);
                    string[] tableTagNames = dtProp0.DefaultView.ToTable(true, "tagname").AsEnumerable().Select(r => r.Field<string>("tagname")).ToArray();
                    // Looping Table Tagname
                    string tableTagName = "";
                    string jsonPropTable = "";
                    string jsonDataTable = "";
                    for (int i = 0; i < tableTagNames.Length; i++)
                    {
                        var dtProp = dtProp0;
                        var dtData =dtData0;
                        // Replace One by One Table
                        tableTagName = tableTagNames[i];

                        // Finding Section
                        string vertical_alignment = "";
                        string text_alignment = "";
                        Section section = myDoc.Sections[0];
                        TextSelection selection = myDoc.FindString(tableTagName, true, true);
                        TextRange range = selection.GetAsOneRange();
                        Paragraph paragraph = range.OwnerParagraph;
                        Body body = paragraph.OwnerTextBody;
                        int index = body.ChildObjects.IndexOf(paragraph);
                        

                        Table table = section.AddTable(true);
                        string[] Header = new string[dtProp.Rows.Count];
                        string xheader_value = "";
                        for (int acol = 0; acol < dtProp.Rows.Count; acol++)
                        {
                            xheader_value = dtProp.Rows[acol]["col_name"].ToString();
                            Header.SetValue(xheader_value, acol);
                        }

                        table.ResetCells(dtData.Rows.Count + 1, dtProp.Rows.Count);
                        //Data Headeer
                        TableRow FRow = table.Rows[0];
                        FRow.IsHeader = true;

                        if (dtProp.Rows.Count > 0)
                        {
                            var drCol = dtProp.Rows[0];
                            //Set Row Height
                            if (drCol["header_height"].ToString() != "")
                            {
                                try
                                {
                                    FRow.Height = float.Parse(drCol["header_height"].ToString());
                                }
                                catch
                                {
                                    FRow.Height = 23;
                                }
                            }
                            // Set BackColor 
                            if (drCol["header_color"].ToString() != "")
                            {
                                try
                                {
                                    FRow.RowFormat.BackColor = Color.FromName(drCol["header_color"].ToString());
                                }
                                catch
                                {
                                    FRow.RowFormat.BackColor = Color.Blue;
                                }
                            }
                        }

                        for (int a = 0; a < dtProp.Rows.Count; a++)
                        {
                            var drCol = dtProp.Rows[a];

                            //Cell Alignment
                            Paragraph p = FRow.Cells[a].AddParagraph();
                            p.Format.AfterAutoSpacing = false;
                            p.Format.AfterSpacing = 0;
                            try { FRow.Cells[a].Width = float.Parse(drCol["col_width"].ToString()); } catch { }
                            try { vertical_alignment = drCol["header_valign"].ToString(); }
                            catch { vertical_alignment = "Middle"; }
                            try { text_alignment = drCol["header_align"].ToString(); }
                            catch { text_alignment = "Center"; }
                            if (vertical_alignment.Contains("Middle"))
                            {
                                FRow.Cells[a].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                            }
                            else if (vertical_alignment.Contains("Top"))
                            {
                                FRow.Cells[a].CellFormat.VerticalAlignment = VerticalAlignment.Top;
                            }
                            else if (vertical_alignment.Contains("Bottom"))
                            {
                                FRow.Cells[a].CellFormat.VerticalAlignment = VerticalAlignment.Bottom;
                            }
                            if (text_alignment.Contains("Center"))
                            {
                                p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                            }
                            else if (text_alignment.Contains("Left"))
                            {
                                p.Format.HorizontalAlignment = HorizontalAlignment.Left;
                            }
                            else if (text_alignment.Contains("Right"))
                            {
                                p.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            }
                            TextRange TR = p.AppendText(Header[a]);
                            if (drCol["header_font"].ToString() != "")
                            {
                                TR.CharacterFormat.FontName = drCol["header_font"].ToString();
                            }
                            else
                            {
                                TR.CharacterFormat.FontName = "Tahoma";
                            }
                            if (drCol["header_fontsize"].ToString() != "")
                            {
                                try
                                {
                                    TR.CharacterFormat.FontSize = float.Parse(drCol["header_fontsize"].ToString());
                                }
                                catch
                                {
                                    TR.CharacterFormat.FontSize = 11;
                                }
                            }
                            else
                            {
                                TR.CharacterFormat.FontSize = 11;
                            }
                            if (drCol["header_fontcolor"].ToString() != "")
                            {
                                TR.CharacterFormat.TextColor = Color.FromName(drCol["header_fontcolor"].ToString());
                            }
                            else { TR.CharacterFormat.TextColor = Color.White; }
                            if (drCol["header_fontbold"].ToString() != "")
                            {
                                if (drCol["header_fontbold"].ToString().ToLower() == "true")
                                { TR.CharacterFormat.Bold = true; }
                                else { TR.CharacterFormat.Bold = false; }
                            }



                        }
                        //Data Row
                        float cell_width = 100;
                        for (int r = 0; r < dtData.Rows.Count; r++)
                        {

                            string[] dataCol = new string[dtProp.Rows.Count];

                            for (int b = 0; b < dtProp.Rows.Count; b++)
                            {
                                var drProp = dtProp.Rows[b];
                                dataCol.SetValue(dtData.Rows[r][b + 1].ToString(), b);
                                TableRow DataRow = table.Rows[r + 1];
                                
                                try { cell_width = float.Parse(drProp["col_width"].ToString()); } catch { cell_width = 100; }
                                if (drProp["row_height"].ToString() != "")
                                {
                                    try
                                    {
                                        DataRow.Height = float.Parse(drProp["row_height"].ToString());
                                    }
                                    catch
                                    {
                                        DataRow.Height = 20;
                                    }

                                }
                                else { DataRow.Height = 20; }
                                DataRow.Cells[b].Width = cell_width;
                                if (drProp["col_color"].ToString() != null)
                                {
                                    try
                                    {
                                        DataRow.RowFormat.BackColor = Color.FromName(drProp["col_color"].ToString());
                                    }
                                    catch { DataRow.RowFormat.BackColor = Color.Transparent; }
                                }
                                else
                                {
                                    DataRow.RowFormat.BackColor = Color.Transparent;
                                }

                                //Cell Alignment
                                
                                try { vertical_alignment = drProp["col_valign"].ToString(); }
                                catch { vertical_alignment = "Middle"; }
                                try { text_alignment = drProp["col_align"].ToString(); }
                                catch { text_alignment = "Center"; }
                                if (vertical_alignment.Contains("Middle"))
                                {
                                    DataRow.Cells[b].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                                }
                                else if (vertical_alignment.Contains("Top"))
                                {
                                    DataRow.Cells[b].CellFormat.VerticalAlignment = VerticalAlignment.Top;
                                }
                                else if (vertical_alignment.Contains("Bottom"))
                                {
                                    DataRow.Cells[b].CellFormat.VerticalAlignment = VerticalAlignment.Bottom;
                                }
                                //Fill Data in Rows
                                Paragraph p2 = DataRow.Cells[b].AddParagraph();
                                p2.Format.AfterAutoSpacing = false;
                                p2.Format.AfterSpacing = 0;
                                TextRange TR2 = p2.AppendText(dataCol[b].Replace("!comma",","));

                                //Format Cells
                                if (text_alignment.Contains("Left"))
                                {
                                    p2.Format.HorizontalAlignment = HorizontalAlignment.Left;
                                }
                                else if (text_alignment.Contains("Center"))
                                {
                                    p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                                }
                                else if (text_alignment.Contains("Right"))
                                {
                                    p2.Format.HorizontalAlignment = HorizontalAlignment.Right;
                                }
                                else { p2.Format.HorizontalAlignment = HorizontalAlignment.Center; }
                                if (drProp["col_font"].ToString() != "")
                                {
                                    TR2.CharacterFormat.FontName = "Tahoma";
                                }
                                else { TR2.CharacterFormat.FontName = "Tahoma"; }
                                if (drProp["col_fontsize"].ToString() != "")
                                {
                                    try
                                    {
                                        TR2.CharacterFormat.FontSize = float.Parse(drProp["col_fontsize"].ToString());
                                    }
                                    catch { TR2.CharacterFormat.FontSize = 10; }

                                }
                                else { TR2.CharacterFormat.FontSize = 10; }
                                if (drProp["col_fontcolor"].ToString() != null)
                                {
                                    try
                                    {
                                        TR2.CharacterFormat.TextColor = Color.FromName(drProp["col_fontcolor"].ToString());
                                    }
                                    catch { TR2.CharacterFormat.TextColor = Color.Black; }
                                }
                                else
                                {
                                    TR2.CharacterFormat.TextColor = Color.Black;
                                }
                                if (drProp["col_color"].ToString() != null)
                                {
                                    try
                                    {
                                        TR2.CharacterFormat.TextBackgroundColor = Color.FromName(drProp["col_color"].ToString());
                                    }
                                    catch { TR2.CharacterFormat.TextBackgroundColor = Color.Transparent; }
                                }
                                else
                                {
                                    TR2.CharacterFormat.TextBackgroundColor = Color.White;
                                }
                            }


                        }
                        body.ChildObjects.Remove(paragraph);
                        body.ChildObjects.Insert(index, table);
                    }
                }
                
                
            }
           
            success_fn = success_fn.Replace("out_", "out2_");
            //  success_fn = temp_processDirectory + "\\" + success_fn; 
            myDoc.SaveToFile(success_fn, FileFormat.Docx);
            xoutput_content = System.IO.File.ReadAllBytes(success_fn);
            System.IO.File.Copy(success_fn, outPutFile, true);

            System.IO.Directory.Delete(temp_processDirectory, true);
            if (delete_output == true) { System.IO.File.Delete(outPutFile); }
            result.Status = "Complete";
            result.Message = outPutFile;

            return xoutput_content;
            
        }
     
        public byte[] executeReplaceDocx(string request_no)
        {
            string result_xnode_id = "";
            ReplaceParameters  para = new ReplaceParameters();
            var zdb = new DbControllerBase();
            var pdf_content = new byte[0]; 
            string sql = "select * from z_replacedocx_log where replacedocx_reqno = '"+request_no+"'";
            var dt = zdb.ExecSql_DataTable(sql, zBPMDB);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                para.jsonTagString = dr["jsonTagString"].ToString();
                para.jsonTableProp = dr["jsonTableProp"].ToString();
                para.jsonTableData = dr["jsonTableData"].ToString();
                para.template_filepath = dr["template_filepath"].ToString();
                para.output_directory = dr["output_directory"].ToString();
                para.output_filepath = dr["output_filepath"].ToString();
                para.delete_output = System.Convert.ToBoolean( dr["delete_output"].ToString());
                var temparr = para.output_filepath.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string fn_docx = "";
                string fn_pdf = "";
                if (temparr.Length > 0)
                {
                    fn_docx = temparr[temparr.Length - 1];
                    fn_pdf = fn_docx.Replace(".docx", ".pdf");
                }
                var docx_content =   ReplaceData2(para.jsonTagString, para.jsonTableProp, para.jsonTableData, para.template_filepath, para.output_directory, para.output_filepath, para.delete_output);
                 pdf_content = convertDOCtoPDF(para.output_filepath, para.output_filepath.Replace(".docx", ".pdf"), false);
               
                sql = " update z_replacedocx_log set  status='Complete' where replacedocx_reqno ='" + request_no + "' ";
                zdb.ExecNonQuery(sql, zBPMDB);

            }
            return pdf_content; 
        }
        public byte[] convertDOCtoPDF(string xpath_doc_filename, string xpath_pdf_filename, bool delete_output)
        {
           
            object misValue = System.Reflection.Missing.Value;
            String PATH_APP_PDF = xpath_pdf_filename;
            var inputFile = xpath_doc_filename;

            var WORD = new Microsoft.Office.Interop.Word.Application();
            WORD.Application.Visible = false;
            Microsoft.Office.Interop.Word.Document doc = WORD.Documents.Open(inputFile);
            doc.Activate();
           
           
            doc.SaveAs(@PATH_APP_PDF, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF,misValue, misValue, misValue,
            misValue, misValue, misValue, misValue, misValue, misValue, misValue);

            doc.Close();
            WORD.Quit();


            ReleaseObject(doc);
            ReleaseObject(WORD);
            byte[] content = new byte[0];
            if (File.Exists(PATH_APP_PDF) == true)
            {
                content = File.ReadAllBytes(PATH_APP_PDF);
                if (delete_output == true)
                {
                    File.Delete(PATH_APP_PDF);
                }
            }
            return content;
        }

        public byte[] convertDOCtoPDFSpite(string xpath_doc_filename, string xpath_pdf_filename, bool delete_output)
        {

            String PATH_APP_PDF = xpath_pdf_filename;
            var inputFile = xpath_doc_filename;

            Document document = new Document();
            document.LoadFromFile(inputFile, Spire.Doc.FileFormat.Docx);
            document.SaveToFile(PATH_APP_PDF, Spire.Doc.FileFormat.PDF);

            byte[] content = new byte[0];
            if (File.Exists(PATH_APP_PDF) == true)
            {
                content = File.ReadAllBytes(PATH_APP_PDF);
                if (delete_output == true)
                {
                    File.Delete(PATH_APP_PDF);
                }
            }
            return content;
        }


        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                //TODO
            }
            finally
            {
                GC.Collect();
            }
        }
        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }
        public DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString); 
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName);
            }
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }

        public class TagData
        {
            //Required
            public bool table_tag { get; set; }
            public bool string_tag { get; set; }
            public bool image_tag { get; set; }
            public string tagname { get; set; }

            // For Replace Text
            public string tagvalue { get; set; }
           
            // For Replace Table
            public DataTable datatable { get; set; }
            public Color datatable_header_backcolor { get; set; }
            public string datatable_header_font_name { get; set; }
            public float datatable_header_font_size { get; set; }
            public Color datatable_header_font_color { get; set; }
            public bool datatable_header_font_bold { get; set; }
            public float[] datatable_header_colwidths { get; set; }
            public string[] datatable_header_textalign_LeftCenterRight { get; set; }
            public string[] datatable_header_valign_TopMiddleBottom { get; set; }
            public int   datatable_header_rowheight { get; set; }
            public int   datatable_row_height { get; set; }
            public Color datatable_row_color { get; set; }
            public string[] datatable_col_textalign_LeftCenterRight { get; set; }
            public string[] datatable_col_valign_TopMiddleBottom { get; set; }
            public string datatable_col_font_name { get; set; }
            public float datatable_col_font_size { get; set; }
            public Color datatable_col_font_color { get; set; }
            
            // For Replace Image
            public byte[] imagecontent { get; set; }

        }

        public class TagStringData
        {
            //Required
            public string table_tag { get; set; }
            public string string_tag { get; set; }
            public string image_tag { get; set; }
            public string tagname { get; set; }

            // For Replace Text
            public string tagvalue { get; set; }

          
        }
        public class TagTableColumns
        {
            public string col_name { get; set; }
            public string col_alignment { get; set; }
            public string col_valignment { get; set; }
        }

        public class TagTableDataValue
        {
            // For Replace Table
            public string tagvalue { get; set; }
        }
        public class TagTableConfig 
        { 
            public string datatable_header_backcolor { get; set; }
            public string datatable_header_font_name { get; set; }
            public string datatable_header_font_size { get; set; }
            public string datatable_header_font_color { get; set; }
            public string datatable_header_font_bold { get; set; }
            public string datatable_header_colwidths { get; set; }
            public string[] datatable_header_textalign_LeftCenterRight { get; set; }
            public string[] datatable_header_valign_TopMiddleBottom { get; set; }
            public int datatable_header_rowheight { get; set; }
            public int datatable_row_height { get; set; }
            public Color datatable_row_color { get; set; }
            public string[] datatable_col_textalign_LeftCenterRight { get; set; }
            public string[] datatable_col_valign_TopMiddleBottom { get; set; }
            public string datatable_col_font_name { get; set; }
            public float datatable_col_font_size { get; set; }
            public Color datatable_col_font_color { get; set; }

           
        }
        public class TagImageData
        {
            // For Replace Image
            public byte[] imagecontent { get; set; }
        }
    }
    public class ReplaceParameters
    {
        public string jsonTagString { get; set; }
        public string jsonTableProp { get; set; }
        public string jsonTableData { get; set; }
        public string template_filepath { get; set; }
        public string output_directory { get; set; }
        public string output_filepath { get; set; }
        public Boolean delete_output { get; set; }
    }
}
