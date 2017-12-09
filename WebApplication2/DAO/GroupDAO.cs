using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface GroupDAO
    {
        IEnumerable<Group> getGroup();
        Group getGroupById(Int16 groupId);
        Group getGroupByName(String name);
        Boolean checkExists(String name);
        void insertGroup(Group group);
        void deleteGroup(Int16 groupId);
        void updateGroup(Group group);
        void saveGroup();
        void dispose();
    }
}