using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.DAO
{
    public class CategoryProductDAOImpl : BaseImpl<CategoryProduct, Int16>, CategoryProductDAO, IDisposable
    {
        protected ProductDAO productDAO;

        public CategoryProductDAOImpl():base()
        {
            productDAO = new ProductDAOImpl();
        }
        public Int16 getProductCategoriesID(short idCategory, short idProduct)
        {
            using (DBContext context = new DBContext())
            {
                var query = from c in context.categoryProducts
                            where c.productId == idProduct && c.categoryId == idCategory
                            select c.id;
                return query.FirstOrDefault();
            }
        }

        public void deleteCategoryProduct(short idcatepro)
        {
            base.delete(idcatepro);
        }

        public void dispose()
        {
            base.Dispose();
        }

        public CategoryProduct getCategoryProduct(short idcatepro)
        {
            return base.getById(idcatepro);
        }

        public IEnumerable<CategoryProduct> getListCategoryProduct()
        {
            return base.get();
        }

        public void insertCategoryProduct(CategoryProduct catepro)
        { 
            base.insert(catepro);
        }

        public void updateCategoryProduct(CategoryProduct catepro)
        {
            catepro.updatedAt = DateTime.Now;
            base.update(catepro);
        }
        public IEnumerable<CategoryProduct> getListCategoryProductByProdId(Int16 prodId)
        {
            return base.get().Where(cp => cp.productId == prodId).ToList();
        }
        PagedResult<Product> CategoryProductDAO.pageView(short categoryId, short pageindex, short pagesize, string orderBy, bool ascending)
        {
            PagedResult<CategoryProduct> pv = new PagedResult<CategoryProduct>();
            using (DBContext context = new DBContext())
            {
                var query = from c in context.categoryProducts select c;
                query = query.Where(cate => cate.categoryId == categoryId);
                query = query.OrderBy(o => o.productId);
                pv = base.PageView(query, pageindex, pagesize);
            }          
            List<Product> listProducts = new List<Product>();
            PagedResult<Product> pvProduct = new PagedResult<Product>();
            pvProduct.rowCount = pv.rowCount;
            pvProduct.pageSize = pv.pageSize;
            pvProduct.currentPage = pv.currentPage;
            pvProduct.pageCount = pv.pageCount;
            for(var i = 0; i < pv.items.Count(); i++)
            {
                CategoryProduct categoryProduct = pv.items[i];
                Product product = productDAO.getProduct(categoryProduct.productId);
                List<ProductAttribute> proAttrs = Service.productAttributeDAO.getProAttrsByProId(product.id);
                product.attributes = proAttrs;
                listProducts.Add(product);
            }
            for (var i = 0; i < listProducts.Count() - 1; i++)
            {
                List<ProductAttribute> proAttrs = listProducts[i].attributes.ToList();
                for(var j = 1; j < listProducts.Count(); j++)
                {
                    List<ProductAttribute> proAttrs2 = listProducts[j].attributes.ToList();
                    switch (orderBy)
                    {
                        case "price":
                            if (ascending)
                            {
                                if(proAttrs[0].key == "price" && proAttrs2[0].key == "price")
                                {
                                    if (int.Parse(proAttrs[0].value) > int.Parse(proAttrs2[0].value))
                                    {
                                        listProducts = Utils.swap(listProducts, i, j);
                                    }
                                }
                            }
                            else
                            {
                                if (proAttrs[0].key == "price" && proAttrs2[0].key == "price")
                                {
                                    if (int.Parse(proAttrs[0].value) < int.Parse(proAttrs2[0].value))
                                    {
                                        listProducts = Utils.swap(listProducts, i, j);
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                
            }
            
            pvProduct.items = listProducts;
            return pvProduct;
        }
    }
}