using System;
using Microsoft.EntityFrameworkCore;

namespace Shop.Services.CartAPI.Data;

public class AppDbContext : DbContext
{   
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Models.CartHeader> CartHeaders { get; set; }
    public DbSet<Models.CartDetails> CartDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}
