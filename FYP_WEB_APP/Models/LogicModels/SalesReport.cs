/*************************************************************************************************
  Required Notice: Copyright (C) EPPlus Software AB. 
  This software is licensed under PolyForm Noncommercial License 1.0.0 
  and may only be used for noncommercial purposes 
  https://polyformproject.org/licenses/noncommercial/1.0.0/

  A commercial license to use this software can be purchased at https://epplussoftware.com
 *************************************************************************************************
  Date               Author                       Change
 *************************************************************************************************
  01/27/2020         EPPlus Software AB           Initial release EPPlus 5
 *************************************************************************************************/
using System;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Drawing;
using OfficeOpenXml.Style;
using System.IO;
//using System.Data.SQLite;

namespace FYP_APP.Models.LogicModels
{
    class SalesReportFromDatabase
    {
        /// <summary>
        /// Sample 3 - Creates a workbook and populates using data from a SQLite database
        /// </summary>
        /// <param name="outputDir">The output directory</param>
        /// <param name="templateDir">The location of the sample template</param>
        /// <param name="connectionString">The connection string to the SQLite database</param>
        public static string Run(string connectionString)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            FileOutputUtil.OutputDir = new DirectoryInfo(@"c:\TestingDirForEx");
            var fi1 = FileOutputUtil.GetFileInfo("Testing.xlsx");

            /*using (StreamWriter sw = fi1.CreateText())
            {
                sw.WriteLine("Hello");
                sw.WriteLine("And");
                sw.WriteLine("Welcome");
            }*/
            using (var xlPackage = new ExcelPackage(fi1))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Testing");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 5;
                int row = startRow;

                //Create Headers and format them 
                worksheet.Cells["A1"].Value = "Fiction Inc.";
                using (ExcelRange r = worksheet.Cells["A1:G1"])
                {
                    r.Merge = true;
                    r.Style.Font.SetFromFont(new Font("Britannic Bold", 22, FontStyle.Italic));
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                }
                worksheet.Cells["A2"].Value = "Sales Report";
                using (ExcelRange r = worksheet.Cells["A2:G2"])
                {
                    r.Merge = true;
                    r.Style.Font.SetFromFont(new Font("Britannic Bold", 18, FontStyle.Italic));
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                }

                worksheet.Cells["A4"].Value = "Company";
                worksheet.Cells["B4"].Value = "Sales Person";
                worksheet.Cells["C4"].Value = "Country";
                worksheet.Cells["D4"].Value = "Order Id";
                worksheet.Cells["E4"].Value = "OrderDate";
                worksheet.Cells["F4"].Value = "Order Value";
                worksheet.Cells["G4"].Value = "Currency";
                worksheet.Cells["A4:G4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A4:G4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                worksheet.Cells["A4:G4"].Style.Font.Bold = true;

                // save the new spreadsheet
                xlPackage.Save();
            }
            return fi1.FullName;
        }
    }
}
