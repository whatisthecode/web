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

        public bool checkColumnExists(string column)
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
            var dbContext = base.getContext().token;
            Token token = dbContext.FirstOrDefault(t => t.accessToken == accessToken);
            if(token != null)
            {
                delete(token.id);
                save();
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
            var dbContext = base.getContext().token;
            return dbContext.FirstOrDefault(t => t.accessToken == accessToken);
        }

        public new Token getById(short id)
        {
            return base.getById(id);
        }

        public Token getByUsername(string userName)
        {
            var dbContext = base.getContext().token;
            return dbContext.FirstOrDefault(t => t.userName == userName);
        }

        public new void insert(Token entity)
        {
            base.insert(entity);
        }

        public new void save()
        {
            base.save();
        }

        public new void update(Token entity)
        {
            base.update(entity);
        }
    }
}