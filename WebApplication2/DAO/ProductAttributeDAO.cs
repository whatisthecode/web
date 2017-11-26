using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    interface ProductAttributeDAO
    {
        ProductAttribute getProductAttributeById(Int16 id);
        IEnumerable<ProductAttribute> getProductAttributeAll();
        void insertProductAttribute(ProductAttribute proat);
        void updateProductAttribute(ProductAttribute proat);
        void deleteProductAttribute(Int16 id);
        void saveProductAttribute();
        void dispose();

    }
}
