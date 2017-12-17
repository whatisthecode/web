using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.RequestModel
{
    public class CreateUserGroupModel
    {
        public CreateUserGroupModel()
        {

        }

        public CreateUserGroupModel(String userId, Int16 groupId)
        {
            this.userId = userId;
            this.groupId = groupId;
        }
        public String userId { get; set; }
        public Int16 groupId { get; set; }
    }
}