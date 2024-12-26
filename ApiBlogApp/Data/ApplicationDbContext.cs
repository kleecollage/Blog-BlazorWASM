using ApiBlogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBlogApp.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) {}
    
    // MODELS
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }
}