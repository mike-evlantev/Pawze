using AutoMapper;
using Pawze.Core.Domain;
using Pawze.Core.Infrastructure;
using Pawze.Core.Models;
using Pawze.Core.Repository;
using Pawze.Core.Services.Finance;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace Pawze.API.Controllers
{
    [Authorize]
    public class SubscriptionsController : ApiController
    {
        private ISubscriptionService _subscriptionService;
        private ISubscriptionRepository _subscriptionRepository;
        private IBoxRepository _boxRepository;
        private IPawzeUserRepository _pawzeUserRepository;
        private IUnitOfWork _unitOfWork;

        public SubscriptionsController(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork, ISubscriptionService subscriptionService, IPawzeUserRepository pawzeUserRepository, IBoxRepository boxRepository)
        {
            _subscriptionService = subscriptionService;
            _subscriptionRepository = subscriptionRepository;
            _boxRepository = boxRepository;
            _unitOfWork = unitOfWork;
            _pawzeUserRepository = pawzeUserRepository;
        }

        // GET: api/Subscriptions
        public IEnumerable<SubscriptionsModel> GetSubscriptions()
        {
            return Mapper.Map<IEnumerable<SubscriptionsModel>>(
                _subscriptionRepository.GetWhere(m => m.PawzeUser.UserName == User.Identity.Name)
            );
        }

        // GET: api/Subscriptions/active
        [Route("api/subscriptions/active")]
        public IEnumerable<SubscriptionsModel> GetActiveSubscriptions()
        {
            return Mapper.Map<IEnumerable<SubscriptionsModel>>(
                _subscriptionRepository.GetWhere(m => m.PawzeUser.UserName == User.Identity.Name && m.StripeSubscriptionId != null)
            );
        }

        // GET: api/Subscriptions/5
        [ResponseType(typeof(SubscriptionsModel))]
        public IHttpActionResult GetSubscription(int id)
        {
            Subscription dbSubscription = _subscriptionRepository.GetById(id);

            if (dbSubscription == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<SubscriptionsModel>(dbSubscription));
        }

        // PUT: api/Subscriptions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSubscription(int id, SubscriptionsModel subscription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subscription.SubscriptionId)
            {
                return BadRequest();
            }

            Subscription dbSubscription = _subscriptionRepository.GetById(id);
            dbSubscription.Update(subscription);

            _subscriptionRepository.Update(dbSubscription);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                if (!SubscriptionExists(id))
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

        // POST: api/Subscriptions
        [ResponseType(typeof(SubscriptionsModel))]
        public IHttpActionResult PostSubscription(SubscriptionsModel subscription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbSubscription = new Subscription();

            dbSubscription.StartDate = DateTime.Now;
            dbSubscription.PawzeUser = _pawzeUserRepository.GetFirstOrDefault(u => u.UserName == User.Identity.Name);
            _subscriptionRepository.Add(dbSubscription);

            _unitOfWork.Commit();

            subscription.SubscriptionId = dbSubscription.SubscriptionId;
            

            return CreatedAtRoute("DefaultApi", new { id = subscription.SubscriptionId }, subscription);
        }

        [HttpPost]
        [Route("api/subscriptions/create")]
        public IHttpActionResult CreateSubscription(StripePaymentParams payment)
        {
            _subscriptionService.Create(
                _pawzeUserRepository.GetFirstOrDefault(u => u.UserName == User.Identity.Name), 
                _boxRepository.GetById(payment.boxId),
                payment.stripeToken
            );

            return Ok();
        }

        [HttpPost]
        [Route("api/subscriptions/cancel")]
        public IHttpActionResult CancelSubscription()
        {
            var user = _pawzeUserRepository.GetFirstOrDefault(u => u.UserName == User.Identity.Name);

            _subscriptionService.Cancel(
                _pawzeUserRepository.GetFirstOrDefault(u => u.UserName == User.Identity.Name),
             _subscriptionRepository.GetFirstOrDefault(u => u.PawzeUserId == user.Id && u.StripeSubscriptionId != null)
            );

            return Ok();
        }

        // DELETE: api/Subscriptions/5
        [ResponseType(typeof(SubscriptionsModel))]
        public IHttpActionResult DeleteSubscription(int id)
        {
            Subscription subscription = _subscriptionRepository.GetById(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _subscriptionRepository.Delete(subscription);
            _unitOfWork.Commit();

            return Ok(Mapper.Map<SubscriptionsModel>(subscription));
        }

        private bool SubscriptionExists(int id)
        {
            return _subscriptionRepository.Count(e => e.SubscriptionId == id) > 0;
        }

        public class StripePaymentParams
        {
            public string stripeToken { get; set; }
            public int boxId { get; set; }
        }
    }
}