using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class ProductAttributeDAOImpl : BaseImpl<ProductAttribute, Int16>, ProductAttributeDAO, IDisposable
    {
        public ProductAttributeDAOImpl(): base() 
        {

        }

        public void deleteProductAttribute(short id)
        {
            base.delete(id);
        }

        public void dispose()
        {
            base.Dispose();
        }

        public IEnumerable<ProductAttribute> getProductAttributeAll()
        {
            return base.get();
        }

        public ProductAttribute getProductAttributeById(Int16 id)
        {
            return base.getById(id);
        }

        public void insertProductAttribute(ProductAttribute proat)
        {
            base.insert(proat);
        }
        public void updateProductAttribute(ProductAttribute proat)
        {
            proat.updatedAt = DateTime.Now;
            base.update(proat);
        }

        public List<ProductAttribute> getProAttrsByProId(short proId)
        {
            using (DBContext context = new DBContext())
            {
                if (proId > 0)
                {
                    var query = from p in context.productAttributes
                                where p.productId == proId
                                select p;
                    return query.ToList();
                }
                return null;
            }
        }
    }
}