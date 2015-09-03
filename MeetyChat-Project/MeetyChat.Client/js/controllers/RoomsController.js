'use strict';

meetyChatApp.controller('RoomsController',
    function RoomsController($scope, $http, $route, $location, roomsService, $routeParams, Notification, getRooms, $timeout) {

        if (getRooms) {
            if (getRooms.length > 0){
                $scope.roomsList = getRooms;
            } else {
                $scope.room = getRooms;
            }
        }

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
                    $('#room').val('');
                    $scope.roomsList.unshift(data);
                    Notification.success('Room successfully added.');
                }, function (error) {
                    $('#room').val('');
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

                        $scope.room.Members.forEach(function (index, member) {
                            if (index !== 0) {
                                $scope.room.Members.splice($scope.room.Members.indexOf(member))
                            }
                        });
                    }
                    $timeout($scope.getLatestLeftUsers(roomId), 1);
                }, function (error) {
                    Notification.error(error.Message);
                })
        }
    });