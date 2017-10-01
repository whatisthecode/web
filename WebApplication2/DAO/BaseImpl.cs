using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class BaseImpl<T, Int16> : Base<T, Int16>, IDisposable where T : class
    {
        private Boolean disposed;
        private DBContext context;

        public BaseImpl(){
            this.context = DatabaseFactory.context;
        }

        public void delete(Int16 id)
        {
            T instance = this.context.Set<T>().Find(id);
            this.context.Set<T>().Remove(instance);
        }

        public IEnumerable<T> get()
        {
            return this.context.Set<T>().ToList<T>();
        }

        public T getById(Int16 id)
        {
            return this.context.Set<T>().Find(id);
        }

        public void insert(T instance)
        {
            this.context.Set<T>().Add(instance);
        }

        public void save()
        {
            this.context.SaveChanges();
        }

        public void update(T instance)
        {
            this.context.Entry<T>(instance).State = EntityState.Modified;
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}