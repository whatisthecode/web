using LaptopWebsite.Models.Mapping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;
using WebApplication2.Models.RequestModel;
using WebApplication2.Services;
using static WebApplication2.Models.RequestModel.FromUri;

namespace WebApplication2.Controllers.API
{
    [Authorize]
    public class ProductController : ApiController
    {

        public ProductController()
        {

        }

        [Route("api/product")]
        [HttpPost]
        [Authorize(Roles = "CREATE_PRODUCT")]
        [AllowAnonymous]
        public IHttpActionResult insertNewProduct([FromBody]CreateProductModel createProductModel)
        {
            Response response = Utils.checkInput(createProductModel, CreateProductModel.required);
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
                    Service.productDAO.saveProduct();

                    //insert data to category product
                    Product newProduct = Service.productDAO.checkexist(product.code);
                    for (int i = 0; i < createProductModel.categories.Length; i++)
                    {
                        Category category = new Category();
                        category.id = createProductModel.categories[i];
                        if (Service.categoryDAO.checkExist(category) != null)
                        {
                            CategoryProduct catepro = new CategoryProduct();
                            catepro.categoryId = createProductModel.categories[i];
                            catepro.productId = newProduct.id;
                            Service.categoryProductDAO.insertCategoryProduct(catepro);
                            Service.categoryProductDAO.save();
                        }
                    }
                    foreach (var attribute in createProductModel.attributes)
                    {
                        ProductAttribute productAttribute = new ProductAttribute(product.id, attribute.Key, attribute.Value.ToString());
                        Service.productAttributeDAO.insertProductAttribute(productAttribute);
                        Service.productAttributeDAO.saveProductAttribute();
                    }

                    response.code = "201";
                    response.results = "Thêm sản phẩm thành công";
                    return Content<Response>(HttpStatusCode.Created, response);
                }

            }

            return Content<Response>(HttpStatusCode.OK, response);
        }

        [Route("api/product/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult getProductWithConditions(short id)
        {
            Response response = new Response();
            Product productflag = Service.productDAO.getProduct(id);
            if (productflag == null)
            {
                response.code = "404";
                response.status = "Sản phẩm cần tìm không có trong danh sách";
                return Content<Response>(HttpStatusCode.NotFound, response);
            }
            else
            {
                response.code = "200";
                response.status = "Sản phẩm cần tìm là: ";
                response.results = productflag;
                return Content<Response>(HttpStatusCode.OK, response);
            }

        }

        [Route("api/products/")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult getProductslist([FromUri] PageRequest pageRequest)
        {
            Response response = new Response();

            PagedResult<Product> pagedResult = Service.productDAO.PageView(pageRequest.pageIndex, pageRequest.pageSize, pageRequest.order, false);
            IList<Product> products = pagedResult.results;
            for (var i = 0; i < pagedResult.pageSize; i++)
            {
                Product product = products[i];
                List<ProductAttribute> productAtts = Service.productAttributeDAO.getProAttrsByProId(product.id);
                //UserInfo userInfo = Service.userInfoDAO.getUserInfo(product.createdBy);
                products[i].attributes = productAtts;
                //products[i].UserInfo = userInfo;
            }
            response.code = "200";
            response.status = "Danh sách sản phẩm hiện tại: ";
            response.results = pagedResult;
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
                    Service.categoryProductDAO.save();
                }

                Int16 updateProductModelLength = (Int16)updateProductModel.categories.Length;
                for (Int16 i = 0; i < updateProductModelLength; i++)
                {
                    
                        Category category = new Category();
                        category.id = updateProductModel.categories[i];
                        if (Service.categoryDAO.checkExist(category) != null)
                        {
                            CategoryProduct catepro = new CategoryProduct();
                            catepro.categoryId = updateProductModel.categories[i];
                            catepro.productId = id;
                            Service.categoryProductDAO.insertCategoryProduct(catepro);
                            Service.categoryProductDAO.save();
                        }
                    

                }
                Service.categoryProductDAO.save();

                productcheck.name = updateProductModel.name;
                productcheck.shortDescription = updateProductModel.shortDescription;
                productcheck.longDescription = updateProductModel.longDescription;
                Service.productDAO.updateProduct(productcheck);
                Service.productDAO.saveProduct();
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
                Service.productDAO.saveProduct();
                return Content<Response>(HttpStatusCode.NotFound, response);

            }

        }

    }
}
