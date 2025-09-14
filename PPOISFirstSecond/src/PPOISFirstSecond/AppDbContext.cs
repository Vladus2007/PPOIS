using Microsoft.EntityFrameworkCore;
using PPOISFirstSecond;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class AppDbContext : DbContext
{
    public DbSet<WordPair> WordPairs { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Add constructor without parameters for design-time operations
    public AppDbContext() : base()
    {
       
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Only configure if not already configured (useful for testing)
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=laba.db");
        }
    }
}