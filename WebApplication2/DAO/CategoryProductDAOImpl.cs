using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;
using WebApplication2.Services;

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
        public IEnumerable<CategoryProduct> getListCategoryProductByProdId(Int16 prodId)
        {
            return base.get().Where(cp => cp.productId == prodId).ToList();
        }
    }
}