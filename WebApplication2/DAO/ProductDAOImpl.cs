using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LaptopWebsite.Models.Mapping;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class ProductDAOImpl : BaseImpl<Product, Int16>, ProductDAO, IDisposable
    {
        public ProductDAOImpl():base()
        {
        }

        

        public void deleteProduct(Int16 idproduct)
        {
            base.delete(idproduct);
        }

        public void dispose()
        {
            base.Dispose();
        }

        public Product getProduct(Int16 id)
        {
            return base.getById(id);
        }

        public IEnumerable<Product> getProducts()
        {

            return base.get().Where(p => p.status >= 0).ToList();
        }
        public Product checkexist(string code)
        {
            Product product2compare = base.context.products.Where(p=> p.code == code).FirstOrDefault();
            if (product2compare != null)
            {
                return product2compare;
            }
            return null;
                
        }
        public void insertProduct(Product product)
        {
            base.insert(product);
        }
        
        public PagedResult<Product> PageView(Int16 indexnum, Int16 pagesize, String Orderby)
        {
            var query = from c in base.getContext().products select c;
            switch (Orderby)
            {
                case "name":
                    query = query.OrderBy(n => n.name);
                    break;

            }
            PagedResult<Product> pv = base.PageView(query,indexnum, pagesize);
            return pv;
            
        }

        public PagedResult<Product> PageView(short indexnum, short pagesize, string Orderby, bool ascending = false)
        {
            var query = from c in base.getContext().products select c;
            query.Where(p => p.status >= 0);
            if (query != null && ascending)
            {
                switch (Orderby)
                {
                    case "name":
                        query = query.OrderBy(n => n.name);
                        break;

                }
            }
            if (query != null && !ascending)
            {
                switch (Orderby)
                {
                    case "name":
                            query = query.OrderByDescending(d => d.name);
                        break;
                }
            }
            PagedResult<Product> pv = base.PageView(query, indexnum, pagesize);
            return pv;
        }

        public void updateProduct(Product product)
        {
            product.updatedAt = DateTime.Now;
            base.update(product);
        }

        public void checkProductCode(Product Product)
        {
            throw new NotImplementedException();
        }

        public void saveProduct()
        {
            base.save();
        }
        public Int16[] getProductCategoriesId(short idProduct)
        {
            var query = from c in base.context.categoryProducts
                        where c.productId == idProduct
                        select c.categoryId;
            return query.ToArray();
        }
     
        /* public void checkProductCode(Product product)
         {*
             var query = from p in base.getContext().products select p;
             query = query.Where(p => p.code == product.code);
             Product pro = base.checkColumnExists(query);  

         }*/
    }
}