﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    interface CategoryProductDAO
    {
        CategoryProduct getCategoryProduct(Int16 idcatepro);
        IEnumerable<CategoryProduct> getListCategoryProduct();
        void insertCategoryProduct(CategoryProduct catepro);
        void updateCategoryProduct(CategoryProduct catepro);
        void deleteCategoryProduct(Int16 idcatepro);
        void save();
        void dispose();

    }
}