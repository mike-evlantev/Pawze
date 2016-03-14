using Pawze.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawze.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private readonly PawzeDataContext _dataContext;

        public PawzeDataContext GetDataContext()
        {
            return _dataContext ?? new PawzeDataContext();
        }

        public DatabaseFactory()
        {
            _dataContext = new PawzeDataContext();
        }

        protected override void DisposeCore()
        {
            if (_dataContext != null) _dataContext.Dispose();
        }
    }
}
