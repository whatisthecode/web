﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.DAO
{
    public interface Base<T, Int16> : IDisposable where T : class
    {
        IEnumerable<T> get();
        T getById(Int16 id);
        void insert(T entity);
        void delete(Int16 id);
        void update(T entity);
        void save();
    }
}