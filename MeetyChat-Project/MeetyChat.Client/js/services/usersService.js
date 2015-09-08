'use strict';

meetyChatApp.factory('usersService',
    function usersService($http, $q, BASE_URL) {
        return {
            getOnlineUsers: function () {
                var deferred = $q.defer();
                $http.get(BASE_URL + '/users/active')
                    .success(function (data) {
                        deferred.resolve(data)
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });

                return deferred.promise;
            }
        }});