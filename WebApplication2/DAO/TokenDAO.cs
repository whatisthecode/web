using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface TokenDAO
    {
        bool checkColumnExists(string column);
        bool checkColumnsExist(Array columns);
        void delete(short id);
        void update(Token token);
        void Dispose();
        IEnumerable<Token> get();
        Token getById(short id);
        void insert(Token entity);
        Token getByAccessToken(string accessToken);
        void delete(string accessToken);
        Token getByUsername(string userName);
    }
}