angular.module('app').factory('AccountResource', function (apiUrl, $resource) {
    return $resource(apiUrl + '/PawzeUser/:PawzeUserId', { PawzeUserId: '@PawzeUserId' },
    {
        'update': {
            method: 'PUT'
        }
    });
});