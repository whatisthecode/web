using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LaptopWebsite.Models.Mapping;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class UserDAOImpl : BaseImpl<User, Int16>, UserDAO, IDisposable
    {
        public UserDAOImpl() : base()
        {
        }

        public void deleteUser(Int16 id)
        {
            base.delete(id);
        }

        public User getUserById(Int16 id)
        {
            return base.getById(id);
        }

        public IEnumerable<User> getUsers()
        {
            return base.get();
        }

        public void insertUser(User user)
        {
            base.insert(user);
        }

        public void saveUser()
        {
            base.save();
        }

        public void updateUser(User user)
        {
            base.update(user);
        }

        public void dispose()
        {
            base.Dispose();
        }


   
        public PagedResult<User> Pageview(int pageindex, int pagesize, string orderby, Boolean ascending)
        {
            var query = from c in base.Dbset() select c;
            if (query != null && ascending)
            {
                switch (orderby)
                {
                    case "id":
                        query = query.OrderBy(n=>n.id);
                        break;
                }
            }
            if (query != null && !ascending)
            {
                switch (orderby)
                {
                    case "id":
                        query = query.OrderByDescending(d => d.id);
                        break;
                }
            }
            PagedResult<User> pv = base.PageView(query, pageindex, pagesize);
            return pv;
        }

        public PagedResult<User> PageReview(int index, int PageSize, string columnname)
        {
            var query = from c in base.Dbset() select c;
            switch (columnname)
            {
                case "id":
                    query = query.OrderBy(n => n.id);
                    break;
            }
            PagedResult<User> pv = base.PageView(query, index, PageSize);
            return pv;
        }
    }
}