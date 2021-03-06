﻿using LaptopWebsite.Models.Mapping;
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
        IEnumerable<CategoryProduct> getListCategoryProductByProdId(Int16 prodId);
        void insertCategoryProduct(CategoryProduct catepro);
        void updateCategoryProduct(CategoryProduct catepro);
        void deleteCategoryProduct(Int16 idcatepro);
        void dispose();
    }
}
