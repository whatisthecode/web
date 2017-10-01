using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface UserTypeDAO
    {
        IEnumerable<UserType> getUserTypes();
        UserType getUserTypeById(Int16 id);
        void insertUserType(UserType userType);
        void deleteUserType(Int16 id);
        void updateUserType(UserType userType);
        void saveUserType();
        void dispose();
    }
}