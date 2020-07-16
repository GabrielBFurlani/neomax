(function () {

    angular.module("app").controller('processClientListController', ['$scope', '$state', '$http', 'CONFIG', '$uibModal', 'userControl', function ($scope, $state, $http, CONFIG, $uibModal, userControl) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.processStatus = CONFIG.processStatus;

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

                    $scope.processes = [
                        { protocolNumber: '0003/2020', situation: $scope.processStatus[1], date: new Date(2020, 04, 19), id: 3 },
                        { protocolNumber: '0002/2020', situation: $scope.processStatus[0], date: new Date(2020, 04, 15), id: 2 },
                        { protocolNumber: '0001/2020', situation: $scope.processStatus[2], date: new Date(2020, 04, 05), id: 1 },
                        { protocolNumber: '0004/2020', situation: $scope.processStatus[3], date: new Date(2020, 04, 03), id: 4 }
                    ];

                    $scope.processesList = [
                        { protocolNumber: '0003/2020', situation: $scope.processStatus[1], date: new Date(2020, 04, 19), id: 3 },
                        { protocolNumber: '0002/2020', situation: $scope.processStatus[0], date: new Date(2020, 04, 15), id: 2 },
                        { protocolNumber: '0001/2020', situation: $scope.processStatus[2], date: new Date(2020, 04, 05), id: 1 },
                        { protocolNumber: '0004/2020', situation: $scope.processStatus[3], date: new Date(2020, 04, 03), id: 4 }
                    ];
                });

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

            $scope.processes = $scope.processesList;

            if ($scope.filter.status != null)
                $scope.processes = $scope.processesList.filter(function (item) { return item.situation == $scope.filter.status })

            if ($scope.filter.protocolNumber != null && $scope.filter.protocolNumber != "")
                $scope.processes = $scope.processesList.filter(function (item) { return item.protocolNumber.includes($scope.filter.protocolNumber) })

            if ($scope.filter.date) {
                console.log($scope.filter.date);
                $scope.processes = $scope.processesList.filter(function (item) { return item.date.getTime() == $scope.filter.date.getTime() })
            }
        }

        //Button: Edit User
        $scope.edit = function (id) {
            $state.go('panel.process.client.edit', { idProcess: id });
        };


        //Button: Back
        $scope.back = function () {
            $state.go("panel.home");
        }

        loadPage();

    }]);
}());