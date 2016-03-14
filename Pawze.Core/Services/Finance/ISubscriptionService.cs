using Pawze.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawze.Core.Services.Finance
{
    public interface ISubscriptionService
    {
        void Create(PawzeUser pawzeUser, Box box, string token);
        void Cancel(PawzeUser pawzeUser, Subscription pawzeSubscription);
    }
}