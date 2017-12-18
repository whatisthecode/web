using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class CategoryDAOImpl : BaseImpl<Category, Int16>, CategoryDAO, IDisposable
    {
        public CategoryDAOImpl() : base()
        {
        }

        public void deleteCategory(Int16 CategoryId)
        {
            base.delete(CategoryId);
        }

        public IEnumerable<Category> getCategory()
        {
            return base.get();
        }

        public Category getCategoryById(Int16 categoryId)
        {
            Category category = base.getById(categoryId);
            return category;
        }
        public void insertCategory(Category category)
        {
            base.insert(category);
        }

        public void updateCategory(Category category)
        {
            category.updatedAt = DateTime.Now;
            base.update(category);
        }

        public void dispose()
        {
            base.Dispose();
        }

        public Category checkExist(Int16 categoryId)
        {
            return base.getById(categoryId);
        }

        //Testing
        public IEnumerable<Product> find(Int16 categoryId)
        {
            using (DBContext context = new DBContext())
            {
                List<Int16> cateProds = (from catePro in context.categoryProducts
                                         where categoryId == catePro.categoryId
                                         select catePro.productId).ToList<Int16>();
                var query = (from pro in context.products
                             where cateProds.Contains(pro.id)
                             select pro);

                List<Product> products = query.ToList();
                return products;
            }
        }

        public PagedResult<Category> PageView(int pageIndex, int pageSize, string columnName)
        {
            using (DBContext context = new DBContext())
            {
                var query = from c in context.categories select c;
                switch (columnName)
                {
                    case "name":
                        query = query.OrderBy(n => n.name);
                        break;
                }

                PagedResult<Category> pv = base.PageView(query, pageIndex, pageSize);
                return pv;
            }

        }
        public Boolean check(String column)
        {
            return base.checkColumnExists(column);
        }
        public PagedResult<Category> PageView(int pageIndex, int pageSize, string orderBy, Boolean ascending)
        {
            using (DBContext context = new DBContext())
            {
                var query = from c in context.categories select c;
                if (orderBy != null && ascending)
                {
                    switch (orderBy)
                    {
                        case "name":
                            query = query.OrderBy(n => n.name);
                            break;
                    }

                }

                if (orderBy != null && !ascending)
                {
                    switch (orderBy)
                    {
                        case "name":
                            query = query.OrderByDescending(d => d.name);
                            break;
                    }
                }

                PagedResult<Category> pv = base.PageView(query, pageIndex, pageSize);
                return pv;
            }
        }

        public IEnumerable<Category> getCategoryByTypeId(short id)
        {
            using (DBContext context = new DBContext())
            {
                return context.categories.Where(cate => cate.typeId.Equals(id)).ToList();
            }
        }
    }
}