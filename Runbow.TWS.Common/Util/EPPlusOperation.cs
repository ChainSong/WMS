using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Runbow.TWS.Common
{
    public class EPPlusOperation
    {
        /// <summary>
        /// Excel导入
        /// </summary>
        /// <param name="hpf"></param>
        /// <returns></returns>
        public static DataSet ReadExcelToDataSet(HttpPostedFileBase hpf)
        {
            try
            {
                string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH;
                string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, Runbow.TWS.Common.Constants.TEMPFOLDER);
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                string fileName = "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(hpf.FileName);
                string fullPath = Path.Combine(targetPath, fileName);
                hpf.SaveAs(fullPath);
                hpf.InputStream.Close();
                string filePath = fullPath;
                DataSet ds = new DataSet();
                DataRow dr;
                object objCellValue;
                string cellValue;
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {

                    ////EPPlus 5.0 以后的版本需要指定 商业证书 或者非商业证书
                    //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        package.Load(fs);
                        foreach (var sheet in package.Workbook.Worksheets)
                        {
                            //if (sheet.Name.ToUpper().Trim() == sheetName.ToUpper().Trim())
                            //{
                            #region
                            if (sheet.Dimension == null) continue;
                            var columnCount = sheet.Dimension.End.Column;
                            var rowCount = sheet.Dimension.End.Row;
                            if (rowCount > 0)
                            {
                                DataTable dt = new DataTable(sheet.Name);
                                for (int j = 0; j < columnCount; j++)//设置DataTable列名
                                {
                                    objCellValue = sheet.Cells[1, j + 1].Value;
                                    cellValue = objCellValue == null ? "" : objCellValue.ToString();
                                    dt.Columns.Add(cellValue, typeof(string));
                                }
                                for (int i = 2; i <= rowCount; i++)
                                {
                                    dr = dt.NewRow();
                                    for (int j = 1; j <= columnCount; j++)
                                    {
                                        objCellValue = sheet.Cells[i, j].Value;
                                        if (objCellValue != null)
                                        {
                                            if (sheet.Cells[i, j].Style.Numberformat.Format.IndexOf("yyyy") > -1 && sheet.Cells[i, j].Value.GetType().ToString() == "System.Double")//注意这里，是处理日期时间格式的关键代码 
                                                objCellValue = sheet.Cells[i, j].GetValue<DateTime>();
                                        }
                                        cellValue = objCellValue == null ? "" : objCellValue.ToString();
                                        dr[j - 1] = cellValue;
                                    }
                                    dt.Rows.Add(dr);
                                }
                                ds.Tables.Add(dt);
                            }
                            #endregion
                            //}
                        }
                    }
                }
                return ds;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        /// <summary>
        /// 使用EPPlus导出Excel(xlsx)
        /// </summary>
        /// <param name="sourceTable">数据源</param>
        /// <param name="strFileName">xlsx文件名(不含后缀名)</param>
        public static void ExportByEPPlus(DataTable sourceTable, string strFileName)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                string sheetName = string.IsNullOrEmpty(sourceTable.TableName) ? "Sheet1" : sourceTable.TableName;
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(sourceTable, true);

                //Format the row
                ExcelBorderStyle borderStyle = ExcelBorderStyle.Thin;
                Color borderColor = Color.FromArgb(155, 155, 155);

                using (ExcelRange rng = ws.Cells[1, 1, sourceTable.Rows.Count + 1, sourceTable.Columns.Count])
                {
                    rng.Style.Font.Name = "宋体";
                    rng.Style.Font.Size = 10;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));

                    rng.Style.Border.Top.Style = borderStyle;
                    rng.Style.Border.Top.Color.SetColor(borderColor);

                    rng.Style.Border.Bottom.Style = borderStyle;
                    rng.Style.Border.Bottom.Color.SetColor(borderColor);

                    rng.Style.Border.Right.Style = borderStyle;
                    rng.Style.Border.Right.Color.SetColor(borderColor);
                }

                //Format the header row
                using (ExcelRange rng = ws.Cells[1, 1, 1, sourceTable.Columns.Count])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(234, 241, 246));  //Set color to dark blue
                    rng.Style.Font.Color.SetColor(Color.FromArgb(51, 51, 51));
                }

                //Write it back to the client
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}.xlsx", HttpUtility.UrlEncode(strFileName, Encoding.UTF8)));
                HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;

                HttpContext.Current.Response.BinaryWrite(pck.GetAsByteArray());
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 使用EPPlus导出Excel(xlsx)
        /// </summary>
        /// <param name="sourceTable">数据源</param>
        /// <param name="strFileName">xlsx文件名(不含后缀名)</param>
        public static void ExportDataSetByEPPlus(DataSet sourceSet, string strFileName)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                for (int i = 0; i < sourceSet.Tables.Count; i++)
                {
                    string sheetName = string.IsNullOrEmpty(sourceSet.Tables[i].TableName) ? "Sheet1" : sourceSet.Tables[i].TableName;
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);

                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    ws.Cells["A1"].LoadFromDataTable(sourceSet.Tables[i], true);

                    //Format the row
                    ExcelBorderStyle borderStyle = ExcelBorderStyle.Thin;
                    Color borderColor = Color.FromArgb(155, 155, 155);

                    using (ExcelRange rng = ws.Cells[1, 1, sourceSet.Tables[i].Rows.Count + 1, sourceSet.Tables[i].Columns.Count])
                    {
                        rng.Style.Font.Name = "宋体";
                        rng.Style.Font.Size = 10;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                        rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));

                        rng.Style.Border.Top.Style = borderStyle;
                        rng.Style.Border.Top.Color.SetColor(borderColor);

                        rng.Style.Border.Bottom.Style = borderStyle;
                        rng.Style.Border.Bottom.Color.SetColor(borderColor);

                        rng.Style.Border.Right.Style = borderStyle;
                        rng.Style.Border.Right.Color.SetColor(borderColor);
                    }

                    //Format the header row
                    using (ExcelRange rng = ws.Cells[1, 1, 1, sourceSet.Tables[i].Columns.Count])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(234, 241, 246));  //Set color to dark blue
                        rng.Style.Font.Color.SetColor(Color.FromArgb(51, 51, 51));
                    }
                }

                //Write it back to the client
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}.xlsx", HttpUtility.UrlEncode(strFileName, Encoding.UTF8)));
                HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;

                HttpContext.Current.Response.BinaryWrite(pck.GetAsByteArray());
                HttpContext.Current.Response.End();
            }
        }
    }
}
