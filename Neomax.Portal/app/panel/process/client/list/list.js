(function () {

    angular.module("app").controller('processClientListController', ['$scope', '$state', '$http', 'CONFIG', '$uibModal', 'userControl', function ($scope, $state, $http, CONFIG, $uibModal, userControl) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.session = userControl.userSession;

        $scope.processStatus = CONFIG.processStatus;

        $scope.maxVisiblePages = 5;

        $scope.paginationResponse = {
            totalResults: 0,
            resultsPerPage: null,
            response: {}
        };

        $scope.isFilterOpen = false;

        $scope.filter = {
            resultsPerPage: 10,
            nameOrUsername: '',
            idProfile: null,
            pageNumber: 1
        };

        $scope.today = new Date();

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

                    $scope.processStatus = response.data.resultData;

                    $scope.search();

                });
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

            $http.post(_apiUrl + '/solicitations/client/search/' + $scope.session.id, $scope.filter)
                .then(function successCallback(response) {

                    $scope.paginationResponse = response.data.resultData;
                    $scope.processes = $scope.paginationResponse.response;
                })
        }

        //Button: Edit User
        $scope.edit = function (id) {
            $state.go('panel.process.client.edit', { idProcess: id });
        };

        $scope.create = function () {
            $state.go('panel.process.client.create');
        }

        //Button: Back
        $scope.back = function () {
            $state.go("panel.home");
        }

        loadPage();

    }]);
}());