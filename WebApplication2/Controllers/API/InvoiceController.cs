using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using WebApplication2.CustomAttribute;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;
using WebApplication2.Services;
using static WebApplication2.Models.RequestModel.FromUri;
using static WebApplication2.Models.RequestModel.InvoiceView;

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

                for ( var j = 0; j < lists.Count(); j++)
                {
                    InvoiceProducts invoiceProduct = viewInvoiceModel.products.First(p => p.productId == lists[j].id);
                    List<ProductAttribute> proAttrs = Service.productAttributeDAO.getProAttrsByProId(lists[j].id);
                    var amount = invoiceProduct.amount;
                    var price = proAttrs[0].value;
                    var discount = proAttrs[2].value;
                    InvoiceDetail invoiceDetail = null;
                    if (double.Parse(discount) > 0)
                    {
                        invoiceDetail = new InvoiceDetail(createInvoice.id, lists[j].id, amount, double.Parse(discount));
                    }
                    else
                    {
                        invoiceDetail = new InvoiceDetail(createInvoice.id, lists[j].id, amount, double.Parse(price));
                    }
                    total = total + invoiceDetail.subTotal; //tính Total của cái hóa đơn
                    Service.invoiceDetailDAO.insertInvoiceDetail(invoiceDetail);

                }
                Invoice updateInvoice = createInvoice;
                updateInvoice.total = total;
                Service.invoiceDAO.updateInvoice(updateInvoice);

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
        [APIAuthorize]
        [HttpGet]
        public async System.Threading.Tasks.Task<IHttpActionResult> getInvoicesAsync([FromUri]PageRequest pageRequest)
        {
            String accessToken = HttpContext.Current.Request.Headers.Get("Authorization").Replace("Bearer ", "");
            Response response = new Response();
            if (accessToken != null)
            {
                Token token = Service.tokenDAO.getByAccessToken(accessToken);
                DateTime currentDate = DateTime.Now;
                DateTime expiresDate = token.expires;
                int result = DateTime.Compare(currentDate, expiresDate);
                if (result <= 0)
                {
                    var user = await Service._userManager.FindByEmailAsync(token.userName);
                    user.groups = Service.userGroupDAO.getUserGroupByUser(user.Id).ToList();

                    UserInfo userInfo = new UserInfo();
                    userInfo = Service.userInfoDAO.getUserInfo(user.userInfoId);

                    bool isAdmin = false;
                    bool isMerchant = false;
                    bool isCustomer = false;
                    foreach(var group in user.groups)
                    {
                        if (group.Group.name == "Admin" || group.Group.name == "SuperAdmin")
                        {
                            isAdmin = true;
                        } else if (group.Group.name == "Merchant")
                        {
                            isMerchant = true;
                        } else if(group.Group.name == "Customer")
                        {
                            isCustomer = true;
                        }
                    }

                    PagedResult<Invoice> pageResult = new PagedResult<Invoice>();

                    if (isAdmin)
                    {
                        pageResult = Service.invoiceDAO.PageView(0, 0, pageRequest.pageIndex, pageRequest.pageSize, pageRequest.ascending);
                    }

                    if(isMerchant)
                    {
                        pageResult = Service.invoiceDAO.PageView(0, user.userInfoId, pageRequest.pageIndex, pageRequest.pageSize, pageRequest.ascending);
                    }

                    if(isCustomer)
                    {
                        pageResult = Service.invoiceDAO.PageView(user.userInfoId, 0, pageRequest.pageIndex, pageRequest.pageSize, pageRequest.ascending);
                    }

                    if(isMerchant && isCustomer)
                    {
                        pageResult = Service.invoiceDAO.PageView(user.userInfoId, user.userInfoId, pageRequest.pageIndex, pageRequest.pageSize, pageRequest.ascending);
                    }

                    PagedResult<SalerInvoice> salerPagedResult = new PagedResult<SalerInvoice>();
                    salerPagedResult.currentPage = pageResult.currentPage;
                    salerPagedResult.pageCount = pageResult.pageCount;
                    salerPagedResult.rowCount = pageResult.rowCount;
                    salerPagedResult.pageSize = pageResult.pageSize;

                    List<SalerInvoice> listSalerInvoices = new List<SalerInvoice>();
                    for(var i = 0; i < pageResult.rowCount; i++)
                    {
                        Invoice invoice = new Invoice();
                        SalerInvoice salerInvoice = new SalerInvoice();
                        invoice = pageResult.items[i];

                        if(invoice.buyer == null)
                        {
                            invoice.buyer = Service.userInfoDAO.getUserInfo(invoice.buyerId);
                        }

                        if(invoice.saler == null)
                        {
                            invoice.saler = Service.userInfoDAO.getUserInfo(invoice.salerId);
                        }

                        IEnumerable<InvoiceDetail> iEListInvoices = Service.invoiceDetailDAO.getListDetailByInvoiceId(invoice.id);

                        List<InvoiceDetail> listInvoices = iEListInvoices.ToList();
                        foreach (var detail in listInvoices)
                        {
                            detail.Product = Service.productDAO.getProduct(detail.product);
                            detail.Product.attributes = Service.productAttributeDAO.getProAttrsByProId(detail.product);
                        }

                        salerInvoice.invoice = invoice;
                        salerInvoice.details = listInvoices;

                        listSalerInvoices.Add(salerInvoice);
                    }

                    salerPagedResult.items = listSalerInvoices;
                    response.code = "200";
                    response.status = "Thành công";
                    response.results = salerPagedResult;
                    return Content<Response>(HttpStatusCode.OK, response);
                }
                else
                {
                    Service.tokenDAO.delete(token.id);     //remove token from database
                    response.status = "Phiên đăng nhập của bạn đã hết hạn, vui lòng đăng nhập lại";
                    response.code = "401";
                    response.results = "";
                    return Content<Response>(HttpStatusCode.OK, response);
                }
            }
            else
            {
                response.status = "Phiên đăng nhập của bạn đã hết hạn, vui lòng đăng nhập lại";
                response.code = "401";
                response.results = "";
                return Content<Response>(HttpStatusCode.OK, response);
            }
            

        }

    }
}
