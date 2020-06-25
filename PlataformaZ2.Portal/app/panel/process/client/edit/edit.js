(function () {

    angular.module("app").controller('processClientEditController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', '$uibModal', function ($scope, $state, $http, CONFIG, $stateParams, $uibModal) {
        var _apiUrl = CONFIG.apiRootUrl;
        var id = $stateParams.idProcess;

        $scope.user = {};

        //Load Page
        function loadPage() {
            /*
            $http.get(_apiUrl + '/profile/all')
                .then(function successCallback(response) {
                    $scope.profiles = response.data.resultData;
                });*/

            //Check if its update operation
            if (id != 0) {

                /*
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
                    });*/
            }
        }

        //Button: Save
        $scope.save = function () {

            //open modal
            var modalInstance = $uibModal.open({
                templateUrl: 'app/panel/client/modals/accept-client.html',
                controller: 'acceptClientController'
            });

        }

        //Button: Back
        $scope.back = function () {
            $state.go("panel.process.client.list");
        }

        //procedural script
        loadPage();

    }]);
}());