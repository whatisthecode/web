using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ApplicationUserGroup : Base
    {
        [Key, Column(Order = 0)]
        public string userId { get; set; }
        [Key, Column(Order = 1)]
        public Int16 groupId { get; set; }

        public virtual ApplicationUser user { get; set; }
        public virtual Group group { get; set; }
    }
}