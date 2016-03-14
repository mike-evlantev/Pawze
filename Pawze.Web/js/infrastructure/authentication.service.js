angular.module('app').factory('AuthenticationService', function ($http, $q, localStorageService, apiUrl) {
    var state = {
        authorized: false
    };

    function initialize() {
        // this is called as soon as the application loads
        var token = localStorageService.get('token');
        if (token) {
            state.authorized = true;
        }
        var user = localStorageService.get('user');
    }

    function registerStaff(registration) {
        return $http.post(apiUrl + '/accounts/register/staff', registration).then(
            function (response) {
                return response;
            }
        );
    }
    function registerCustomer(registration) {
        return $http.post(apiUrl + '/accounts/register/customers', registration).then(
            function (response) {
                return response;
            }
        );
    }
    function registerAdmin(registration) {
        // this wil call /api/accounts/register
        return $http.post(apiUrl + '/accounts/register/admin', registration).then(
            function (response) {
                return response;
            }
        );
    }
    

    // loginData is a JS object that contains username and password from the login form
    function login(loginData) {
        logout();

        // this will call /api/token
        var data = 'grant_type=password&username=' + loginData.username + '&password=' + loginData.password;

        //setting up a promise manually
        var deferred = $q.defer();
        $http.post(apiUrl + '/token', data, {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        }).success(function (response) {
            console.log(response);
            localStorageService.set('token', {
                token: response.access_token
            });
            localStorageService.set('user', {
                user: loginData.username
            });
            state.authorized = true;
            deferred.resolve(response);

        }).error(function (err, status) {
            logout();
            deferred.reject(err);

        });
        return deferred.promise;
    }

    function logout() {
        // this will log the user out
        localStorageService.remove('token');
        localStorageService.remove('user');
        state.authorized = false;
    }

    return {
        state: state,
        initialize: initialize,
        registerCustomer: registerCustomer,
        registerAdmin: registerAdmin,
        registerStaff: registerStaff,
        login: login,
        logout: logout
    };
});