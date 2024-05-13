using API.Enums;

namespace API.Responses;

public class StaffsResponse
{
    public string Id { get; set; }
    public string StaffId { get; set; }
    public string FullName { get; set; }
    public Gender Gender { get; set; }
    public string Birthday { get; set; }
}