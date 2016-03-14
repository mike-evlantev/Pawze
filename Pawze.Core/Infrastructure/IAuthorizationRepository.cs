using Microsoft.AspNet.Identity;
using Pawze.Core.Domain;
using Pawze.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawze.Core.Infrastructure
{
    public interface IAuthorizationRepository
    {
        Task<PawzeUser> FindUser(string username, string password);
        Task<IdentityResult> RegisterAdmin(RegistrationModel model);
        Task<IdentityResult> RegisterCustomer(RegistrationModel model);
        Task<IdentityResult> RegisterStaff(RegistrationModel model);
    }
}
