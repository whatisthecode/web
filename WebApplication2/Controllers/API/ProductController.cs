using LaptopWebsite.Models.Mapping;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApplication2.CustomAttribute;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;
using WebApplication2.Models.RequestModel;
using WebApplication2.Services;
using static WebApplication2.Models.RequestModel.FromUri;

namespace WebApplication2.Controllers.API
{
    public class ProductController : ApiController
    {
        static Uri baseUrl = new Uri("http://localhost:54962/");
        public ProductController()
        {

        }

        [Route("api/product")]
        [HttpPost]
        [APIAuthorize(Roles = "CREATE_PRODUCT")]
        public async Task<IHttpActionResult> insertNewProduct([FromBody]CreateProductModel createProductModel)
        {
            Response response = Utils.checkInput(createProductModel, CreateProductModel.required);
            String accessToken = HttpContext.Current.Request.Headers.Get("Authorization").Replace("Bearer ","");
            Token token = Service.tokenDAO.getByAccessToken(accessToken);
            ApplicationUser appUser = Service._userManager.FindByEmailAsync(token.userName).Result;
            createProductModel.createdBy = appUser.userInfoId;
            if (response.code != "422")
            {
                Product productcheck = Service.productDAO.checkexist(createProductModel.code);
                UserInfo user = Service.userInfoDAO.getUserInfo(createProductModel.createdBy);
                if (user == null)
                {
                    response.code = "404";
                    response.status = "Người dùng hiện tại chưa có quyền thêm sản phẩm";
                    return Content<Response>(HttpStatusCode.NotFound, response);

                }
                else if (productcheck != null)
                {
                    response.code = "409";
                    response.status = "Trùng sản phẩm, xin nhập lại";
                    return Content<Response>(HttpStatusCode.Conflict, response);
                }
                else
                {
                                      
                    //create Product object to insert data
                    Product product = new Product();
                    product.code = createProductModel.code;
                    product.shortDescription = createProductModel.shortDescription;
                    product.longDescription = createProductModel.longDescription;
                    product.name = createProductModel.name;
                    product.createdBy = createProductModel.createdBy;
                    Service.productDAO.insertProduct(product);

                    //insert data to category product
                    Product newProduct = Service.productDAO.checkexist(product.code);
                    for (int i = 0; i < createProductModel.categories.Length; i++)
                    {
                        if (Service.categoryDAO.checkExist(createProductModel.categories[i]) != null)
                        {
                            CategoryProduct catepro = new CategoryProduct();
                            catepro.categoryId = createProductModel.categories[i];
                            catepro.productId = newProduct.id;
                            Service.categoryProductDAO.insertCategoryProduct(catepro);
                        }
                    }
                    foreach (var attribute in createProductModel.attributes)
                    {
                        ProductAttribute productAttribute = new ProductAttribute(product.id, attribute.Key, attribute.Value.ToString());
                        Service.productAttributeDAO.insertProductAttribute(productAttribute);

                    }

                    foreach (var thumbnail in createProductModel.thumbnails)
                    {
                        HttpClient httpClient = new HttpClient();
                        httpClient.BaseAddress = baseUrl;
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Add("Authorization", accessToken);

                        CreateImageModel createImageModel = new CreateImageModel();
                        createImageModel.productId = product.id;
                        createImageModel.url = thumbnail.GetValue("url").ToString();
                        createImageModel.type = "thumbnail";

                        HttpContent httpContent = new ObjectContent<CreateImageModel>(createImageModel, new JsonMediaTypeFormatter());
                        await httpClient.PostAsync("api/image/upload/base64", httpContent);
                    }

                    foreach (var detail in createProductModel.details)
                    {
                        HttpClient httpClient = new HttpClient();
                        httpClient.BaseAddress = baseUrl;
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Add("Authorization", accessToken);

                        CreateImageModel createImageModel = new CreateImageModel();
                        createImageModel.productId = product.id;
                        createImageModel.url = detail.GetValue("url").ToString();
                        createImageModel.type = "detail";

                        HttpContent httpContent = new ObjectContent<CreateImageModel>(createImageModel, new JsonMediaTypeFormatter());
                        await httpClient.PostAsync("api/image/upload/base64", httpContent);
                    }

                    response.code = "201";
                    response.status = "Thêm sản phẩm thành công";
                    response.results = product;
                    return Content<Response>(HttpStatusCode.Created, response);
                }

            }

            return Content<Response>(HttpStatusCode.OK, response);
        }

