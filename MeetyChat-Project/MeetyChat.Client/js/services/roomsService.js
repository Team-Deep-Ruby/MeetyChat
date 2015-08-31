'use strict';

meetyChatApp.factory('roomsService',
    function roomsService($http, $q, BASE_URL) {
        return {
            getAllRooms: function () {
                var deferred = $q.defer();
                $http.get(BASE_URL + '/rooms')
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error);
                    });
                return deferred.promise;
            },

            getRoomById: function (id) {
                var deferred = $q.defer();
                $http.get(BASE_URL + '/rooms/' + id)
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });
                return deferred.promise;
            },

            joinRoom: function (room) {
                var deferred = $q.defer();
                $http.put(BASE_URL + '/rooms/' + room.Id + '/join')
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });
                return deferred.promise;
            },

            leaveRoom: function (id) {
                var deferred = $q.defer();
                $http.put(BASE_URL + '/rooms/' + id + '/leave')
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });
                return deferred.promise;
            },
        }});