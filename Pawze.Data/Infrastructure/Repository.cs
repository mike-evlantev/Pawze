using Pawze.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pawze.Data.Infrastructure
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected IDatabaseFactory DatabaseFactory { get; set; }

        private PawzeDataContext _dataContext;
        protected PawzeDataContext DataContext
        {
            get { return _dataContext ?? (_dataContext = DatabaseFactory.GetDataContext()); }
            set { _dataContext = value; }
        }
        protected IDbSet<TEntity> DbSet { get; set; }

        protected Repository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;

            // Go get the table in the datacontext of type TEntity
            DbSet = DataContext.Set<TEntity>();
        }

        // Create
        public TEntity Add(TEntity entity)
        {
            return DbSet.Add(entity);
        }

        // Read
        public TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return DbSet;
        }
        public IEnumerable<TEntity> GetWhere(Expression<Func<TEntity, bool>> where)
        {
            return DbSet.Where(where);
        }
        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> where)
        {
            return DbSet.FirstOrDefault(where);
        }
        public int Count()
        {
            return DbSet.Count();
        }
        public int Count(Func<TEntity, bool> where)
        {
            return DbSet.Count(where);
        }
        public bool Any(Expression<Func<TEntity, bool>> where)
        {
            return DbSet.Any(where);
        }

        // Update
        public void Update(TEntity entity)
        {
            var t = DbSet.Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        // Delete
        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }
    }
}
