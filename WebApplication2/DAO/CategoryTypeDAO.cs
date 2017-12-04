using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    interface CategoryTypeDAO
    {
        IEnumerable<CategoryType> getCategoryType();
        CategoryType getCategoryTypeById(Int16 categoryTypeId);
        CategoryType getCategoryTypeByCode(String code);
        CategoryType checkExist(CategoryType categoryType);
        void insertCategoryType(CategoryType categoryType);

        void deleteCategoryType(Int16 categoryTypeId);
        void updateCategoryType(CategoryType categoryType);
        void saveCategoryType();
        Boolean check(String column);
        PagedResult<CategoryType> PageView(int pageIndex, int pageSize, string columnName);
        void dispose();
    }
}
