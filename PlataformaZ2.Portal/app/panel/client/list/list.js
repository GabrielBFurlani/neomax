(function () {

    angular.module("app").controller('clientListController', ['$scope', '$state', '$http', 'CONFIG', '$uibModal', 'userControl', function ($scope, $state, $http, CONFIG, $uibModal, userControl) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.maxVisiblePages = 5;

        $scope.paginationResponse = {
            totalResults: 2,
            resultsPerPage: 10,
            response:
                {}
        };

        $scope.clients = [
            { name: 'João Antonio', cpf: '78803885161', cnpj: '38042126000100', id: 1},
            { name: 'Pedro Dotaviano', cpf: '62311649850', cnpj: '33983543000125', id: 2 }
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
            /*
            $http.post(_apiUrl + '/user/management/search', $scope.filter)
                .then(function successCallback(response) {

                    $scope.paginationResponse = response.data.resultData;;
                    $scope.users = $scope.paginationResponse.response;
                })*/
        }

        //Button: Edit User
        $scope.edit = function () {
            //$state.go('panel.user.management.edit', { idUser: id });

             
            //open modal
            var modalInstance = $uibModal.open({
                templateUrl: 'app/panel/client/modals/accept-client.html',
                controller: 'acceptClientController'
            });

        };

        //Button: Edit User
        $scope.detail = function (id) {
            $state.go('panel.client.detail', { idClient: id });
        };

        loadPage();

    }]);
}());