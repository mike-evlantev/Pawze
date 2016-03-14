angular.module('app').factory('InventoryResource', function (apiUrl, $resource) {
    return $resource(apiUrl + '/inventories/:inventoryId', { inventoryId: '@InventoryId' },
        {
            'update': {
                method: 'PUT'
            }
        });
});