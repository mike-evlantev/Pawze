using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawze.Core.Domain
{
    public class UserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual PawzeUser User { get; set; }
        public virtual Role Role { get; set; }
    }
}
