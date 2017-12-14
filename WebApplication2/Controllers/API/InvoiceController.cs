using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.CustomAttribute;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;
using WebApplication2.Services;
using static WebApplication2.Models.RequestModel.Invoice;

namespace WebApplication2.Controllers.API
{
    public class InvoiceController : ApiController
    {

        public InvoiceController()
        {

        }

        [APIAuthorize(Roles = "CREATE_INVOICE")]
        [Route("api/invoice")]
        [HttpPost]
        public IHttpActionResult insertAsync([FromBody]ViewInvoiceModel viewInvoiceModel)
        {
            Response response = new Response();
            //if (Service.invoiceDAO.checkExist(viewInvoiceModel) != null)
            //{
            //    response = new Response("409", "Hóa đơn này đã tồn tại", null);
            //    return Content<Response>(HttpStatusCode.Conflict, response);
            //}
            List<Product> products = new List<Product>();
            for (var i = 0; i < viewInvoiceModel.products.Count(); i++)
            {
                var product = viewInvoiceModel.products[i];
                Product saleProduct = Service.productDAO.getProduct(product.productId);
                saleProduct.attributes = Service.productAttributeDAO.getProAttrsByProId(saleProduct.id);
                products.Add(saleProduct);
            }

            List<Int16> salers = new List<Int16>();
            for(var i = 0; i < products.Count(); i++)
            {
                Int16 saler = products[i].createdBy;
                if(viewInvoiceModel.buyer == saler)
                {
                    response.code = "409";
                    response.status = "Bạn không thể mua sản phẩm của chính mình";
                    return Content<Response>(HttpStatusCode.Conflict, response);
                }
                bool existSaler = salers.Contains(saler);
                if(existSaler == false)
                {
                    salers.Add(saler);
                }
            }

            for(var i = 0; i < salers.Count(); i++)
            {
                var code = "HD" + salers[i].ToString() + Utils.RandomString(6);
                IEnumerable<Product> saleProducts = products.Where(p => p.createdBy == salers[i]);
                List<Product> lists = saleProducts.ToList();
                Double total = 0;
                Invoice createInvoice = new Invoice(code, viewInvoiceModel.buyer, salers[i], total);
                Service.invoiceDAO.insertInvoice(createInvoice);
                Service.invoiceDAO.saveInvoice();
                for ( var j = 0; j < lists.Count(); j++)
                {
                    InvoiceProducts invoiceProduct = viewInvoiceModel.products.First(p => p.productId == lists[j].id);
                    List<ProductAttribute> proAttrs = Service.productAttributeDAO.getProAttrsByProId(lists[j].id);
                    var amount = invoiceProduct.amount;
                    var price = proAttrs[0].value;
                    Double subTotal = amount * int.Parse(price);
                    total = total + subTotal;
                    InvoiceDetail invoiceDetail = new InvoiceDetail(createInvoice.id, lists[j].id, amount, int.Parse(price), subTotal);
                    Service.invoiceDetailDAO.insertInvoiceDetail(invoiceDetail);
                    Service.invoiceDetailDAO.saveInvoiceDetail();
                }
                Invoice updateInvoice = createInvoice;
                updateInvoice.total = total;
                Service.invoiceDAO.updateInvoice(updateInvoice);
                Service.invoiceDAO.saveInvoice();
                //var message = "<p>Thông tin đơn hàng của bạn</p> " +
                //              "<p>Mã hóa đơn: " + createInvoice.id +"</p>" +
                //              "<p></p>";
            }
            //await Service._userManager.SendEmailAsync(viewInvoiceModel.buyer, "Thông tin đơn hàng", );
            response = new Response("201", "Hóa đơn đã được thêm", products);
            return Content<Response>(HttpStatusCode.Created, response);
        }

        [Route("api/invoice")]
        [HttpPut]
        public IHttpActionResult update([FromBody]Invoice invoice)
        {
            Invoice invoice1 = Service.invoiceDAO.checkExist(invoice);
            Response response = new Response();
            if (invoice1 != null && invoice1.id != invoice.id)
            {
                response = new Response("409", "Hóa đơn đã tồn tại", null);
                return Content<Response>(HttpStatusCode.NotFound, response);
            }
            else if (Service.invoiceDAO.getInvoiceById(invoice.id) == null)
            {
                response = new Response("404", "Hóa đơn không tồn tại", null);
                return Content<Response>(HttpStatusCode.NotFound, response);
            }
            else
            {
                invoice1 = Service.invoiceDAO.getInvoiceById(invoice.id);
                invoice1.code = invoice.code;
                invoice1.buyerId = invoice.buyerId;
                invoice1.salerId = invoice.salerId;
                invoice1.total = invoice.total;
                Service.invoiceDAO.updateInvoice(invoice1);
                Service.invoiceDAO.saveInvoice();
                response = new Response("200", "Cập nhật hóa đơn thành công", invoice);
                return Content<Response>(HttpStatusCode.OK, response);
            }
        }

        [Route("api/invoice/{id}")]
        [HttpDelete]
        public IHttpActionResult delete(short id)
        {
            Invoice invo = Service.invoiceDAO.getInvoiceById(id);
            Response response = new Response();
            if (invo == null)
            {
                response.code = "404";
                response.status = "Invoice not exist";
                return Content<Response>(HttpStatusCode.NotFound, response);
            }

            Service.invoiceDAO.deleteInvoice(id);
            response.code = "200";
            response.status = "Xóa hóa đơn thành công";
            return Content<Response>(HttpStatusCode.OK, response);
        }

        [Route("api/invoice/getby/{id}")]
        [HttpGet]
        public IHttpActionResult getInvoice(short id)
        {
            Invoice invoicetemp = Service.invoiceDAO.getInvoiceById(id);
            Response response = new Response();
            if (invoicetemp == null)
            {
                response.code = "404";
                response.status = ("Không tìm thấy hóa đơn");
                return Content<Response>(HttpStatusCode.NotFound, response);
            }

            response.code = "200";
            response.status = "Loại sản phẩm cần tìm";
            response.results = invoicetemp;
            return Content<Response>(HttpStatusCode.OK, response);

        }
        [Route("api/invoice")]
        [HttpGet]
        public IHttpActionResult getInvoices()
        {
            IEnumerable<Invoice> invoices = Service.invoiceDAO.getInvoice();
            Response response = new Response();
            response.code = "200";
            response.status = "Thành công";
            response.results = invoices.ToList();
            return Content<Response>(HttpStatusCode.OK, response);

        }

    }
}
