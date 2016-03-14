angular.module('app').factory('AuthenticationInterceptor', function ($q, localStorageService) {
    function InterceptRequest(request) {
        var token = localStorageService.get('token');

        if (token) {
            request.headers.Authorization = 'Bearer ' + token.token;
        }

        return request;
    }

    function InterceptResponse(response) {
        if (response.status === 401) {
            location.replace('/#/login')
        }

        return $q.reject(response);
    }

    return {
        request: InterceptRequest,
        responseError: InterceptResponse
    };
});