'use strict';

meetyChatApp.controller('RegisterController',
    function RegisterController($scope, $location, authService, registerService, Notification) {

        $scope.register = function (userData) {
            registerService.register(userData)
                .then(function (data) {
                    authService.setCredentials(data);
                    Notification.success('Successfully registered!');
                    $location.path('/');
                }, function (error) {
                    console.log(error);
                    if (error.Message != 'The request is invalid.'){
                        Notification.error(error.Message);
                    } else {
                        var errorMsg = error.ModelState[Object.keys(error.ModelState)[0]][0];
                        Notification.error(errorMsg);
                    }
                })
        }
    });