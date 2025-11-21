using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Models;

namespace RealEstateApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<SavedProperty> SavedProperties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Agent> Agents { get; set; } 

    }
}