        [Route("api/product/{id}")]
        [HttpGet]
        public IHttpActionResult getProductWithConditions(short id)
        {
            Response response = new Response();
            Product product = Service.productDAO.getProduct(id);
            if (product == null)
            {
                response.code = "404";
                response.status = "Sản phẩm cần tìm không có trong danh sách";
                return Content<Response>(HttpStatusCode.NotFound, response);
            }
            else
            {
                ProductDetail productDetail = new ProductDetail();
                response.code = "200";
                response.status = "Sản phẩm cần tìm là: ";
                productDetail.id = product.id;
                productDetail.code = product.code;
                productDetail.name = product.name;
                productDetail.status = product.status;
                productDetail.shortDescription = product.shortDescription;
                productDetail.longDescription = product.longDescription;
                productDetail.userInfo = Service.userInfoDAO.getUserInfo(product.createdBy);
                productDetail.attributes = Service.productAttributeDAO.getProAttrsByProId(product.id);
                productDetail.choseCategories = Service.categoryProductDAO.getListCategoryProductByProdId(product.id).ToList();
                productDetail.thumbnails = Service.imageDAO.getThumbnail(product.id).ToList();
                productDetail.details = Service.imageDAO.getDetailImage(product.id).ToList();
                response.results = productDetail;
                return Content<Response>(HttpStatusCode.OK, response);
            }

        }

        [Route("api/products/")]
        [HttpGet]
        public IHttpActionResult getProductslist([FromUri] PageRequest pageRequest)
        {
            Response response = new Response();

            PagedResult<Product> pagedResult = new PagedResult<Product>();
            pagedResult =  Service.productDAO.PageView(pageRequest.pageIndex, pageRequest.pageSize, pageRequest.order, false);

            PagedResult<ProductDetail> pagedResultDetail = new PagedResult<ProductDetail>();
            pagedResultDetail.currentPage = pagedResult.currentPage;
            pagedResultDetail.pageCount = pagedResult.pageCount;
            pagedResultDetail.pageSize = pagedResult.pageSize;
            pagedResultDetail.rowCount = pagedResult.rowCount;

            List<ProductDetail> listDetails = new List<ProductDetail>();
            IList<Product> products = pagedResult.items;
            Int16 productsLength = (Int16)products.Count;
            for (Int16 i = 0; i < productsLength; i++)
            {
                Product product = products[i];
                ProductDetail productDetail = new ProductDetail();
                productDetail.id = product.id;
                productDetail.code = product.code;
                productDetail.name = product.name;
                productDetail.status = product.status;
                productDetail.userInfo = Service.userInfoDAO.getUserInfo(product.createdBy);
                productDetail.attributes = Service.productAttributeDAO.getProAttrsByProId(product.id);
                productDetail.choseCategories = Service.categoryProductDAO.getListCategoryProductByProdId(product.id).ToList();
                foreach(var pro in productDetail.choseCategories)
                {
                    pro.Category = Service.categoryDAO.getCategoryById(pro.categoryId);
                }
                productDetail.thumbnails = Service.imageDAO.getThumbnail(product.id).ToList();
                listDetails.Add(productDetail);
            }
            pagedResultDetail.items = listDetails;
            response.code = "200";
            response.status = "Danh sách sản phẩm hiện tại: ";
            response.results = pagedResultDetail;
            return Content<Response>(HttpStatusCode.OK, response);

        }

        [Route("api/product/{id}")]
        [HttpPut]
        public IHttpActionResult updateProduct([FromBody]CreateProductModel updateProductModel, Int16 id)
        {
            Response response = new Response();
            Product productcheck = Service.productDAO.getProduct(id);

            if (productcheck == null)
            {
                response.code = "404";
                response.status = "Sản phẩm cần cập nhật không có trong danh sách";
                return Content<Response>(HttpStatusCode.NotFound, response);

            }
            else
            if (productcheck.status == 1)
            {
                response.code = "409";
                response.status = "Không thể cập nhật sản phẩm khi bài đăng đã được công khai";
                return Content<Response>(HttpStatusCode.Conflict, response);
            }
            else
            {

                response.code = "200";
                response.status = "Cập nhật sản phẩm thành công";
                Int16[] listCategoryId = Service.productDAO.getProductCategoriesId(id);

                for (int i = 0; i < listCategoryId.Length; i++)
                {
                    
                    Int16 idCatPro = Service.categoryProductDAO.getProductCategoriesID(listCategoryId[i], id);
                    Service.categoryProductDAO.deleteCategoryProduct(idCatPro);

                }

                Int16 updateProductModelLength = (Int16)updateProductModel.categories.Length;
                for (Int16 i = 0; i < updateProductModelLength; i++)
                {
                    
                        if (Service.categoryDAO.checkExist(updateProductModel.categories[i]) != null)
                        {
                            CategoryProduct catepro = new CategoryProduct();
                            catepro.categoryId = updateProductModel.categories[i];
                            catepro.productId = id;
                            Service.categoryProductDAO.insertCategoryProduct(catepro);

                        }
                    

                }

                productcheck.name = updateProductModel.name;
                productcheck.shortDescription = updateProductModel.shortDescription;
                productcheck.longDescription = updateProductModel.longDescription;
                Service.productDAO.updateProduct(productcheck);

                return Content<Response>(HttpStatusCode.OK, response);
            }

        }

