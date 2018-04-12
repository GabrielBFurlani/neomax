(function () {

    angular.module("app").controller('userAreaEditController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', 'userControl', '$uibModal', function ($scope, $state, $http, CONFIG, $stateParams, userControl, $uibModal) {
        var _apiUrl = CONFIG.apiRootUrl;
        var id = $stateParams.idUser;

        $scope.phoneTypes = CONFIG.phoneType;

        $scope.user = {};
        $scope.hasPhoto = false;
        
        //"Image input and cropping" functions and variables
        $scope.originalImage = null;
        $scope.croppedlImage = null;

        var handleFileSelect = function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();

            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.originalImage = evt.target.result;
                });
            };

            reader.readAsDataURL(file);
        };

        //Load Page
        function loadPage() {

            //get the user data
            if (id != 0) {

                $http.get(_apiUrl + '/user/userArea/' + id)
                    .then(function successCallback(response) {
                        var httpResultModel = response.data;

                        if (httpResultModel.operationSuccess) {
                            $scope.user = httpResultModel.data;

                            //checks if the user has photo
                            if ($scope.user.photo) {
                                $scope.hasPhoto = true;
                            }
                            else {
                                $scope.hasPhoto = false;
                            }
                        }
                    });
            }            
        }

        //watch the variable "$scope.croppedlImage" to update the "$scope.user.photo"
        $scope.$watch('croppedlImage', function () {
            if ($scope.croppedlImage) {
                
                var indexMimeTypeStart = ($scope.croppedlImage).indexOf("data:") + 5;
                var indexMimeTypeEnd = ($scope.croppedlImage).indexOf(";base64");
                var indexImageData = ($scope.croppedlImage).indexOf("base64,") + 7;

                $scope.user.photo = {
                    mimeType: ($scope.croppedlImage).substring(indexMimeTypeStart, indexMimeTypeEnd),
                    imageData: ($scope.croppedlImage).substring(indexImageData)                  
                }

                $scope.hasPhoto = true;
            }
        });

        //Button: Delete Photo
        $scope.deletePhoto = function () {
            $scope.user.photo = null;
            $scope.hasPhoto = false;
        };

        //Button: Edit Phone
        $scope.editPhone = function (phone, index) {
            var modalInstance = $uibModal.open({
                templateUrl: "app/panel/user/modals/phone-edit.html",
                controller: "phoneEdit",
                resolve: {
                    phoneDto: function () {

                        if (phone != null) {
                            return angular.copy(phone);
                        }
                        else {
                            return null;
                        }
                    }
                }
            });

            modalInstance.result.then(function (phoneDto) {

                //check if it is phone update or insertion
                if (index != null) {
                    //phone update at specific 'index' (removes one element and substitute for another)
                    $scope.user.phones.splice(index, 1, phoneDto);
                }
                else {
                    //new phone insertion
                    if (!$scope.user.phones) {
                        $scope.user.phones = [];
                    }

                    $scope.user.phones.push(phoneDto);
                }
            });
        };

        //Button: Delete Phone
        $scope.deletePhone = function (index) {
            var modalInstance = $uibModal.open({
                templateUrl: "app/panel/user/modals/phone-delete.html",
                controller: "phoneDelete"
            });

            modalInstance.result.then(function () {
                //delete one element at the 'index' position
                $scope.user.phones.splice(index, 1);
            });
        };

        //Button: Save
        $scope.save = function () {
            
            //save the user data (if there are no errors)
            if ($scope.user.phones != null && $scope.user.phones.length > 0) {

                $http.post(_apiUrl + '/user/userArea/update', $scope.user)
                    .then(function successCallback(response) {
                        var httpResultModel = response.data;

                        if (httpResultModel.operationSuccess) {

                            //get the updated user session
                            $state.go("login");
                        }
                    })
            }
        }

        //Button: Back
        $scope.back = function () {
            $state.go("panel.home");
        }

        //procedural script
        loadPage();

        angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);
        
    }]);
}());