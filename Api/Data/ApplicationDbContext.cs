using ApiDemo.Models;

using Microsoft.EntityFrameworkCore;

namespace ApiDemo.Data;

public class ApplicationDbContext : DbContext
{

    // We have to configure the DB URL in program.cs
    // Configuration will be passed to this constructor via options parameter  
    // This lets the ApplicationDbContext know the Db URL (via base consturctor)
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Shirt> Shirts { get; set; }
    // DbSet represents a Db Table 
    // properties in Shirt class => will be translated into columns in Db

    // use this method to configure Db
    // ie. configure relationships btwn tables, override PK, column names
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // data seeding
        modelBuilder.Entity<Shirt>().HasData(
            new Shirt { ShirtId = 1, Brand = "Levi's", Color = "Blue", Gender = "Men", Price = 30, Size = 10 },
            new Shirt { ShirtId = 2, Brand = "MLH", Color = "Red", Gender = "Men", Price = 35, Size = 12 },
            new Shirt { ShirtId = 3, Brand = "Apple", Color = "White", Gender = "Women", Price = 22, Size = 8 },
            new Shirt { ShirtId = 4, Brand = "Nike", Color = "Black", Gender = "Women", Price = 33, Size = 9 }
        );
    }
}