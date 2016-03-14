angular.module('app').controller('RegisterController', function ($scope, $timeout, AuthenticationService) {
    $scope.registration = {};

    $scope.register = function () {
        AuthenticationService.registerCustomer($scope.registration).then(
            function (response) {
                bootbox.alert("Registration Complete");
                $timeout(function () {
                    location.replace('/#/login');
                }, 2000);
            },
            function (error) {
                bootbox.alert("Failed To Register");
            }
        );
    };
});