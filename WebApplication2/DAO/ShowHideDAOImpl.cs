using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class ShowHideDAOImpl : BaseImpl<ShowHideProduct, Int16>, ShowHideDAO, IDisposable
    {
        public void deleteShowHideProduct(short id)
        {
            base.delete(id);
        }

        public ShowHideProduct getShowHideProduct(short id)
        {
            return base.getById(id);
        }

        public IEnumerable<ShowHideProduct> getShowHideProducts()
        {
            return base.get();
        }

        public void insertShowHideProduct(ShowHideProduct showHideProduct)
        {
            base.insert(showHideProduct);
        }

        public void updateShowHideProduct(ShowHideProduct showHideProduct)
        {
            base.update(showHideProduct);
        }

        public IEnumerable<ShowHideProduct> findShowHideProducts(DateTime startDate, DateTime endDate, DayOfWeek dayOfWeek, DateTime fromTime, DateTime toTime, Int16 productId, Int16 show)
        {
            using (DBContext context = new DBContext())
            {
                return context.showHideProducts.Where(sc => sc.startDate <= startDate &&
                                                        sc.startDate <= endDate &&
                                                        sc.endDate >= startDate &&
                                                        sc.endDate >= endDate &&
                                                        sc.dayOfWeek == dayOfWeek &&
                                                        sc.fromTime <= toTime &&
                                                        sc.fromTime <= fromTime &&
                                                        sc.toTime >= fromTime &&
                                                        sc.toTime >= toTime &&
                                                        sc.productId == productId &&
                                                        sc.show == show).ToList();
            }
        }

        public IEnumerable<ShowHideProduct> findShowHideProducts(DateTime startDate, DateTime endDate, DayOfWeek dayOfWeek)
        {
            using (DBContext context = new DBContext())
            {
                return context.showHideProducts.Where(sc => sc.startDate <= startDate &&
                                                        sc.startDate <= endDate &&
                                                        sc.endDate >= startDate &&
                                                        sc.endDate >= endDate &&
                                                        sc.dayOfWeek == dayOfWeek).ToList();
            }
        }
    }
}