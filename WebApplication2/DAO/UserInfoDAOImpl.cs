using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LaptopWebsite.Models.Mapping;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class UserInfoDAOImpl : BaseImpl<UserInfo, Int16>, UserInfoDAO, IDisposable
    {

        public UserInfoDAOImpl() :base()
        {

        }

        public void deleteUserInfo(short id)
        {
            base.delete(id);
        }

        public void dispose()
        {
            base.Dispose();
        }

        public UserInfo getUserInfo(short id)
        {
            return base.getById(id);
        }

        public IEnumerable<UserInfo> getUserInfos()
        {
            return base.get();
        }

        public void insertUserInfo(UserInfo userInfo)
        {
            base.insert(userInfo);
        }

        public void updateUserInfo(UserInfo userInfo)
        {
            base.update(userInfo);
        }

        public void saveUserinfo()
        {
            base.save();
        }

        public UserInfo checkExist(string field, string value)
        {
            var query = from u in base.getContext().userInfos select u;
            switch(field)
            {
                case "identityNumber":
                   query = query.Where(q => q.identityNumber == value);
                break;
                default:
                break;
            }
            var user = query.FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            return null;
        }
    }
}