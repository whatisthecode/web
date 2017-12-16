using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.Mapping
{
    public class CategoryViewModels
    {
        public class CategoryViewMenu
        {
            public CategoryType categoryType { get; set; }
            public List<Category> Categories { get; set; }
        }
    }
}