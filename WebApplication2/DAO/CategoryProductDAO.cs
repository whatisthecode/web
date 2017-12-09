using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface CategoryProductDAO
    {
        Int16 getProductCategoriesID(short idCategory, short idProduct);
        CategoryProduct getCategoryProduct(Int16 idcatepro);
        IEnumerable<CategoryProduct> getListCategoryProduct();
        void insertCategoryProduct(CategoryProduct catepro);
        void updateCategoryProduct(CategoryProduct catepro);
        void deleteCategoryProduct(Int16 idcatepro);
        void save();
        void dispose();
        PagedResult<Product> pageView(Int16 categoryId, Int16 pageindex, Int16 pagesize, string orderBy, bool ascending);
    }
}
