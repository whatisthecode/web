using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LaptopWebsite.Models.Mapping;
using WebApplication2.Models;
using System.Data.Entity;

namespace WebApplication2.DAO
{
    public class ProductDAOImpl : BaseImpl<Product, Int16>, ProductDAO, IDisposable
    {
        public ProductDAOImpl() : base()
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

            IList<Product> products = base.get().Where(p => p.status >= 0).ToList();
            Int16 productsLength = (Int16)products.Count;
            for (Int16 productIndex = 0; productIndex < productsLength; productIndex++)
            {
                products[productIndex].UserInfo = Service.userInfoDAO.getUserInfo(products[productIndex].id);
                using (DBContext context = new DBContext())
                {
                    context.Entry<Product>(products[productIndex]).Collection(p => p.attributes).Load();
                }
            }
            return products;
        }
        public Product checkexist(string code)
        {
            Product product2compare = null;
            using (DBContext context = new DBContext())
            {
                product2compare = context.products.Where(p => p.code == code).FirstOrDefault();
            }
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
            using (DBContext context = new DBContext())
            {
                var query = from c in context.products select c;
                switch (Orderby)
                {
                    case "name":
                        query = query.OrderBy(n => n.name);
                        break;

                }
                PagedResult<Product> pv = base.PageView(query, indexnum, pagesize);
                return pv;
            }
        }

        public PagedResult<Product> PageView(short indexnum, short pagesize, string Orderby, bool ascending = false)
        {
            using (DBContext context = new DBContext())
            {
                var query = from c in context.products select c;
                query.Where(p => p.status > 0);
                query = query.OrderBy(n => n.name);
                PagedResult<Product> pv = base.PageView(query, indexnum, pagesize);
                return pv;
            }
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

        public Int16[] getProductCategoriesId(short idProduct)
        {
            using (DBContext context = new DBContext())
            {
                var query = from c in context.categoryProducts
                            where c.productId == idProduct
                            select c.categoryId;
                return query.ToArray();
            }
        }

        /* public void checkProductCode(Product product)
         {*
             var query = from p in base.getContext().products select p;
             query = query.Where(p => p.code == product.code);
             Product pro = base.checkColumnExists(query);  

         }*/

        public PagedResult<Product> AdminPageView(Int16 userId, Int16 indexnum, Int16 pagesize, String Orderby)
        {
            using (DBContext context = new DBContext())
            {
                var query = from c in context.products select c;
                query = query.Where(u => u.createdBy == userId);
                switch (Orderby)
                {
                    case "name":
                        query = query.OrderBy(n => n.name);
                        break;

                }
                PagedResult<Product> pv = base.PageView(query, indexnum, pagesize);
                return pv;
            }
        }

        PagedResult<Product> ProductDAO.pageViewByCategoryId(short categoryId, short pageindex, short pagesize)
        {
            using (DBContext context = new DBContext())
            {
                var query = from c in context.categoryProducts
                            where c.categoryId == categoryId
                            join pro in context.products on c.productId equals pro.id
                            where pro.status > 0
                            orderby pro.name
                            select pro;

                PagedResult<Product> pv = base.PageView(query, pageindex, pagesize);
                return pv;
            }
        }
    }
}