using Pawze.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pawze.Core.Domain
{
    public class Shipment
    {
        public Shipment()
        {

        }
        public Shipment(ShipmentsModel shipment)
        {
            this.Update(shipment);
        }

        public void Update(ShipmentsModel shipment)
        {
            ShipmentId = shipment.ShipmentId;
            Tracking = shipment.Tracking;
            PawzeUserId = shipment.PawzeUserId;
            ShipmentDate = shipment.ShipmentDate;
        }

        public int ShipmentId { get; set; }
        public string Tracking { get; set; }
        public int PawzeUserId { get; set; }
        public DateTime ShipmentDate { get; set; }

        public virtual Subscription Subscription { get; set;}
    }
}