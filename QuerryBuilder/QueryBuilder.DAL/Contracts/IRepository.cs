﻿using System;
using System.Collections.Generic;

namespace QueryBuilder.DAL.Contracts
{
    public interface IRepository <T> where T: class
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        IEnumerable<T> Find(Func<T, bool> predicate);

        void Create(T item);

        void Update(T item);

        void Delete(int id);

        void Delete(T item);
    }
}
