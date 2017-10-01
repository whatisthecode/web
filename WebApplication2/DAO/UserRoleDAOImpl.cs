using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class UserRoleDAOImpl : BaseImpl<UserRole, Int16>, UserRoleDAO, IDisposable
    {
        public UserRoleDAOImpl() : base()
        {

        }

        public void deleteUserRole(Int16 id)
        {
            base.delete(id);
        }

        public UserRole getUserRoleById(Int16 id)
        {
            return base.getById(id);
        }

        public IEnumerable<UserRole> getUserRoles()
        {
            return base.get();
        }

        public void insertUserRole(UserRole userRole)
        {
            base.insert(userRole);
        }

        public void saveUserRole()
        {
            base.save();
        }

        public void updateUserRole(UserRole userRole)
        {
            base.update(userRole);
        }

        public void dispose()
        {
            base.Dispose();
        }
    }
}