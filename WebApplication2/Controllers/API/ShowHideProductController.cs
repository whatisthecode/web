using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;
using static WebApplication2.Models.RequestModel.ShowHideProductViewModel;

namespace WebApplication2.Controllers.API
{
    public class ShowHideProductController : ApiController
    {
        public ShowHideProductController()
        {

        }

        [HttpPost]
        [Route("api/show-hide-produc/create")]
        public IHttpActionResult createShowHideProduct([FromBody] ModelCreate modelCreate)
        {
            Response response = new Response();
            if(modelCreate.fromDate == null || modelCreate.endDate == null)
            {
                response.code = "400";
                response.status = "chưa chọn khoản ngày tháng";
                return Content<Response>(System.Net.HttpStatusCode.OK, response);
            }

            if(modelCreate.DaysOfWeek.Count == 0)
            {
                response.code = "400";
                response.status = "chưa chọn thứ";
                return Content<Response>(System.Net.HttpStatusCode.OK, response);
            }

            if (modelCreate.times.Count == 0)
            {
                response.code = "400";
                response.status = "chưa chọn khoản giờ";
                return Content<Response>(System.Net.HttpStatusCode.OK, response);
            }

            if (modelCreate.productId.Count == 0)
            {
                response.code = "400";
                response.status = "chưa chọn sản phẩm";
                return Content<Response>(System.Net.HttpStatusCode.OK, response);
            }

            foreach(var day in modelCreate.DaysOfWeek)
            {
                for(var i = 0; i < modelCreate.times.Count; i++)
                {
                    var time = modelCreate.times[i];
                    foreach(var product in modelCreate.productId)
                    {
                        IEnumerable<ShowHideProduct> showHideProduct = Service.showHideDAO.findShowHideProducts(modelCreate.fromDate, modelCreate.endDate, day, time.fromTime, time.toTime, product, time.show);
                        if (showHideProduct.Count() > 0)
                        {
                            response.code = "409";
                            response.status = "Sản phẩm này đã bị trùng lịch";
                            return Content<Response>(System.Net.HttpStatusCode.OK, response);
                        }
                    }
                }
            }

            foreach (var day in modelCreate.DaysOfWeek)
            {
                for (var i = 0; i < modelCreate.times.Count; i++)
                {
                    var time = modelCreate.times[i];
                    ShowHideProduct create = new ShowHideProduct();
                    create.startDate = modelCreate.fromDate;
                    create.endDate = modelCreate.endDate;
                    create.dayOfWeek = day;
                    create.fromTime = time.fromTime;
                    create.toTime = time.toTime;
                    create.show = time.show;
                    foreach(var id in modelCreate.productId)
                    {
                        create.productId = id;
                        Service.showHideDAO.insertShowHideProduct(create);
                    }
                }
            }

            response.code = "200";
            response.status = "Tạo lịch thành công";
            return Content<Response>(System.Net.HttpStatusCode.OK, response);
        }

        [HttpDelete]
        [Route("api/show-hide-product/{id}")]
        public IHttpActionResult DeleteSchedule([FromUri] Int16 id)
        {
            Service.showHideDAO.deleteShowHideProduct(id);
            Response response = new Response("200", "Xóa lịch thành công", null);
            return Content<Response>(System.Net.HttpStatusCode.OK, response);
        }
    }
}