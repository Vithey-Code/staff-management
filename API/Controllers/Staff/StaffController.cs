using API.Enums;
using API.Interfaces;
using API.Requests;
using API.Responses;
using API.Wrapper;
using Asp.Versioning;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Staff;

[ApiVersion("1")]
public class StaffController : BaseApiController<StaffController>
{
    private readonly IStaffService _staffService;
    private readonly IExcelService _excelService;

    public StaffController(IStaffService staffService, IExcelService excelService)
    {
        _staffService = staffService;
        _excelService = excelService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddStaffAsync(AddStaffRequest request, CancellationToken cancellationToken)
    {
        var result = await _staffService.AddStaffAsync(request, cancellationToken);
        if (result.Succeeded) 
            return Ok(await _staffService.GetStaffQuery(result.Data, cancellationToken));
        return NotFound(result);
    }   
    
    [HttpPut("{id}")]
    public async Task<IActionResult> EditStaffAsync(EditStaffRequest request, CancellationToken cancellationToken)
    {
        var result = await _staffService.EditStaffAsync(request, cancellationToken);
        if (result.Succeeded) 
            return Ok(await _staffService.GetStaffQuery(request.Id, cancellationToken));
        return NotFound(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaffAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _staffService.DeleteStaffAsync(id, cancellationToken);
        if (result.Succeeded)
            return Ok(await _staffService.GetStaffQuery(id, cancellationToken));
        return NotFound(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetStaffsQuery(int pageNumber, int pageSize, string? staffId, Gender? gender,
        DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken)
    {
        var result = await _staffService.GetStaffsQuery(pageNumber, pageSize, staffId, gender, fromDate, toDate,
            cancellationToken);
        if (result.Succeeded) 
            return Ok(result);
        return NotFound(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStaffQuery(string id, CancellationToken cancellationToken)
    {
        var result = await _staffService.GetStaffQuery(id, cancellationToken);
        if (result.Succeeded) 
            return Ok(result);
        return NotFound(result.Messages);
    }
    
    // I didn't focused on the beautiful and UX, but I focused on functionalities only for exporting pdf and excel
    [HttpGet("export-to-pdf")]
    public async Task<IActionResult> ExportToPdfAsync(int pageNumber, int pageSize, string? staffId, Gender? gender, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken)
    {
        var result = await _staffService.GetStaffsQuery(pageNumber, pageSize, staffId, gender, fromDate, toDate, cancellationToken);

        using (MemoryStream stream = new MemoryStream())
        {
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            PdfWriter.GetInstance(pdfDoc, stream);
            pdfDoc.Open();

            PdfPTable table = new PdfPTable(5);
            
            foreach (var c in new [] {"Id", "Staff Id", "FullName", "Gender", "Birthday"})
            {
                table.AddCell(c);
            }

            foreach (var staff in result.Data)
            {
                table.AddCell(staff.Id.ToString());
                table.AddCell(staff.StaffId.ToString());
                table.AddCell(staff.FullName);
                table.AddCell(staff.Gender.ToString());
                table.AddCell(staff.Birthday.ToString());
            }
        
            pdfDoc.Add(table);
            pdfDoc.Close();

            return File(stream.ToArray(), "application/pdf", "StaffData.pdf");
        }
    }
    
    [HttpGet("export-to-excel")]
    public async Task<IActionResult> ExportToExcelAsync(int pageNumber, int pageSize, string? staffId, Gender? gender,
        DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken)
    {
        var result = await _staffService.GetStaffsQuery(pageNumber, pageSize, staffId, gender, fromDate, toDate,
            cancellationToken);
        var mappings = new Dictionary<string, Func<StaffsResponse, object>>
        {
            { "Id", s => s.Id },
            { "Staff Id", s => s.StaffId },
            { "FullName", s => s.FullName },
            { "Gender", s => s.Gender },
            { "Birthday", s => s.Birthday }
        };
        
        var base64Excel = await _excelService.ExportAsync<StaffsResponse>(result.Data, mappings);
        var byteArray = Convert.FromBase64String(base64Excel); 
        return File(byteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StaffsData.xlsx");
    }
}