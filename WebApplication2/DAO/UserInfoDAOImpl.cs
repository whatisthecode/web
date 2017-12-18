using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LaptopWebsite.Models.Mapping;
using WebApplication2.Models;
using WebApplication2.Models.RequestModel;
using System.Globalization;

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

        public UserInfo checkExist(string field, string value)
        {
            using (DBContext context = new DBContext())
            {
                UserInfo user = new UserInfo();
                var query = from u in context.userInfos
                            where u.identityNumber.Equals(value)
                            select u;
                user = query.FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
                return null;
            }
        }

        public Boolean isAdmin(UserInfo userInfo)
        {
            return true;
        }

        public PagedResult<UserInfo> PageView(short indexnum, short pagesize, string Orderby)
        {
            using (DBContext context = new DBContext())
            {
                var query = from uI in context.userInfos select uI;
                switch (Orderby)
                {
                    case "id":
                        query = query.OrderBy(uI => uI.id);
                        break;

                }
                PagedResult<UserInfo> pv = base.PageView(query, indexnum, pagesize);
                return pv;
            }

        }

        public PagedResult<UserInfo> PageView(short indexnum, short pagesize, string Orderby, bool ascending)
        {
            using (DBContext context = new DBContext())
            {
                var query = from uI in context.userInfos select uI;
                if (query != null && ascending)
                {
                    switch (Orderby)
                    {
                        case "id":
                            query = query.OrderBy(uI => uI.id);
                            break;

                    }
                }
                if (query != null && !ascending)
                {
                    switch (Orderby)
                    {
                        case "id":
                            query = query.OrderByDescending(uI => uI.id);
                            break;
                    }
                }
                PagedResult<UserInfo> pv = base.PageView(query, indexnum, pagesize);
                return pv;
            }
        }

        public PagedResult<UserInfo> AdminPageView(short userId, short indexnum, short pagesize, string Orderby)
        {
            using (DBContext context = new DBContext())
            {
                var query = from uI in context.userInfos select uI;
                query = query.Where(u => u.id != userId);
                switch (Orderby)
                {
                    case "id":
                        query = query.OrderBy(uI => uI.id);
                        break;

                }
                PagedResult<UserInfo> pv = base.PageView(query, indexnum, pagesize);
                return pv;
            }
        }

        public UserDetail getUserDetail(string appUserId)
        {
            UserDetail userDetail = new UserDetail();
            ApplicationUser user = Service._userManager.FindByIdAsync(appUserId).Result;
            Token token = Service.tokenDAO.getByUsername(user.Email);
            UserInfo userInfo = Service.userInfoDAO.getUserInfo(user.userInfoId);
            userDetail.Id = user.Id;
            userDetail.createdAt = userInfo.createdAt;
            userDetail.dobDisplay = ((DateTime)userInfo.dob).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            userDetail.Email = user.Email;
            userDetail.firstName = userInfo.firstName;
            userDetail.lastName = userInfo.lastName;
            userDetail.Password = user.PasswordHash;
            userDetail.status = userInfo.status;
            userDetail.PhoneNumber = user.PhoneNumber;
            userDetail.isLogin = token != null ? token.isLogin : false;
            userDetail.identityNumber = userInfo.identityNumber;
            return userDetail;
        }

        public ApplicationUser getApplicationUserByUserInfoId(Int16 userInfoId)
        {
            using (DBContext context = new DBContext())
            {
                return context.Users.Where(au => au.userInfoId == userInfoId).FirstOrDefault();
            }
        }
    }
}