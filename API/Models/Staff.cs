using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Enums;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public class Staff : BasedEntity<string>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column(TypeName = "varchar(8)")]
    public string StaffId { get; set; }
    
    [Column(TypeName = "varchar(100)")]
    public string FullName { get; set; }
    
    [Column(TypeName = "int")]
    public Gender Gender { get; set; }
    [Column(TypeName = "DateTime")]
    public DateTime Birthday { get; set; }
}