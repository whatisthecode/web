using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface ImageDAO
    {
        IEnumerable<Image> getImage();
        Image getImageById(Int16 imageId);
        Image checkExist(Image image);
        void insertImage(Image image);

        void deleteImage(Int16 imageId);
        void updateImage(Image image);
        void saveImage();
        IEnumerable<Image> getThumbnail(Int16 prodId);
        IEnumerable<Image> getDetailImage(Int16 prodId);
        Boolean check(String column);
        PagedResult<Image> PageView(int pageIndex, int pageSize, string columnName);
        void dispose();
    }
}