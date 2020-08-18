using DesafioCSharpDotNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioCSharpDotNetCore.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            :base(options)
        {
            //there are not options here, because I'm working with DB in memory.
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
