'use strict';

meetyChatApp.controller('UsersController',
    function profileController($scope, $location, Notification, usersService, $timeout, $route) {

        $scope.getOnlineUsers = function () {
            usersService.getOnlineUsers()
                .then(function (data) {
                    $scope.onlineUsers = filterByName(data);
                    $timeout(function () {
                        if ($route.current.$$route.originalPath == '/rooms') {
                            $scope.getOnlineUsers();
                        }
                    }, 20000);
                }, function error(error) {
                    Notification.error(error.Message);
                }
            )
        };
    }
);

function filterByName(arr) {
    var f = [];
    return arr.filter(function (n) {
        return f.indexOf(n.Name) == -1 && f.push(n.Name)
    })
}