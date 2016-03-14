using Pawze.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pawze.Core.Domain
{
    public class Inventory
    {
        public Inventory()
        {

        }
        public Inventory(InventoriesModel inventory)
        {
            this.Update(inventory);
        }

        public void Update(InventoriesModel inventory)
        {
            InventoryId = inventory.InventoryId;
            Name = inventory.Name;
            Description = inventory.Description;
            QuantityOnHand = inventory.QuantityOnHand;
        }

        public int InventoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuantityOnHand { get; set; }

        public virtual ICollection<BoxItem> BoxItems { get; set; }
    }
}