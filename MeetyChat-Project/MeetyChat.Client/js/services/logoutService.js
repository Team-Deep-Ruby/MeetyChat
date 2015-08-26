'use strict';

meetyChatApp.factory('logoutService',
    function logoutService($http, $q, BASE_URL) {
        return {
            logout: function () {
                var deferred = $q.defer();
                $http.post(BASE_URL + '/account/logout')
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });

                return deferred.promise;
            }
        }});