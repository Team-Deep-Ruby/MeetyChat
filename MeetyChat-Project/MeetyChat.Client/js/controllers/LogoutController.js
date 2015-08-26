'use strict';

meetyChatApp.controller('LogoutController',
    function LogoutController($scope, $location, authService, logoutService, Notification) {

        $scope.logout = function () {
            logoutService.logout()
                .then(function (data) {
                    authService.clearCredentials();
                    Notification.success(data.message);
                    $location.path('/');
                }, function (error) {
                    Notification.error(error.message);
                })
        }
    });