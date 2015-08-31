'use strict';

meetyChatApp.controller('MessageController',
    function MessageController($scope, $http, $route, $location, messageService, $routeParams, Notification, $timeout) {

        $scope.getAllMessages = function () {
            messageService.getAllMessages($routeParams.id)
                .then(function (data) {
                    $scope.messages = data;
                }, function (error) {
                    Notification.error(error.Message);
                })
        };

        $scope.getLatestMessages = function () {
            messageService.getLatestMessages($routeParams.id)
                .then(function (data) {
                    console.log($scope.messages);
                    if ($scope.messages) {
                        $scope.messages.push(data[0]);
                    } else {
                        $scope.messages = data;
                    }
                    $timeout($scope.getLatestMessages, 1);
                }, function (error) {
                    Notification.error(error.Message);
                });
        };

        $scope.sendMessage = function (messageContent) {
             messageService.sendMessage($routeParams.id, messageContent)
                .then(function (data) {
                    $('#messageContent').val('');
                    Notification.success('Message successfully sent.');
                }, function (error) {
                    $('#messageContent').val('');
                    Notification.error(error.message);
                })
        };
    });