using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class GroupDAOImpl : BaseImpl<Group, Int16>, GroupDAO, IDisposable
    {
        public void deleteGroup(Int16 groupId)
        {
            base.delete(groupId);
        }

        public void dispose()
        {
            base.Dispose();
        }

        public Group getGroupById(Int16 groupId)
        {
            return base.getById(groupId);
        }

        public IEnumerable<Group> getGroup()
        {
            return base.get();
        }

        public Group getGroupByName(string name)
        {
            Group group = base.get().Where(g => g.name == name).FirstOrDefault();
            return group;
        }

        public void insertGroup(Group group)
        {
            base.insert(group);
        }

        public void saveGroup()
        {
            base.save();
        }

        public void updateGroup(Group group)
        {
            base.update(group);
        }

        public Boolean checkExists(String name)
        {
            Group group = base.get().Where(g => g.name == name).FirstOrDefault();
            return group != null ? true : false;
        }
    }
}