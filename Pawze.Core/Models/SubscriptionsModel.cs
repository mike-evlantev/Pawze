using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pawze.Core.Models
{
    public class SubscriptionsModel
    {
        public int SubscriptionId { get; set; }
      
        public string PawzeUserId { get; set; }
        public string StripeSubscriptionId { get; set; }

        public DateTime StartDate { get; set; }
    }
}