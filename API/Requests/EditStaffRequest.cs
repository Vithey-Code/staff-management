using System.ComponentModel.DataAnnotations;
using API.Enums;

namespace API.Requests;

public class EditStaffRequest
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string StaffId { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public DateTime Birthday { get; set; }
    [Required]
    public Gender Gender { get; set; }
}