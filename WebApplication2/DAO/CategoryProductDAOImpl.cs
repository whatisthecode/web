using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class CategoryProductDAOImpl : BaseImpl<CategoryProduct, Int16>, CategoryProductDAO, IDisposable
    {
        protected ProductDAO productDAO;

        public CategoryProductDAOImpl():base()
        {
            productDAO = new ProductDAOImpl();
        }
        public Int16 getProductCategoriesID(short idCategory, short idProduct)
        {
            var query = from c in base.getContext().categoryProducts
                        where c.productId == idProduct && c.categoryId == idCategory
                        select c.id;
            return query.FirstOrDefault();
        }

        public void deleteCategoryProduct(short idcatepro)
        {
            base.delete(idcatepro);
        }

        public void dispose()
        {
            base.Dispose();
        }

        public CategoryProduct getCategoryProduct(short idcatepro)
        {
            return base.getById(idcatepro);
        }

        public IEnumerable<CategoryProduct> getListCategoryProduct()
        {
            return base.get();
        }

        public void insertCategoryProduct(CategoryProduct catepro)
        { 
            base.insert(catepro);
        }

        public void updateCategoryProduct(CategoryProduct catepro)
        {
            catepro.updatedAt = DateTime.Now;
            base.update(catepro);
        }

        PagedResult<Product> CategoryProductDAO.pageView(short categoryId, short pageindex, short pagesize, string orderBy, bool ascending)
        {
            var query = from c in base.getContext().categoryProducts select c;
            query = query.Where(cate => cate.categoryId == categoryId);
            query = query.OrderBy(o => o.productId);
            PagedResult<CategoryProduct> pv = base.PageView(query, pageindex, pagesize);
            List<Product> listProducts = new List<Product>();
            PagedResult<Product> pvProduct = new PagedResult<Product>();
            pvProduct.rowCount = pv.rowCount;
            pvProduct.pageSize = pv.pageSize;
            pvProduct.currentPage = pv.currentPage;
            pvProduct.pageCount = pv.pageCount;
            for(var i = 0; i < pv.results.Count(); i++)
            {
                CategoryProduct categoryProduct = pv.results[i];
                Product product = productDAO.getProduct(categoryProduct.productId);
                listProducts.Add(product);
            }
            pvProduct.results = listProducts;
            return pvProduct;
        }
    }
}