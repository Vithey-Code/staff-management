using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using API.Interfaces;
using ExcelPackage = OfficeOpenXml.ExcelPackage;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace API.Services
{
    public class ExcelService : IExcelService
    {
        public async Task<string> ExportAsync<TData>(IEnumerable<TData> data
            , Dictionary<string, Func<TData, object>> mappers
            , string sheetName = "Staffs")
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var p = new ExcelPackage();
                p.Workbook.Properties.Author = "Staff Info";
                p.Workbook.Worksheets.Add("Staffs");
                var ws = p.Workbook.Worksheets[0];
                ws.Name = sheetName;
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.Font.Name = "Calibri";

                var colIndex = 1;
                var rowIndex = 1;

                var headers = mappers.Keys.Select(x => x).ToList();

                foreach (var header in headers)
                {
                    var cell = ws.Cells[rowIndex, colIndex];

                    var fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.LightBlue);

                    var border = cell.Style.Border;
                    border.Bottom.Style =
                        border.Top.Style =
                            border.Left.Style =
                                border.Right.Style = ExcelBorderStyle.Thin;

                    cell.Value = header;

                    colIndex++;
                }

                var dataList = data.ToList();
                foreach (var item in dataList)
                {
                    colIndex = 1;
                    rowIndex++;

                    var result = headers.Select(header => mappers[header](item));

                    foreach (var value in result)
                    {
                        ws.Cells[rowIndex, colIndex++].Value = value;
                    }
                }

                using (ExcelRange autoFilterCells = ws.Cells[1, 1, dataList.Count + 1, headers.Count])
                {
                    autoFilterCells.AutoFilter = true;
                    autoFilterCells.AutoFitColumns();
                }

                var byteArray = await p.GetAsByteArrayAsync();
                return Convert.ToBase64String(byteArray);
            }
    }
}