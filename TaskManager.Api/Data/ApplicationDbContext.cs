using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Models;


namespace TaskManager.Api.Data;


public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


public DbSet<TaskItem> Tasks => Set<TaskItem>();


protected override void OnModelCreating(ModelBuilder builder)
{
base.OnModelCreating(builder);


builder.Entity<TaskItem>()
.HasIndex(t => new { t.OwnerId, t.Status });


builder.Entity<TaskItem>()
.Property(t => t.Status)
.HasConversion<int>();


builder.Entity<TaskItem>()
.Property(t => t.Priority)
.HasConversion<int>();
}
}