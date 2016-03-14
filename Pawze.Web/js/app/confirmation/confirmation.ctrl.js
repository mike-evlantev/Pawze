angular.module('app').controller('ConfirmationController', function ($scope, SubscriptionResource, $state, localStorageService, apiUrl, $http) {


    // Fetches the user's box items to be displayed
    function activate() {
        //$scope.subscriptionId = SubscriptionResource.query().SubscriptionId;
        //$scope.subscription = SubscriptionResource.query();
        //$scope.subscriptionId = $scope.subscription.SubscriptionId;

        $http.get(apiUrl + '/pawzeuser/user')
           .then(function (response) {
               $scope.user = response.data;
               $scope.email = response.data.Email;
           })
           .catch(function (err) {
               //bootbox.alert('Failed to get the user');
           });
        $http.get(apiUrl + '/boxes/user')
             .then(function (response) {
                 $scope.box = response.data;
                 // Grabs total for box items
                 $scope.getTotal = function () {
                     var total = 0;
                     for (var i = 0; i < $scope.box.BoxItems.length; i++) {
                         var bxItm = $scope.box.BoxItems[i];
                         total += (bxItm.BoxItemPrice);
                     }
                     return total;
                 };
                 $scope.getSubTotal = function () {
                     var total = 0;
                     total += $scope.getTotal();
                     return total;
                 };

                 $scope.getOrderTotal = function () {
                     var total = 0;
                     total += $scope.getSubTotal();
                     return total;
                 }
             })
             .catch(function (err) {
                 bootbox.alert('Failed to get the box');
             });
    }
    $scope.email = ""
    activate();

    var getUserEmail = function () {
        var userEmail = $scope.user.Email;
        return userEmail;
    };
});