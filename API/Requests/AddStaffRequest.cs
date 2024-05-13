using System.ComponentModel.DataAnnotations;
using API.Enums;

namespace API.Requests;

public class AddStaffRequest
{
    [Required]
    [MinLength(8), MaxLength(8)]
    public string StaffId { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public DateTime Birthday { get; set; }
    [Required]
    public Gender Gender { get; set; }
}