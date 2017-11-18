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
        public CategoryProductDAOImpl():base()
        {
            
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
            base.update(catepro);
        }
        PagedResult<CategoryProduct> pageView(Int16 pageindex, Int16 pagesize, string orderBy, Boolean ascending)
        {
            var query = from c in base.getContext().categoryProducts select c;
            if (query != null && ascending)
            {
                switch (orderBy)
                {
                    case "id":
                        query = query.OrderBy(c => c.categoryId);
                        break;
                }
            }
            if (query != null && !ascending)
            {
                switch (orderBy)
                {
                    case "id":
                        query = query.OrderByDescending(c => c.categoryId);
                        break;

                }

            }
            PagedResult<CategoryProduct> pv = base.PageView(query, pageindex, pagesize);
            return pv;

        }
    }
}