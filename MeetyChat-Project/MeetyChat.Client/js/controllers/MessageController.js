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
                        $scope.hasMoreMessages = false;
                    }
                }, function (error){
                    Notification.error(error.Message);
                });
        };

        $scope.getLatestMessages = function () {
            messageService.getLatestMessages($route.current.params.id)
                .then(function (data) {
                    $timeout(function () {
                            $scope.getLatestMessages();
                    }, 1);
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

        $scope.dateFromNow = function (date) {
            return moment(date).add(3, 'hours').fromNow();
        };

        $scope.sendPicture = function (fileInputField) {

            var sizeLimit = 512000;

            var file = fileInputField.files[0];
            if (file && file.type.match(/image\/.*/) && file.size < sizeLimit) {
                var reader = new FileReader();

                reader.onload = function () {
                    var message = {
                        Content: reader.result
                    };

                    messageService.sendMessage($routeParams.id, message)
                        .then(function () {

                            Notification.success('Picture successfully sent.');
                        }, function (error) {
                            Notification.error(error.message);
                        })
                };
                reader.readAsDataURL(file);
            } else {
                file.size > sizeLimit ? Notification.info('Profile picture size limit is 512kb.') :
                    Notification.info('File not supported!');
            }
            fileInputField.value = "";
        };
    });