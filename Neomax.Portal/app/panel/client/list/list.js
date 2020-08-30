(function () {

    angular.module("app").controller('clientListController', ['$scope', '$state', '$http', 'CONFIG', '$uibModal', 'userControl', function ($scope, $state, $http, CONFIG, $uibModal, userControl) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.maxVisiblePages = 5;

        $scope.paginationResponse = {
            totalResults: 2,
            ResultsPerPage: 10,
            response:{}
        };

        $scope.clients = [];

        $scope.isFilterOpen = false;

        $scope.filter = {
            nameOrUsername: '',
            idProfile: null,
            pageNumber: 1,
            ResultsPerPage: 10
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

            $http.post(_apiUrl + '/clients/busca', $scope.filter)
                .then(function successCallback(response) {
                    $scope.paginationResponse = response.data.resultData;;
                    $scope.clients = $scope.paginationResponse.response;
                })
        }

        //Button: Edit User
        $scope.detail = function (id) {
            $state.go('panel.client.detail', { idClient: id });
        };

        $scope.back = function () {
            $state.go('panel.home');
        }

        loadPage();

    }]);
}());