(function () {

    angular.module("app").controller('newUserCreateController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', '$uibModal', function ($scope, $state, $http, CONFIG, $stateParams, $uibModal) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.newUser = { }

        //Load Page
        function loadPage() {

            $http.post(_apiUrl + '/user/newUserLinkValidation/' + $stateParams.newUserToken + '/')
                .then(function successCallback(response) {

                    if (response.data.resultData == null)
                        $state.go("login");

                    else {

                        $scope.newUser = {
                            email: response.data.resultData.email
                        }
                    }
                });
        }

        //Button: Delete Photo
        $scope.deletePhoto = function () {
            $scope.user.photo = null;
            $scope.hasPhoto = false;
        };

        //Button: Save
        $scope.save = function () {

            $http.post(_apiUrl + '/user/registrar', $scope.newUser)
                .then(function successCallback(response) {
                    $state.go("login");
                })
        }

        //procedural script
        loadPage();

    }]);
}());