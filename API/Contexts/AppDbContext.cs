using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Staff> Staff { get; set; }
}