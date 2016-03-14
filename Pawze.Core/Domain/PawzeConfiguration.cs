using Pawze.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pawze.Core.Domain
{

    public class PawzeConfiguration
    {
        public PawzeConfiguration()
        {

        }
        public PawzeConfiguration(PawzeConfigurationsModel configuration)
        {
            this.Update(configuration);
        }

        public void Update(PawzeConfigurationsModel configuration)
        {
            PawzeConfigurationId = configuration.PawzeConfigurationId;
            CurrentBoxItemPrice = configuration.CurrentBoxItemPrice;
        }

        public int PawzeConfigurationId { get; set; }
        public decimal CurrentBoxItemPrice { get; set; }
    }
}