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

        $scope.uploadImage = function (fileInputField) {
            var sizeLimit = 128000;

            var file = fileInputField.files[0];
            if (file && file.type.match(/image\/.*/) && file.size < sizeLimit) {
                var reader = new FileReader();
                reader.onload = function () {
                    $('#profileImgPreview').attr('src', reader.result);
                    $scope.userData.NewProfileImage = reader.result;
                };
                reader.readAsDataURL(file);
            } else if (file) {
                $('#profileImgPreview').attr('src', $scope.userData.ProfileImage);
                fileInputField.value = "";
                file.size > sizeLimit ? Notification.info('Profile picture size limit is 128kb.') :
                    Notification.info('File not supported!');
            }
        };
    }
);