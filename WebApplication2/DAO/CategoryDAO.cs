﻿using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface CategoryDAO
    {

        IEnumerable<Category> getCategory();
        Category getCategoryById(Int16 categoryId);
        Category checkExist(Int16 id);
        IEnumerable<Product> find(Int16 categoryId);
        void insertCategory(Category category);
        IEnumerable<Category> getCategoryByTypeId(short id);
        void deleteCategory(Int16 CategoryId);
        void updateCategory(Category category);
        Boolean check(String column);
        PagedResult<Category> PageView(int pageIndex, int pageSize, string columnName);
        void dispose();
        Category getCategoryByCode(string code);
    }
}