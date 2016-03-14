using AutoMapper;
using Pawze.Core.Domain;
using Pawze.Core.Infrastructure;
using Pawze.Core.Models;
using Pawze.Core.Repository;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace Pawze.API.Controllers
{
    [Authorize]
    public class ShipmentsController : ApiController
    {
        private IShipmentRepository _shipmentRepository;
        private IUnitOfWork _unitOfWork;

        public ShipmentsController(IShipmentRepository shipmentRepository, IUnitOfWork unitOfWork)
        {
            _shipmentRepository = shipmentRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Shipments
        public IEnumerable<ShipmentsModel> GetShipments()
        {
            return Mapper.Map<IEnumerable<ShipmentsModel>>(
                _shipmentRepository.GetAll()
            );
        }

        // GET: api/Shipments/5
        [ResponseType(typeof(ShipmentsModel))]
        public IHttpActionResult GetShipment(int id)
        {
            Shipment dbShipment = _shipmentRepository.GetById(id);

            if (dbShipment == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<ShipmentsModel>(dbShipment));
        }

        // PUT: api/Shipments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutShipment(int id, ShipmentsModel shipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shipment.ShipmentId)
            {
                return BadRequest();
            }

            Shipment dbShipment = _shipmentRepository.GetById(id);
            dbShipment.Update(shipment);

            _shipmentRepository.Update(dbShipment);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                if (!ShipmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Shipments
        [ResponseType(typeof(ShipmentsModel))]
        public IHttpActionResult PostShipment(ShipmentsModel shipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbShipment = new Shipment();
            _shipmentRepository.Add(dbShipment);

            _unitOfWork.Commit();

            shipment.ShipmentId = dbShipment.ShipmentId;

            return CreatedAtRoute("DefaultApi", new { id = shipment.ShipmentId }, shipment);
        }

        // DELETE: api/Shipments/5
        [ResponseType(typeof(ShipmentsModel))]
        public IHttpActionResult DeleteShipment(int id)
        {
            Shipment shipment = _shipmentRepository.GetById(id);
            if (shipment == null)
            {
                return NotFound();
            }

            _shipmentRepository.Delete(shipment);
            _unitOfWork.Commit();

            return Ok(Mapper.Map<ShipmentsModel>(shipment));
        }

        private bool ShipmentExists(int id)
        {
            return _shipmentRepository.Count(e => e.ShipmentId == id) > 0;
        }
    }
}