(function () {

    angular.module("app").controller('clientDetailController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', '$uibModal', 'fileDownloadUpload', function ($scope, $state, $http, CONFIG, $stateParams, $uibModal, fileDownloadUpload) {
        var _apiUrl = CONFIG.apiRootUrl;
        var id = $stateParams.idClient;
        var fromLocation = $stateParams.fromLocation;
        var idSolicitation = $stateParams.idSolicitation;

        $scope.client = {};

        //if ($scope.id == 1) {
        //    $scope.client = { name: 'João Antonio', cpf: '78803885161', cnpj: '38042126000100', id: 1, telephones: "19993162405 / 33427677", gender: "Masculino", nickname: "Joâo", email: "joaoAntonio459@gmail.com"};
        //}
        //else {
        //    $scope.client = {
        //        name: 'Pedro Dotaviano', cpf: '62311649850', cnpj: '33983543000125', id: 2, telephones: "19993162405 / 33427677", gender: "Masculino", nickname: "Pedro", email: "pedrotaviano@gmail.com",
        //        noteType: "Nota de Produto", annualBilling: "De R$ 1 milhão até R$ 2 milhões", companyNature: "Empresa de Responsabilidade Limitada (LTDA)", contactDays: "Quarta/Quinta/Sexta - 09h/10h/11h/14h/15h/16h",
        //        banks: [{ bank: "Banco do Brasil", agency: "098556", account: "12659" }]
        //    };

        //}

        //Load Page
        function loadPage() {
            /*
            $http.get(_apiUrl + '/profile/all')
                .then(function successCallback(response) {
                    $scope.profiles = response.data.resultData;
                });*/

            //Check if its update operation
            if (id != 0) {

                //get data
                $http.get(_apiUrl + '/user/userArea/' + id)
                    .then(function successCallback(response) {

                        $scope.user = response.data.resultData;
                        $scope.client = $scope.user.clientDto;

                        //checks if the user has photo
                        if ($scope.user.photo) {
                            $scope.hasPhoto = true;
                        }
                        else {
                            $scope.hasPhoto = false;
                        }

                        console.log($scope.user);

                        if ($scope.user.photo != null) {

                            var initialDataPatch = $scope.user.photo.mimeType == "image/png" ? "data:image/png;base64," : ($scope.user.photo.mimeType == "application/pdf" ? "data:application/pdf;base64," : "data:image/jpeg;base64,/9j/");

                            $scope.user.photo.fileData = initialDataPatch + "" + $scope.user.photo.imageBase64;
                        }

                        $scope.client.listDocumentsBase64.map(function (item) {

                            var initialDataPatch = item.mimeType == "image/png" ? "data:image/png;base64," : "data:image/jpg;base64,/9j/";

                            item.fileData = initialDataPatch + "" + item.imageBase64;
                        })
                    });

            }
        }

        $scope.downloadFile = function (idFile) {
            fileDownloadUpload.downloadFileToUrl(_apiUrl + '/user/anexo/baixar/' + idFile);
        };

        //Button: Edit User
        $scope.edit = function (id) {
            //$state.go('panel.user.management.edit', { idUser: id });


            //open modal
            var modalInstance = $uibModal.open({
                templateUrl: 'app/panel/client/modals/accept-client.html',
                controller: 'acceptClientController'
            });

        };

        //Button: Save
        $scope.save = function () {

        }

        //Button: Back
        $scope.back = function () {
            if (fromLocation == 1)
                $state.go("panel.client.list");
            else
                $state.go('panel.process.admin.detail', { idProcess: idSolicitation });
        }

        //procedural script
        loadPage();

    }]);
}());