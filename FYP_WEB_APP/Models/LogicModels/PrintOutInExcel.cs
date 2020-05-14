using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models.LogicModels
{
	public class PrintOutInExcel
	{

		string temp;
		string himidty;

		public PrintOutInExcel()
		{
            using (var package = new ExcelPackage(new FileInfo("Book.xlsx")))
            {

                var firstSheet = package.Workbook.Worksheets["First Sheet"];
                Console.WriteLine("Sheet 1 Data");
                Console.WriteLine($"Cell A2 Value   : {firstSheet.Cells["A2"].Text}");
                Console.WriteLine($"Cell A2 Color   : {firstSheet.Cells["A2"].Style.Font.Color.LookupColor()}");
                Console.WriteLine($"Cell B2 Formula : {firstSheet.Cells["B2"].Formula}");
                Console.WriteLine($"Cell B2 Value   : {firstSheet.Cells["B2"].Text}");
                Console.WriteLine($"Cell B2 Border  : {firstSheet.Cells["B2"].Style.Border.Top.Style}");
                Console.WriteLine("");

                var secondSheet = package.Workbook.Worksheets["Second Sheet"];
                Console.WriteLine($"Sheet 2 Data");
                Console.WriteLine($"Cell A2 Formula : {secondSheet.Cells["A2"].Formula}");
                Console.WriteLine($"Cell A2 Value   : {secondSheet.Cells["A2"].Text}");
            }

        }



	}
}
