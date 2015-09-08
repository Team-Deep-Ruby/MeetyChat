'use strict';

meetyChatApp.controller('AppController',
    function AppController($scope, $http, $route, $location, authService) {
        $scope.authService = authService;

        $scope.hasJoinedRoom = false;

        //set headers after refresh
        if (sessionStorage.currentUser != undefined) {
            $http.defaults.headers.common.Authorization = 'Bearer ' + authService.getCurrentUser().access_token;
        }
    });