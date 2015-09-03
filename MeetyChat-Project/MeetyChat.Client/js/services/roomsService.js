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

            addRoom: function (roomModel) {
                var url = BASE_URL + '/rooms';

                var deferred = $q.defer();
                $http.post(url, roomModel)
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });

                return deferred.promise;
            },

            deleteRoom : function (room) {
                var url = BASE_URL + '/rooms/' + room.Id;

                var deferred = $q.defer();
                $http.delete(url)
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });

                return deferred.promise;
            },

            getLatestUsers: function (roomId) {
                var url = BASE_URL + '/rooms/' + roomId + '/users/latest/joined';

                var deferred = $q.defer();
                $http.get(url)
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });

                return deferred.promise;
            },

            getLatestLeftUsers: function (roomId) {
                var url = BASE_URL + '/rooms/' + roomId + '/users/latest/left';

                var deferred = $q.defer();
                $http.get(url)
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });

                return deferred.promise;
            }
        }});