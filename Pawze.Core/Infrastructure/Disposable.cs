using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawze.Core.Infrastructure
{
    // THis is a UTILITY CLASS
    // helps dispose of objects in the correct manner
    public class Disposable : IDisposable
    {
        private bool _disposed;

        ~Disposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                DisposeCore();
            }
            _disposed = true;
        }

        protected virtual void DisposeCore()
        {
        }
    }
}
