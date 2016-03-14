using Pawze.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pawze.Core.Domain
{
    public class BoxItem
    {
        public BoxItem()
        {

        }

        public BoxItem(BoxItemsModel boxItem)
        {
            this.Update(boxItem);
        }

        public void Update(BoxItemsModel boxItem)
        {
            BoxItemId = boxItem.BoxItemId;
            BoxId = boxItem.BoxId;
            InventoryId = boxItem.InventoryId;
            BoxItemPrice = boxItem.BoxItemPrice;
        }

        public int BoxItemId { get; set; }
        public int BoxId { get; set; }
        public int InventoryId { get; set; }
        public decimal BoxItemPrice { get; set; }

        public virtual Box Box { get; set; }
        public virtual Inventory Inventory { get; set; }
    }
}