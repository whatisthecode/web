using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.RequestModel
{
    public class CreateCategoryModel
    {
        public Int16 id { get; private set; }

        public String code { get; set; }

        public String name { get; set; }

        public Int16 typeId { get; set; }

        public Int16? parentId { get; set; }

        public CreateCategoryModel(string code, string name)
        {
            this.code = code;
            this.name = name;
        }
        public CreateCategoryModel()
        {

        }
        public Category toCategory()
        {
            Category entity = new Category();
            entity.code = this.code;
            entity.name = this.name;
            entity.typeId = this.typeId;
            entity.parentId = this.parentId;
            return entity;
        }
    }
}