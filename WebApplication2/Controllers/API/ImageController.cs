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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebApplication2.Models.RequestModel;

namespace WebApplication2.Controllers.API
{
    [RoutePrefix("api/image")]
    public class ImageController : ApiController
    {
        [Route("upload")]
        [HttpPost]
        public async Task<IHttpActionResult> uploadFile()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            String storagePath = HttpContext.Current.Server.MapPath("~/Assets/IMG/Content");

            var provider = new MultipartFormDataStreamProvider(storagePath);

            await Request.Content.ReadAsMultipartAsync(provider);

            Int16 productId = 0;
            String type = "";
            foreach (String key in provider.FormData.AllKeys)
            {
                foreach (String val in provider.FormData.GetValues(key))
                {
                    if (key == "productId")
                        productId = Int16.Parse(val);
                    else if (key == "type")
                        type = val;
                }
            }
            IList<Image> images = new List<Image>();
            foreach (MultipartFileData file in provider.FileData)
            {
                Image image = new Image();
                image.productId = productId;
                image.type = type;
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
                image.url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/assets/img/content/" + fileName;
                Service.imageDAO.insertImage(image);
                Service.imageDAO.saveImage();
                images.Add(image);
            }
            Response response = new Response("200", "Ảnh đã được tải lên thành công", images);
            return Content<Response>(HttpStatusCode.OK, response);
        }
        [Route("upload/base64")]
        [HttpPost]
        public async Task<IHttpActionResult> uploadFileBase64(CreateImageModel createImageModel)
        {
            var matches = Regex.Match(createImageModel.url, @"data:image/(?<type>.+?),(?<data>.+)");
            var base64Data = matches.Groups["data"].Value;
            String extension = matches.Groups["type"].Value.Replace(";base64", "");
            var binData = Convert.FromBase64String(base64Data);
           
            String storagePath = HttpContext.Current.Server.MapPath("~/Assets/IMG/Content");
            String fileName = DateTime.Now.Ticks.ToString() + "_" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + DateTime.Now.Millisecond + "." + extension;
            String path = storagePath + "\\" + fileName;
            var stream = new MemoryStream(binData);
            FileStream file = new FileStream(path, FileMode.Create,FileAccess.ReadWrite);
            await stream.CopyToAsync(file);

            Image image = new Image();
            image.productId = createImageModel.productId;
            image.type = createImageModel.type;
            image.url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/assets/img/content/" + fileName;

            Service.imageDAO.insertImage(image);
            Service.imageDAO.saveImage();

            Response response = new Response("200", "Ảnh đã được tải lên thành công", image);
            return Content<Response>(HttpStatusCode.OK, response);
        }
    }
}