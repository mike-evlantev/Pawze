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
    public class InventoriesController : ApiController
    {
        private IInventoryRepository _inventoryRepository;
        private IUnitOfWork _unitOfWork;

        public InventoriesController(IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork)
        {
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Inventories
        [AllowAnonymous]
        public IEnumerable<InventoriesModel> GetInventories()
        {
            return Mapper.Map<IEnumerable<InventoriesModel>>(
                _inventoryRepository.GetAll()
            );
        }

        // GET: api/Inventories/5
        [AllowAnonymous]
        [ResponseType(typeof(InventoriesModel))]
        public IHttpActionResult GetInventory(int id)
        {
            Inventory dbInventory = _inventoryRepository.GetById(id);

            if (dbInventory == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<InventoriesModel>(dbInventory));
        }

        // PUT: api/Inventories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInventory(int id, InventoriesModel inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inventory.InventoryId)
            {
                return BadRequest();
            }

            Inventory dbInventory = _inventoryRepository.GetById(id);
            dbInventory.Update(inventory);

            _inventoryRepository.Update(dbInventory);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                if (!InventoryExists(id))
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

        // POST: api/Inventories
        [ResponseType(typeof(InventoriesModel))]
        public IHttpActionResult PostInventory(InventoriesModel inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbInventory = new Inventory();
            _inventoryRepository.Add(dbInventory);

            _unitOfWork.Commit();

            inventory.InventoryId = dbInventory.InventoryId;

            return CreatedAtRoute("DefaultApi", new { id = inventory.InventoryId }, inventory);
        }

        // DELETE: api/Inventories/5
        [ResponseType(typeof(InventoriesModel))]
        public IHttpActionResult DeleteInventory(int id)
        {
            Inventory inventory = _inventoryRepository.GetById(id);
            if (inventory == null)
            {
                return NotFound();
            }

            _inventoryRepository.Delete(inventory);
            _unitOfWork.Commit();

            return Ok(Mapper.Map<InventoriesModel>(inventory));
        }

        private bool InventoryExists(int id)
        {
            return _inventoryRepository.Count(e => e.InventoryId == id) > 0;
        }
    }
}