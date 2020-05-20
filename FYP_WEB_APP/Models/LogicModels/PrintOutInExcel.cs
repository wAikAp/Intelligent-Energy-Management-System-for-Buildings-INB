using System;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Drawing;
using OfficeOpenXml.Style;
using System.IO;
using FYP_APP.Controllers;
using FYP_WEB_APP.Models.LogicModels;
using System.Collections.Generic;
//using System.Data.SQLite;

namespace FYP_APP.Models.LogicModels
{
    class PrintOutInExcel
    {

        public static string Run()
        {

            /*using (StreamWriter sw = fi1.CreateText()) //testing to output a text file
            {
                sw.WriteLine("Hello");
                sw.WriteLine("And");
                sw.WriteLine("Welcome");
            }*/
            DevicesPowerUseOutputUtil poweruseUil = new DevicesPowerUseOutputUtil();
            int count = poweruseUil.getCountOfDifferentRooms();
            List<DailyUsageModel> dailyUsages = poweruseUil.Dailyusage();
            List<String> roomList = poweruseUil.getRoomList();
            String alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";


            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            FileOutputUtil.OutputDir = new DirectoryInfo(@"c:\TestingDirForEx");        //File dicectory which you want to store in
            var fi1 = FileOutputUtil.GetFileInfo("Testing.xlsx");          //File Name and type
            using (var xlPackage = new ExcelPackage(fi1))               //Fill data
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Testing"); //Add worksheets
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);


                worksheet.Cells["A1"].Value = "INB";    //Create Headers and format them 
                using (ExcelRange r = worksheet.Cells["A1:G1"])
                {
                    r.Merge = true;
                    r.Style.Font.SetFromFont(new Font("Britannic Bold", 22, FontStyle.Italic));
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                }
                worksheet.Cells["A2"].Value = "Power Consumption Report";
                using (ExcelRange r = worksheet.Cells["A2:G2"])
                {
                    r.Merge = true;
                    r.Style.Font.SetFromFont(new Font("Britannic Bold", 18, FontStyle.Italic));
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                }
               

                worksheet.Cells["A4"].Value = "Rooms";
                worksheet.Cells["A4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                worksheet.Cells["A4"].Style.Font.Bold = true;

                var startCellPoint = "B5";
                String EndCellPoint ="B5";
                for (int i = 0; i < count; i++)
                {
                    worksheet.Cells[alphabet[(1 + i)] + "4"].Value = roomList[i];
                    worksheet.Cells[alphabet[(1 + i)] + "4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[alphabet[(1 + i)] + "4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[alphabet[(1 + i)] + "4"].Style.Font.Bold = true;
                    worksheet.Cells[alphabet[(1 + i)] + "4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    EndCellPoint = alphabet[(1 + i)] + "5";

                    int y = 0;
                    foreach(DailyUsageModel usageRecord in dailyUsages)
                    {
                        System.Diagnostics.Debug.WriteLine("usageRecord.roomId == roomList[i]" + usageRecord.roomId + roomList[i]);
                        if (usageRecord.roomId == roomList[i])
                        {
                            
                            String cellpoint = alphabet[(1 + i)] +""+ (6 + y);
                            System.Diagnostics.Debug.WriteLine("new day" + cellpoint);
                            worksheet.Cells[cellpoint].Value = usageRecord.recorded_date + " " + usageRecord.power_used + "kWh ";
                            worksheet.Cells[cellpoint].AutoFitColumns();
                            y++;
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("not new day");
                        }
                        
                    }

                }

                worksheet.Cells["B5"].Value = "Daily Usage (kWh)";
                worksheet.Cells["A5"].Value = "Date:";
                using (ExcelRange r = worksheet.Cells[startCellPoint+":"+ EndCellPoint])
                {
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                }
                //System.Diagnostics.Debug.WriteLine("list count: " + dailyUsages.Count);
              


                // save the new spreadsheet
                xlPackage.Save();
            }
            return fi1.FullName;
        }
    }
}
