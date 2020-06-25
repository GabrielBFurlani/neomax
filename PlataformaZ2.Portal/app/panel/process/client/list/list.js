(function () {

    angular.module("app").controller('processClientListController', ['$scope', '$state', '$http', 'CONFIG', '$uibModal', 'userControl', function ($scope, $state, $http, CONFIG, $uibModal, userControl) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.maxVisiblePages = 5;

        $scope.paginationResponse = {
            totalResults: 0,
            resultsPerPage: null,
            response: {}
        };

        $scope.processes = [
            { protocolNumber: '0002/2020', situation: 'Aguardando Aprovação', date: '15/04/2020', id: 2 },
            { protocolNumber: '0001/2020', situation: 'Aprovado', date: '05/04/2020', id: 1 },
        ];

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

        }

        //Button: Edit User
        $scope.edit = function (id) {
            $state.go('panel.process.client.edit', { idProcess: id });
        };

        loadPage();

    }]);
}());