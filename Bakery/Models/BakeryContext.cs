using Microsoft.EntityFrameworkCore;

namespace Bakery.Models
{
  public class BakeryContext : DbContext
  {
    public DbSet<Flavor> Flavors { get; set; }
    public DbSet<Treat> Treats { get; set; }
    public DbSet<FlavorTreat> FlavorTreats { get; set; }

    public BakeryContext(DbContextOptions options) : base(options) { }
  }
}