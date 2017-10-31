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
        Product getProductAttributeById();
        IEnumerable<ProductAttribute> getProductAttributeAll();
        void insertProductAttribute(ProductAttribute proat);
        void updateProductAttribute(ProductAttribute proat, Int16 id);
        void deleteProductAttribute(Int16 id);



    }
}
