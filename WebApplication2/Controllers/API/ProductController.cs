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
        private ProductDAO productDao;
        private ProductAttributeDAO productAttributeDAO;
        private UserInfoDAO userInfoDAO;
        private CategoryDAO categoryDAO;
        private CategoryProductDAO categoryProductDAO;

        public ProductController()
        {
            this.productDao = new ProductDAOImpl();
            this.productAttributeDAO = new ProductAttributeDAOImpl();
            this.userInfoDAO = new UserInfoDAOImpl();
            this.categoryDAO = new CategoryDAOImpl();
            this.categoryProductDAO = new CategoryProductDAOImpl();
        }

        [Route("api/product")]
        [HttpPost]
        [Authorize(Roles = "CREATE_PRODUCT")]
        public IHttpActionResult insertNewProduct([FromBody]CreateProductModel createProductModel)
        {
            Response response = Utils.checkInput(createProductModel, CreateProductModel.required);
            if (response.code != "422")
            {
                Product productcheck = this.productDao.checkexist(createProductModel.code);
                UserInfo user = this.userInfoDAO.getUserInfo(createProductModel.createdBy);
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
                    this.productDao.insertProduct(product);
                    this.productDao.saveProduct();

                    //insert data to category product
                    Product newProduct = productDao.checkexist(product.code);
                    for (int i = 0; i < createProductModel.categories.Length; i++)
                    {
                        Category category = new Category();
                        category.id = createProductModel.categories[i];
                        if (this.categoryDAO.checkExist(category) != null)
                        {
                            CategoryProduct catepro = new CategoryProduct();
                            catepro.categoryId = createProductModel.categories[i];
                            catepro.productId = newProduct.id;
                            this.categoryProductDAO.insertCategoryProduct(catepro);
                            this.categoryProductDAO.save();
                        }
                    }
                    foreach (var attribute in createProductModel.attributes)
                    {
                        ProductAttribute productAttribute = new ProductAttribute(product.id, attribute.Key, attribute.Value.ToString());
                        this.productAttributeDAO.insertProductAttribute(productAttribute);
                        this.productAttributeDAO.saveProductAttribute();
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
        public IHttpActionResult getProductWithConditions(short id)
        {
            Response response = new Response();
            Product productflag = this.productDao.getProduct(id);
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
            PagedResult<Product> pagedResult = this.productDao.PageView(pageRequest.pageIndex, pageRequest.pageSize, pageRequest.order, false);
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
            Product productcheck = this.productDao.getProduct(id);

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
                Int16[] listCategoryId = productDao.getProductCategoriesId(id);

                for (int i = 0; i < listCategoryId.Length; i++)
                {
                    
                    Int16 idCatPro = categoryProductDAO.getProductCategoriesID(listCategoryId[i], id);
                    categoryProductDAO.deleteCategoryProduct(idCatPro);
                    categoryProductDAO.save();
                }

                Int16 updateProductModelLength = (Int16)updateProductModel.categories.Length;
                for (Int16 i = 0; i < updateProductModelLength; i++)
                {
                    
                        Category category = new Category();
                        category.id = updateProductModel.categories[i];
                        if (this.categoryDAO.checkExist(category) != null)
                        {
                            CategoryProduct catepro = new CategoryProduct();
                            catepro.categoryId = updateProductModel.categories[i];
                            catepro.productId = id;
                            this.categoryProductDAO.insertCategoryProduct(catepro);
                            this.categoryProductDAO.save();
                        }
                    

                }
                categoryProductDAO.save();

                productcheck.name = updateProductModel.name;
                productcheck.shortDescription = updateProductModel.shortDescription;
                productcheck.longDescription = updateProductModel.longDescription;
                this.productDao.updateProduct(productcheck);
                this.productDao.saveProduct();
                return Content<Response>(HttpStatusCode.OK, response);
            }

        }

        [Route("api/product/{iddel}")]
        [HttpDelete]
        public IHttpActionResult deleteProduct([FromUri]short iddel)
        {
            Response response = new Response();
            Product productcheck = this.productDao.getProduct(iddel);
            if (productcheck == null)
            {
                response.code = "404";
                response.status = "không tồn tại sản phẩm cần xóa";
                return Content<Response>(HttpStatusCode.NotFound, response);
            }
            else
            {
                Product productDel = this.productDao.getProduct(iddel);
                productDel.status = -1;
                response.code = "200";
                response.status = "Xóa sản phẩm thành công";
                this.productDao.updateProduct(productDel);
                this.productDao.saveProduct();
                return Content<Response>(HttpStatusCode.NotFound, response);

            }

        }

    }
}
