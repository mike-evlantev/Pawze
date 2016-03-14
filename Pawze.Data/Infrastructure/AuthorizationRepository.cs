using Microsoft.AspNet.Identity;
using Pawze.Core.Domain;
using Pawze.Core.Infrastructure;
using Pawze.Core.Models;
using System;
using System.Threading.Tasks;

namespace Pawze.Data.Infrastructure
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly IUserStore<PawzeUser> _userStore;
        private readonly IDatabaseFactory _databaseFactory;
        private readonly UserManager<PawzeUser> _userManager;

        private PawzeDataContext db;
        protected PawzeDataContext Db => db ?? (db = _databaseFactory.GetDataContext());

        public AuthorizationRepository(IDatabaseFactory databaseFactory, IUserStore<PawzeUser> userStore)
        {
            _userStore = userStore;
            _databaseFactory = databaseFactory;
            _userManager = new UserManager<PawzeUser>(userStore);
        }
        public async Task<IdentityResult> RegisterCustomer(RegistrationModel model)
        {
            // create a user
            var pawzeUser = new PawzeUser
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address1 = model.Address1,
                Address2 = model.Address2,
                City = model.City,
                State = model.State,
                PostCode = model.PostCode
            };

            // save the user
            var result = await _userManager.CreateAsync(pawzeUser, model.Password);

            await _userManager.AddToRoleAsync(pawzeUser.Id, "Customer");

            return result;
        }

        // Assigned Admin role
        public async Task<IdentityResult> RegisterAdmin(RegistrationModel model)
        {
            // create a user
            var pawzeUser = new PawzeUser
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress
            };

            // save the user
            var result = await _userManager.CreateAsync(pawzeUser, model.Password);

            await _userManager.AddToRoleAsync(pawzeUser.Id, "Admin");

            return result;
        }
        // Assigned staff role
        public async Task<IdentityResult> RegisterStaff(RegistrationModel model)
        {
            // create a user
            var pawzeUser = new PawzeUser
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress
            };

            // save the user
            var result = await _userManager.CreateAsync(pawzeUser, model.Password);

            await _userManager.AddToRoleAsync(pawzeUser.Id, "Staff");
            
            return result;
        }

        public async Task<PawzeUser> FindUser(string username, string password)
        {
            return await _userManager.FindAsync(username, password);
        }

    }
}