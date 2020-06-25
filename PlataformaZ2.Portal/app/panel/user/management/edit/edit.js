(function () {

    angular.module("app").controller('userManagementEditController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', '$uibModal', function ($scope, $state, $http, CONFIG, $stateParams, $uibModal) {
        var _apiUrl = CONFIG.apiRootUrl;
        var id = $stateParams.idUser;
        
        $scope.user = {};
        $scope.hasPhoto = false;

        //"Image input and cropping" functions and variables
        $scope.originalImage = null;
        $scope.croppedImage = null;

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
            /*
            $http.get(_apiUrl + '/profile/all')
                .then(function successCallback(response) {
                    $scope.profiles = response.data.resultData;
                });*/

            //Check if its update operation
            if (id != 0) {

                //get data
                $http.get(_apiUrl + '/user/management/' + id)
                    .then(function successCallback(response) {

                        $scope.user = response.data.resultData;

                        //checks if the user has photo
                        if ($scope.user.photo) {
                            $scope.hasPhoto = true;
                        }
                        else {
                            $scope.hasPhoto = false;
                        }
                    });
            }            
        }

        //watch the variable "$scope.croppedImage" to update the "$scope.user.photo"
        $scope.$watch('croppedImage', function () {
            if ($scope.croppedImage) {

                var indexMimeTypeStart = ($scope.croppedImage).indexOf("data:") + 5;
                var indexMimeTypeEnd = ($scope.croppedImage).indexOf(";base64");
                var indexImageData = ($scope.croppedImage).indexOf("base64,") + 7;

                $scope.user.photo = {
                    mimeType: ($scope.croppedImage).substring(indexMimeTypeStart, indexMimeTypeEnd),
                    imageData: ($scope.croppedImage).substring(indexImageData)
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
            
            $http.post(_apiUrl + '/user/management/save', $scope.user)
                .then(function successCallback(response) {
                    $state.go("panel.user.management.list");
                })
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