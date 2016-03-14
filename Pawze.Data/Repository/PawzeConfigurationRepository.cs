using Pawze.Core.Domain;
using Pawze.Core.Repository;
using Pawze.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawze.Data.Repository
{
    public class PawzeConfigurationRepository : Repository<PawzeConfiguration>, IPawzeConfigurationRepository
    {
        public PawzeConfigurationRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }
    }
}
