'use strict';

meetyChatApp.controller('LogoutController',
    function LogoutController($scope, $location, authService, logoutService, Notification, roomsService, $routeParams) {

        $scope.logout = function () {
            if ($routeParams.id) {
                roomsService.leaveRoom($routeParams.id)
            }

            logoutService.logout()
                .then(function (data) {
                    authService.clearCredentials();
                    Notification.success(data.message);
                    $location.path('/');
                }, function (error) {
                    Notification.error(error.message);
                });
        }
    });