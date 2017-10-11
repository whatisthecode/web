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
            return base.getById(categoryId);
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
            base.update(category);
        }

        public void dispose()
        {
            base.Dispose();
        }
        public Category checkExist(Category category)
        {
            Category category1 = base.Dbset().Where(c => c.code == category.code || c.name == category.name).FirstOrDefault();
            if (category1 != null ){
                return category1;
            }
            base.detach(category);
            return null;
        }
        public PagedResult<Category> PageView(int pageIndex, int pageSize, string columnName)
        {
            var query = from c in base.Dbset() select c;
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
            var query = from c in base.Dbset() select c;
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