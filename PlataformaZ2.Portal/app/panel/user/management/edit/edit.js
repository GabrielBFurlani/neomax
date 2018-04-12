(function () {

    angular.module("app").controller('userManagementEditController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', '$uibModal', function ($scope, $state, $http, CONFIG, $stateParams, $uibModal) {
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

            $http.get(_apiUrl + '/user/profile/all')
                .then(function successCallback(response) {
                    var httpResultModel = response.data;

                    if (httpResultModel.operationSuccess) {
                        $scope.profiles = httpResultModel.data;
                    }
                });


            //get the user data
            if (id != 0) {

                $http.get(_apiUrl + '/user/management/' + id)
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
        

        //Button: Save
        $scope.save = function () {
            
            //save the user data (if there are no errors)
            if ($scope.user.phones != null && $scope.user.phones.length > 0) {

                $http.post(_apiUrl + '/user/management/save', $scope.user)
                    .then(function successCallback(response) {
                        var httpResultModel = response.data;

                        if (httpResultModel.operationSuccess) {

                            $state.go("panel.user.management.list");
                        }
                    })
            }
        }

        //Button: Back
        $scope.back = function () {
            $state.go("panel.user.management.list");
        }

        //procedural script
        loadPage();

        angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);
        
    }]);
}());