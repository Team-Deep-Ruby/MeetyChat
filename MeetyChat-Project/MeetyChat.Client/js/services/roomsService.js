'use strict';

meetyChatApp.factory('roomsService',
    function roomsService($http, $q, BASE_URL) {
        var hasJoinedRoom = false;

        return {

            getAllRooms: function () {
                var deferred = $q.defer();
                $http.get(BASE_URL + '/publicRooms', {
                    ignoreLoadingBar: true
                })
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error);
                    });
                return deferred.promise;
            },

            getPrivateRooms: function () {
                var deferred = $q.defer();
                $http.get(BASE_URL + '/privateRooms', {
                    ignoreLoadingBar: true
                })
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
                $http.get(BASE_URL + '/publicRooms/' + id, {
                    ignoreLoadingBar: true
                })
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });
                return deferred.promise;
            },

            getPrivateRoomById: function (id) {
                var deferred = $q.defer();
                $http.get(BASE_URL + '/privateRooms/' + id, {
                    ignoreLoadingBar: true
                })
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
                $http.put(BASE_URL + '/publicRooms/' + room.Id + '/join')
                    .success(function (data) {
                        hasJoinedRoom = true;
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });
                return deferred.promise;
            },

            leaveRoom: function (id) {
                var deferred = $q.defer();
                if (hasJoinedRoom) {
                    $http.put(BASE_URL + '/publicRooms/' + id + '/leave')
                        .success(function (data) {
                            hasJoinedRoom = false;
                            deferred.resolve(data);
                        })
                        .error(function (error) {
                            deferred.reject(error)
                        });
                    return deferred.promise;
                }
            },

            addRoom: function (roomModel) {
                var url = BASE_URL + '/publicRooms';

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

            addPrivateRoom: function (privateRoomModel) {
                var url = BASE_URL + '/privateRooms/';

                var deferred = $q.defer();
                $http.post(url, privateRoomModel)
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });

                return deferred.promise;
            },

            deleteRoom: function (room) {
                var url = BASE_URL + '/publicRooms/' + room.Id;

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
                var url = BASE_URL + '/publicRooms/' + roomId + '/users/latest/joined';

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
                var url = BASE_URL + '/publicRooms/' + roomId + '/users/latest/left';

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

            getUsersByRoom: function (roomId) {
                var url = BASE_URL + '/publicRooms/' + roomId + '/users';

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
        }
    });