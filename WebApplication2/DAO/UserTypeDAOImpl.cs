using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class UserTypeDAOImpl : BaseImpl<UserType, Int16>, UserTypeDAO
    {
        public UserTypeDAOImpl() : base()
        {
        }

        public void deleteUserType(Int16 id)
        {
            base.delete(id);
        }

        public UserType getUserTypeById(Int16 id)
        {
            return base.getById(id);
        }

        public IEnumerable<UserType> getUserTypes()
        {
            return base.get();
        }

        public void insertUserType(UserType userType)
        {
            base.insert(userType);
        }

        public void saveUserType()
        {
            base.save();
        }

        public void updateUserType(UserType userType)
        {
            base.update(userType);
        }

        public void dispose()
        {
            base.Dispose();
        }
    }
}