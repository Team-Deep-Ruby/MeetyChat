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
            .when('/profile', {
                templateUrl: 'templates/profile.html',
                controller: 'ProfileController',
                resolve : {
                    isLogged: function (authService) {
                        authService.isLogged();
                    }
                }
            })
            .when('/profile/changepassword', {
                templateUrl: 'templates/change-password.html',
                controller: 'ProfileController',
                resolve : {
                    isLogged: function (authService) {
                        authService.isLogged();
                    }
                }
            })
            .when('/rooms', {
                templateUrl: 'templates/roomsList.html',
                controller: 'RoomsController',
                resolve : {
                    isLogged: function (authService) {
                        authService.isLogged();
                    },
                    leaveRoom: function (roomsService, $routeParams) {
                        if ($routeParams.id) {
                            roomsService.leaveRoom($routeParams.id)
                        }
                    }
                }
            })
            .when('/rooms/:id', {
                templateUrl: 'templates/room.html',
                controller: 'RoomsController',
                resolve : {
                    isLogged: function (authService) {
                        authService.isLogged();
                    }
                }
            })
            .when('/privateRooms/:id', {
                templateUrl: 'templates/privateRoom.html',
                controller: 'RoomsController',
                resolve : {
                    isLogged: function (authService) {
                        authService.isLogged();
                    }
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