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
    public class BaseImpl<T, Int16> : Base<T, Int16>, IDisposable where T : class
    {
        private Boolean disposed;
        private DBContext context;

        public BaseImpl()
        {
            this.context = DatabaseFactory.context;
        }

        public DBContext getContext()
        {
            return this.context;
        }

        public void detach(T entity)
        {
            this.context.Entry<T>(entity).State = EntityState.Detached;
            this.save();
        }

        public void delete(Int16 id)
        {
            T instance = this.getById(id);
            this.context.Set<T>().Remove(instance);
        }

        public virtual void Dispose(Boolean disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public IEnumerable<T> get()
        {
            return this.context.Set<T>().AsNoTracking<T>().ToList<T>();
        }

        public T getById(Int16 id)
        {
            return this.context.Set<T>().Find(id);
        }

        public void insert(T entity)
        {
            this.context.Set<T>().Add(entity);
        }

        public void save()
        {
            this.context.SaveChanges();
        }

        public void update(T entity)
        {
            this.context.Set<T>().Attach(entity);
            this.context.Entry<T>(entity).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Dispose(true);
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