using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pawze.Core.Models
{
    public class ShipmentsModel
    {
        public int ShipmentId { get; set; }
        public string Tracking { get; set; }
        public int PawzeUserId { get; set; }
        public DateTime ShipmentDate { get; set; }
    }
}