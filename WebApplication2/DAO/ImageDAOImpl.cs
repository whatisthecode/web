using System;
using System.Collections.Generic;
using LaptopWebsite.Models.Mapping;
using WebApplication2.Models;
using System.Linq;

namespace WebApplication2.DAO
{
    public class ImageDAOImpl : BaseImpl<Image, Int16>, ImageDAO, IDisposable
    {
        public bool check(string column)
        {
            throw new NotImplementedException();
        }

        public Image checkExist(Image image)
        {
            throw new NotImplementedException();
        }

        public void deleteImage(short imageId)
        {
            base.delete(imageId);
        }

        public void dispose()
        {
            base.Dispose();
        }

        public IEnumerable<Image> getDetailImage(short prodId)
        {
            return base.get().Where(i => i.productId == prodId && i.type == "detail").ToList();
        }

        public IEnumerable<Image> getImage()
        {
            return base.get();
        }

        public Image getImageById(short imageId)
        {
            return base.get().Where(i => i.id == imageId).FirstOrDefault();
        }

        public IEnumerable<Image> getThumbnail(short prodId)
        {
            return base.get().Where(i => i.productId == prodId && i.type == "thumbnail").ToList();
        }

        public void insertImage(Image image)
        {
            base.insert(image);
        }

        public PagedResult<Image> PageView(int pageIndex, int pageSize, string columnName)
        {
            throw new NotImplementedException();
        }

        public void saveImage()
        {
            base.save();
        }

        public void updateImage(Image image)
        {
            base.update(image);
        }
    }
}