using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class DBContext : IdentityDbContext<ApplicationUser>
    {
        public DBContext() : base("DBContext")
        { }

        public static DBContext Create()
        {
            return new DBContext();
        }

        public DbSet<UserInfo> userInfos { get; set; }
        public DbSet<UserType> userTypes { get; set; }
        public DbSet<UserRole> userRoles { get; set; }
        public DbSet<UserTypeRole> userTypesRoles { get; set; }
        public DbSet<Rating> ratings { get; set; }

        public DbSet<Category> categories { get; set; }
        public DbSet<CategoryType> categoryTypes { get; set; }
        public DbSet<Image> images { get; set; }
        public DbSet<CategoryProduct> categoryProducts { get; set; }

        public DbSet<Product> products { get; set; }
        public DbSet<ProductAttribute> productAttributes { get; set; }

        public DbSet<Invoice> invoices { get; set; }
        public DbSet<InvoiceDetail> invoiceDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DBContext>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserInfo>().HasKey(k => k.id)
                        .Property(c => c.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationUser>().HasRequired(t => t.userInfo);

            modelBuilder.Entity<Product>().HasRequired(n => n.userInfo)
                        .WithMany(a => a.products)
                        .HasForeignKey(n => n.createdBy)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>().HasRequired(n => n.buyer)
                        .WithMany(a => a.buyerInvoices)
                        .HasForeignKey(n => n.buyerId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>().HasRequired(n => n.saler)
                        .WithMany(a => a.salerInvoices)
                        .HasForeignKey(n => n.salerId)
                        .WillCascadeOnDelete(false);


            modelBuilder.Entity<UserInfo>().HasRequired(n => n.UserType)
                        .WithMany(a => a.userInfo)
                        .HasForeignKey(n => n.type)
                        .WillCascadeOnDelete(false);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToOneConstraintIntroductionConvention>();
        }
    }
}