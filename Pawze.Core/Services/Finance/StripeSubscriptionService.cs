using Pawze.Core.Domain;
using Pawze.Core.Infrastructure;
using Pawze.Core.Repository;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawze.Core.Services.Finance
{
    public class StripeSubscriptionService : ISubscriptionService
    {
        private const string APIKey = "sk_test_4abUSbJ1Khdhi6X3GWGkHoQ2";

        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StripeSubscriptionService(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public void Cancel(PawzeUser pawzeUser, Subscription pawzeSubscription)
        {
            var subscriptionService = new Stripe.StripeSubscriptionService(APIKey);
            StripeSubscription subscription = subscriptionService.Cancel(pawzeUser.StripeCustomerId, pawzeSubscription.StripeSubscriptionId);

            pawzeSubscription.StripeSubscriptionId = null;

            _subscriptionRepository.Update(pawzeSubscription);

            _unitOfWork.Commit();
        }

        /// <summary>
        /// Before calling this method - make sure you have saved the box to the database
        /// </summary>
        public void Create(PawzeUser pawzeUser, Box box, string token)
        {
            if (string.IsNullOrEmpty(pawzeUser.StripeCustomerId))
            {
                var customerService = new StripeCustomerService(APIKey);

                var customer = customerService.Create(new StripeCustomerCreateOptions
                {
                    Email = pawzeUser.Email
                });

                pawzeUser.StripeCustomerId = customer.Id;
            }

            var subscriptionService = new Stripe.StripeSubscriptionService(APIKey);

            StripeSubscription subscription = subscriptionService.Create(pawzeUser.StripeCustomerId, "box", new StripeSubscriptionCreateOptions
            {
                Card = new StripeCreditCardOptions
                {
                    TokenId = token
                }
            });

            var pawzeSubscription = new Subscription
            {
                PawzeUserId = pawzeUser.Id,
                StartDate = DateTime.Now,
                StripeSubscriptionId = subscription.Id
            };

            pawzeSubscription.Boxes.Add(box);

            _subscriptionRepository.Add(pawzeSubscription);

            _unitOfWork.Commit();
        }
    }
}