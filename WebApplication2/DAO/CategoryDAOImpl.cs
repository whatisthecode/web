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
        protected ProductDAO productDAO;
        public CategoryDAOImpl() : base()
        {
            productDAO = new ProductDAOImpl();
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
            base.context.Entry<Category>(category).Collection(c => c.subCategories).Load();
            return category;
        }
        public void insertCategory(Category category)
        {
            base.insert(category);
        }

        public void saveCategory()
        {
            base.save();
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

        public Category checkExist(Category category)
        {
            var query = from q in base.getContext().categories select q;
            query = query.Where(q => q.code == category.code);
            Category cate = query.FirstOrDefault();
            return cate;
        }

        //Testing
        public IEnumerable<Product> find(Int16 categoryId)
        {
            List<Int16> cateProds = (from catePro in base.getContext().categoryProducts
                         where categoryId == catePro.categoryId
                         select catePro.productId).ToList<Int16>();

            var query = (from pro in base.getContext().products
                         where cateProds.Contains(pro.id)
                         select pro);

            List < Product > products = query.ToList();
            return products;
        }

        public PagedResult<Category> PageView(int pageIndex, int pageSize, string columnName)
        {
            var query = from c in base.getContext().categories select c;
            switch (columnName)
            {
                case "name":
                    query = query.OrderBy(n => n.name);
                    break;
            }

            PagedResult<Category> pv = base.PageView(query, pageIndex, pageSize);
            return pv;
        }
        public Boolean check(String column)
        {
            return base.checkColumnExists(column);
        }
        public PagedResult<Category> PageView(int pageIndex, int pageSize, string orderBy, Boolean ascending)
        {
            var query = from c in base.getContext().categories select c;
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
}