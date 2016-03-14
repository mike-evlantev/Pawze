angular.module('app', ['ngResource', 'ui.router', 'LocalStorageModule', 'smoothScroll']);

angular.module('app').value('apiUrl', 'http://localhost:55442/api');

angular.module('app').config(function ($stateProvider, $urlRouterProvider, $httpProvider) {
    $httpProvider.interceptors.push('AuthenticationInterceptor');

    $urlRouterProvider.otherwise('/home');

    $stateProvider
        .state('home', { url: '/home', templateUrl: '/templates/home/home.html', controller: 'HomeController' })
        .state('register', { url: '/register', templateUrl: '/templates/home/register/register.html', controller: 'RegisterController' })
        .state('login', { url: '/login', templateUrl: '/templates/home/login/login.html', controller: 'LoginController' })

        .state('app', { url: '/app', templateUrl: '/templates/app/app.html', controller: 'AppController' })
            .state('app.dashboard', { url: '/dashboard', templateUrl: '/templates/app/dashboard/dashboard.html', controller: 'DashboardController' })

            .state('app.accountsettings', { url: '/accountsettings', templateUrl: '/templates/app/accountsettings/accountsettings.html', controller: 'AccountSettingsController' })

            .state('app.confirmation', { url: '/confirmation', templateUrl: '/templates/app/confirmation/confirmation.html', controller: 'ConfirmationController' })

            .state('app.summary', { url: '/summary', templateUrl: '/templates/app/summary/summary.html', controller: 'SummaryController' });
});

angular.module('app').run(function (AuthenticationService) {
    AuthenticationService.initialize();
});