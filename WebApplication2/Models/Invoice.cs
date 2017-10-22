using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Invoice : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 status { get; set; }
        public Int16 id { get; set; }
        public String code { get; set; }
        [ForeignKey("User")]
        public Int16 buyer { get; set; }
        public Double total { get; set; }
        public User User { get; set; }
        public Invoice(string code, Int16 buyer, Double total)
        {
            this.code = code;
            this.buyer = buyer;
            this.total = total;
            this.status = 0;
        }
        public Invoice()
        {

        }
    }
}