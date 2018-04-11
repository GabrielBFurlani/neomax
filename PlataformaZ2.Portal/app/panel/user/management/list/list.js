(function () {

    angular.module("app").controller('userManagementListController', ['$scope', '$state', '$http', 'CONFIG', '$uibModal', 'userControl', function ($scope, $state, $http, CONFIG, $uibModal, userControl) {
        var _apiUrl = CONFIG.apiRootUrl;
        
        $scope.maxVisiblePages = 5;

        $scope.paginationResponse = {
            totalResults: 0,
            resultsPerPage: null,
            response: {}
        };

        $scope.filter = {
            nameOrUsername: '',
            idProfile: null,
            pageNumber: 1
        };

        function loadPage() {           

            $http.get(_apiUrl + '/profile/all')
                .success(function (httpResultModel) {
                    if (httpResultModel.operationSuccess) {
                        $scope.profiles = httpResultModel.data;
                    }                    
                });

            $scope.search();
        }

        //search using filter (including pagination)
        $scope.search = function () {

            $http.post(_apiUrl + '/user/management/search', $scope.filter)
                .success(function (httpResultModel) {
                    if (httpResultModel.operationSuccess) {
                        $scope.paginationResponse = httpResultModel.data;
                        $scope.users = $scope.paginationResponse.response;
                    }
                    else {
                        $scope.users = null;
                    }
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
                controller: "userDelete",
                resolve: {
                    idUser: function () {
                        return id;
                    }
                }
            });

            modalInstance.result.then(function (idUser) {
                $http.delete(_apiUrl + '/user/management/' + id)
                     .success(function (httpResultModel) {
                         if (httpResultModel.operationSuccess) {
                             loadPage();
                         }                         
                     })
            });
        };
        
        loadPage();

    }]);
}());