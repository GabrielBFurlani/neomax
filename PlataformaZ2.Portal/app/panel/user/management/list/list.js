(function () {

    angular.module("app").controller('userManagementListController', ['$scope', '$state', '$http', 'CONFIG', '$uibModal', 'userControl', function ($scope, $state, $http, CONFIG, $uibModal, userControl) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.maxVisiblePages = 5;

        $scope.paginationResponse = {
            totalResults: 0,
            resultsPerPage: null,
            response: {}
        };

        $scope.isFilterOpen = false;

        $scope.filter = {
            nameOrUsername: '',
            idProfile: null,
            pageNumber: 1
        };

        //loadPage()
        function loadPage() {
            /*
            $http.get(_apiUrl + '/profile/all')
                .then(function successCallback(response) {
                    $scope.profiles = response.data.resultData;                   
                });

            $scope.search();
        */
        }

        //Button: Search
        $scope.newSearch = function () {

            //set filter back to first page
            $scope.filter.pageNumber = 1;

            //search
            $scope.search();
        };

        //Search (also called when pagination changes)
        $scope.search = function () {

            $http.post(_apiUrl + '/user/management/search', $scope.filter)
                .then(function successCallback(response) {

                    $scope.paginationResponse = response.data.resultData;;
                    $scope.users = $scope.paginationResponse.response;
                })
        }

        //Button: Edit User
        $scope.edit = function (id) {
            $state.go('panel.user.management.edit', { idUser: id });
        };

        //Button: Delete User
        $scope.delete = function (id) {
            var modalInstance = $uibModal.open({
                templateUrl: "app/panel/user/modals/user-delete.html",
                controller: "userDelete"
            });

            modalInstance.result.then(function (idUser) {

                $http.delete(_apiUrl + '/user/management/' + id)
                    .then(function successCallback(response) {
                        loadPage();
                    })
            });
        };

        //Button: Back
        $scope.back = function () {
            $state.go("panel.home");
        }

        loadPage();

    }]);
}());