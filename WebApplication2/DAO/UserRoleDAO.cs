using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface UserRoleDAO
    {
        IEnumerable<UserRole> getUserRoles();
        UserRole getUserRoleById(Int16 id);
        void insertUserRole(UserRole userRole);
        void deleteUserRole(Int16 id);
        void updateUserRole(UserRole userRole);
        void saveUserRole();
        void dispose();
    }
}