        [Route("api/product/{iddel}")]
        [HttpDelete]
        public IHttpActionResult deleteProduct([FromUri]short iddel)
        {
            Response response = new Response();
            Product productcheck = Service.productDAO.getProduct(iddel);
            if (productcheck == null)
            {
                response.code = "404";
                response.status = "không tồn tại sản phẩm cần xóa";
                return Content<Response>(HttpStatusCode.NotFound, response);
            }
            else
            {
                Product productDel = Service.productDAO.getProduct(iddel);
                productDel.status = -1;
                response.code = "200";
                response.status = "Xóa sản phẩm thành công";
                Service.productDAO.updateProduct(productDel);

                return Content<Response>(HttpStatusCode.NotFound, response);

            }

        }


        [Route("api/admin/products/")]
        [HttpGet]
        public async System.Threading.Tasks.Task<IHttpActionResult> getAdminProductslistAsync([FromUri] PageRequest pageRequest, [FromBody] Token token)
        {
            Response response = new Response();
            if(token.accessToken == null)
            {
                response.code = "400";
                response.status = "Bổ sung token";
                return Content<Response>(HttpStatusCode.OK, response);
            }
            var accessToken = token.accessToken;
            Token valiToken = Service.tokenDAO.getByAccessToken(accessToken);
            ApplicationUser user = await Service._userManager.FindByEmailAsync(valiToken.userName);
            PagedResult<Product> pv = Service.productDAO.AdminPageView(user.userInfoId, pageRequest.pageIndex, pageRequest.pageSize, pageRequest.order);
            response.code = "200";
            response.status = "Success";
            response.results = pv;
            return Content<Response>(HttpStatusCode.OK, response);
        }

        [Route("api/admin/product/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async System.Threading.Tasks.Task<IHttpActionResult> getAdminProductslistAsync([FromUri] Int16 id, [FromBody] Token token)
        {
            Response response = new Response();
            var accessToken = token.accessToken;
            if (accessToken == null)
            {
                response.code = "400";
                response.status = "Bổ sung token";
                return Content<Response>(HttpStatusCode.OK, response);
            }
            Token valiToken = Service.tokenDAO.getByAccessToken(accessToken);
            ApplicationUser user = await Service._userManager.FindByEmailAsync(valiToken.userName);
            Product product = Service.productDAO.getProduct(id);
            if(product.createdBy != user.userInfoId)
            {
                response.code = "400";
                response.status = "Sản phẩm không thuộc quyền sở hửu của bạn";
                response.results = "";
                return Content<Response>(HttpStatusCode.OK, response);
            }
            response.code = "200";
            response.status = "Lấy sản phẩm thành công";
            response.results = product;
            return Content<Response>(HttpStatusCode.OK, response);
        }

        [Route("api/category/{categoryId}/products/")]
        [HttpGet]
        public IHttpActionResult getProductByCategory(Int16 categoryId, [FromUri] PageRequest pageRequest)
        {
            Response response = new Response();

            PagedResult<Product> pageResults = Service.productDAO.pageViewByCategoryId(categoryId, pageRequest.pageIndex, pageRequest.pageSize);

            PagedResult<ProductDetail> pagedResultDetails = new PagedResult<ProductDetail>();
            pagedResultDetails.pageCount = pageResults.pageCount;
            pagedResultDetails.pageSize = pageResults.pageSize;
            pagedResultDetails.rowCount = pageResults.rowCount;
            pagedResultDetails.currentPage = pageResults.currentPage;
            List<ProductDetail> lists = new List<ProductDetail>();
            for (var i = 0; i < pageResults.rowCount; i++)
            {
                var item = pageResults.items[i];
                ProductDetail productDetail = new ProductDetail();
                productDetail.id = item.id;
                productDetail.code = item.code;
                productDetail.name = item.name;
                productDetail.status = item.status;
                productDetail.attributes = Service.productAttributeDAO.getProAttrsByProId(item.id);
                productDetail.choseCategories = Service.categoryProductDAO.getListCategoryProductByProdId(item.id).ToList();
                productDetail.thumbnails = Service.imageDAO.getThumbnail(item.id).ToList();
                lists.Add(productDetail);
            }

            pagedResultDetails.items = lists;
            response.code = "200";
            response.status = "Thành công";
            response.results = pagedResultDetails;
            return Content<Response>(HttpStatusCode.OK, response); ;
        }

        //HttpGet api/search?keyword=param
        [Route("api/search")]
        [HttpGet]
        public IHttpActionResult searchProduct([FromUri]Search search)
        {
            if(search.keyword != null)
            {
                PagedResult<Product> pagedResult = Service.productDAO.pageViewForSearch(search.keyword);
                List<ProductDetail> list = new List<ProductDetail>();
                for(var i = 0; i < pagedResult.items.Count; i++)
                {
                    ProductDetail product = new ProductDetail();
                    product.id = pagedResult.items[i].id;
                    product.name = pagedResult.items[i].name;
                    product.status = pagedResult.items[i].status;
                    product.choseCategories = Service.categoryProductDAO.getListCategoryProductByProdId(pagedResult.items[i].id).ToList();
                    list.Add(product);
                }
                Response res = new Response("200", "Thành công", list);
                return Content<Response>(HttpStatusCode.OK, res);
            }
            Response response = new Response("200", "Thành công", null);
            return Content<Response>(HttpStatusCode.OK, response);
        }
    }
}
