angular.module('app').controller('SummaryController', function ($scope, $state, localStorageService, apiUrl, $http) { 


    // Fetches the user's box items to be displayed
    function activate() {
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

    $('#customButton').on('click', function (e) {
        var handler = StripeCheckout.configure({
            key: 'pk_test_fhxY8eOWWpOb6dE1K7rBCgik',
            locale: 'auto',
            allowRememberMe: false,
            email: getUserEmail(), //TODO: update to pull from user
            token: function (token) {
                $http.post(apiUrl + '/subscriptions/create', {
                    stripeToken: token.id,
                    boxId: $scope.box.BoxId, //TODO: This is where you'll need the box id to create a subscription for. --done
                }).success(function () {
                    //bootbox.alert('charged ya'); //delete
                    $state.go('app.confirmation');
                }).error(function () {
                    bootbox.alert('could not charge you, please retry payment'); //delete
                });
            }
        });
        var chargeAmt = $scope.getOrderTotal() * 100;
        // Open Checkout with further options
        handler.open({
            description: 'Monthly Charge - 1 Box',
            amount: chargeAmt
        });
        e.preventDefault();
    });

    // Close Checkout on page navigation
    $(window).on('popstate', function () {
        handler.close();
    });

    

    //  $scope.saveUser = function () {
    //      $scope.user.$update();
    //  };
});