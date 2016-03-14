using Pawze.API.Infrastructure;
using Pawze.Core.Infrastructure;
using Pawze.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Pawze.API.Controllers
{
    public class AccountsController : ApiController
    {
        private IAuthorizationRepository _repo;

        public AccountsController(IAuthorizationRepository repo)
        {
            _repo = repo;
        }

        [AllowAnonymous]
        [Route("api/accounts/register/customers")]
        public async Task<IHttpActionResult> RegisterCustomer(RegistrationModel registration)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.RegisterCustomer(registration);

            if(result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Registration form was invalid.");
            }
        }

        [AllowAnonymous]
        //  [Authorize(Roles = "Admin")]
        [Route("api/accounts/register/admin")]
        public async Task<IHttpActionResult> RegisterAdmin(RegistrationModel registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.RegisterAdmin(registration);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Registration form was invalid.");
            }
        }

        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [Route("api/accounts/register/staff")]
        public async Task<IHttpActionResult> RegisterStaff(RegistrationModel registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.RegisterStaff(registration);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Registration form was invalid.");
            }
        }
    }
}
