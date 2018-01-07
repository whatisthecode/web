using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Quartz;
using WebApplication2.Models;
using System.Globalization;

namespace WebApplication2.Schedules
{
    public class ShowHideProductJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            DateTime now = DateTime.Now;
            DateTime time = default(DateTime).Add(now.TimeOfDay);
            DateTime newTime = DateTime.Parse(time.TimeOfDay.ToString(), CultureInfo.InvariantCulture);
            List <ShowHideProduct> listSH = Service.showHideDAO.findShowHideProducts(now, now, now.DayOfWeek).ToList();
            if(listSH.Count() > 0)
            {
                for (var i = 0; i < listSH.Count(); i++)
                {
                    ShowHideProduct showHideProduct = new ShowHideProduct();
                    Product product = new Product();
                    showHideProduct = listSH[i];
                    product = Service.productDAO.getProduct(showHideProduct.productId);

                    string dateTimeFromTime = showHideProduct.fromTime.ToString();
                    string timeFromTime = dateTimeFromTime.Split(' ')[1] + " " + dateTimeFromTime.Split(' ')[2];
                    DateTime newFromTime = DateTime.Parse(timeFromTime, CultureInfo.InvariantCulture);

                    string dateTimeToTime = showHideProduct.toTime.ToString();
                    string timeToTime = dateTimeToTime.Split(' ')[1] + " " + dateTimeToTime.Split(' ')[2];
                    DateTime newToTime = DateTime.Parse(timeToTime, CultureInfo.InvariantCulture);

                    var compareTime = newTime < newFromTime;
                    var compareTime2 = newTime.CompareTo(newFromTime);
                    if (newTime.CompareTo(newFromTime) >= 0 && newTime.CompareTo(newToTime) <= 0 && (showHideProduct.show != product.status))
                    {
                        product.status = listSH[i].show;
                        Service.productDAO.updateProduct(product);
                    }
                    else if(newTime.CompareTo(newFromTime) >= 0 && newTime.CompareTo(newToTime) >= 0 && (showHideProduct.show == product.status))
                    {
                        switch(showHideProduct.show)
                        {
                            case 0:
                                product.status = 1;
                                break;
                            default:
                                product.status = 0;
                                break;
                        }
                        Service.productDAO.updateProduct(product);
                    }
                }
            }
        }
    }
}