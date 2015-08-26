'use strict';

var meetyChatApp = angular
    .module('meetyChatApp', ['ngRoute', 'ui-notification'])
    .constant({
        'BASE_URL': 'http://localhost:33257/api'
    })
    .config(function ($routeProvider) {
        $routeProvider
            .when('/', {
            })
            .when('/login', {
                templateUrl: 'templates/login.html',
                controller: 'LoginController',
                resolve: {
                    isLoggedIn: isLoggedIn
                }
            })
            .when('/register', {
                templateUrl: 'templates/register.html',
                controller: 'RegisterController',
                resolve: {
                    isLoggedIn: isLoggedIn
                }
            })
            .otherwise({
                redirectTo: '/'
            });
    });

var isLoggedIn = function ($location, authService, Notification) {
    if (authService.isLoggedIn()) {
        $location.path('/');
        Notification.info("You are already logged in.");
    }
};