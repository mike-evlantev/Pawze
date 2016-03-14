angular.module('app').factory('BoxItemResource', function (apiUrl, $resource) {
    return $resource(apiUrl + '/boxitems/:boxItemId', { boxItemId: '@BoxItemId' },
        {
            'update': {
                method: 'PUT'
            }
        });
});