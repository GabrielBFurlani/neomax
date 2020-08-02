(function () {

    angular.module("app").controller('processAdminListController', ['$scope', '$state', '$http', 'CONFIG', '$uibModal', 'userControl', function ($scope, $state, $http, CONFIG, $uibModal, userControl) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.maxVisiblePages = 5;

        $scope.paginationResponse = {
            totalResults: 0,
            resultsPerPage: null,
            response: {}
        };

        $scope.processes = [

        ];

        $scope.isFilterOpen = false;

        $scope.filter = {
            nameOrUsername: '',
            idProfile: null,
            pageNumber: 1,
            resultsPerPage: 10
        };

        //date picker 
        $scope.datePicker = {
            opened: false
        };

        //Button: Open datepicker
        $scope.datePickerOpen = function () {
            $scope.datePicker.opened = true;
        };

        //loadPage()
        function loadPage() {

            $http.get(_apiUrl + '/clients/solicitationStatus')
                .then(function successCallback(response) {
                    console.log(response);

                    $scope.processStatus = response.data.resultData;

                    $scope.search();
                })
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

            $http.post(_apiUrl + '/solicitations/admin/search', $scope.filter)
                .then(function successCallback(response) {
                    console.log(response);

                    $scope.paginationResponse = response.data.resultData;
                    $scope.processes = $scope.paginationResponse.response;
                })
        }

        //Button: Edit User
        $scope.detail = function (id) {
            $state.go('panel.process.admin.detail', { idProcess: id });
        };

        loadPage();

    }]);
}());