using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using we = System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Runbow.TWS.Biz;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Entity.WMS.Shelves;
using Runbow.TWS.MessageContracts.WMS.Shelves;
using Runbow.TWS.Web.Areas.WMS.Models.ShelvesManagement;
using Runbow.TWS.Web.Common;
using MyFile = System.IO.File;
using UtilConstants = Runbow.TWS.Common.Constants;
using Runbow.TWS.Common;
using System.Text;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

namespace Runbow.TWS.Web.Common
{
    public class ExportDataToExcelHelper
    {
        /// <summary>
        ///  将DataSet写入到Excel文件中
        /// </summary>
        /// <param name="DataSet"></param>
        /// <param name="fname"></param>
        /// <returns></returns>

        public static void ExportDataSetToExcel(DataSet ds, string fname)
        {
            string[,] head1 = new string[4, ds.Tables[0].Columns.Count - 1];
            //string path = "D:\\";
            //path = System.Configuration.ConfigurationManager.AppSettings["Email"].ToString();
            AppLibrary.WriteExcel.XlsDocument doc = new AppLibrary.WriteExcel.XlsDocument();
            string Time = DateTime.Now.ToString("yyyy-MM-dd");
            //doc.FileName = fname+ ".xls";
            doc.FileName = HttpUtility.UrlEncode(fname, System.Text.Encoding.UTF8);
            string SheetName = string.Empty;

            //计算当前多少个SHEET
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                if (ds.Tables[i].Rows.Count > 50000)
                {
                    SheetName = ds.Tables[i].TableName;
                    AppLibrary.WriteExcel.Worksheet sheet2 = doc.Workbook.Worksheets.Add(SheetName);
                    AppLibrary.WriteExcel.Cells cells2 = sheet2.Cells;
                    AppLibrary.WriteExcel.XF center2 = doc.NewXF();//添加样式的  
                    center2.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
                    center2.Font.FontName = "宋体";//字体  
                    center2.TextDirection = AppLibrary.WriteExcel.TextDirections.LeftToRight;//文本位置 
                    
                    AppLibrary.WriteExcel.ColumnInfo colInfo2 = new AppLibrary.WriteExcel.ColumnInfo(doc, sheet2);
                    colInfo2.ColumnIndexStart = 0;  //开始列  
                    colInfo2.ColumnIndexEnd = (ushort)(ds.Tables[i].Columns.Count - 1);   //结束列
                    colInfo2.Width = 4000;   //列宽
                    colInfo2.Collapsed = true;

                    sheet2.AddColumnInfo(colInfo2);
                    //边框线的样式
                    center2.DiagonalLineStyle = new AppLibrary.WriteExcel.LineStyle();
                    center2.BottomLineStyle = 1;
                    center2.LeftLineStyle = 1;
                    center2.TopLineStyle = 1;
                    center2.RightLineStyle = 1;
                    center2.RightLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    center2.LeftLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    center2.TopLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    center2.BottomLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    sheet2.AddColumnInfo(colInfo2);
                    AppLibrary.WriteExcel.XF Head2 = doc.NewXF();//添加样式的  
                    Head2.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
                    Head2.Font.FontName = "宋体";//字体  

                    Head2.PatternColor = AppLibrary.WriteExcel.Colors.Default16;
                    Head2.Font.Bold = true;//加粗  
                    Head2.Pattern = 1;
                    Head2.Font.Height = 230;

                    //边框线的样式
                    Head2.DiagonalLineStyle = new AppLibrary.WriteExcel.LineStyle();
                    Head2.BottomLineStyle = 1;
                    Head2.LeftLineStyle = 1;
                    Head2.TopLineStyle = 1;
                    Head2.RightLineStyle = 1;
                    Head2.RightLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    Head2.LeftLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    Head2.TopLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    Head2.BottomLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    AppLibrary.WriteExcel.XF XFstyle2 = doc.NewXF();//添加样式的  

                    XFstyle2.Font.Bold = false;//加粗  
                    //边框线的样式
                    XFstyle2.DiagonalLineStyle = new AppLibrary.WriteExcel.LineStyle();

                    for (int cols = 0; cols < ds.Tables[i].Columns.Count; cols++)
                    {
                        cells2.Add(1, cols + 1, ds.Tables[i].Columns[cols].ToString(), Head2);
                    }
                    for (int rows = 0; rows < 50000; rows++)
                    {
                        for (int cols = 0; cols < ds.Tables[i].Columns.Count; cols++)//因为表一有一列城市是不需要的  所以采用这种写法
                        {
                            cells2.Add(rows + 2, cols + 1, ds.Tables[i].Rows[rows][cols].ToString(), center2);
                        }
                    }
                    AppLibrary.WriteExcel.Worksheet sheet3 = doc.Workbook.Worksheets.Add(SheetName+"-2");
                    AppLibrary.WriteExcel.Cells cells3 = sheet3.Cells;
                    AppLibrary.WriteExcel.XF center3 = doc.NewXF();//添加样式的  
                    center3.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
                    center3.Font.FontName = "宋体";//字体  
                    center3.TextDirection = AppLibrary.WriteExcel.TextDirections.LeftToRight;//文本位置

                    AppLibrary.WriteExcel.ColumnInfo colInfo3 = new AppLibrary.WriteExcel.ColumnInfo(doc, sheet3);
                    colInfo3.ColumnIndexStart = 0;  //开始列  
                    colInfo3.ColumnIndexEnd = (ushort)(ds.Tables[i].Columns.Count - 1);   //结束列
                    colInfo3.Width = 4000;   //列宽
                    colInfo3.Collapsed = true;

                    sheet3.AddColumnInfo(colInfo3);
                    //边框线的样式
                    center3.DiagonalLineStyle = new AppLibrary.WriteExcel.LineStyle();
                    center3.BottomLineStyle = 1;
                    center3.LeftLineStyle = 1;
                    center3.TopLineStyle = 1;
                    center3.RightLineStyle = 1;
                    center3.RightLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    center3.LeftLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    center3.TopLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    center3.BottomLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    sheet3.AddColumnInfo(colInfo3);
                    AppLibrary.WriteExcel.XF Head3 = doc.NewXF();//添加样式的  
                    Head3.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
                    Head3.Font.FontName = "宋体";//字体  

                    Head3.PatternColor = AppLibrary.WriteExcel.Colors.Default16;
                    Head3.Font.Bold = true;//加粗  
                    Head3.Pattern = 1;
                    Head3.Font.Height = 230;

                    //边框线的样式
                    Head3.DiagonalLineStyle = new AppLibrary.WriteExcel.LineStyle();
                    Head3.BottomLineStyle = 1;
                    Head3.LeftLineStyle = 1;
                    Head3.TopLineStyle = 1;
                    Head3.RightLineStyle = 1;
                    Head3.RightLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    Head3.LeftLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    Head3.TopLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    Head3.BottomLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    AppLibrary.WriteExcel.XF XFstyle3 = doc.NewXF();//添加样式的  

                    //XFstyle.TextDirection = AppLibrary.WriteExcel.TextDirections.Left;//文本位置  
                    XFstyle3.Font.Bold = false;//加粗  
                    //边框线的样式
                    XFstyle3.DiagonalLineStyle = new AppLibrary.WriteExcel.LineStyle();

                    for (int cols = 0; cols < ds.Tables[i].Columns.Count; cols++)
                    {
                        cells3.Add(1, cols + 1, ds.Tables[i].Columns[cols].ToString(), Head3);
                    }
                    for (int rows = 50000; rows < ds.Tables[i].Rows.Count; rows++)
                    {
                        for (int cols = 0; cols < ds.Tables[i].Columns.Count; cols++)//因为表一有一列城市是不需要的  所以采用这种写法
                        {
                            cells3.Add(rows - 50000 + 2, cols + 1, ds.Tables[i].Rows[rows][cols].ToString(), center3);
                        }
                    }
                }
                else
                {
                    SheetName = ds.Tables[i].TableName;
                    AppLibrary.WriteExcel.Worksheet sheet = doc.Workbook.Worksheets.Add(SheetName);
                    AppLibrary.WriteExcel.Cells cells = sheet.Cells;
                    AppLibrary.WriteExcel.XF center = doc.NewXF();//添加样式的  
                    center.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
                    center.Font.FontName = "宋体";//字体  
                    center.TextDirection = AppLibrary.WriteExcel.TextDirections.LeftToRight;//文本位置  
                                                                                            //center.Font.Bold = true;//加粗  
                    AppLibrary.WriteExcel.ColumnInfo colInfo = new AppLibrary.WriteExcel.ColumnInfo(doc, sheet);
                    colInfo.ColumnIndexStart = 0;  //开始列  
                    colInfo.ColumnIndexEnd = (ushort)(ds.Tables[i].Columns.Count - 1);   //结束列
                    colInfo.Width = 4000;   //列宽
                    colInfo.Collapsed = true;
                    //colInfo.Width = 150;

                    sheet.AddColumnInfo(colInfo);
                    //边框线的样式
                    center.DiagonalLineStyle = new AppLibrary.WriteExcel.LineStyle();
                    center.BottomLineStyle = 1;
                    center.LeftLineStyle = 1;
                    center.TopLineStyle = 1;
                    center.RightLineStyle = 1;
                    center.RightLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    center.LeftLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    center.TopLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    center.BottomLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    sheet.AddColumnInfo(colInfo);
                    AppLibrary.WriteExcel.XF Head = doc.NewXF();//添加样式的  
                    Head.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
                    Head.Font.FontName = "宋体";//字体  

                    Head.PatternColor = AppLibrary.WriteExcel.Colors.Default16;
                    //Head.TextDirection = AppLibrary.WriteExcel.TextDirections.Default;//文本位置  
                    Head.Font.Bold = true;//加粗  
                    Head.Pattern = 1;
                    Head.Font.Height = 230;

                    //边框线的样式
                    Head.DiagonalLineStyle = new AppLibrary.WriteExcel.LineStyle();
                    Head.BottomLineStyle = 1;
                    Head.LeftLineStyle = 1;
                    Head.TopLineStyle = 1;
                    Head.RightLineStyle = 1;
                    Head.RightLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    Head.LeftLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    Head.TopLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    Head.BottomLineColor = AppLibrary.WriteExcel.Colors.Grey;
                    AppLibrary.WriteExcel.XF XFstyle = doc.NewXF();//添加样式的  

                    //XFstyle.TextDirection = AppLibrary.WriteExcel.TextDirections.Left;//文本位置  
                    XFstyle.Font.Bold = false;//加粗  
                                              //边框线的样式
                    XFstyle.DiagonalLineStyle = new AppLibrary.WriteExcel.LineStyle();

                    for (int cols = 0; cols < ds.Tables[i].Columns.Count; cols++)
                    {
                        cells.Add(1, cols + 1, ds.Tables[i].Columns[cols].ToString(), Head);
                    }

                    //第一行表头
                    //第一行显示表的列名
                    //标题占四行
                    for (int rows = 0; rows < ds.Tables[i].Rows.Count; rows++)
                    {
                        for (int cols = 0; cols < ds.Tables[i].Columns.Count; cols++)//因为表一有一列城市是不需要的  所以采用这种写法
                        {
                            cells.Add(rows + 2, cols + 1, ds.Tables[i].Rows[rows][cols].ToString(), center);
                        }
                    }
                }
            }
            GC.Collect();
            doc.Send();
        }

