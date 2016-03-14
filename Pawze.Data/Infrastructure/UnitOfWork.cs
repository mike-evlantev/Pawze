using Pawze.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawze.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private PawzeDataContext _dataContext;

        protected PawzeDataContext DataContext
        {
            get
            {
                return _dataContext ?? (_dataContext = _databaseFactory.GetDataContext());
            }
        }

        public void Commit()
        {
            DataContext.SaveChanges();
        }

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
    }
}
