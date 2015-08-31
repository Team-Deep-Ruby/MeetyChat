'use strict';

meetyChatApp.controller('RoomsController',
    function RoomsController($scope, $http, $route, $location, roomsService, $routeParams, Notification, $timeout) {

        $scope.getRooms = function () {
            roomsService.getAllRooms()
                .then(function (data) {
                    $scope.roomsList = data;
                    if ($route.current.$$route.originalPath == '/rooms'){
                        //$timeout($scope.getRooms, 5000);
                    }
                }, function (error) {
                    Notification.error(error.Message);
                })
        };

        $scope.getRoomById = function () {
            roomsService.getRoomById($routeParams.id)
                .then(function (data) {
                    $scope.room = data;
                    if ($route.current.$$route.originalPath == '/rooms/:id'){
                        //$timeout($scope.getRoomById, 5000);
                    }
                }, function (error) {
                    Notification.error(error.Message)
                })
        };

        $scope.joinRoom = function (room) {
            roomsService.joinRoom(room)
                .then(function () {
                    $location.path('/rooms/' + room.Id);
                }, function (error) {
                    Notification.error(error.Message);
                })
        };

        $scope.leaveRoom = function (room) {
            roomsService.leaveRoom(room)
                .then(function () {
                    $scope.room.MembersCount--;
                }, function (error) {
                    Notification.error(error.Message);
                })
        };
    });