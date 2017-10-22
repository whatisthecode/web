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
            return base.get();
        }
        public Product checkexist(Product product)
        {
<<<<<<< HEAD
            Product product2compare = base.Dbset().Where(p=> p.code == product.code ).FirstOrDefault();
=======
            Product product2compare = base.getContext().products.Where(p=> p.code == product.code || p.name== product.name || p.shortDescription==product.longDescription).FirstOrDefault();
>>>>>>> ed6761a2fa49829da336113a63180e073dbc8faf
            if (product2compare != null)
            {
                return product;
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

        public PagedResult<Product> PageView(short indexnum, short pagesize, string Orderby, bool ascending)
        {
            var query = from c in base.getContext().products select c;
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
            base.update(product);
        }
    }
}