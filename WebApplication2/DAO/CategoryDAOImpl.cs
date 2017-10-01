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

        public void deleteCategory(Int16 id)
        {
            base.delete(id);
        }

        public Category getCategoryById(Int16 id)
        {
            return base.getById(id);
        }

        public IEnumerable<Category> getCategories()
        {
            return base.get();
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
    }
}