using Microsoft.AspNet.Identity;
using Pawze.Core.Domain;
using Pawze.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawze.Data.Infrastructure
{
    public class UserStore : Disposable,
                             IUserStore<PawzeUser>,
                             IUserPasswordStore<PawzeUser>,
                             IUserSecurityStampStore<PawzeUser>,
                             IUserRoleStore<PawzeUser>
    {
        private readonly IDatabaseFactory _databaseFactory;

        private PawzeDataContext _db;
        protected PawzeDataContext Db
        {
            get
            {
                return _db ?? (_db = _databaseFactory.GetDataContext());
            }
        }

        public UserStore(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region IUserStore
        public Task CreateAsync(PawzeUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.Factory.StartNew(() => {
                user.Id = Guid.NewGuid().ToString();
                Db.Users.Add(user);
                Db.SaveChanges();
            });
        }

        public Task UpdateAsync(PawzeUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.Factory.StartNew(() =>
            {
                Db.Users.Attach(user);
                Db.Entry(user).State = EntityState.Modified;

                Db.SaveChanges();
            });
        }

        public Task DeleteAsync(PawzeUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.Factory.StartNew(() =>
            {
                Db.Users.Remove(user);
                Db.SaveChanges();
            });
        }

        public Task<PawzeUser> FindByIdAsync(string userId)
        {
            return Task.Factory.StartNew(() => Db.Users.Find(userId));
        }

        public Task<PawzeUser> FindByNameAsync(string userName)
        {
            return Task.Factory.StartNew(() => Db.Users.FirstOrDefault(u => u.UserName == userName));
        }
        #endregion

        #region IUserPasswordStore
        public Task SetPasswordHashAsync(PawzeUser user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(PawzeUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(PawzeUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }
        #endregion

        #region IUserSecurityStampStore
        public Task SetSecurityStampAsync(PawzeUser user, string stamp)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(PawzeUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.SecurityStamp);
        }
        #endregion

        #region IUserRoleStore

        public Task AddToRoleAsync(PawzeUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            return Task.Factory.StartNew(() =>
            {
                if (!Db.Roles.Any(r => r.Name == roleName))
                {
                    Db.Roles.Add(new Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = roleName
                    });
                    Db.SaveChanges();
                }

                Db.UserRoles.Add(new UserRole
                {
                    RoleId = Db.Roles.FirstOrDefault(r => r.Name == roleName).Id,
                    UserId = user.Id
                });

                Db.SaveChanges();
            });
        }

        public Task RemoveFromRoleAsync(PawzeUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            return Task.Factory.StartNew(() =>
            {
                var userRole = user.Roles.FirstOrDefault(r => r.Role.Name == roleName);

                if (userRole == null)
                {
                    throw new InvalidOperationException("User does not have that role");
                }

                Db.UserRoles.Remove(userRole);

                Db.SaveChanges();
            });
        }

        public Task<IList<string>> GetRolesAsync(PawzeUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                return (IList<string>)Db.Roles.Select(r => r.Name).ToList();
            });
        }

        public Task<bool> IsInRoleAsync(PawzeUser user, string roleName)
        {
            return Task.Factory.StartNew(() =>
            {
                return user.Roles.Any(r => r.Role.Name == roleName);
            });
        }

        #endregion
    }
}
