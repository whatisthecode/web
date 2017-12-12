using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication2.CustomAttribute;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;

namespace WebApplication2.Controllers.API
{
    public class TestController : ApiController
    {

        public TestController()
        {
        }

        [Route("api/test")]
        [HttpGet]
        [APIAuthorize(Roles = "VIEW_USER")]
        public IHttpActionResult aaa()
        {
            Response response = new Response("200", "Clear user groups success", "");
            
            return Content<Response>(HttpStatusCode.OK, response);
        }
        [Route("api/initialize")]
        [HttpPost]
        public async Task<IHttpActionResult> initializeAsync()
        {
            Service.groupDAO.insertGroup(new Group("SuperAdmin"));
            Service.groupDAO.insertGroup(new Group("Admin"));
            Service.groupDAO.insertGroup(new Group("Merchant"));
            Service.groupDAO.insertGroup(new Group("Customer"));
            Service.groupDAO.saveGroup();
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
            Group superAdmin = Service.groupDAO.getGroupByName("SuperAdmin");
            for (Int16 roleIndex = 0; roleIndex < rolesLength; roleIndex++)
            {
                Service._roleManager.Create(new ApplicationRole(roles[roleIndex],""));
                Service.groupRoleManagerDAO.AddRoleToGroup(superAdmin.id, roles[roleIndex]);
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


            Group customer = Service.groupDAO.getGroupByName("Customer");
            Int16 customerRolesLength = (Int16)customerRoles.Length;
            for (Int16 customerRoleIndex = 0; customerRoleIndex < customerRolesLength; customerRoleIndex++)
            {
                Service.groupRoleManagerDAO.AddRoleToGroup(customer.id, customerRoles[customerRoleIndex]);
            }

            String[] merchantRoles = productRoles.Where(p => p != "VIEW_PRODUCT")
                                                 .Concat(productAttributeRoles.Where(pa => pa != "VIEW_PRODUCT_ATTRIBUTE"))
                                                 .Concat(categoryRoles.Where(c => c != "VIEW_CATEGORY"))
                                                 .Concat(categoryProductRoles.Where(cp => cp != "VIEW_CATEGORY_PRODUCT"))
                                                 .Concat(invoiceRoles.Where(i => i == "UPDATE_INVOICE"))
                                                 .ToArray();
            Group merchant = Service.groupDAO.getGroupByName("Merchant");
            Int16 merchantRolesLength = (Int16)merchantRoles.Length;
            for (Int16 merchantRoleIndex = 0; merchantRoleIndex < merchantRolesLength; merchantRoleIndex++)
            {
                Service.groupRoleManagerDAO.AddRoleToGroup(merchant.id, merchantRoles[merchantRoleIndex]);
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
            Group admin = Service.groupDAO.getGroupByName("Admin");
            Int16 adminRolesLength = (Int16)adminRoles.Length;
            for (Int16 adminRoleIndex = 0; adminRoleIndex < adminRolesLength; adminRoleIndex++)
            {
                Service.groupRoleManagerDAO.AddRoleToGroup(admin.id, adminRoles[adminRoleIndex]);
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

            IdentityResult userSuperAdmin = await Service._userManager.CreateAsync(uSuperAdmin, "123456");
            IdentityResult userAdmin = await Service._userManager.CreateAsync(uAdmin, "123456");
            IdentityResult userMerchant1 = await Service._userManager.CreateAsync(uMerchant1, "123456");
            IdentityResult userMerchant2 = await Service._userManager.CreateAsync(uMerchant2, "123456");
            IdentityResult userCustomer1 = await Service._userManager.CreateAsync(uCustomer1, "123456");
            IdentityResult userCustomer2 = await Service._userManager.CreateAsync(uCustomer2, "123456");
            if (userSuperAdmin.Succeeded && userAdmin.Succeeded && userMerchant1.Succeeded && userMerchant2.Succeeded && userCustomer1.Succeeded && userCustomer2.Succeeded)
            {
                Service.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uSuperAdmin.Id, superAdmin.id));

                Service.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uAdmin.Id, admin.id));

                Service.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uMerchant1.Id, customer.id));
                Service.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uMerchant1.Id, merchant.id));

                Service.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uMerchant2.Id, customer.id));
                Service.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uMerchant2.Id, merchant.id));

                Service.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uCustomer1.Id, customer.id));

                Service.userGroupDAO.AddUserToGroup(new ApplicationUserGroup(uCustomer2.Id, customer.id));
                Service.userGroupDAO.saveUserGroup();
            }

            //TODO : CREATE 3 category type brand, product and attribute
            Service.categoryTypeDAO.insertCategoryType(new CategoryType() { code = "BRAND", name = "Thương hiệu" });
            Service.categoryTypeDAO.insertCategoryType(new CategoryType() { code = "PRODUCT", name = "Sản phẩm" });
            Service.categoryTypeDAO.insertCategoryType(new CategoryType() { code = "ATTRIBUTE", name = "Thuộc tính" });
            Service.categoryTypeDAO.saveCategoryType();

            //TODO : CREATE 2 category type product
            Service.categoryDAO.insertCategory(new Category() { code = "MOUSE", name = "Chuột", typeId = Service.categoryTypeDAO.getCategoryTypeByCode("PRODUCT").id});
            Service.categoryDAO.insertCategory(new Category(){ code = "KEYBOARD", name = "Bàn phím", typeId = Service.categoryTypeDAO.getCategoryTypeByCode("PRODUCT").id });
            Service.categoryDAO.saveCategory();

            Response response = new Response("200", "Initializing a test enviroment!", null);
            return Content<Response>(HttpStatusCode.OK, response);
        }
    }
}
