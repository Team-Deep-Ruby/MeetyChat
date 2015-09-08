'use strict';

meetyChatApp.controller('MessageController',
    function MessageController($scope, $http, $route, $location, messageService, $routeParams, Notification, $timeout) {

        $scope.getAllMessages = function () {
            messageService.getAllMessages($routeParams.id)
                .then(function (data) {
                    $scope.hasMoreMessages = false;
                    $scope.messages = data.reverse();
                }, function (error) {
                    Notification.error(error.Message);
                })
        };

        var skip = 0;

        $scope.getMessages = function () {
            messageService.getMessages($route.current.params.id, 5, skip)
                .then(function (data) {
                    $scope.hasMoreMessages = true;
                    skip += 5;
                    if (data.length > 0) {
                        if ($scope.messages) {
                            $scope.messages = data.reverse().concat($scope.messages);
                        } else {
                            $scope.messages = data.reverse();
                        }
                    } else if ($scope.messages) {
                        $scope.hasMoreMessages = false;
                        Notification.info("No more messages.");
                    } else {
                        $scope.hasMoreMessages = true;
                    }
                }, function (error){
                    Notification.error(error.Message);
                });
        };

        $scope.getLatestMessages = function () {
            messageService.getLatestMessages($route.current.params.id)
                .then(function (data) {
                    $timeout($scope.getLatestMessages, 1);
                    if (data) {
                        if ($scope.messages) {
                            $scope.messages.push(data[0]);
                        } else {
                            $scope.messages = data;
                        }
                    }
                }, function (error) {
                    Notification.error(error.Message);
                });
        };

        $scope.sendMessage = function (messageContent) {
             messageService.sendMessage($routeParams.id, messageContent)
                .then(function () {
                    $('#messageContent').val('');
                    Notification.success('Message successfully sent.');
                }, function (error) {
                    $('#messageContent').val('');
                    Notification.error(error.message);
                })
        };
    });