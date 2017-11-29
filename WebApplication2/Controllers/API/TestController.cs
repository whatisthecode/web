using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.DAO;
using WebApplication2.Models.Mapping;

namespace WebApplication2.Controllers.API
{
    public class TestController : ApiController
    {
        private GroupRoleManagerDAO groupRoleManagerDAO;
        private ProductDAO productDAO;
        private CategoryDAO categoryDAO;
        private CategoryProductDAO categoryProductDAO;
        private ProductAttributeDAO productAttributeDAO;
        private InvoiceDAO invoiceDAO;
        private InvoiceDetailDAO invoiceDetailDAO;


        public TestController()
        {
            this.groupRoleManagerDAO = new GroupRoleManagerDAOImpl();
            this.productDAO = new ProductDAOImpl();
            this.categoryDAO = new CategoryDAOImpl();
            this.categoryProductDAO = new CategoryProductDAOImpl();
            this.productAttributeDAO = new ProductAttributeDAOImpl();
            this.invoiceDAO = new InvoiceDAOImpl();
            this.invoiceDetailDAO = new InvoiceDetailDAOImpl();
        }

        [Route("api/initialize")]
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public IHttpActionResult initialize()
        {
            this.groupRoleManagerDAO.CreateGroup("SuperAdmin");
            this.groupRoleManagerDAO.CreateGroup("Admin");
            this.groupRoleManagerDAO.CreateGroup("Merchant");
            this.groupRoleManagerDAO.CreateGroup("Customer");
            String[] userRoles = { "CREATE_USER", "VIEW_USER", "DELETE_USER", "UPDATE_USER" };
            String[] productRoles = { "CREATE_PRODUCT", "VIEW_PRODUCT", "DELETE_PRODUCT", "UPDATE_PRODUCT" };
            String[] productAttributeRoles = { "CREATE_PRODUCT_ATTRIBUTE", "VIEW_PRODUCT_ATTRIBUTE", "DELETE_PRODUCT_ATTRIBUTE", "UPDATE_PRODUCT_ATTRIBUTE" };
            String[] ratingRoles = { "CREATE_RATING", "VIEW_RATING", "UPDATE_RATING", "DELETE_RATING" };
            String[] categoryRoles = { "CREATE_CATEGORY", "VIEW_CATEGORY", "DELETE_CATEGORY", "UPDATE_CATEGORY" };
            String[] categoryProductRoles = { "CREATE_CATEGORY_PRODUCT", "VIEW_CATEGORY_PRODUCT", "UPDATE_CATEGORY_PRODUCT", "DELETE_CATEGORY_PRODUCT" };
            String[] invoiceRoles = { "CREATE_INVOICE", "VIEW_INVOICE", "UPDATE_INVOICE", "DELETE_INVOICE" };
            String[] invoiceDetailRoles = { "CREATE_INVOICE_DETAIL", "VIEW_INVOICE_DETAIL", "UPDATE_INVOICE_DETAIL", "DELETE_INVOICE_DETAIL" };
            String[] categoryTypeRoles = { "CREATE_CATEGORY_TYPE", "VIEW_CATEGORY_TYPE", "UPDATE_CATEGORY_TYPE", "DELETE_CATEGORY_TYPE" };
            String[] roles = { };
            
            Int16 rolesLength = (Int16)roles.Length;

            for(Int16 roleIndex = 0; roleIndex < rolesLength; roleIndex++)
            {
                this.groupRoleManagerDAO.createRole(roles[roleIndex]);
            }

            Response response = new Response("200", "Initializing a test enviroment!", null);
            return Content<Response>(HttpStatusCode.OK, response);
        }
    }
}
