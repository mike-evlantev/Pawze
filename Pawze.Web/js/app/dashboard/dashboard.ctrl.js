angular.module('app').controller('DashboardController', function ($scope, BoxResource, BoxItemResource, InventoryResource, SubscriptionResource, apiUrl, $http, $state) {

    function activate() {
        $http.get(apiUrl + '/boxes/user')
             .then(function (response) {
                 $scope.box = response.data;
             })
             .catch(function (err) {
                 //bootbox.alert('Failed to get the box');
             });
        $http.get(apiUrl + '/subscriptions/active')
            .then(function (response) {
                $scope.subscriptions = response.data;
            })
             .catch(function (err) {
                 //bootbox.alert('Failed to get the box');
             });
        $scope.inventories = InventoryResource.query();
    }

    activate();


    $scope.selectInventory = function (inventory, boxItem) {
        boxItem.InventoryId = inventory.InventoryId;
        boxItem.Inventory = inventory;
    };

    $scope.redirect = function () {
        if ($scope.subscriptions.length == 0) {
            $state.go('app.summary');
        }
        else {
            $state.go('app.confirmation');
        }
    }

    $scope.save = function () {
        if ($scope.box.BoxId === 0) {
            BoxResource.save($scope.box, function (newBox) {
                bootbox.alert('Saved box, box id is now ' + newBox.BoxId);
                $scope.redirect();
            });
        } else {
            $http.put(apiUrl + '/boxes/' + $scope.box.BoxId, $scope.box)
                 .then(function () {
                     bootbox.alert('Updated box successfully');
                     $scope.redirect();
                 })
                 .catch(function (err) {
                     bootbox.alert('Couldn\'t update the box');
                 });
        }
    };

    //$scope.subsave = function () {
    //    if (SubscriptionResource.query().length < 1) {
    //        SubscriptionResource.save($scope.sub, function (data) {
    //            $scope.box.SubscriptionId = data.SubscriptionId;
    //        })
    //    }
    //    else {
    //        $scope.box.SubscriptionId = SubscriptionResource.query().SubscriptionId; //change to .first?
    //    }
    //}

    //$scope.save = function () {

    //    SubscriptionResource.save($scope.sub, function (data) {
    //        $scope.box.SubscriptionId = data.SubscriptionId;
    //    });


    //    BoxResource.save($scope.box, function (data) {
    //        // putting boxitems 
    //        $scope.boxItem1.BoxItemPrice = 7;
    //        $scope.boxItem1.BoxId = data.BoxId;
    //        BoxItemResource.save($scope.boxItem1)
    //        $scope.boxItem2.BoxItemPrice = 7;
    //        $scope.boxItem2.BoxId = data.BoxId;
    //        BoxItemResource.save($scope.boxItem2)
    //        $scope.boxItem3.BoxItemPrice = 7;
    //        $scope.boxItem3.BoxId = data.BoxId;
    //        BoxItemResource.save($scope.boxItem3)
    //        $scope.boxItem4.BoxItemPrice = 7;
    //        $scope.boxItem4.BoxId = data.BoxId;
    //        BoxItemResource.save($scope.boxItem4)
    //    });
    //};
});