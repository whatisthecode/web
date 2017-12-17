using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.DAO;
using WebApplication2.Models;

namespace WebApplication2
{
    public static class Service
    {
        public static CategoryDAO categoryDAO = new CategoryDAOImpl();
        public static CategoryProductDAO categoryProductDAO = new CategoryProductDAOImpl();
        public static CategoryTypeDAO categoryTypeDAO = new CategoryTypeDAOImpl();
        public static GroupDAO groupDAO = new GroupDAOImpl();
        public static GroupRoleManagerDAO groupRoleManagerDAO = new GroupRoleManagerDAOImpl();
        public static InvoiceDAO invoiceDAO = new InvoiceDAOImpl();
        public static InvoiceDetailDAO invoiceDetailDAO = new InvoiceDetailDAOImpl();
        public static ProductAttributeDAO productAttributeDAO = new ProductAttributeDAOImpl();
        public static ProductDAO productDAO = new ProductDAOImpl();
        public static RoleDAO roleDAO = new RoleDAOImpl();
        public static TokenDAO tokenDAO = new TokenDAOImpl();
        public static UserGroupDAO userGroupDAO = new UserGroupDAOImpl();
        public static UserInfoDAO userInfoDAO = new UserInfoDAOImpl();
        public static UserRoleDAO userRoleDAO = new UserRoleDAOImpl();
        public static ImageDAO imageDAO = new ImageDAOImpl();
        public static UserManager<ApplicationUser> _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(DBContext.Create()));
        public static RoleManager<ApplicationRole> _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(DBContext.Create()));
    }
}