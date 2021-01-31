using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext dataContext;
        internal DbSet<T> dbSet;
        public SQLRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            this.dbSet = dataContext.Set<T>();
        }
        public IQueryable<T> Collection()
        {
            return dbSet;
        }

        public void Commit()
        {
            dataContext.SaveChanges();
        }

        public void Delete(string id)
        {
            var t = Find(id);
            if (dataContext.Entry(t).State == EntityState.Detached)
                dbSet.Attach(t);
            dbSet.Remove(t);
        }

        public T Find(string id)
        {
            return dbSet.Find(id);
        }

        public void Insert(T t)
        {
            dbSet.Add(t);
        }

        public void Update(T t)
        {
            dbSet.Attach(t);
            dataContext.Entry(t).State = EntityState.Modified;
        }
    }
}
