angular.module('app').controller('AccountSettingsController', function ($scope, $state, $http, apiUrl, SubscriptionResource) {

    function activate() {
        $http.get(apiUrl + '/pawzeuser/user')
               .then(function (response) {
                   $scope.user = response.data;
               })
               .catch(function (err) {
                   //bootbox.alert('Failed to get the user');
               });

        $http.get(apiUrl + '/subscriptions/active')
            .then(function (response) {
                $scope.subscription = response.data;

                if ($scope.subscription.length > 0) {
                    $scope.cancellation = true;
                }
                else {
                    $scope.cancellation = false;
                }
            })
             .catch(function (err) {
                 //bootbox.alert('Failed to get the box');
             });
    }

    activate();

    $scope.save = function () {
        $http.put(apiUrl + '/pawzeuser/user', $scope.user)
                 .then(function () {
                     bootbox.alert("User Information Has Been Updated")
                 })
                 .catch(function (err) {
                     bootbox.alert('Couldn\'t update your settings.');
                 });
    };

    $scope.cancel = function () {
        if ($scope.subscription.length > 0) {
            bootbox.confirm("Are you sure you want to cancel?", function (result) {
                $http.post(apiUrl + '/subscriptions/cancel', {
                    stripeToken: $scope.subscription.StripeSubscriptionId
                }).success(function () {
                    bootbox.alert('Subscription Cancelled.'); //delete
                    activate();
                }).error(function () {
                    bootbox.alert('Could not cancel subscription, Sorry bout it.'); //delete
                });
            });
        }
        else
        {
            bootbox.alert("You don't have an active subscription.");
        }
    };

    

});