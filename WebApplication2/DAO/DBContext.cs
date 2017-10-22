using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DBContext")
        { }

        public DbSet<UserType> userTypes { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<UserRole> userRoles { get; set; }
        public DbSet<UserTypeRole> userTypesRoles { get; set; }

        public DbSet<Category> categories { get; set; }
        public DbSet<CategoryProduct> categoryProducts { get; set; }

        public DbSet<Product> products { get; set; }
        public DbSet<ProductAttribute> productAttributes { get; set; }

        public DbSet<Invoice> invoices { get; set; }
        public DbSet<InvoiceDetail> invoiceDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DBContext>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToOneConstraintIntroductionConvention>();
        }
    }
}