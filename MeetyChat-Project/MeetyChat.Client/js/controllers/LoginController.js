'use strict';

meetyChatApp.controller('LoginController',
    function LoginController($scope, $location, authService, loginService, Notification) {

        $scope.login = function (userData) {
            loginService.login(userData)
                .then(function (data) {
                    authService.setCredentials(data);
                    Notification.success('Successfully logged in!');
                    $location.path('/');
                }, function (error) {
                    Notification.error(error.error_description);
                })
        }
    });