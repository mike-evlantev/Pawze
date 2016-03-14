using Pawze.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Pawze.Core.Domain
{
    public class Box
    {
        public Box()
        {
            BoxItems = new Collection<BoxItem>();
        }

        public Box(BoxesModel box)
        {
            this.Update(box);
        }

        public void Update(BoxesModel box)
        {
            BoxId = box.BoxId;
            SubscriptionId = box.SubscriptionId;
            PawzeUserId = box.PawzeUserId;

            if(BoxId == 0)
            {
                // IF IT'S NEW
                foreach(var boxItem in box.BoxItems)
                {
                    var dbBoxItem = new BoxItem();
                    dbBoxItem.Update(boxItem);
                    BoxItems.Add(dbBoxItem);
                }
            }
            else
            {
                // if it exists
                foreach (var modelBoxItem in box.BoxItems)
                {
                    var databaseBoxItem = BoxItems.FirstOrDefault(bi => bi.BoxItemId == modelBoxItem.BoxItemId);

                    databaseBoxItem.Update(modelBoxItem);
                }
            }
        }

        public int BoxId { get; set; }
        public int? SubscriptionId { get; set; }
        public string PawzeUserId { get; set; }

        public virtual ICollection<BoxItem> BoxItems { get; set; }
        public virtual PawzeUser PawzeUser { get; set; }
        public virtual Subscription Subscription { get; set; }

    }
}