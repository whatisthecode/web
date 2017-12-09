using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;

namespace WebApplication2.Controllers.API
{
    public class InvoiceController : ApiController
    {

        public InvoiceController()
        {

        }

        [Route("api/invoice")]
        [HttpPost]
        public IHttpActionResult insert([FromBody]Invoice invoice)
        {
            Response response = new Response();
            if (Service.invoiceDAO.checkExist(invoice) != null)
            {
                response = new Response("409", "Hóa đơn này đã tồn tại", null);
                return Content<Response>(HttpStatusCode.Conflict, response);
            }
            else
            {
                Service.invoiceDAO.insertInvoice(invoice);
                Service.invoiceDAO.saveInvoice();
                response = new Response("201", "Hóa đơn đã được thêm", invoice);
                return Content<Response>(HttpStatusCode.Created, response);
            }
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
