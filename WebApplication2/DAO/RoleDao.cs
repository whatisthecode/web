using System;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface RoleDAO
    {
        Task<Boolean> deleteRoleAsync(ApplicationRole applicationRole);
    }
}