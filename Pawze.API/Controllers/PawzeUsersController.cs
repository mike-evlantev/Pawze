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
    public class PawzeUsersController : ApiController
    {
        private IPawzeUserRepository _pawzeUserRepository;
        private IUnitOfWork _unitOfWork;

        public PawzeUsersController(IPawzeUserRepository pawzeUserRepository, IUnitOfWork unitOfWork)
        {
            _pawzeUserRepository = pawzeUserRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/PawzeUser
        public IEnumerable<PawzeUsersModel> GetPawzeUser()
        {
            return Mapper.Map<IEnumerable<PawzeUsersModel>>(
                _pawzeUserRepository.GetAll()
            );
        }

        // GET: /api/PawzeUser/user
        [Route("api/pawzeuser/user")]
        public IHttpActionResult GetCurrentUser()
        {
            var currentUser = _pawzeUserRepository.GetFirstOrDefault(u => u.UserName == User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PawzeUsersModel>(currentUser));

        }

        // GET: api/PawzeUser/5
        [ResponseType(typeof(PawzeUsersModel))]
        public IHttpActionResult GetPawzeUser(int id)
        {
            PawzeUser dbPawzeUser = _pawzeUserRepository.GetById(id);

            if (dbPawzeUser == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PawzeUsersModel>(dbPawzeUser));
        }

        // PUT: api/PawzeUser/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPawzeUser(string id, PawzeUsersModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            PawzeUser dbPawzeUser = _pawzeUserRepository.GetById(id);
            dbPawzeUser.Update(user);

            _pawzeUserRepository.Update(dbPawzeUser);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                if (!PawzeUserExists(id))
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


        // PUT: api/PawzeUser/user
        [Route("api/pawzeuser/user")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCurrentPawzeUser(PawzeUsersModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PawzeUser dbPawzeUser = _pawzeUserRepository.GetFirstOrDefault(u => u.UserName == User.Identity.Name);
            dbPawzeUser.Update(user);

            _pawzeUserRepository.Update(dbPawzeUser);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                if (!PawzeUserExists(user.Id))
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

        // POST: api/PawzeUser
        [ResponseType(typeof(PawzeUsersModel))]
        public IHttpActionResult PostPawzeUser(PawzeUsersModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbPawzeUser = new PawzeUser();
            _pawzeUserRepository.Add(dbPawzeUser);

            _unitOfWork.Commit();

            user.Id = dbPawzeUser.Id;

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/PawzeUser/5
        [ResponseType(typeof(PawzeUsersModel))]
        public IHttpActionResult DeletePawzeUser(int id)
        {
            PawzeUser user = _pawzeUserRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            _pawzeUserRepository.Delete(user);
            _unitOfWork.Commit();

            return Ok(Mapper.Map<PawzeUsersModel>(user));
        }

        private bool PawzeUserExists(string id)
        {
            return _pawzeUserRepository.Count(e => e.Id == id) > 0;
        }
    }
}