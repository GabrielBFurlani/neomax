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
            $scope.search();
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

            $http.post(_apiUrl + '/user/search', $scope.filter)
                .then(function successCallback(response) {

                    $scope.paginationResponse = response.data.resultData;;
                    $scope.users = $scope.paginationResponse.response;

                    $scope.users.map(function (item) { item.username = item.username.substring(0, 3) + "." + item.username.substring(3, 6) + "." + item.username.substring(6, 9) + "-" + item.username.substring(9, 11)})
                })
        }

        //Button: Edit User
        $scope.newEmail = function () {
            var modalInstance = $uibModal.open({
                templateUrl: "app/panel/user/modals/add-email-user.html",
                controller: "addEmailUserController"
            });

            modalInstance.result.then(function () {

            });
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