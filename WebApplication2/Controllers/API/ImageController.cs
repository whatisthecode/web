using System;
using System.Diagnostics;
using System.IO;
using WebApplication2.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApplication2.Models.Mapping;

namespace WebApplication2.Controllers.API
{
    [RoutePrefix("api/image")]
    public class ImageController : ApiController
    {
        [Route("upload")]
        [HttpPost]
        public async Task<IHttpActionResult> uploadFile()
        {
            Image image = new Image();
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            String storagePath = HttpContext.Current.Server.MapPath("~/Assets/IMG/Content");

            var provider = new MultipartFormDataStreamProvider(storagePath);

            await Request.Content.ReadAsMultipartAsync(provider);

            foreach(String key in provider.FormData.AllKeys)
            {
                foreach(String val in provider.FormData.GetValues(key))
                {
                    if (key == "productId")
                        image.productId = Int16.Parse(val);
                    else if (key == "type")
                        image.type = val;
                }
            }

            foreach (MultipartFileData file in provider.FileData)
            {
                String tempFileName = file.Headers.ContentDisposition.FileName;
                if (tempFileName.StartsWith("\"") && tempFileName.EndsWith("\""))
                {
                    tempFileName = tempFileName.Trim('"');
                }
                if (tempFileName.Contains(@"/") || tempFileName.Contains(@"\"))
                {
                    tempFileName = Path.GetFileName(tempFileName);
                }
                String fileName = DateTime.Now.Ticks.ToString() + "_" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + DateTime.Now.Millisecond + "." + tempFileName.Split('.')[1];
                File.Move(file.LocalFileName, Path.Combine(storagePath, fileName));
                image.url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/assets/img/content/" +  fileName;
            }
            Service.imageDAO.insertImage(image);
            Service.imageDAO.saveImage();
            Response response = new Response("200", "Ảnh đã được tải lên thành công", image);
            return Content<Response>(HttpStatusCode.OK, response);
        }
    }
}