        /// <summary>
        ///  将DataTable写入到Excel文件中
        /// </summary>
        /// <param name="DataTable"></param>
        /// <param name="fname"></param>
        /// <returns></returns>

        public static void ExportDataSetToExcel(DataTable td, string fname, string Library)
        {
            string[,] head1 = new string[4, td.Columns.Count - 1];
            //string path = "D:\\";
            // path = System.Configuration.ConfigurationManager.AppSettings["Email"].ToString();
            AppLibrary.WriteExcel.XlsDocument doc = new AppLibrary.WriteExcel.XlsDocument();
            string Time = DateTime.Now.ToString("yyyy-MM-dd");
            //doc.FileName = fname+ ".xls";
            doc.FileName = HttpUtility.UrlEncode(fname, System.Text.Encoding.UTF8);
            string SheetName = string.Empty;

            //计算当前多少个SHEET
           
                SheetName ="Sheet1";
                AppLibrary.WriteExcel.Worksheet sheet = doc.Workbook.Worksheets.Add(SheetName);
                AppLibrary.WriteExcel.Cells cells = sheet.Cells;
                AppLibrary.WriteExcel.XF center = doc.NewXF();//添加样式的  
                center.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
                center.Font.FontName = "宋体";//字体  
                center.TextDirection = AppLibrary.WriteExcel.TextDirections.LeftToRight;//文本位置  
                //center.Font.Bold = true;//加粗  
                AppLibrary.WriteExcel.ColumnInfo colInfo = new AppLibrary.WriteExcel.ColumnInfo(doc, sheet);
             
                colInfo.ColumnIndexStart = 0;  //开始列  
                colInfo.ColumnIndexEnd = 50;   //结束列
                colInfo.Width = 9000;   //列宽
                colInfo.Collapsed = true;
             
                //  colInfo.Width = 150;
                sheet.AddColumnInfo(colInfo);
                //边框线的样式
                center.DiagonalLineStyle = new AppLibrary.WriteExcel.LineStyle();
                center.BottomLineStyle = 1;
                center.LeftLineStyle = 1;
                center.TopLineStyle = 1;
                center.RightLineStyle = 1;
                center.RightLineColor = AppLibrary.WriteExcel.Colors.Grey;
                center.LeftLineColor = AppLibrary.WriteExcel.Colors.Grey;
                center.TopLineColor = AppLibrary.WriteExcel.Colors.Grey;
                center.BottomLineColor = AppLibrary.WriteExcel.Colors.Grey;
                sheet.AddColumnInfo(colInfo);
                AppLibrary.WriteExcel.XF Head = doc.NewXF();//添加样式的  
                Head.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
                Head.Font.FontName = "宋体";//字体  

                Head.PatternColor = AppLibrary.WriteExcel.Colors.Default16;
                //Head.TextDirection = AppLibrary.WriteExcel.TextDirections.Default;//文本位置  
                Head.Font.Bold = true;//加粗  
                Head.Pattern = 1;
                Head.Font.Height = 230;
           
                //边框线的样式
                Head.DiagonalLineStyle = new AppLibrary.WriteExcel.LineStyle();
                Head.BottomLineStyle = 1;
                Head.LeftLineStyle = 1;
                Head.TopLineStyle = 1;
                Head.RightLineStyle = 1;
                Head.RightLineColor = AppLibrary.WriteExcel.Colors.Grey;
                Head.LeftLineColor = AppLibrary.WriteExcel.Colors.Grey;
                Head.TopLineColor = AppLibrary.WriteExcel.Colors.Grey;
                Head.BottomLineColor = AppLibrary.WriteExcel.Colors.Grey;
                AppLibrary.WriteExcel.XF XFstyle = doc.NewXF();//添加样式的  

                //XFstyle.TextDirection = AppLibrary.WriteExcel.TextDirections.Left;//文本位置  
                XFstyle.Font.Bold = false;//加粗  
                //边框线的样式
                XFstyle.DiagonalLineStyle = new AppLibrary.WriteExcel.LineStyle();

                for (int cols = 0; cols < td.Columns.Count; cols++)
                {
                    cells.Add(1, cols + 1, td.Columns[cols].ToString(), Head);
                }
                //第一行表头
                //第一行显示表的列名
                //标题占四行

                for (int rows = 0; rows < td.Rows.Count; rows++)
                {
                    for (int cols = 0; cols <td.Columns.Count; cols++)//因为表一有一列城市是不需要的  所以采用这种写法
                    {
                        cells.Add(rows + 2, cols + 1, td.Rows[rows][cols].ToString(), center);
                    }
                }
            
            GC.Collect();
            doc.Send();

        }

        /// <summary>
        ///  将Datatable写入到Excel文件中
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fname"></param>
        /// <returns></returns>
        public static void ExportDataTableToExcel(DataTable dt, string FileName)
        {
        
            var sbHtml = new StringBuilder();
            sbHtml.Append("<style>td{mso-number-format:\"\\@\";}</style>");
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", dt.Columns[i].ColumnName);
            }

