using Cm.Domain.Products;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Microsoft.EntityFrameworkCore;

namespace Cm.Domain.Common.Dal
{
    /// <summary>
    /// Db context
    /// </summary>
    public class ApplicationContext : DbContext
    {
        /// <summary>
        /// Creates the instance of the class
        /// </summary>
        /// <param name="options"></param>
        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Setup configuration
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            ConfigureProducts(modelBuilder);
            ConfigureUsersAndRoles(modelBuilder);
        }

        /// <summary>
        /// Add data about products
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void ConfigureProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Product>().HasKey(product => product.Id);
            modelBuilder.Entity<Product>().Property(product => product.Name).IsRequired();

        }

        /// <summary>
        /// Add data about users and roles
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void ConfigureUsersAndRoles(ModelBuilder modelBuilder)
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