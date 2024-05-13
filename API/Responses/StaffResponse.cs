using API.Enums;

namespace API.Responses;

public class StaffResponse
{
    public string Id { get; set; }
    public string StaffId { get; set; }
    public string FullName { get; set; }
    public Gender Gender { get; set; }
    public DateTime Birthday { get; set; }
    public Status Status { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}