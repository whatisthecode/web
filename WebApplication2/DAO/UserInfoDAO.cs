using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;
using WebApplication2.Models.RequestModel;

namespace WebApplication2.DAO
{
    public interface UserInfoDAO
    {
        IEnumerable<UserInfo> getUserInfos();
        UserInfo getUserInfo(Int16 id);
        UserDetail getUserDetail(String appUserId);
        void updateUserInfo(UserInfo userInfo);
        void insertUserInfo(UserInfo userInfo);
        void deleteUserInfo(Int16 id);
        void saveUserinfo();
        void dispose();
        UserInfo checkExist(string field, string value);

        PagedResult<UserInfo> PageView(Int16 indexnum, Int16 pagesize, String Orderby);
        PagedResult<UserInfo> PageView(Int16 indexnum, Int16 pagesize, String Orderby, Boolean ascending);
        PagedResult<UserInfo> AdminPageView(Int16 userId, Int16 indexnum, Int16 pagesize, String Orderby);
    }
}