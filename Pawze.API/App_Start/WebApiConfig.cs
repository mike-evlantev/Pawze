using AutoMapper;
using Pawze.Core.Domain;
using Pawze.Core.Models;
using System.Linq;
using System.Web.Http;

namespace Pawze.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application / xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            CreateMaps();
        }
        public static void CreateMaps()
        {
            Mapper.CreateMap<Box, BoxesModel>();
            Mapper.CreateMap<BoxItem, BoxItemsModel>();
            Mapper.CreateMap<PawzeConfiguration, PawzeConfigurationsModel>();
            Mapper.CreateMap<Inventory, InventoriesModel>();
            Mapper.CreateMap<PawzeUser, PawzeUsersModel>();
            Mapper.CreateMap<Shipment, ShipmentsModel>();
            Mapper.CreateMap<Subscription, SubscriptionsModel>();
        }
    }
}
