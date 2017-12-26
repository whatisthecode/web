using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.RequestModel
{
    public class ShowHideProductViewModel
    {
        public class ModelCreate
        {
            [Column(TypeName = "datetime2")]
            public DateTime fromDate { get; set; }

            [Column(TypeName = "datetime2")]
            public DateTime endDate { get; set; }

            public List<DayOfWeek> DaysOfWeek { get; set; }

            public List<Times> times { get; set; }

            public List<Int16> productId { get; set; }

        }

        public class Times
        {
            [DataType(DataType.Time)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
            public DateTime fromTime { get; set; }

            [DataType(DataType.Time)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
            public DateTime toTime { get; set; }

            public Int16 show { get; set; }
        }
    }
}