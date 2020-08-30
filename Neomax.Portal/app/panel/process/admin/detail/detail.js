(function () {

    angular.module("app").controller('processAdminDetailController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', '$uibModal', function ($scope, $state, $http, CONFIG, $stateParams, $uibModal) {
        var _apiUrl = CONFIG.apiRootUrl;
        var id = $stateParams.idProcess;

        $scope.user = {};

        $scope.processes = {
            protocolNumber: '0002/2020', clientName: 'Pedro Dotaviano', date: '15/04/2020', cnpj: "80.280.007/0001-43",
            products: [{ id: 2, product: "produto X", situation: 'Aguardando Aprovação' },
            { protocolNumber: '0001/2020', situation: 'Aprovado', clientName: 'Kléber Silva', date: '05/04/2020', id: 1, product: "produto Y" }],
        };

        //Load Page
        function loadPage() {

            //Check if its update operation
            if (id != 0) {

                $http.get(_apiUrl + '/solicitations/' + id)
                    .then(function successCallback(response) {
                        $scope.processes = response.data;
                    });
            }
        }


        //Button: Edit User
        $scope.edit = function (id, title) {
            //$state.go('panel.user.management.edit', { idUser: id });


            //open modal
            var modalInstance = $uibModal.open({
                templateUrl: 'app/panel/process/modals/accept-client.html',
                controller: 'defineProcessController',
                backdrop: 'static',
                resolve: {
                    title: function () {
                        return title;
                    },
                    id: function () {
                        return id;
                    }
                }
            });

            modalInstance.result.then(function () {
                loadPage();
            });

        };

        //Button: Back
        $scope.back = function () {
            $state.go("panel.process.admin.list");
        }

        $scope.ClientDetails = function (idClient) {
            $state.go('panel.client.detail', { idClient: idClient, fromLocation: 2, idSolicitation: id});
        }

        //procedural script
        loadPage();

    }]);
}());