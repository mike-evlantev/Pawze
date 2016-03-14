angular.module('app').factory('RoleResource', function (apiUrl, $resource) {
    return $resource(apiUrl + '/role/:roleId', { roleId: '@RoleId' },
        {
            'update': {
                method: 'PUT'
            }
        });
});