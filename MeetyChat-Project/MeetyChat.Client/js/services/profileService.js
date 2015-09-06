'use strict';

meetyChatApp.factory('profileService',
    function profileService($http, $q, BASE_URL) {
        return {
            getDataAboutMe: function () {
                var deferred = $q.defer();
                $http.get(BASE_URL + '/profile')
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error);
                    });
                return deferred.promise;
            },

            editProfile: function (data) {
                var deferred = $q.defer();
                $http.put(BASE_URL + '/profile', data)
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });

                return deferred.promise;
            },

            changePassword: function (data) {
                var deferred = $q.defer();
                $http.put(BASE_URL + '/profile/changepassword', data)
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });

                return deferred.promise;
            }
        };
    }
);