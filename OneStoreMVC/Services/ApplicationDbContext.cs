using Microsoft.EntityFrameworkCore;
using OneStoreMVC.Models;
namespace OneStoreMVC.Services;


    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }


