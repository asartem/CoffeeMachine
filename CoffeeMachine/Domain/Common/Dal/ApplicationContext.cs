using Cm.Domain.Products;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Microsoft.EntityFrameworkCore;

namespace Cm.Domain.Common.Dal
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            ConfigureProducts(modelBuilder);
            ConfigureUsers(modelBuilder);
        }

        private void ConfigureProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Product>().HasKey(product => product.Id);
            modelBuilder.Entity<Product>().Property(product => product.Name).IsRequired();

        }

        private void ConfigureUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(user => user.Id);
            modelBuilder.Entity<User>().Property(user => user.Name).IsRequired();


            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserRole>().HasKey(user => user.Id);
            modelBuilder.Entity<UserRole>().Property(user => user.Name);

        }
    }
}