angular.module('app').factory('BoxResource', function (apiUrl, $resource) {
    return $resource(apiUrl + '/boxes/:boxId', { boxId: '@BoxId' },
        {
            'update': {
                method: 'PUT'
            }
        });
});