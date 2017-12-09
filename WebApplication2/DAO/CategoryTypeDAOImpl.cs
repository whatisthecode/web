using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LaptopWebsite.Models.Mapping;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class CategoryTypeDAOImpl : BaseImpl<CategoryType, Int16>, CategoryTypeDAO, IDisposable
    {
        public CategoryTypeDAOImpl() : base()
        {

        }
        public bool check(string column)
        {
            throw new NotImplementedException();
        }

        public CategoryType checkExist(CategoryType categoryType)
        {
            throw new NotImplementedException();
        }

        public void deleteCategoryType(short categoryTypeId)
        {
            base.delete(categoryTypeId);
        }


        public void dispose()
        {
            base.Dispose();
        }

        public IEnumerable<CategoryType> getCategoryType()
        {
            return base.get();
        }

        public CategoryType getCategoryTypeById(short categoryTypeId)
        {
            CategoryType categoryType = base.getById(categoryTypeId);
            return categoryType;
        }

        public CategoryType getCategoryTypeByCode(String code)
        {
            CategoryType categoryType = base.getContext().categoryTypes.Where(ct => ct.code == code).FirstOrDefault();
            if(categoryType != null)
            {
                return categoryType;
            }
            return null;
        }

        public void insertCategoryType(CategoryType categoryType)
        {
            base.insert(categoryType);
        }

        public PagedResult<CategoryType> PageView(int pageIndex, int pageSize, string columnName)
        {
            throw new NotImplementedException();
        }

        public void saveCategoryType()
        {
            base.save();
        }

        public void updateCategoryType(CategoryType categoryType)
        {
            categoryType.updatedAt = DateTime.Now;
        }
    }
}