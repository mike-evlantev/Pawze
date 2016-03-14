using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawze.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        PawzeDataContext GetDataContext();
    }
}
