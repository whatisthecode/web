using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;

namespace WebApplication2.Controllers.API
{
    public class TestController : ApiController
    {
        private GroupRoleManagerDAO groupRoleManagerDAO;
        private GroupDAO groupDAO;
        private ProductDAO productDAO;
        private CategoryDAO categoryDAO;
        private CategoryTypeDAO categoryTypeDAO;
        private CategoryProductDAO categoryProductDAO;
        private ProductAttributeDAO productAttributeDAO;
        private InvoiceDAO invoiceDAO;
        private InvoiceDetailDAO invoiceDetailDAO;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        private UserGroupDAO userGroupDAO;


        public TestController()
        {
            this.groupRoleManagerDAO = new GroupRoleManagerDAOImpl();
            this.groupDAO = new GroupDAOImpl();
            this.productDAO = new ProductDAOImpl();
            this.categoryDAO = new CategoryDAOImpl();
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new DBContext()));
            this._roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(new DBContext()));
            this.categoryProductDAO = new CategoryProductDAOImpl();
            this.productAttributeDAO = new ProductAttributeDAOImpl();
            this.invoiceDAO = new InvoiceDAOImpl();
            this.invoiceDetailDAO = new InvoiceDetailDAOImpl();
            this.categoryTypeDAO = new CategoryTypeDAOImpl();
            this.userGroupDAO = new UserGroupDAOImpl();
        }


        [Route("api/initialize")]
        [HttpPost]
        public async Task<IHttpActionResult> initializeAsync()
        {
            this.groupDAO.insertGroup(new Group("SuperAdmin"));
            this.groupDAO.insertGroup(new Group("Admin"));
            this.groupDAO.insertGroup(new Group("Merchant"));
            this.groupDAO.insertGroup(new Group("Customer"));
            this.groupDAO.saveGroup();
            String[] userRoles = { "CREATE_USER", "VIEW_USER", "DELETE_USER", "UPDATE_USER" };
            String[] productRoles = { "CREATE_PRODUCT", "VIEW_PRODUCT", "DELETE_PRODUCT", "UPDATE_PRODUCT" };
            String[] productAttributeRoles = { "CREATE_PRODUCT_ATTRIBUTE", "VIEW_PRODUCT_ATTRIBUTE", "DELETE_PRODUCT_ATTRIBUTE", "UPDATE_PRODUCT_ATTRIBUTE" };
            String[] ratingRoles = { "CREATE_RATING", "VIEW_RATING", "UPDATE_RATING", "DELETE_RATING" };
            String[] imageRoles = { "CREATE_IMAGE", "VIEW_IMAGE", "UPDATE_IMAGE", "DELETE_IMAGE" };
            String[] categoryRoles = { "CREATE_CATEGORY", "VIEW_CATEGORY", "DELETE_CATEGORY", "UPDATE_CATEGORY" };
            String[] categoryProductRoles = { "CREATE_CATEGORY_PRODUCT", "VIEW_CATEGORY_PRODUCT", "UPDATE_CATEGORY_PRODUCT", "DELETE_CATEGORY_PRODUCT" };
            String[] invoiceRoles = { "CREATE_INVOICE", "VIEW_INVOICE", "UPDATE_INVOICE", "DELETE_INVOICE" };
            String[] invoiceDetailRoles = { "CREATE_INVOICE_DETAIL", "VIEW_INVOICE_DETAIL", "UPDATE_INVOICE_DETAIL", "DELETE_INVOICE_DETAIL" };
            String[] roles = userRoles.Concat(productRoles)
                                      .Concat(productAttributeRoles)
                                      .Concat(ratingRoles)
                                      .Concat(imageRoles)
                                      .Concat(categoryRoles)
                                      .Concat(categoryProductRoles)
                                      .Concat(invoiceRoles)
                                      .Concat(invoiceDetailRoles)
                                      .ToArray();
            Int16 rolesLength = (Int16)roles.Length;
            Group superAdmin = this.groupDAO.getGroupByName("SuperAdmin");
            for (Int16 roleIndex = 0; roleIndex < rolesLength; roleIndex++)
            {
                this._roleManager.Create(new ApplicationRole(roles[roleIndex],""));
                this.groupRoleManagerDAO.AddRoleToGroup(superAdmin.id, roles[roleIndex]);
            }

            String[] customerRoles = ratingRoles.Concat(invoiceDetailRoles)
                                                .Concat(imageRoles)
                                                .Concat(userRoles.Where(u => u == "VIEW_USER" || u == "UPDATE_USER"))
                                                .Concat(productRoles.Where(p => p == "VIEW_PRODUCT"))
                                                .Concat(productAttributeRoles.Where(pa => pa == "VIEW_PRODUCT_ATTRIBUTE"))
                                                .Concat(categoryRoles.Where(c => c == "VIEW_CATEGORY"))
                                                .Concat(categoryProductRoles.Where(cp => cp == "VIEW_CATEGORY_PRODUCT"))
                                                .Concat(invoiceRoles.Where(i => i == "CREATE_INVOICE" || i == "VIEW_INVOICE" || i == "DELETE_INVOICE"))
                                                .ToArray();


            Group customer = this.groupDAO.getGroupByName("Customer");
            Int16 customerRolesLength = (Int16)customerRoles.Length;
            for (Int16 customerRoleIndex = 0; customerRoleIndex < customerRolesLength; customerRoleIndex++)
            {
                this.groupRoleManagerDAO.AddRoleToGroup(customer.id, customerRoles[customerRoleIndex]);
            }

            String[] merchantRoles = productRoles.Where(p => p != "VIEW_PRODUCT")
                                                 .Concat(productAttributeRoles.Where(pa => pa != "VIEW_PRODUCT_ATTRIBUTE"))
                                                 .Concat(categoryRoles.Where(c => c != "VIEW_CATEGORY"))
                                                 .Concat(categoryProductRoles.Where(cp => cp != "VIEW_CATEGORY_PRODUCT"))
                                                 .Concat(invoiceRoles.Where(i => i == "UPDATE_INVOICE"))
                                                 .ToArray();
            Group merchant = this.groupDAO.getGroupByName("Merchant");
            Int16 merchantRolesLength = (Int16)merchantRoles.Length;
            for (Int16 merchantRoleIndex = 0; merchantRoleIndex < merchantRolesLength; merchantRoleIndex++)
            {
                this.groupRoleManagerDAO.AddRoleToGroup(merchant.id, merchantRoles[merchantRoleIndex]);
            }

            String[] adminRoles = userRoles.Concat(imageRoles)
                                           .Concat(categoryRoles)
                                           .Concat(productRoles.Where(p => p == "VIEW_PRODUCT"))
                                           .Concat(productAttributeRoles.Where(pa => pa == "VIEW_PRODUCT_ATTRIBUTE"))
                                           .Concat(ratingRoles.Where(r => r == "VIEW_RATING"))
                                           .Concat(categoryProductRoles.Where(cp => cp == "VIEW_CATEGORY_PRODUCT"))
                                           .Concat(invoiceRoles.Where(i => i == "VIEW_INVOICE"))
                                           .Concat(invoiceDetailRoles.Where(id => id == "VIEW_INVOICE_DETAIL"))
                                           .ToArray();
            Group admin = this.groupDAO.getGroupByName("Admin");
            Int16 adminRolesLength = (Int16)adminRoles.Length;
            for (Int16 adminRoleIndex = 0; adminRoleIndex < adminRolesLength; adminRoleIndex++)
            {
                this.groupRoleManagerDAO.AddRoleToGroup(admin.id, adminRoles[adminRoleIndex]);
            }

            //TODO : CREATE 6 account superAdmin, admin, 2 merchants, 2 customers
            ApplicationUser uSuperAdmin = new ApplicationUser() { UserName = "superadmin@aletaobao.com", Email = "superadmin@aletaobao.com" };
            ApplicationUser uAdmin = new ApplicationUser() { UserName = "admin@aletaobao.com", Email = "admin@aletaobao.com" };
            ApplicationUser uMerchant1 = new ApplicationUser() { UserName = "hggntg@gmail.com", Email = "hggntg@gmail.com" };
            ApplicationUser uMerchant2 = new ApplicationUser() { UserName = "ledinhnam1131@gmail.com", Email = "ledinhnam1131@gmail.com" };
            ApplicationUser uCustomer1 = new ApplicationUser() { UserName = "hungtienphan1995@gmail.com", Email = "hungtienphan1995@gmail.com" };
            ApplicationUser uCustomer2 = new ApplicationUser() { UserName = "ngodonghong@gmail.com", Email = "ngodonghong@gmail.com" };

            uSuperAdmin.userInfo = new UserInfo(true, "Super", "Admin", new DateTime(1989, 11, 1), "262816900");
            uAdmin.userInfo = new UserInfo(true, "Admin", "", new DateTime(1992, 8, 1), "202916901");
            uMerchant1.userInfo = new UserInfo(true, "Ngô Bảo", "Hoàng Minh", new DateTime(1994, 06, 25), "332226911");
            uMerchant2.userInfo = new UserInfo(true, "Đào Hữu", "Tình", new DateTime(1988, 12, 30), "122006320");
            uCustomer1.userInfo = new UserInfo(true, "Đoàn", "Dự", new DateTime(1992, 06, 10), "332666999");
            uCustomer2.userInfo = new UserInfo(true, "Đoàn Chính", "Thuần", new DateTime(1993, 11, 11), "272333978");

            IdentityResult userSuperAdmin = await this._userManager.CreateAsync(uSuperAdmin, "123456");
            IdentityResult userAdmin = await this._userManager.CreateAsync(uAdmin, "123456");
            IdentityResult userMerchant1 = await this._userManager.CreateAsync(uMerchant1, "123456");
            IdentityResult userMerchant2 = await this._userManager.CreateAsync(uMerchant2, "123456");
            IdentityResult userCustomer1 = await this._userManager.CreateAsync(uCustomer1, "123456");
            IdentityResult userCustomer2 = await this._userManager.CreateAsync(uCustomer2, "123456");
            if (userSuperAdmin.Succeeded && userAdmin.Succeeded && userMerchant1.Succeeded && userMerchant2.Succeeded && userCustomer1.Succeeded && userCustomer2.Succeeded)
            {
                this.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uSuperAdmin.Id, superAdmin.id));

                this.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uAdmin.Id, admin.id));

                this.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uMerchant1.Id, customer.id));
                this.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uMerchant1.Id, merchant.id));

                this.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uMerchant2.Id, customer.id));
                this.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uMerchant2.Id, merchant.id));

                this.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uCustomer1.Id, customer.id));

                this.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uCustomer2.Id, customer.id));
                this.userGroupDAO.saveUserGroup();
            }

            //TODO : CREATE 3 category type brand, product and attribute
            this.categoryTypeDAO.insertCategoryType(new CategoryType() { code = "BRAND", name = "Thương hiệu" });
            this.categoryTypeDAO.insertCategoryType(new CategoryType() { code = "PRODUCT", name = "Sản phẩm" });
            this.categoryTypeDAO.insertCategoryType(new CategoryType() { code = "ATTRIBUTE", name = "Thuộc tính" });
            this.categoryTypeDAO.saveCategoryType();

            //TODO : CREATE 2 category type product
            this.categoryDAO.insertCategory(new Category() { code = "MOUSE", name = "Chuột", typeId = this.categoryTypeDAO.getCategoryTypeByCode("PRODUCT").id});
            this.categoryDAO.insertCategory(new Category(){ code = "KEYBOARD", name = "Bàn phím", typeId = this.categoryTypeDAO.getCategoryTypeByCode("PRODUCT").id });
            this.categoryDAO.saveCategory();

            Response response = new Response("200", "Initializing a test enviroment!", null);
            return Content<Response>(HttpStatusCode.OK, response);
        }
    }
}
