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
        Product getProduct(Int16 id);
        void updateProduct(Product product);
        void insertProduct(Product product);
        void deleteProduct(Int16 id);
        void save();
        
        void dispose();
        Product checkexist(Product product);
        PagedResult<Product> PageView(Int16 indexnum, Int16 pagesize, String Orderby);
        PagedResult<Product> PageView(Int16 indexnum, Int16 pagesize, String Orderby, Boolean ascending);
    }
}