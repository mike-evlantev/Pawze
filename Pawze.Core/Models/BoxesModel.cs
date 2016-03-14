using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pawze.Core.Models
{

    public class BoxesModel
    {
        public int BoxId { get; set; }
        public int? SubscriptionId { get; set; }
        public string PawzeUserId { get; set; }

        public IEnumerable<BoxItemsModel> BoxItems { get; set; }
    }
}