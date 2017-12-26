using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class DBContext : IdentityDbContext<ApplicationUser>
    {
        public DBContext() : base("DBContext")
        {
            //this.Configuration.LazyLoadingEnabled = true;
            //this.Configuration.ProxyCreationEnabled = false;
        }

        public static DBContext Create()
        {
            return new DBContext();
        }

        public DbSet<UserInfo> userInfos { get; set; }
        public DbSet<Rating> ratings { get; set; }

        public DbSet<Category> categories { get; set; }
        public DbSet<CategoryType> categoryTypes { get; set; }
        public DbSet<Image> images { get; set; }
        public DbSet<CategoryProduct> categoryProducts { get; set; }

        public DbSet<Product> products { get; set; }
        public DbSet<ProductAttribute> productAttributes { get; set; }

        public DbSet<Invoice> invoices { get; set; }
        public DbSet<InvoiceDetail> invoiceDetails { get; set; }
        public DbSet<Group> groups { get; set; }
        public DbSet<Token> token { get; set; }
        public DbSet<ApplicationUserGroup> userGroups { get; set; } 
        public DbSet<ApplicationRoleGroup> roleGroups { get; set; }
        public DbSet<ShowHideProduct> showHideProducts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DBContext>(null);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().HasRequired(t => t.UserInfo);

            // And Here:
            EntityTypeConfiguration<Group> groupsConfig = modelBuilder.Entity<Group>().ToTable("Groups");
            groupsConfig.Property((Group r) => r.name).IsRequired();
            // Add this, so that IdentityRole can share a table with ApplicationRole:
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToOneConstraintIntroductionConvention>();
        }
    }
}