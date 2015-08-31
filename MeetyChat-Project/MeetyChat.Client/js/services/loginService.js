'use strict';

meetyChatApp.factory('loginService',
    function loginService($http, $q, BASE_URL) {
        return {
            login: function (userData) {
                var deferred = $q.defer();
                $http.post(BASE_URL + '/account/login', userData)
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });

                return deferred.promise;
            }
    }});