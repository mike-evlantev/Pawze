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
    public class BoxItemsController : ApiController
    {
        //private PawzeDataContext db = new PawzeDataContext();
        private IBoxItemRepository _boxItemRepository;
        private IPawzeUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;

        public BoxItemsController(IBoxItemRepository boxItemRepository, IUnitOfWork unitOfWork, IPawzeUserRepository userRepository)
        {
            _boxItemRepository = boxItemRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        // GET: api/BoxItems
        public IEnumerable<BoxItemsModel> GetBoxItems()
        {
            return Mapper.Map<IEnumerable<BoxItemsModel>>(
                _boxItemRepository.GetWhere(b => b.Box.PawzeUser.UserName == User.Identity.Name)
                );
        }

        // GET: api/BoxItems/5
        [ResponseType(typeof(BoxItemsModel))]
        public IHttpActionResult GetBoxItem(int id)
        {
            BoxItem dbBoxItem = _boxItemRepository.GetFirstOrDefault(b => b.Box.PawzeUser.UserName == User.Identity.Name && b.BoxItemId == id);
            if (dbBoxItem == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<BoxItemsModel>(dbBoxItem));
        }

        // PUT: api/BoxItems/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBoxItem(int id, BoxItemsModel boxItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BoxItem dbBoxItem = _boxItemRepository.GetFirstOrDefault(b => b.Box.PawzeUser.UserName == User.Identity.Name && b.BoxItemId == id);

            if (id != boxItem.BoxItemId)
            {
                return BadRequest();
            }

            dbBoxItem.Update(boxItem);

            _boxItemRepository.Update(dbBoxItem);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                if (!BoxItemExists(id))
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

        // POST: api/BoxItems
        [ResponseType(typeof(BoxItemsModel))]
        public IHttpActionResult PostBoxItem(BoxItemsModel boxItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbBoxItem = new BoxItem();

            dbBoxItem.Update(boxItem);

            _boxItemRepository.Add(dbBoxItem);
            _unitOfWork.Commit();

            boxItem.BoxItemId = dbBoxItem.BoxItemId;

            return CreatedAtRoute("DefaultApi", new { id = boxItem.BoxItemId }, boxItem);
        }

        // DELETE: api/BoxItems/5
        [ResponseType(typeof(BoxItemsModel))]
        public IHttpActionResult DeleteBoxItem(int id)
        {
            BoxItem dbBoxItem = _boxItemRepository.GetFirstOrDefault(b => b.Box.PawzeUser.UserName == User.Identity.Name && b.BoxItemId == id);

            if (dbBoxItem == null)
            {
                return NotFound();
            }

            _boxItemRepository.Delete(dbBoxItem);
            _unitOfWork.Commit();

            return Ok(Mapper.Map<BoxItemsModel>(dbBoxItem));
        }

        private bool BoxItemExists(int id)
        {
            return _boxItemRepository.Any(e => e.BoxItemId == id);
        }
    }
}