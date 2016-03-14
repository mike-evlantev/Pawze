angular.module('app').factory('ShipmentResource', function (apiUrl, $resource) {
    return $resource(apiUrl + '/shipment/:shipmentId', { shipmentId: '@ShipmentId' },
        {
            'update': {
                method: 'PUT'
            }
        });
});