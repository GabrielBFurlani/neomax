(function () {

    angular.module("app").controller('processClientEditController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', '$uibModal', function ($scope, $state, $http, CONFIG, $stateParams, $uibModal) {
        var _apiUrl = CONFIG.apiRootUrl;
        var id = $stateParams.idProcess;

        $scope.user = {};

        $scope.step = 1;

        $scope.hasPhoto = false;
        $scope.newPhoto = false;
        $scope.loadPhoto = false;

        $scope.conteudoArquivoFoto = null;

        $scope.productSelected = null;

        $scope.fileUpload = {
            fileList: []
        }

        $scope.newProduct = {
            title: ""
        };

        $scope.products = [];

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

        loadPhoto = function (brazon) {

        /*    if (brazon != null) {

                $scope.newPhoto = true;
                $scope.hasPhoto = false;
                $scope.client.brasao = {
                    imageData: "",
                    mimeType: brazon.type
                }

                const reader = new FileReader();
                reader.readAsDataURL(brazon);
                $scope.loadPhoto = true;
                reader.onload = () => $scope.conteudoArquivoFoto = reader.result;
                reader.onerror = error => reject(error);
                reader.onloadend = () => {
                    $scope.loadPhoto = false;
                    $scope.hasPhoto = true;
                    document.getElementsByTagName('img')[1].src = $scope.conteudoArquivoFoto;
                    $scope.client.brasao.imageData = $scope.conteudoArquivoFoto;
                }

            }*/

        }

        $scope.$watch('fileUpload.fileList', function (newVal, oldVal) {
            if (newVal) {
                loadPhoto(newVal[0]);
            }
            else {
            }
        });

        //Button: Save
        $scope.save = function () {

            //open modal
            var modalInstance = $uibModal.open({
                templateUrl: 'app/panel/client/modals/accept-client.html',
                controller: 'acceptClientController'
            });

        }

        //Button: Back
        $scope.back = function () {
            if($scope.step == 1)
                $state.go("panel.process.client.list");

            if ($scope.step == 2)
                $scope.step--;
        }

        $scope.add = function () {

            $scope.products.push({ type: $scope.productSelected == 1 ? "Direito de Endosso" : "Direito de Crédito", title: $scope.newProduct.title, files: $scope.fileUpload.fileList.length })

            $scope.newProduct = {
                title: ""
            };

            $scope.productSelected = null;

            $scope.step--;  
        }

        //procedural script
        loadPage();

    }]);
}());