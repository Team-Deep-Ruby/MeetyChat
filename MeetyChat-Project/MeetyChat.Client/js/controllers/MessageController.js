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
            messageService.getLatestMessages($route.current.params.id)
                .then(function (data) {
                    $timeout($scope.getLatestMessages, 0);
                    if (data) {
                        if ($scope.messages) {
                            $scope.messages.unshift(data[0]);
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