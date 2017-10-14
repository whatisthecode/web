using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface UserDAO
    {
        IEnumerable<User> getUsers();
        User getUserById(Int16 id);
        void insertUser(User user);
        void deleteUser(Int16 id);
        void updateUser(User user);
        void saveUser();
        void dispose();
        PagedResult<User> PageReview(int index, int PageSize, string columnname);
    }
}