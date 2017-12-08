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
        public Int16 id { get; set; }

        [Column("saler_id")]
        public Int16 salerId { get; set; }

        [Column("buyer_id")]
        public Int16 buyerId { get; set; }

        public Int16 status { get; set; }

        public String code { get; set; }

        public Double total { get; set; }

        [ForeignKey("salerId")]
        public virtual UserInfo saler { get; set; }

        [ForeignKey("buyerId")]
        public virtual UserInfo buyer { get; set; }

        public Invoice(string code, Int16 buyerId, Int16 salerId, Double total)
        {
            this.code = code;
            this.buyerId = buyerId;
            this.salerId = salerId;
            this.total = total;
            this.status = 0;
        }
        public Invoice()
        {

        }
    }
}