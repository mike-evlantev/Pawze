using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Pawze.Core.Domain;
using Pawze.Core.Infrastructure;
using Pawze.Core.Repository;
using Pawze.Core.Models;
using AutoMapper;

namespace Pawze.API.Controllers
{
    [Authorize]
    public class PawzeConfigurationsController : ApiController
    {
        //  private PawzeDataContext db = new PawzeDataContext();
        private IPawzeConfigurationRepository _pawzeConfigurationRepository;
        private IPawzeUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;

        public PawzeConfigurationsController(IPawzeConfigurationRepository pawzeConfigurationRepository, IUnitOfWork unitOfWork, IPawzeUserRepository userRepository)
        {
            _pawzeConfigurationRepository = pawzeConfigurationRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        //// GET: api/Configurations
        //public IEnumerable<PawzeConfiguration> GetConfigurations()
        //{
        //    return db.PawzeConfigurations;
        //}

        // GET: api/Configurations/5
        [ResponseType(typeof(PawzeConfigurationsModel))]
        public IHttpActionResult GetConfiguration(int id)
        {
            // PawzeConfiguration configuration = db.PawzeConfigurations.Find(id);
            PawzeConfiguration dbConfiguration = _pawzeConfigurationRepository.GetById(id);
            if (dbConfiguration == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PawzeConfigurationsModel>(dbConfiguration));
        }

        // PUT: api/Configurations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutConfiguration(int id, PawzeConfiguration configuration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != configuration.PawzeConfigurationId)
            {
                return BadRequest();
            }

            _pawzeConfigurationRepository.Update(configuration);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                if (!ConfigurationExists(id))
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

        // POST: api/Configurations
        [ResponseType(typeof(PawzeConfiguration))]
        public IHttpActionResult PostConfiguration(PawzeConfiguration configuration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _pawzeConfigurationRepository.Add(configuration);
            _unitOfWork.Commit();

            return CreatedAtRoute("DefaultApi", new { id = configuration.PawzeConfigurationId }, configuration);
        }

        // DELETE: api/Configurations/5
        [ResponseType(typeof(PawzeConfiguration))]
        public IHttpActionResult DeleteConfiguration(int id)
        {
            PawzeConfiguration configuration = _pawzeConfigurationRepository.GetById(id);
            if (configuration == null)
            {
                return NotFound();
            }

            _pawzeConfigurationRepository.Delete(configuration);
            _unitOfWork.Commit();

            return Ok(configuration);
        }

        private bool ConfigurationExists(int id)
        {
            return _pawzeConfigurationRepository.Any(e => e.PawzeConfigurationId == id);
        }
    }
}