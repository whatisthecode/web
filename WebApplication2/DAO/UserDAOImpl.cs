using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}