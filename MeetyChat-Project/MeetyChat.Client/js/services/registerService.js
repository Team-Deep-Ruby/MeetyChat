'use strict';

meetyChatApp.factory('registerService',
    function registerService($http, $q, BASE_URL) {
        return {
            register: function (userData) {
                var deferred = $q.defer();
                $http.post(BASE_URL + '/account/register', userData)
                    .success(function (data) {
                        deferred.resolve(data)
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });

                return deferred.promise;
            }
        }});