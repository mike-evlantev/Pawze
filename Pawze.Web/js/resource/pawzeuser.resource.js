angular.module('app').factory('PawzeUserResource', function (apiUrl, $resource) {
    return $resource(apiUrl + '/pawzeuser/:pawzeUserId', { pawzeUserId: '@PawzeUserId' },
        {
            'update': {
                method: 'PUT'
            }
        });
});