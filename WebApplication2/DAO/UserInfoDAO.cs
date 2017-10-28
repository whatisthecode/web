using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface UserInfoDAO
    {
        IEnumerable<UserInfo> getUserInfos();
        UserInfo getUserInfo(Int16 id);
        void updateUserInfo(UserInfo userInfo);
        void insertUserInfo(UserInfo userInfo);
        void deleteUserInfo(Int16 id);
        void saveUserinfo();
        void dispose();
        UserInfo checkExist(string field, string value);
    }
}