            sbHtml.Append("</tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sbHtml.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i][j].ToString());
                }
                sbHtml.Append("</tr>");
            }

            sbHtml.Append("</table>");
            HttpResponse Response;
            Response = we.HttpContext.Current.Response;
            Response.Charset = "UTF-8";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AppendHeader("content-disposition", "attachment;filename=" + FileName);
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sbHtml.ToString());
            Response.Flush();
            Response.End();
        }

        /// <summary>
        /// 百姓网订单报表导出(特殊样式)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filepath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string SaveDateSetExportNew(DataTable dt, string filepath, string filename)
        {
            AppLibrary.WriteExcel.XlsDocument doc = new AppLibrary.WriteExcel.XlsDocument();
            doc.FileName = filename;

            //DataTable dt = dataSet.Tables[0];
            string SheetName = dt.TableName;
            AppLibrary.WriteExcel.Worksheet sheet = doc.Workbook.Worksheets.Add(SheetName);
            AppLibrary.WriteExcel.Cells cells = sheet.Cells;

            //添加样式的  
            AppLibrary.WriteExcel.XF center = doc.NewXF();
            center.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
            center.VerticalAlignment = AppLibrary.WriteExcel.VerticalAlignments.Centered;
            center.Font.FontName = "宋体";//字体  
            center.TextDirection = AppLibrary.WriteExcel.TextDirections.ByContext;//文本位置  
            center.Font.Bold = false;//加粗  
            center.Font.Height = 300;
            center.Font.ColorIndex = 1;
            center.PatternColor = AppLibrary.WriteExcel.Colors.Red; //背景色(与上共用)
            //小头部
            AppLibrary.WriteExcel.XF SamllHeader3 = doc.NewXF();
            SamllHeader3.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
            SamllHeader3.VerticalAlignment = AppLibrary.WriteExcel.VerticalAlignments.Centered;
            SamllHeader3.Font.FontName = "宋体";//字体  
            SamllHeader3.TextDirection = AppLibrary.WriteExcel.TextDirections.ByContext;//文本位置  
            SamllHeader3.Font.Bold = false;//加粗  
            SamllHeader3.Font.Height = 260;
            SamllHeader3.Pattern = 1;  //背景色(与下共用)
            SamllHeader3.PatternColor = AppLibrary.WriteExcel.Colors.EgaGreen; //背景色(与上共用)


            //头部
            AppLibrary.WriteExcel.XF Header = doc.NewXF();
            Header.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
            Header.VerticalAlignment = AppLibrary.WriteExcel.VerticalAlignments.Centered;
            Header.Font.FontName = "宋体";//字体  
            Header.TextDirection = AppLibrary.WriteExcel.TextDirections.ByContext;//文本位置  
            Header.Font.Bold = false;//加粗  
            Header.Font.Height = 400;

            //设置列宽
            AppLibrary.WriteExcel.ColumnInfo colInfo = new AppLibrary.WriteExcel.ColumnInfo(doc, sheet);
            colInfo.ColumnIndexStart = 0;  //开始列  
            colInfo.ColumnIndexEnd = 32;   //结束列
            colInfo.Width = 5000;   //列宽
            colInfo.Collapsed = false;
            sheet.AddColumnInfo(colInfo);

            //小头部
            AppLibrary.WriteExcel.XF SamllHeader = doc.NewXF();
            SamllHeader.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
            SamllHeader.VerticalAlignment = AppLibrary.WriteExcel.VerticalAlignments.Centered;
            SamllHeader.Font.FontName = "宋体";//字体  
            SamllHeader.TextDirection = AppLibrary.WriteExcel.TextDirections.ByContext;//文本位置  
            SamllHeader.Font.Bold = false;//加粗  
            SamllHeader.Font.Height = 300;
            SamllHeader.Pattern = 1;  //背景色(与下共用)
            SamllHeader.PatternColor = AppLibrary.WriteExcel.Colors.Silver; //背景色(与上共用)

            //小头部
            AppLibrary.WriteExcel.XF SamllHeader2 = doc.NewXF();
            SamllHeader2.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
            SamllHeader2.VerticalAlignment = AppLibrary.WriteExcel.VerticalAlignments.Centered;
            SamllHeader2.Font.FontName = "宋体";//字体  
            SamllHeader2.TextDirection = AppLibrary.WriteExcel.TextDirections.ByContext;//文本位置  
            SamllHeader2.Font.Bold = false;//加粗  
            SamllHeader2.Font.Height = 300;
            SamllHeader2.Pattern = 1;  //背景色(与下共用)
            SamllHeader2.PatternColor = AppLibrary.WriteExcel.Colors.Olive; //背景色(与上共用)

            //订单样式
            AppLibrary.WriteExcel.XF DingDan = doc.NewXF();
            DingDan.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;
            DingDan.VerticalAlignment = AppLibrary.WriteExcel.VerticalAlignments.Centered;
            DingDan.Font.FontName = "宋体";//字体  
            DingDan.TextDirection = AppLibrary.WriteExcel.TextDirections.ByContext;//文本位置  
            DingDan.Font.Bold = false;//加粗  
            DingDan.Font.Height = 300;
            DingDan.Pattern = 1;
            DingDan.PatternColor = AppLibrary.WriteExcel.Colors.Yellow; //背景色(与上共用)

            //合并列
            sheet.Cells.Merge(1, 1, 1, 5);
            sheet.Cells.Merge(1, 1, 6, 16);
            cells.Add(1, 6, "百姓网订单信息", Header);
            sheet.Cells.Merge(1, 1, 17, 27);
            cells.Add(1, 17, "快递公司实际信息", Header);

            sheet.Cells.Merge(2, 2, 1, 5);
            cells.Add(2, 1, "订单信息", DingDan);
            sheet.Cells.Merge(2, 2, 6, 9);
            cells.Add(2, 6, "始发信息", SamllHeader2);
            sheet.Cells.Merge(2, 2, 10, 13);
            cells.Add(2, 10, "货物信息", SamllHeader2);
            sheet.Cells.Merge(2, 2, 14, 17);
            cells.Add(2, 14, "到货信息", SamllHeader2);

            sheet.Cells.Merge(2, 2, 18, 21);
            cells.Add(2, 18, "始发信息", SamllHeader);

            sheet.Cells.Merge(2, 2, 22, 25);
            cells.Add(2, 22, "货物信息", SamllHeader);

            sheet.Cells.Merge(2, 2, 26, 29);
            cells.Add(2, 26, "到货信息", SamllHeader);

            int colIndex = 0;
            //生成字段名称 
            foreach (DataColumn col in dt.Columns)
            {
                colIndex++;
                cells.Add(3, colIndex, col.ColumnName, SamllHeader3);

            }

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    cells.Add(r + 4, c + 1, dt.Rows[r][c].ToString());
                }
                //Application.DoEvents();
            }
            GC.Collect();
            doc.Send();
            //doc.Save(filepath, true);

            return filepath + "\\" + filename + ".xls";
        }

        /// <summary>
        /// NPOI导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filepath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static void ExportExcel(DataTable dt, string strFileName, string strSheetName)
        {
            XSSFWorkbook book = new XSSFWorkbook();
            ISheet sheet = book.CreateSheet(strSheetName);

            IRow headerrow = sheet.CreateRow(0);
            ICellStyle style = book.CreateCellStyle();
            style.Alignment = HorizontalAlignment.CENTER;
            style.VerticalAlignment = VerticalAlignment.CENTER;

            XSSFRow dataRow = (XSSFRow)sheet.CreateRow(0);
            string strColumns = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strColumns += dt.Columns[i].ColumnName + ",";
            }
            strColumns = strColumns.Substring(0, strColumns.Length - 1);
            string[] strArry = strColumns.Split(',');
            for (int i = 0; i < strArry.Length; i++)
            {
                dataRow.CreateCell(i).SetCellValue(strArry[i]);
                dataRow.GetCell(i).CellStyle = style;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataRow = (XSSFRow)sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string ValueType = "";
                    string Value = "";
                    if (dt.Rows[i][j].ToString() != null)
                    {
                        ValueType = dt.Rows[i][j].GetType().ToString();
                        Value = dt.Rows[i][j].ToString();
                    }
                    switch (ValueType)
                    {
                        case "System.String"://字符串类型
                            dataRow.CreateCell(j).SetCellValue(Value);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(Value, out dateV);
                            dataRow.CreateCell(j).SetCellValue(dateV);
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(Value, out boolV);
                            dataRow.CreateCell(j).SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(Value, out intV);
                            dataRow.CreateCell(j).SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(Value, out doubV);
                            dataRow.CreateCell(j).SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            dataRow.CreateCell(j).SetCellValue("");
                            break;
                        default:
                            dataRow.CreateCell(j).SetCellValue("");
                            break;
                    }
                    dataRow.GetCell(j).CellStyle = style;
                    //设置宽度
                    sheet.SetColumnWidth(j, (Value.Length + 10) * 256);
                }
            }
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", HttpUtility.UrlEncode(strFileName, Encoding.UTF8)));
            System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            System.Web.HttpContext.Current.Response.End();
            book = null;
            ms.Close();
            ms.Dispose();
        }

        /// <summary>
        /// NPOI导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filepath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static void ExportExcel(DataSet ds, string strFileName, string strSheetName, string strSheetName2)
        {
            XSSFWorkbook book = new XSSFWorkbook();
            for (int k = 0; k < ds.Tables.Count; k++)
            {
                #region 导出单sheet
                ISheet sheet;
                if (k == 0)
                {
                    sheet = book.CreateSheet(strSheetName);
                    IRow headerrow = sheet.CreateRow(0);
                    ICellStyle style = book.CreateCellStyle();//表头样式
                    style.Alignment = HorizontalAlignment.CENTER;//水平居中
                    style.VerticalAlignment = VerticalAlignment.CENTER;
                    style.BorderBottom = BorderStyle.THIN;//边框
                    style.BorderLeft = BorderStyle.THIN;
                    style.BorderRight = BorderStyle.THIN;
                    style.BorderTop = BorderStyle.THIN;

                    IFont font = book.CreateFont();
                    font.FontHeightInPoints = 10;
                    //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                    font.FontName = "宋体";
                    style.SetFont(font);//字体样式
                    //加范围边框
                    //AddRengionBorder(0, 8, 0, 7);

                    //设置一个合并单元格区域，使用上下左右定义CellRangeAddress区域
                    //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 6));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 5, 3, 3));
                    sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 2));
                    sheet.AddMergedRegion(new CellRangeAddress(4, 4, 4, 6));
                    sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 6));

                    XSSFRow dataRow = (XSSFRow)sheet.CreateRow(0);
                    string strColumns = "";
                    for (int i = 0; i < ds.Tables[k].Columns.Count; i++)
                    {
                        strColumns += ds.Tables[k].Columns[i].ColumnName + ",";
                    }
                    strColumns = strColumns.Substring(0, strColumns.Length - 1);
                    string[] strArry = strColumns.Split(',');
                    for (int i = 0; i < strArry.Length; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(strArry[i]);
                        dataRow.GetCell(i).CellStyle = style;
                    }
                    for (int i = 0; i < ds.Tables[k].Rows.Count; i++)
                    {
                        dataRow = (XSSFRow)sheet.CreateRow(i + 1);
                        for (int j = 0; j < ds.Tables[k].Columns.Count; j++)
                        {
                            string ValueType = "";
                            string Value = "";
                            if (ds.Tables[k].Rows[i][j].ToString() != null)
                            {
                                ValueType = ds.Tables[k].Rows[i][j].GetType().ToString();
                                Value = ds.Tables[k].Rows[i][j].ToString();
                            }
                            switch (ValueType)
                            {
                                case "System.String"://字符串类型
                                    dataRow.CreateCell(j).SetCellValue(Value);
                                    break;
                                case "System.DateTime"://日期类型
                                    DateTime dateV;
                                    DateTime.TryParse(Value, out dateV);
                                    dataRow.CreateCell(j).SetCellValue(dateV);
                                    break;
                                case "System.Boolean"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(Value, out boolV);
                                    dataRow.CreateCell(j).SetCellValue(boolV);
                                    break;
                                case "System.Int16"://整型
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    int intV = 0;
                                    int.TryParse(Value, out intV);
                                    dataRow.CreateCell(j).SetCellValue(intV);
                                    break;
                                case "System.Decimal"://浮点型
                                case "System.Double":
                                    double doubV = 0;
                                    double.TryParse(Value, out doubV);
                                    dataRow.CreateCell(j).SetCellValue(doubV);
                                    break;
                                case "System.DBNull"://空值处理
                                    dataRow.CreateCell(j).SetCellValue("");
                                    break;
                                default:
                                    dataRow.CreateCell(j).SetCellValue("");
                                    break;
                            }
                            dataRow.GetCell(j).CellStyle = style;
                            sheet.SetColumnWidth(j, (Value.Length + 10) * 256);//设置宽度
                        }
                    }
                    for (int i = 0; i <= ds.Tables[k].Rows.Count; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                }
                else
                {
                    sheet = book.CreateSheet(strSheetName2);
                    IRow headerrow = sheet.CreateRow(0);
                    ICellStyle style = book.CreateCellStyle();//表头样式
                    style.Alignment = HorizontalAlignment.CENTER;//水平居中
                    style.VerticalAlignment = VerticalAlignment.CENTER;
                    style.BorderBottom = BorderStyle.THIN;//边框
                    style.BorderLeft = BorderStyle.THIN;
                    style.BorderRight = BorderStyle.THIN;
                    style.BorderTop = BorderStyle.THIN;

                    IFont font = book.CreateFont();
                    font.FontHeightInPoints = 10;
                    //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                    font.FontName = "宋体";
                    style.SetFont(font);//字体样式

                    XSSFRow dataRow = (XSSFRow)sheet.CreateRow(0);
                    string strColumns = "";
                    for (int i = 0; i < ds.Tables[k].Columns.Count; i++)
                    {
                        strColumns += ds.Tables[k].Columns[i].ColumnName + ",";
                    }
                    strColumns = strColumns.Substring(0, strColumns.Length - 1);
                    string[] strArry = strColumns.Split(',');
                    for (int i = 0; i < strArry.Length; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(strArry[i]);
                        dataRow.GetCell(i).CellStyle = style;
                    }
                    for (int i = 0; i < ds.Tables[k].Rows.Count; i++)
                    {
                        dataRow = (XSSFRow)sheet.CreateRow(i + 1);
                        for (int j = 0; j < ds.Tables[k].Columns.Count; j++)
                        {
                            string ValueType = "";
                            string Value = "";
                            if (ds.Tables[k].Rows[i][j].ToString() != null)
                            {
                                ValueType = ds.Tables[k].Rows[i][j].GetType().ToString();
                                Value = ds.Tables[k].Rows[i][j].ToString();
                            }
                            switch (ValueType)
                            {
                                case "System.String"://字符串类型
                                    dataRow.CreateCell(j).SetCellValue(Value);
                                    break;
                                case "System.DateTime"://日期类型
                                    DateTime dateV;
                                    DateTime.TryParse(Value, out dateV);
                                    dataRow.CreateCell(j).SetCellValue(dateV);
                                    break;
                                case "System.Boolean"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(Value, out boolV);
                                    dataRow.CreateCell(j).SetCellValue(boolV);
                                    break;
                                case "System.Int16"://整型
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    int intV = 0;
                                    int.TryParse(Value, out intV);
                                    dataRow.CreateCell(j).SetCellValue(intV);
                                    break;
                                case "System.Decimal"://浮点型
                                case "System.Double":
                                    double doubV = 0;
                                    double.TryParse(Value, out doubV);
                                    dataRow.CreateCell(j).SetCellValue(doubV);
                                    break;
                                case "System.DBNull"://空值处理
                                    dataRow.CreateCell(j).SetCellValue("");
                                    break;
                                default:
                                    dataRow.CreateCell(j).SetCellValue("");
                                    break;
                            }
                            dataRow.GetCell(j).CellStyle = style;
                            sheet.SetColumnWidth(j, (Value.Length + 10) * 256);//设置宽度
                        }
                    }
                    for (int i = 0; i <= ds.Tables[k].Rows.Count; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                }
                #endregion
            }
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", HttpUtility.UrlEncode(strFileName, Encoding.UTF8)));
            System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            System.Web.HttpContext.Current.Response.End();
            book = null;
            ms.Close();
            ms.Dispose();
        }


        /// <summary>
        /// NPOI生成Excel到文件夹中        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filepath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static void ExportExcelToURL(DataSet ds,string filePath)
        {
            try
            {
                XSSFWorkbook book = new XSSFWorkbook();
                string SheetName = string.Empty;
                for (int k = 0; k < ds.Tables.Count; k++)
                {
                    #region 导出单sheet
                    ISheet sheet;
                    SheetName = ds.Tables[k].TableName;
                    //if (k == 0)
                    //{
                        sheet = book.CreateSheet(SheetName);
                        IRow headerrow = sheet.CreateRow(0);
                        ICellStyle style = book.CreateCellStyle();//表头样式
                        style.Alignment = HorizontalAlignment.CENTER;//水平居中
                        style.VerticalAlignment = VerticalAlignment.CENTER;
                        style.BorderBottom = BorderStyle.THIN;//边框
                        style.BorderLeft = BorderStyle.THIN;
                        style.BorderRight = BorderStyle.THIN;
                        style.BorderTop = BorderStyle.THIN;
                        style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.RED.index;
                        IFont font = book.CreateFont();
                        font.FontHeightInPoints = 10;

                        //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                        font.FontName = "宋体";
                        style.SetFont(font);//字体样式
                        headerrow.RowStyle = style;
                        //加范围边框
                        //AddRengionBorder(0, 8, 0, 7);

                        //设置一个合并单元格区域，使用上下左右定义CellRangeAddress区域
                        //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                        //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 6));
                        //sheet.AddMergedRegion(new CellRangeAddress(2, 5, 3, 3));
                        //sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 2));
                        //sheet.AddMergedRegion(new CellRangeAddress(4, 4, 4, 6));
                        //sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 6));

                        XSSFRow dataRow = (XSSFRow)sheet.CreateRow(0);
                        string strColumns = "";
                        for (int i = 0; i < ds.Tables[k].Columns.Count; i++)
                        {
                            strColumns += ds.Tables[k].Columns[i].ColumnName + ",";
                        }
                        strColumns = strColumns.Substring(0, strColumns.Length - 1);
                        string[] strArry = strColumns.Split(',');
                        for (int i = 0; i < strArry.Length; i++)
                        {
                            dataRow.CreateCell(i).SetCellValue(strArry[i]);
                            dataRow.GetCell(i).CellStyle = style;
                        }
                        dataRow = (XSSFRow)sheet.CreateRow(0);
                        dataRow.CreateCell(0).SetCellValue("你好");
                        dataRow.GetCell(0).CellStyle = style;
                    for (int i = 0; i < ds.Tables[k].Rows.Count; i++)
                        {
                            dataRow = (XSSFRow)sheet.CreateRow(i + 1);
                            for (int j = 0; j < ds.Tables[k].Columns.Count; j++)
                            {
                                string ValueType = "";
                                string Value = "";
                                if (ds.Tables[k].Rows[i][j].ToString() != null)
                                {
                                    ValueType = ds.Tables[k].Rows[i][j].GetType().ToString();
                                    Value = ds.Tables[k].Rows[i][j].ToString();
                                }
                                switch (ValueType)
                                {
                                    case "System.String"://字符串类型
                                        dataRow.CreateCell(j).SetCellValue(Value);
                                        break;
                                    case "System.DateTime"://日期类型
                                        DateTime dateV;
                                        DateTime.TryParse(Value, out dateV);
                                        dataRow.CreateCell(j).SetCellValue(dateV);
                                        break;
                                    case "System.Boolean"://布尔型
                                        bool boolV = false;
                                        bool.TryParse(Value, out boolV);
                                        dataRow.CreateCell(j).SetCellValue(boolV);
                                        break;
                                    case "System.Int16"://整型
                                    case "System.Int32":
                                    case "System.Int64":
                                    case "System.Byte":
                                        int intV = 0;
                                        int.TryParse(Value, out intV);
                                        dataRow.CreateCell(j).SetCellValue(intV);
                                        break;
                                    case "System.Decimal"://浮点型
                                    case "System.Double":
                                        double doubV = 0;
                                        double.TryParse(Value, out doubV);
                                        dataRow.CreateCell(j).SetCellValue(doubV);
                                        break;
                                    case "System.DBNull"://空值处理
                                        dataRow.CreateCell(j).SetCellValue("");
                                        break;
                                    default:
                                        dataRow.CreateCell(j).SetCellValue("");
                                        break;
                                }
                                dataRow.GetCell(j).CellStyle = style;
                                sheet.SetColumnWidth(j, (Value.Length + 30) * 256);//设置宽度
                            }
                        }
                        //for (int i = 0; i <= ds.Tables[k].Rows.Count; i++)
                        //{
                        //    sheet.AutoSizeColumn(i);
                        //}
                    //}
                    //else
                    //{
                    //    sheet = book.CreateSheet(SheetName);
                    //    IRow headerrow = sheet.CreateRow(0);
                    //    ICellStyle style = book.CreateCellStyle();//表头样式
                    //    style.Alignment = HorizontalAlignment.CENTER;//水平居中
                    //    style.VerticalAlignment = VerticalAlignment.CENTER;
                    //    style.BorderBottom = BorderStyle.THIN;//边框
                    //    style.BorderLeft = BorderStyle.THIN;
                    //    style.BorderRight = BorderStyle.THIN;
                    //    style.BorderTop = BorderStyle.THIN;
                    //    style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.RED.index;
                    //    IFont font = book.CreateFont();
                    //    font.FontHeightInPoints = 10;
                    //    //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                    //    font.FontName = "宋体";
                    //    style.SetFont(font);//字体样式

                    //    XSSFRow dataRow = (XSSFRow)sheet.CreateRow(0);
                    //    string strColumns = "";
                    //    for (int i = 0; i < ds.Tables[k].Columns.Count; i++)
                    //    {
                    //        strColumns += ds.Tables[k].Columns[i].ColumnName + ",";
                    //    }
                    //    strColumns = strColumns.Substring(0, strColumns.Length - 1);
                    //    string[] strArry = strColumns.Split(',');
                    //    for (int i = 0; i < strArry.Length; i++)
                    //    {
                    //        dataRow.CreateCell(i).SetCellValue(strArry[i]);
                    //        dataRow.GetCell(i).CellStyle = style;
                    //    }
                    //    for (int i = 0; i < ds.Tables[k].Rows.Count; i++)
                    //    {
                    //        dataRow = (XSSFRow)sheet.CreateRow(i + 1);
                    //        for (int j = 0; j < ds.Tables[k].Columns.Count; j++)
                    //        {
                    //            string ValueType = "";
                    //            string Value = "";
                    //            if (ds.Tables[k].Rows[i][j].ToString() != null)
                    //            {
                    //                ValueType = ds.Tables[k].Rows[i][j].GetType().ToString();
                    //                Value = ds.Tables[k].Rows[i][j].ToString();
                    //            }
                    //            switch (ValueType)
                    //            {
                    //                case "System.String"://字符串类型
                    //                    dataRow.CreateCell(j).SetCellValue(Value);
                    //                    break;
                    //                case "System.DateTime"://日期类型
                    //                    DateTime dateV;
                    //                    DateTime.TryParse(Value, out dateV);
                    //                    dataRow.CreateCell(j).SetCellValue(dateV);
                    //                    break;
                    //                case "System.Boolean"://布尔型
                    //                    bool boolV = false;
                    //                    bool.TryParse(Value, out boolV);
                    //                    dataRow.CreateCell(j).SetCellValue(boolV);
                    //                    break;
                    //                case "System.Int16"://整型
                    //                case "System.Int32":
                    //                case "System.Int64":
                    //                case "System.Byte":
                    //                    int intV = 0;
                    //                    int.TryParse(Value, out intV);
                    //                    dataRow.CreateCell(j).SetCellValue(intV);
                    //                    break;
                    //                case "System.Decimal"://浮点型
                    //                case "System.Double":
                    //                    double doubV = 0;
                    //                    double.TryParse(Value, out doubV);
                    //                    dataRow.CreateCell(j).SetCellValue(doubV);
                    //                    break;
                    //                case "System.DBNull"://空值处理
                    //                    dataRow.CreateCell(j).SetCellValue("");
                    //                    break;
                    //                default:
                    //                    dataRow.CreateCell(j).SetCellValue("");
                    //                    break;
                    //            }
                    //            dataRow.GetCell(j).CellStyle = style;
                    //            sheet.SetColumnWidth(j, (Value.Length + 10) * 256);//设置宽度
                    //        }
                    //    }
                    //    for (int i = 0; i <= ds.Tables[k].Rows.Count; i++)
                    //    {
                    //        sheet.AutoSizeColumn(i);
                    //    }
                    //}
                    #endregion
                }
                FileStream file = new FileStream(filePath, FileMode.Create);
                book.Write(file);
                book = null;
                file.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// NIke箱清单报表
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="filePath"></param>
        public static void ExportEpacklistExcelToURL(DataSet ds, string filePath)
        {
            try
            {
                XSSFWorkbook book = new XSSFWorkbook();
                string SheetName = string.Empty;
                for (int k = 0; k < ds.Tables.Count; k++)
                {
                    #region 导出单sheet
                    ISheet sheet;
                    SheetName = ds.Tables[k].TableName;
                    //if (k == 0)
                    //{
                    sheet = book.CreateSheet(SheetName);
                    IRow headerrow = sheet.CreateRow(0);
                    ICellStyle style = book.CreateCellStyle();//表头样式
                    style.Alignment = HorizontalAlignment.CENTER;//水平居中
                    style.VerticalAlignment = VerticalAlignment.CENTER;
                    style.BorderBottom = BorderStyle.THIN;//边框
                    style.BorderLeft = BorderStyle.THIN;
                    style.BorderRight = BorderStyle.THIN;
                    style.BorderTop = BorderStyle.THIN;
                    style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.RED.index;
                    IFont font = book.CreateFont();
                    font.FontHeightInPoints = 10;

                    //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                    font.FontName = "宋体";
                    style.SetFont(font);//字体样式
                    headerrow.RowStyle = style;
                    //加范围边框
                    //AddRengionBorder(0, 8, 0, 7);

                    //设置一个合并单元格区域，使用上下左右定义CellRangeAddress区域
                    //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                    //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 6));
                    //sheet.AddMergedRegion(new CellRangeAddress(2, 5, 3, 3));
                    //sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 2));
                    //sheet.AddMergedRegion(new CellRangeAddress(4, 4, 4, 6));
                    //sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 6));

                    XSSFRow dataRow0 = (XSSFRow)sheet.CreateRow(0);
                    dataRow0.CreateCell(0).SetCellValue(ds.Tables[k].Rows[0][0].ToString());
                    dataRow0.CreateCell(1).SetCellValue(ds.Tables[k].Rows[0][1].ToString());
                    dataRow0.CreateCell(2).SetCellValue(ds.Tables[k].Rows[0][2].ToString());
                    dataRow0.GetCell(0).CellStyle = style;
                    dataRow0.GetCell(1).CellStyle = style;
                    dataRow0.GetCell(2).CellStyle = style;

                    XSSFRow dataRow = (XSSFRow)sheet.CreateRow(1);
                    string strColumns = "";
                    for (int i = 0; i < ds.Tables[k].Columns.Count; i++)
                    {
                        strColumns += ds.Tables[k].Columns[i].ColumnName + ",";
                    }
                    strColumns = strColumns.Substring(0, strColumns.Length - 1);
                    string[] strArry = strColumns.Split(',');
                    for (int i = 0; i < strArry.Length; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(strArry[i]);
                        dataRow.GetCell(i).CellStyle = style;
                    }
                    


                    for (int i = 1; i < ds.Tables[k].Rows.Count; i++)
                    {
                        dataRow = (XSSFRow)sheet.CreateRow(i+1);
                        for (int j = 0; j < ds.Tables[k].Columns.Count; j++)
                        {
                            string ValueType = "";
                            string Value = "";
                            if (ds.Tables[k].Rows[i][j].ToString() != null)
                            {
                                ValueType = ds.Tables[k].Rows[i][j].GetType().ToString();
                                Value = ds.Tables[k].Rows[i][j].ToString();
                            }
                            switch (ValueType)
                            {
                                case "System.String"://字符串类型
                                    dataRow.CreateCell(j).SetCellValue(Value);
                                    break;
                                case "System.DateTime"://日期类型
                                    DateTime dateV;
                                    DateTime.TryParse(Value, out dateV);
                                    dataRow.CreateCell(j).SetCellValue(dateV);
                                    break;
                                case "System.Boolean"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(Value, out boolV);
                                    dataRow.CreateCell(j).SetCellValue(boolV);
                                    break;
                                case "System.Int16"://整型
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    int intV = 0;
                                    int.TryParse(Value, out intV);
                                    dataRow.CreateCell(j).SetCellValue(intV);
                                    break;
                                case "System.Decimal"://浮点型
                                case "System.Double":
                                    double doubV = 0;
                                    double.TryParse(Value, out doubV);
                                    dataRow.CreateCell(j).SetCellValue(doubV);
                                    break;
                                case "System.DBNull"://空值处理
                                    dataRow.CreateCell(j).SetCellValue("");
                                    break;
                                default:
                                    dataRow.CreateCell(j).SetCellValue("");
                                    break;
                            }
                            dataRow.GetCell(j).CellStyle = style;
                            sheet.SetColumnWidth(j, (Value.Length + 30) * 256);//设置宽度
                        }
                        
                    }
                   
                    #endregion
                }
                FileStream fs=null ;
                try
                {
                     //fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
                    //1、创建文件流（字节流）
                    using (fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        book.Write(fs);
                        book = null;
                      
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    //3、关闭流
                    fs.Close();
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Nike出货日报表
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="filePath"></param>
        public static void ExportDayOrderReportExcelToURL(DataSet ds, string filePath,string StoreKey)
        {
            try
            {
                XSSFWorkbook book = new XSSFWorkbook();
                string SheetName = string.Empty;
                for (int k = 0; k < ds.Tables.Count; k++)
                {
                    #region 导出单sheet
                    ISheet sheet;
                    SheetName = ds.Tables[k].TableName;
                    //if (k == 0)
                    //{
                    sheet = book.CreateSheet(SheetName);
                    IRow headerrow = sheet.CreateRow(0);
                    ICellStyle style = book.CreateCellStyle();
                    style.Alignment = HorizontalAlignment.CENTER;//水平居中
                    style.VerticalAlignment = VerticalAlignment.CENTER;
                    style.BorderBottom = BorderStyle.THIN;//边框
                    style.BorderLeft = BorderStyle.THIN;
                    style.BorderRight = BorderStyle.THIN;
                    style.BorderTop = BorderStyle.THIN;

                    IFont font = book.CreateFont();
                    font.FontHeightInPoints = 10;

                    //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                    font.FontName = "宋体";
                    style.SetFont(font);//字体样式



                    //样式二
                    ICellStyle stylek2 = book.CreateCellStyle();
                    stylek2.Alignment = HorizontalAlignment.CENTER;//水平居中
                    stylek2.VerticalAlignment = VerticalAlignment.CENTER;
                    stylek2.BorderBottom = BorderStyle.THIN;//边框
                    stylek2.BorderLeft = BorderStyle.THIN;
                    stylek2.BorderRight = BorderStyle.THIN;
                    stylek2.BorderTop = BorderStyle.THIN;

                    IFont fontk2 = book.CreateFont();
                    fontk2.FontHeightInPoints = 12;

                    //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                    fontk2.FontName = "黑体";
                    stylek2.SetFont(fontk2);//字体样式



                    headerrow.RowStyle = style;
                    //加范围边框
                    //AddRengionBorder(0, 8, 0, 7);

                    //设置一个合并单元格区域，使用上下左右定义CellRangeAddress区域
                    //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                    //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 6));
                    //sheet.AddMergedRegion(new CellRangeAddress(2, 5, 3, 3));
                    //sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 2));
                    //sheet.AddMergedRegion(new CellRangeAddress(4, 4, 4, 6));
                    //sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 6));
                    if (k == 0)
                    {
                        XSSFRow dataRow0 = (XSSFRow)sheet.CreateRow(0);
                        dataRow0.CreateCell(0).SetCellValue(StoreKey+"-CSC Daily Outbound Report");
                        ICellStyle style2 = book.CreateCellStyle();
                        style2.Alignment = HorizontalAlignment.CENTER;//水平居中
                        style2.BorderBottom = BorderStyle.THIN;//边框
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        IFont font2 = book.CreateFont();
                        font2.FontHeightInPoints = 20;

                        //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                        font2.FontName = "黑体";
                        dataRow0.GetCell(0).CellStyle = style2;
                        dataRow0.GetCell(0).CellStyle.SetFont(font2);
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 11));

                    }
                    else
                    {
                        XSSFRow dataRow0 = (XSSFRow)sheet.CreateRow(0);
                        XSSFRow dataRow3 = (XSSFRow)sheet.CreateRow(3);
                        dataRow0.CreateCell(0).SetCellValue(StoreKey+"-CSC Daily Outbound summary Report");
                        dataRow3.CreateCell(0).SetCellValue("合计");
                        dataRow3.CreateCell(1).SetCellValue("");
                        dataRow3.CreateCell(2).SetCellValue(ds.Tables[k].Rows.Count>0? ds.Tables[k].Rows[0][2].ToString():"");
                        ICellStyle style2 = book.CreateCellStyle();
                        style2.Alignment = HorizontalAlignment.CENTER;//水平居中
                        style2.BorderBottom = BorderStyle.THIN;//边框
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        IFont font2 = book.CreateFont();
                        font2.FontHeightInPoints = 14;

                        //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                        font2.FontName = "黑体";
                        dataRow0.GetCell(0).CellStyle = style2;
                        dataRow0.GetCell(0).CellStyle.SetFont(font2);
                        dataRow3.GetCell(0).CellStyle = style2;
                        dataRow3.GetCell(0).CellStyle.SetFont(font2);
                        dataRow3.GetCell(1).CellStyle = style2;
                        dataRow3.GetCell(1).CellStyle.SetFont(font2);
                        dataRow3.GetCell(2).CellStyle = style2;
                        dataRow3.GetCell(2).CellStyle.SetFont(font2);
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 2));
                        sheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 1));
                    }

                    XSSFRow dataRow = (XSSFRow)sheet.CreateRow(1);
                    string strColumns = "";
                    for (int i = 0; i < ds.Tables[k].Columns.Count; i++)
                    {
                        strColumns += ds.Tables[k].Columns[i].ColumnName + ",";
                    }
                    strColumns = strColumns.Substring(0, strColumns.Length - 1);
                    string[] strArry = strColumns.Split(',');
                    for (int i = 0; i < strArry.Length; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(strArry[i]);
                        if (k == 0)
                        {
                            dataRow.GetCell(i).CellStyle = style;
                        }
                        else
                        {
                            dataRow.GetCell(i).CellStyle = stylek2;
                        }
                        
                    }



                    for (int i = 0; i < ds.Tables[k].Rows.Count; i++)
                    {
                        dataRow = (XSSFRow)sheet.CreateRow(i + 2);
                        for (int j = 0; j < ds.Tables[k].Columns.Count; j++)
                        {
                            string ValueType = "";
                            string Value = "";
                            if (ds.Tables[k].Rows[i][j].ToString() != null)
                            {
                                ValueType = ds.Tables[k].Rows[i][j].GetType().ToString();
                                Value = ds.Tables[k].Rows[i][j].ToString();
                            }
                            switch (ValueType)
                            {
                                case "System.String"://字符串类型
                                    dataRow.CreateCell(j).SetCellValue(Value);
                                    break;
                                case "System.DateTime"://日期类型
                                    DateTime dateV;
                                    DateTime.TryParse(Value, out dateV);
                                    dataRow.CreateCell(j).SetCellValue(dateV);
                                    break;
                                case "System.Boolean"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(Value, out boolV);
                                    dataRow.CreateCell(j).SetCellValue(boolV);
                                    break;
                                case "System.Int16"://整型
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    int intV = 0;
                                    int.TryParse(Value, out intV);
                                    dataRow.CreateCell(j).SetCellValue(intV);
                                    break;
                                case "System.Decimal"://浮点型
                                case "System.Double":
                                    double doubV = 0;
                                    double.TryParse(Value, out doubV);
                                    dataRow.CreateCell(j).SetCellValue(doubV);
                                    break;
                                case "System.DBNull"://空值处理
                                    dataRow.CreateCell(j).SetCellValue("");
                                    break;
                                default:
                                    dataRow.CreateCell(j).SetCellValue("");
                                    break;
                            }
                            if (k == 0)
                            {
                                dataRow.GetCell(j).CellStyle = style;
                            }
                            else
                            {
                                dataRow.GetCell(j).CellStyle = stylek2;
                            }
                            if (k == 0)
                            {
                                sheet.SetColumnWidth(j, (Value.Length + 10) * 256);//设置宽度
                            }
                            else
                            {
                                sheet.SetColumnWidth(j, (Value.Length + 30) * 256);//设置宽度
                                
                            }
                        }

                    }

                    #endregion
                }
                FileStream file = new FileStream(filePath, FileMode.Create);
                book.Write(file);
                book = null;
                file.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// NIke每日库存报表
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="filePath"></param>
        public static void ExportDayInventoryExcelToURL(DataSet ds, string filePath)
        {
            try
            {
                XSSFWorkbook book = new XSSFWorkbook();
                string SheetName = string.Empty;
                for (int k = 0; k < ds.Tables.Count; k++)
                {
                    #region 导出单sheet
                    ISheet sheet;
                    SheetName = ds.Tables[k].TableName;
                    //if (k == 0)
                    //{
                    sheet = book.CreateSheet(SheetName);
                    IRow headerrow = sheet.CreateRow(0);
                    ICellStyle style = book.CreateCellStyle();//表头样式
                    style.Alignment = HorizontalAlignment.CENTER;//水平居中
                    style.VerticalAlignment = VerticalAlignment.CENTER;
                    style.BorderBottom = BorderStyle.THIN;//边框
                    style.BorderLeft = BorderStyle.THIN;
                    style.BorderRight = BorderStyle.THIN;
                    style.BorderTop = BorderStyle.THIN;
                    style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.RED.index;
                    IFont font = book.CreateFont();
                    font.FontHeightInPoints = 10;

                    //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                    font.FontName = "宋体";
                    style.SetFont(font);//字体样式
                    headerrow.RowStyle = style;
                    //加范围边框
                    //AddRengionBorder(0, 8, 0, 7);

                    //设置一个合并单元格区域，使用上下左右定义CellRangeAddress区域
                    //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                    //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 6));
                    //sheet.AddMergedRegion(new CellRangeAddress(2, 5, 3, 3));
                    //sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 2));
                    //sheet.AddMergedRegion(new CellRangeAddress(4, 4, 4, 6));
                    //sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 6));


                    ICellStyle style2 = book.CreateCellStyle();//表头样式
                    style2.Alignment = HorizontalAlignment.CENTER;//水平居中
                    style2.VerticalAlignment = VerticalAlignment.CENTER;
                    IFont font2 = book.CreateFont();
                    font2.FontHeightInPoints = 10;

                    font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                    font2.FontName = "Times New Roman";
                    style2.SetFont(font2);//字体样式

                    XSSFRow dataRow0 = (XSSFRow)sheet.CreateRow(0);

                    XSSFRow dataRow1 = (XSSFRow)sheet.CreateRow(1);
                    dataRow1.CreateCell(0).SetCellValue("To:");
                    dataRow1.CreateCell(3).SetCellValue("NIKE");
                    dataRow1.CreateCell(9).SetCellValue("Report : RP050/REV.01");
                    dataRow1.GetCell(0).CellStyle = style2;
                    dataRow1.GetCell(3).CellStyle = style2;
                    dataRow1.GetCell(9).CellStyle = style2;

                    XSSFRow dataRow2 = (XSSFRow)sheet.CreateRow(2);
                    dataRow2.CreateCell(0).SetCellValue("Supplier 供应商");
                    dataRow2.CreateCell(4).SetCellValue("上海虹迪物流科技有限公司");                   
                    dataRow2.GetCell(0).CellStyle = style2;
                    dataRow2.GetCell(4).CellStyle = style2;

                    XSSFRow dataRow3 = (XSSFRow)sheet.CreateRow(3);
                    dataRow3.CreateCell(9).SetCellValue("报告");
                    dataRow3.GetCell(9).CellStyle = style2;

                    XSSFRow dataRow4 = (XSSFRow)sheet.CreateRow(4);
                    dataRow4.CreateCell(0).SetCellValue("From:");
                    dataRow4.CreateCell(3).SetCellValue("WH8");
                    dataRow4.CreateCell(9).SetCellValue("Cutoff date:"+DateTime.Now.ToString("yyyy-MM-dd"));
                    dataRow4.GetCell(0).CellStyle = style2;
                    dataRow4.GetCell(3).CellStyle = style2;
                    dataRow4.GetCell(9).CellStyle = style2;

                    XSSFRow dataRow5 = (XSSFRow)sheet.CreateRow(5);
                    dataRow5.CreateCell(0).SetCellValue("RDC 分发中心");
                    dataRow5.CreateCell(9).SetCellValue("截止日期");
                    dataRow5.GetCell(0).CellStyle = style2;
                    dataRow5.GetCell(9).CellStyle = style2;

                    XSSFRow dataRow6 = (XSSFRow)sheet.CreateRow(6);
            
                    XSSFRow dataRow7= (XSSFRow)sheet.CreateRow(7);

                    XSSFRow dataRow8 = (XSSFRow)sheet.CreateRow(8);

                    XSSFRow dataRow9 = (XSSFRow)sheet.CreateRow(9);
                    dataRow9.CreateCell(0).SetCellValue("S/No.");
                    dataRow9.CreateCell(3).SetCellValue("Product No.");
                    dataRow9.CreateCell(4).SetCellValue("Product  Description");
                    dataRow9.CreateCell(5).SetCellValue("Free");
                    dataRow9.CreateCell(6).SetCellValue("Total");
                    dataRow9.CreateCell(9).SetCellValue("Received");
                    dataRow9.GetCell(0).CellStyle = style2;
                    dataRow9.GetCell(3).CellStyle = style2;
                    dataRow9.GetCell(4).CellStyle = style2;
                    dataRow9.GetCell(5).CellStyle = style2;
                    dataRow9.GetCell(6).CellStyle = style2;
                    dataRow9.GetCell(9).CellStyle = style2;

                    XSSFRow dataRow10 = (XSSFRow)sheet.CreateRow(10);
                    dataRow10.CreateCell(5).SetCellValue("Qty");
                    dataRow10.CreateCell(6).SetCellValue("Stock");
                    dataRow10.CreateCell(9).SetCellValue("Date");
                    dataRow10.GetCell(5).CellStyle = style2;
                    dataRow10.GetCell(6).CellStyle = style2;
                    dataRow10.GetCell(9).CellStyle = style2;

                    XSSFRow dataRow11 = (XSSFRow)sheet.CreateRow(11);

                    XSSFRow dataRow = (XSSFRow)sheet.CreateRow(12);
                    string strColumns = "";
                    for (int i = 0; i < ds.Tables[k].Columns.Count; i++)
                    {
                        strColumns += ds.Tables[k].Columns[i].ColumnName + ",";
                    }
                    strColumns = strColumns.Substring(0, strColumns.Length - 1);
                    string[] strArry = strColumns.Split(',');
                    for (int i = 0; i < strArry.Length; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(strArry[i]);
                        dataRow.GetCell(i).CellStyle = style;
                    }



                    for (int i = 0; i < ds.Tables[k].Rows.Count; i++)
                    {
                        dataRow = (XSSFRow)sheet.CreateRow(i + 13);
                        for (int j = 0; j < ds.Tables[k].Columns.Count; j++)
                        {
                            string ValueType = "";
                            string Value = "";
                            if (ds.Tables[k].Rows[i][j].ToString() != null)
                            {
                                ValueType = ds.Tables[k].Rows[i][j].GetType().ToString();
                                Value = ds.Tables[k].Rows[i][j].ToString();
                            }
                            switch (ValueType)
                            {
                                case "System.String"://字符串类型
                                    if (ds.Tables[k].Columns[j].ToString() == "库存数量" || ds.Tables[k].Columns[j].ToString() == "可用数量")
                                    {
                                        dataRow.CreateCell(j).SetCellValue(int.Parse(Value));
                                    }
                                    else
                                    {
                                        dataRow.CreateCell(j).SetCellValue(Value);
                                    }
                                    break;
                                case "System.DateTime"://日期类型
                                    DateTime dateV;
                                    DateTime.TryParse(Value, out dateV);
                                    dataRow.CreateCell(j).SetCellValue(dateV);
                                    break;
                                case "System.Boolean"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(Value, out boolV);
                                    dataRow.CreateCell(j).SetCellValue(boolV);
                                    break;
                                case "System.Int16"://整型
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    int intV = 0;
                                    int.TryParse(Value, out intV);
                                    dataRow.CreateCell(j).SetCellValue(intV);
                                    break;
                                case "System.Decimal"://浮点型
                                case "System.Double":
                                    double doubV = 0;
                                    double.TryParse(Value, out doubV);
                                    dataRow.CreateCell(j).SetCellValue(doubV);
                                    break;
                                case "System.DBNull"://空值处理
                                    dataRow.CreateCell(j).SetCellValue("");
                                    break;
                                default:
                                    dataRow.CreateCell(j).SetCellValue("");
                                    break;
                            }
                            dataRow.GetCell(j).CellStyle = style;
                            sheet.SetColumnWidth(j, (Value.Length + 10) * 256);//设置宽度
                        }

                    }

                    #endregion
                }
                FileStream file = new FileStream(filePath, FileMode.Create);
                book.Write(file);
                book = null;
                file.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// NIke每日收货及退货报表
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="filePath"></param>
        public static void ExportDayReceiptReportExcelToURL(DataSet ds, string filePath,string StoreKey)
        {
            try
            {
                XSSFWorkbook book = new XSSFWorkbook();
                string SheetName = string.Empty;
                for (int k = 0; k < ds.Tables.Count; k++)
                {
                    #region 导出单sheet
                    ISheet sheet;
                    SheetName = ds.Tables[k].TableName;
                    //if (k == 0)
                    //{
                    sheet = book.CreateSheet(SheetName);
                    IRow headerrow = sheet.CreateRow(0);
                    ICellStyle style = book.CreateCellStyle();
                    style.Alignment = HorizontalAlignment.CENTER;//水平居中
                    style.VerticalAlignment = VerticalAlignment.CENTER;
                    style.BorderBottom = BorderStyle.THIN;//边框
                    style.BorderLeft = BorderStyle.THIN;
                    style.BorderRight = BorderStyle.THIN;
                    style.BorderTop = BorderStyle.THIN;

                    IFont font = book.CreateFont();
                    font.FontHeightInPoints = 10;

                    //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                    font.FontName = "宋体";
                    style.SetFont(font);//字体样式



                    //样式二
                    ICellStyle stylek2 = book.CreateCellStyle();
                    stylek2.Alignment = HorizontalAlignment.CENTER;//水平居中
                    stylek2.VerticalAlignment = VerticalAlignment.CENTER;
                    stylek2.BorderBottom = BorderStyle.THIN;//边框
                    stylek2.BorderLeft = BorderStyle.THIN;
                    stylek2.BorderRight = BorderStyle.THIN;
                    stylek2.BorderTop = BorderStyle.THIN;

                    IFont fontk2 = book.CreateFont();
                    fontk2.FontHeightInPoints = 14;

                    //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                    fontk2.FontName = "黑体";
                    stylek2.SetFont(fontk2);//字体样式



                    headerrow.RowStyle = style;
                    //加范围边框
                    //AddRengionBorder(0, 8, 0, 7);

                    //设置一个合并单元格区域，使用上下左右定义CellRangeAddress区域
                    //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                    //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 6));
                    //sheet.AddMergedRegion(new CellRangeAddress(2, 5, 3, 3));
                    //sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 2));
                    //sheet.AddMergedRegion(new CellRangeAddress(4, 4, 4, 6));
                    //sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 6));
                    if (k == 0)
                    {
                        XSSFRow dataRow0 = (XSSFRow)sheet.CreateRow(0);
                        dataRow0.CreateCell(0).SetCellValue(StoreKey+"-CSC Receiving Report");
                        ICellStyle style2 = book.CreateCellStyle();
                        style2.Alignment = HorizontalAlignment.CENTER;//水平居中
                        style2.BorderBottom = BorderStyle.THIN;//边框
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.GREY_50_PERCENT.index;
                        style2.FillPattern = FillPatternType.SOLID_FOREGROUND;
                        IFont font2 = book.CreateFont();
                        font2.FontHeightInPoints = 24;

                        font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                        font2.FontName = "宋体";
                        dataRow0.GetCell(0).CellStyle = style2;
                        dataRow0.GetCell(0).CellStyle.SetFont(font2);
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 7));

                    }
                    else
                    {
                        XSSFRow dataRow2 = (XSSFRow)sheet.CreateRow(2);
                        dataRow2.CreateCell(0).SetCellValue("合计");
                        dataRow2.CreateCell(1).SetCellValue("");
                        dataRow2.CreateCell(2).SetCellValue(ds.Tables[k].Rows.Count > 0 ? ds.Tables[k].Rows[0][2].ToString() : "");
                        dataRow2.CreateCell(3).SetCellValue("");
                        ICellStyle style2 = book.CreateCellStyle();
                        style2.Alignment = HorizontalAlignment.CENTER;//水平居中
                        style2.BorderBottom = BorderStyle.THIN;//边框
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        IFont font2 = book.CreateFont();
                        font2.FontHeightInPoints = 14;

                        //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                        font2.FontName = "黑体";
                        dataRow2.GetCell(0).CellStyle = style2;
                        dataRow2.GetCell(0).CellStyle.SetFont(font2);
                        dataRow2.GetCell(1).CellStyle = style2;
                        dataRow2.GetCell(1).CellStyle.SetFont(font2);
                        dataRow2.GetCell(2).CellStyle = style2;
                        dataRow2.GetCell(2).CellStyle.SetFont(font2);
                        dataRow2.GetCell(3).CellStyle = style2;
                        dataRow2.GetCell(3).CellStyle.SetFont(font2);
                        sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 1));
                    }

                    XSSFRow dataRow = null;
                    if (k == 0)
                    {
                        dataRow=(XSSFRow)sheet.CreateRow(1);
                    }
                    else
                    {
                        dataRow = (XSSFRow)sheet.CreateRow(0);
                    }
                    string strColumns = "";
                    for (int i = 0; i < ds.Tables[k].Columns.Count; i++)
                    {
                        strColumns += ds.Tables[k].Columns[i].ColumnName + ",";
                    }
                    strColumns = strColumns.Substring(0, strColumns.Length - 1);
                    string[] strArry = strColumns.Split(',');
                    for (int i = 0; i < strArry.Length; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(strArry[i]);
                        if (k == 0)
                        {
                            ICellStyle style3 = book.CreateCellStyle();
                            style3.Alignment = HorizontalAlignment.CENTER;//水平居中
                            style3.BorderBottom = BorderStyle.THIN;//边框
                            style3.BorderLeft = BorderStyle.THIN;
                            style3.BorderRight = BorderStyle.THIN;
                            style3.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.YELLOW.index;
                            style3.FillPattern = FillPatternType.SOLID_FOREGROUND;
                            IFont font3 = book.CreateFont();
                            font3.FontHeightInPoints = 10;

                            //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                            font3.FontName = "宋体";
                            dataRow.GetCell(i).CellStyle = style3;
                        }
                        else
                        {
                            dataRow.GetCell(i).CellStyle = stylek2;
                        }

                    }



                    for (int i = 0; i < ds.Tables[k].Rows.Count; i++)
                    {
                        if (k == 0)
                        {
                            dataRow = (XSSFRow)sheet.CreateRow(i + 2);
                        }
                        else
                        {
                            dataRow = (XSSFRow)sheet.CreateRow(i + 1);
                        }
                        for (int j = 0; j < ds.Tables[k].Columns.Count; j++)
                        {
                            string ValueType = "";
                            string Value = "";
                            if (ds.Tables[k].Rows[i][j].ToString() != null)
                            {
                                ValueType = ds.Tables[k].Rows[i][j].GetType().ToString();
                                Value = ds.Tables[k].Rows[i][j].ToString();
                            }
                            switch (ValueType)
                            {
                                case "System.String"://字符串类型
                                    dataRow.CreateCell(j).SetCellValue(Value);
                                    break;
                                case "System.DateTime"://日期类型
                                    DateTime dateV;
                                    DateTime.TryParse(Value, out dateV);
                                    dataRow.CreateCell(j).SetCellValue(dateV);
                                    break;
                                case "System.Boolean"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(Value, out boolV);
                                    dataRow.CreateCell(j).SetCellValue(boolV);
                                    break;
                                case "System.Int16"://整型
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    int intV = 0;
                                    int.TryParse(Value, out intV);
                                    dataRow.CreateCell(j).SetCellValue(intV);
                                    break;
                                case "System.Decimal"://浮点型
                                case "System.Double":
                                    double doubV = 0;
                                    double.TryParse(Value, out doubV);
                                    dataRow.CreateCell(j).SetCellValue(doubV);
                                    break;
                                case "System.DBNull"://空值处理
                                    dataRow.CreateCell(j).SetCellValue("");
                                    break;
                                default:
                                    dataRow.CreateCell(j).SetCellValue("");
                                    break;
                            }
                            if (k == 0)
                            {
                                dataRow.GetCell(j).CellStyle = style;
                            }
                            else
                            {
                                dataRow.GetCell(j).CellStyle = stylek2;
                            }
                            if (k == 0)
                            {
                                sheet.SetColumnWidth(j, (Value.Length + 10) * 256);//设置宽度
                            }
                            else
                            {
                                sheet.SetColumnWidth(j, (Value.Length + 30) * 256);//设置宽度

                            }
                        }

                    }

                    #endregion
                }
                FileStream file = new FileStream(filePath, FileMode.Create);
                book.Write(file);
                book = null;
                file.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 加范围边框
        /// </summary>
        /// <param name="firstRow">起始行</param>
        /// <param name="lastRow">结束行</param>
        /// <param name="firstCell">起始列</param>
        /// <param name="lastCell">结束列</param>
        /// <returns></returns>
        //public void AddRengionBorder(int firstRow, int lastRow, int firstCell, int lastCell)
        //{
        //    //HSSFCellStyle Style = (HSSFCellStyle)workbook2.CreateCellStyle();
        //    for (int i = firstRow; i < lastRow; i++)
        //    {
        //        for (int n = firstCell; n < lastCell; n++)
        //        {
        //            ICell cell;
        //            cell = sheet1.GetRow(i).GetCell(n);
        //            if (cell == null)
        //            {
        //                cell = sheet1.GetRow(i).CreateCell(n);
        //                cell.SetCellValue(" ");
        //            }
        //            HSSFCellStyle Style = workbook2.CreateCellStyle() as HSSFCellStyle;
        //            ////为首行加上方边框
        //            if (i == firstRow)
        //            {
        //                Style.BorderTop = ss.UserModel.BorderStyle.THIN;
        //            }
        //            //为末行加下方边框
        //            if (i == lastRow - 1)
        //            {
        //                Style.BorderBottom = ss.UserModel.BorderStyle.THIN;
        //            }
        //            //为首列加左边框
        //            if (n == firstCell)
        //            {
        //                Style.BorderLeft = ss.UserModel.BorderStyle.THIN;
        //            }
        //            //为末列加右边框
        //            if (n == lastCell - 1)
        //            {
        //                Style.BorderRight = ss.UserModel.BorderStyle.THIN;
        //            }
        //            cell.CellStyle = Style;
        //        }
        //    }
        //}

    }
}