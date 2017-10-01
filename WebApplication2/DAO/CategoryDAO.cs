using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface CategoryDAO
    {
        IEnumerable<Category> getCategories();
        Category getCategoryById(Int16 id);
        void insertCategory(Category category);
        void deleteCategory(Int16 id);
        void updateCategory(Category category);
        void saveCategory();
        void dispose();
    }
}