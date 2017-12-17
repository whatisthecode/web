using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LaptopWebsite.Models.Mapping;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class TokenDAOImpl : BaseImpl<Token, Int16>, TokenDAO, IDisposable
    {
        public TokenDAOImpl() : base()
        {

        }

        public new bool checkColumnExists(string column)
        {
            return base.checkColumnExists(column);   
        }

        public new bool checkColumnsExist(Array columns)
        {
            return base.checkColumnsExist(columns);
        }

        public new void delete(short id)
        {
            base.delete(id);
        }

        public void delete(string accessToken)
        {
            Token token = new Token();
            using (DBContext context = new DBContext())
            {
                token = context.token.FirstOrDefault(t => t.accessToken == accessToken);
            }
            if (token != null)
            {
                base.delete(token.id);
            }
        }

        public new void Dispose()
        {
            base.Dispose();
        }

        public new IEnumerable<Token> get()
        {
            return base.get();
        }

        public Token getByAccessToken(string accessToken)
        {
            using (DBContext context = new DBContext())
            {
                return context.token.FirstOrDefault(t => t.accessToken == accessToken);
            }
        }

        public new Token getById(short id)
        {
            return base.getById(id);
        }

        public Token getByUsername(string userName)
        {
            using (DBContext context = new DBContext())
            {
                return context.token.FirstOrDefault(t => t.userName == userName);
            }          
        }

        public new void insert(Token entity)
        {
            base.insert(entity);
        }

        public new void update(Token entity)
        {
            base.update(entity);
        }
    }
}