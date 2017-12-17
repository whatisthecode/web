using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface ProductDAO
    {
        IEnumerable<Product> getProducts();
        Int16[] getProductCategoriesId(short idProduct);
        
        Product getProduct(Int16 id);
        void updateProduct(Product product);
        void insertProduct(Product product);
        void deleteProduct(Int16 id);
        void checkProductCode(Product Product);
        void dispose();
        Product checkexist(string code);
        PagedResult<Product> PageView(Int16 indexnum, Int16 pagesize, String Orderby);
        PagedResult<Product> PageView(Int16 indexnum, Int16 pagesize, String Orderby, Boolean ascending);
        PagedResult<Product> AdminPageView(Int16 userId, Int16 indexnum, Int16 pagesize, String Orderby);
        PagedResult<Product> pageViewByCategoryId(short categoryId, short pageindex, short pagesize);
    }
}