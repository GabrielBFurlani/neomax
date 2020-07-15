(function () {

    angular.module("app").controller('userAreaEditController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', 'userControl', '$uibModal', function ($scope, $state, $http, CONFIG, $stateParams, userControl, $uibModal) {
        var _apiUrl = CONFIG.apiRootUrl;
        
        $scope.user = {};
        $scope.hasPhoto = false;
        
        //Load Page
        function loadPage() {
            
            //get user data
            $http.get(_apiUrl + '/user/userSession/' + $scope.userControl.userSession.username)
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

        //Button: Delete Photo
        $scope.deletePhoto = function () {
            $scope.user.photo = null;
            $scope.hasPhoto = false;
        };
                
        //Button: Save
        $scope.save = function () {
            
            $http.post(_apiUrl + '/user/userArea/update', $scope.user)
                .then(function successCallback(response) {

                    //get the updated user session
                    $state.go("panel.home");
                })
        }

        //Button: Back
        $scope.back = function () {
            $state.go("panel.home");
        }

        //procedural script
        loadPage();
        
    }]);
}());