'use strict';

meetyChatApp.factory('messageService',
    function messageService($http, $q, BASE_URL) {
        return {
            getAllMessages: function (id) {
                var deferred = $q.defer();
                $http.get(BASE_URL + '/rooms/' + id + "/messages")
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error);
                    });
                return deferred.promise;
            },

            getLatestMessages: function (id) {
                var deferred = $q.defer();
                $http.get(BASE_URL + '/rooms/' + id + "/messages/latest")
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error);
                    });
                return deferred.promise;
            },

            sendMessage: function (id, content) {
                var url = BASE_URL + '/rooms/' + id + '/messages';

                var deferred = $q.defer();
                $http.post(url, content)
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (error) {
                        deferred.reject(error)
                    });
                return deferred.promise;
            }
        }});