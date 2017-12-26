using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface ShowHideDAO
    {
        ShowHideProduct getShowHideProduct(Int16 id);
        IEnumerable<ShowHideProduct> getShowHideProducts();
        void insertShowHideProduct(ShowHideProduct showHideProduct);
        void updateShowHideProduct(ShowHideProduct showHideProduct);
        void deleteShowHideProduct(Int16 id);
        IEnumerable<ShowHideProduct> findShowHideProducts(DateTime startDate, DateTime endDate, DayOfWeek dayOfWeek, DateTime fromTime, DateTime toTime, Int16 productId, Int16 show);
        IEnumerable<ShowHideProduct> findShowHideProducts(DateTime startDate, DateTime endDate, DayOfWeek dayOfWeek);
    }
}