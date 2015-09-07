'use strict';

meetyChatApp.controller('RoomsController',
    function RoomsController($scope, $http, $route, $location, roomsService, $routeParams, Notification, $timeout, authService) {

        $scope.getRooms = function () {
            roomsService.getAllRooms()
                .then(function (data) {
                    $scope.roomsList = data;
                    $timeout(function () {
                        if ($route.current.$$route.originalPath == '/rooms'){
                            $scope.getRooms();
                            //$scope.getPrivateRooms();
                        }
                    }, 20000);
                }, function (error) {
                    Notification.error(error.Message);
                })
        };

        $scope.getPrivateRooms = function () {
            roomsService.getPrivateRooms()
                .then(function (data) {
                    var privateRoomsList = [];
                    data.forEach(function (data) {
                        data.Name = getPrivateRoomName(data);
                        privateRoomsList.push(data);
                    });
                    $scope.privateRoomsList = privateRoomsList;
                    $timeout(function () {
                        if ($route.current.$$route.originalPath == '/rooms'){
                            $scope.getPrivateRooms();
                        }
                    }, 20000);
                }, function (error) {
                    Notification.error(error.Message);
                })
        };

        $scope.getRoomById = function () {
            roomsService.getRoomById($routeParams.id)
                .then(function (data) {
                    $scope.room = data;
                    $timeout(function () {
                        if ($route.current.$$route.originalPath == '/rooms/:id'){
                            $scope.getRoomById()
                        }
                    }, 20000);

                }, function (error) {
                    Notification.error(error.Message)
                })
        };

        $scope.getPrivateRoomById = function () {
            roomsService.getPrivateRoomById($routeParams.id)
                .then(function (data) {
                    $scope.username = getPrivateRoomName(data);
                    $timeout(function () {
                        if ($route.current.$$route.originalPath == '/privateRooms/:id'){
                            $scope.getPrivateRoomById()
                        }
                    }, 20000);
                }, function (error) {
                    $location.path('/rooms');
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
            if ($routeParams.id) {
                roomsService.leaveRoom(room)
                    .then(function () {
                    }, function (error) {
                        Notification.error(error.Message);
                    })
            }
        };

        $scope.addRoom = function (data) {
            var room = {
                Name: data
            };

            roomsService.addRoom(room)
                .then(function (data) {
                    $('#room.Name').val('');
                    $scope.roomsList.unshift(data);
                    Notification.success('Room successfully added.');
                }, function (error) {
                    $('#room.Name').val('');
                    Notification.error(error.Message);
                })
        };

        $scope.addPrivateRoom = function (memberName) {
            var room = {
                FirstUsername: this.authService.getCurrentUser().userName,
                SecondUsername: memberName
            };

            roomsService.addPrivateRoom(room)
                .then(function (data) {
                    $location.path('/privateRooms/' + data);
                }, function (error) {
                    Notification.error(error.Message);
                })
        };

        $scope.deleteRoom = function (room) {

            roomsService.deleteRoom(room)
                .then(function (data) {
                    Notification.success(data.Message);
                }, function (error) {
                    Notification.error('Failed deleting room.');
                })
        };

        $scope.getLatestUsers = function (roomId) {
            roomsService.getLatestUsers(roomId)
                .then(function (data) {
                    if (data) {
                        $scope.room.MembersCount++;
                        $scope.room.Members.push({
                            Name: data[0].Username
                        });
                    }
                    $timeout($scope.getLatestUsers(roomId), 1);
                }, function (error) {
                    Notification.error(error.Message);
                })
        };

        $scope.getLatestLeftUsers = function (roomId) {
            roomsService.getLatestLeftUsers(roomId)
                .then(function (data) {
                    if (data) {
                        $scope.room.MembersCount--;
                        var i = $scope.room.Members.length;
                        while (i--) {
                            if ($scope.room.Members[i].Name === data[0].Username){
                                $scope.room.Members.splice(i);
                                break;
                            }
                        }
                    }
                    $timeout($scope.getLatestLeftUsers(roomId), 1);
                }, function (error) {
                    Notification.error(error.Message);
                })
        };

        var getPrivateRoomName = function (data) {
            var usernames = data.Name.split(" ");
            if (authService.getCurrentUser().userName == usernames[0]) {
                return usernames[1];
            } else {
                return usernames[0];
            }
        }
    });