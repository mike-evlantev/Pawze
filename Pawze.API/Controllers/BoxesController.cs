using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

using Pawze.API.Infrastructure;
using AutoMapper;
using Pawze.Core.Models;
using Pawze.Core.Domain;
using Pawze.Core.Repository;
using Pawze.Core.Infrastructure;

namespace Pawze.API.Controllers
{
    [Authorize]
    public class BoxesController : ApiController
    {
        //private PawzeDataContext db = new PawzeDataContext();
        private IBoxRepository _boxRepository;
        private IBoxItemRepository _boxItemRepository;
        private IPawzeUserRepository _userRepository;
        private IPawzeConfigurationRepository _configRepository;
        private IUnitOfWork _unitOfWork;

        public BoxesController(IBoxRepository boxRepository, IPawzeConfigurationRepository configRepository, IBoxItemRepository boxItemRepository, IUnitOfWork unitOfWork, IPawzeUserRepository userRepository)
        {
            _boxRepository = boxRepository;
            _boxItemRepository = boxItemRepository;
            _configRepository = configRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        // GET: api/Boxes
        public IEnumerable<BoxesModel> GetBoxes()
        {
            return Mapper.Map<IEnumerable<BoxesModel>>( 
                _boxRepository.GetWhere(b => b.PawzeUser.UserName == User.Identity.Name)
            );
        }

        // GET: /api/Boxes/5/BoxItems
        [Route("api/boxes/{boxId}/boxitems")]
        public IEnumerable<BoxItemsModel> GetBoxItemsForBox(int id)
        {
            var boxItems = _boxItemRepository.GetWhere(bi => bi.BoxId == id);

            return Mapper.Map<IEnumerable<BoxItemsModel>>(boxItems);
        }

        // GET: /api/Boxes/user
        [Route("api/boxes/user")]
        public BoxesModel GetBoxForCurrentUser()
        {
            var currentUser = _userRepository.GetFirstOrDefault(u => u.UserName == User.Identity.Name);

            var dbBox = _boxRepository.GetFirstOrDefault(b => b.PawzeUser.UserName == currentUser.UserName);

            if(dbBox == null)
            {
                return new BoxesModel
                {
                    BoxItems = new BoxItemsModel[]
                    {
                        new BoxItemsModel(), new BoxItemsModel(), new BoxItemsModel(), new BoxItemsModel()
                    }
                };
            }
            else
            {
                return Mapper.Map<BoxesModel>(dbBox);
            }
        }

        // GET: api/Boxes/5
        [ResponseType(typeof(BoxesModel))]
        public IHttpActionResult GetBox(int id)
        {
            // Box box = db.Boxes.Find(id);
            Box dbBox = _boxRepository.GetFirstOrDefault(b => b.PawzeUser.UserName == User.Identity.Name && b.BoxId == id);
            if (dbBox == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<BoxesModel>(dbBox));
        }

        // PUT: api/Boxes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBox(int id, BoxesModel box)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Box dbBox = _boxRepository.GetFirstOrDefault(b => b.PawzeUser.UserName == User.Identity.Name && b.BoxId == id);

            if (id != box.BoxId)
            {
                return BadRequest();
            }

            dbBox.Update(box);

            _boxRepository.Update(dbBox);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                if (!BoxExists(id))
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

        // POST: api/Boxes
        [ResponseType(typeof(BoxesModel))]
        public IHttpActionResult PostBox(BoxesModel box)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbBox = new Box();

            dbBox.PawzeUser = _userRepository.GetFirstOrDefault(u => u.UserName == User.Identity.Name);

            dbBox.Update(box);

            foreach (var boxItem in dbBox.BoxItems)
            {
                boxItem.BoxItemPrice = _configRepository.GetAll().First().CurrentBoxItemPrice;
            }

            _boxRepository.Add(dbBox);

            _unitOfWork.Commit();

            box.BoxId = dbBox.BoxId;

            return CreatedAtRoute("DefaultApi", new { id = box.BoxId }, box);
        }

        // DELETE: api/Boxes/5
        [ResponseType(typeof(BoxesModel))]
        public IHttpActionResult DeleteBox(int id)
        {
            // Box box = db.Boxes.Find(id);
            Box dbBox = _boxRepository.GetFirstOrDefault(b => b.PawzeUser.UserName == User.Identity.Name && b.BoxId == id);

            if (dbBox == null)
            {
                return NotFound();
            }

            _boxRepository.Delete(dbBox);
            _unitOfWork.Commit();

            return Ok(Mapper.Map<BoxesModel>(dbBox));
        }

        private bool BoxExists(int id)
        {
            return _boxRepository.Any(e => e.BoxId == id);
        }
    }
}