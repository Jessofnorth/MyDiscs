using MyDiscs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MyDiscs.Data
{
    public class MydiscContext : IdentityDbContext
    {
        public MydiscContext(DbContextOptions<MydiscContext> options)
            : base(options)
        {
        }

        public DbSet<Disc> Discs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
    }
}