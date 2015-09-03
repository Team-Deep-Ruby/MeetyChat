'use strict';

meetyChatApp.factory('authService',
    function authService($http, $location, Notification) {
        return {
            isLoggedIn: function () {
                return sessionStorage.currentUser != undefined;
            },

            getCurrentUser: function () {
                var userStorageData = sessionStorage.currentUser;
                if (userStorageData) {
                    return JSON.parse(sessionStorage.currentUser);
                }
            },

            setCredentials: function (data) {
                sessionStorage.currentUser = JSON.stringify(data);
                $http.defaults.headers.common.Authorization =
                    'Bearer ' + data.access_token;
            },

            clearCredentials: function () {
                delete sessionStorage.currentUser;
                delete $http.defaults.headers.common.Authorization;
            },

            isLogged : function () {
                if (!this.isLoggedIn()) {
                    $location.path('/');
                    Notification.info("Please log in.");
                }
            }
    }});