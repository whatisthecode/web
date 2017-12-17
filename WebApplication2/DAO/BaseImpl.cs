using LaptopWebsite.Models;
using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApplication2.DAO
{
    public class BaseImpl<T, Int16> : Base<T, Int16> where T : class
    {

        public BaseImpl()
        {
        }


        public void delete(Int16 id)
        {
            using (DBContext context = new DBContext())
            {
                T instance = this.getById(id);
                context.Set<T>().Remove(instance);
                context.SaveChanges();
            }

        }

        public IEnumerable<T> get()
        {
            using (DBContext context = new DBContext())
            {
                return context.Set<T>().AsNoTracking<T>().ToList<T>();
            }          
        }

        public T getById(Int16 id)
        {
            using (DBContext context = new DBContext())
            {
                try
                {
                    T result = context.Set<T>().Find(id);
                    return result;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return null;
                }
            }
        }

        public void insert(T entity)
        {
            using (DBContext context = new DBContext())
            {
                try
                {
                    context.Set<T>().Add(entity);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
            }
        }

        public void update(T entity)
        {
            using (DBContext context = new DBContext())
            {
                context.Set<T>().Attach(entity);
                context.Entry<T>(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PagedResult<T> PageView(IQueryable<T> query, int page, int pageSize)
        {
            PagedResult<T> result = new PagedResult<T>();
            result.currentPage = page;
            result.pageSize = pageSize;
            result.rowCount = query.Count();
            var pageCount = (double)result.rowCount / pageSize;
            result.pageCount = (int)Math.Ceiling(pageCount);
            var skip = (page - 1) * pageSize;

            result.items = Queryable.Skip(query, skip).Take(pageSize).ToList();
            return result;
        }

        public Boolean checkColumnExists(string column)
        {
            var columns = typeof(T).GetProperties().Select(property => property.Name).ToArray();
            return columns.Contains(column);
        }

        public Boolean checkColumnsExist(Array columns)
        {
            var length = columns.Length;
            for (var i = 0; i < length; i++)
            {
                if (!this.checkColumnExists(columns.GetValue(i).ToString()))
                {
                    return false;
                }
            }
            return true;
        }
    }
}