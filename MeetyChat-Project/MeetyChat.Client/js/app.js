'use strict';

var meetyChatApp = angular
    .module('meetyChatApp', ['ngRoute', 'ui-notification'])
    .constant({
        'BASE_URL': 'http://chattest2.cloudapp.net/api'
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
                controller: 'ProfileController'
            })
            .when('/profile/changepassword', {
                templateUrl: 'templates/change-password.html',
                controller: 'ProfileController'
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
                    },
                    getRooms: function (authService, roomsService) {
                        return roomsService.getAllRooms();
                    }
                }
            })
            .when('/rooms/:id', {
                templateUrl: 'templates/room.html',
                controller: 'RoomsController',
                resolve : {
                    isLogged: function (authService) {
                        authService.isLogged();
                    },
                    getRooms: function (authService, roomsService, $route) {
                        return roomsService.getRoomById($route.current.params.id);
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