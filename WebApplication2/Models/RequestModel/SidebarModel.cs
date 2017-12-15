using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.RequestModel
{
    public class SidebarModel
    {
        public String name { get; set; }
        public String url { get; set; }
        public String icon { get; set; }
        public String actionName { get; set; }

        public SidebarModel()
        {

        }
        public SidebarModel(String name, String url, String icon, String actionName)
        {
            this.name = name;
            this.url = url;
            this.icon = icon;
            this.actionName = actionName;
        }
    }
}