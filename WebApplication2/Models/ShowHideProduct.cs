using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ShowHideProduct: Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int16 id { get; set; }

        [Column("start_date", TypeName = "datetime2")]
        public DateTime startDate { get; set; }
        [Column("end_date", TypeName = "datetime2")]
        public DateTime endDate { get; set; }

        [Column("date_of_week")]
        public DayOfWeek dayOfWeek { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime fromTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime toTime { get; set; }

        [ForeignKey("Product")]
        public Int16 productId { get; set; }

        public Product Product { get; set; }

        public Int16 show { get; set; }

        public ShowHideProduct()
        {

        }

        public static implicit operator JObject(ShowHideProduct v)
        {
            throw new NotImplementedException();
        }
    }
}