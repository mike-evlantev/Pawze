angular.module('app').factory('SubscriptionResource', function (apiUrl, $resource) {
    return $resource(apiUrl + '/subscriptions/:subscriptionId', { subscriptionId: '@SubscriptionId' },
        {
            'update': {
                method: 'PUT'
            }
        });
});