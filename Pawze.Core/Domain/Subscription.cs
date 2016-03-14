using Pawze.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Pawze.Core.Domain
{
    public class Subscription
    {
        public Subscription()
        {
            Boxes = new Collection<Box>();
        }
        public Subscription(SubscriptionsModel subscription)
        {
            this.Update(subscription);
        }

        public void Update(SubscriptionsModel subscription)
        {
            SubscriptionId = subscription.SubscriptionId;
            StartDate = subscription.StartDate;
            PawzeUserId = subscription.PawzeUserId;
            StripeSubscriptionId = subscription.StripeSubscriptionId;
      
        }

        public int SubscriptionId { get; set; }
        public DateTime StartDate { get; set; }
        public string PawzeUserId{ get; set; }
        public string StripeSubscriptionId { get; set; }

        public virtual ICollection<Shipment> Shipments { get; set; }
        public virtual ICollection<Box> Boxes { get; set; }
        public virtual PawzeUser PawzeUser { get; set; }
    }
}