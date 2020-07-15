(function () {

    angular.module("app").controller('processAdminDetailController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', '$uibModal', function ($scope, $state, $http, CONFIG, $stateParams, $uibModal) {
        var _apiUrl = CONFIG.apiRootUrl;
        var id = $stateParams.idProcess;

        $scope.user = {};

        $scope.processes = {
            protocolNumber: '0002/2020', clientName: 'Pedro Dotaviano', date: '15/04/2020', cnpj: "80.280.007/0001-43",
            products: [{ id: 2, product: "produto X", situation: 'Aguardando Aprovação'},
                { protocolNumber: '0001/2020', situation: 'Aprovado', clientName: 'Kléber Silva', date: '05/04/2020', id: 1, product: "produto Y" }],
        };

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


        //Button: Edit User
        $scope.edit = function (id) {
            //$state.go('panel.user.management.edit', { idUser: id });


            //open modal
            var modalInstance = $uibModal.open({
                templateUrl: 'app/panel/process/modals/accept-client.html',
                controller: 'defineProcessController'
            });

        };

        //Button: Back
        $scope.back = function () {
            $state.go("panel.process.admin.list");
        }

        //procedural script
        loadPage();

    }]);
}());