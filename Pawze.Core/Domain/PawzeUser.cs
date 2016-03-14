using Microsoft.AspNet.Identity;
using Pawze.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pawze.Core.Domain
{
    public class PawzeUser : IUser<string>
    {
        public PawzeUser()
        {

        }
        public PawzeUser(PawzeUsersModel pawzeUser)
        {
            this.Update(pawzeUser);
        }

        public void Update(PawzeUsersModel pawzeUser)
        {
            UserName = pawzeUser.UserName;
            FirstName = pawzeUser.FirstName;
            LastName = pawzeUser.LastName;
            Address1 = pawzeUser.Address1;
            Address2 = pawzeUser.Address2;
            Address3 = pawzeUser.Address3;
            Address4 = pawzeUser.Address4;
            Address5 = pawzeUser.Address5;
            City = pawzeUser.City;
            State = pawzeUser.State;
            PostCode = pawzeUser.PostCode;
            International = pawzeUser.International;
            PhoneNumber = pawzeUser.PhoneNumber;
            Email = pawzeUser.Email;
            StripeCustomerId = pawzeUser.StripeCustomerId;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string StripeCustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public bool? International { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //public string Telephone { get; set; } use PhoneNumber
        //public string EmailAddress { get; set; }  use Email
        

        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<Box> Boxes { get; set; }
        public virtual ICollection<UserRole> Roles { get; set; }

    }
}