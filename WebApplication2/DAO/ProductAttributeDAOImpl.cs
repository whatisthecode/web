using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class ProductAttributeDAOImpl : BaseImpl<ProductAttribute, Int16>,ProductAttributeDAO, IDisposable
    {
        public void deleteProductAttribute(short id)
        {
            throw new NotImplementedException();
        }

        public void dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductAttribute> getProductAttributeAll()
        {
            throw new NotImplementedException();
        }

        public Product getProductAttributeById()
        {
            throw new NotImplementedException();
        }

        public void insertProductAttribute(ProductAttribute proat)
        {

        }

        public void updateProductAttribute(ProductAttribute proat, short id)
        {
            throw new NotImplementedException();
        }
    }
}