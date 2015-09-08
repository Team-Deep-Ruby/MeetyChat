'use strict';

meetyChatApp.controller('ProfileController',
    function profileController($scope, $location, Notification, profileService, authService) {

        $scope.getDataAboutMe = function () {
            profileService.getDataAboutMe()
                .then(function (data) {
                    $scope.userData = data;
                }, function error(error) {
                    Notification.error(error.Message);
                }
            )
        };

        $scope.editProfile = function (data) {
            if (authService.isLoggedIn()) {
                profileService.editProfile(data)
                    .then(function () {
                        Notification.success('Successfully edited profile!');
                        $location.path('/');
                    }, function error(error) {
                        Notification.error(error.Message);
                    }
                )
            }
        };

        $scope.changePassword = function (data) {
            if (authService.isLoggedIn()) {
                profileService.changePassword(data)
                    .then(function () {
                        Notification.success('Successfully changed password!');
                        $location.path('/');
                    }, function (error) {
                        Notification.error(error.Message);
                    }
                )
            }
        };

        $scope.profilePicture = function (fileInputField) {
            var file = fileInputField.files[0];
            if (file.type.match(/image\/.*/)) {
                var reader = new FileReader();
                reader.onload = function () {
                    $scope.userData.profileImage = reader.result;
                    $("#uploadProfileImg").attr('src', reader.result);
                };
                reader.readAsDataURL(file);
            } else {
                $(".image-box").html("<p>File type not supported!</p>");
            }
        };
    }
);