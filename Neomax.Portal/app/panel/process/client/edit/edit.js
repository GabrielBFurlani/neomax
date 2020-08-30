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

        $scope.products = { products: [], idclient: null };

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


            if (id == 0 || id == null) {
                //create

                $http.post(_apiUrl + '/solicitations', $scope.products)
                    .then(function successCallback(response) {
                        $state.go('panel.process.client.list')
                    })
            }
            else {
                //update
            }

        }

        //Button: Back
        $scope.back = function () {
            if ($scope.step == 1)
                $state.go("panel.process.client.list");

            if ($scope.step == 2)
                $scope.step--;
        }

        $scope.add = function () {

            $scope.products.products.push({ productType: $scope.productSelected == 1 ? "Direito de Endosso" : "Direito de Crédito", title: $scope.newProduct.title, nFiles: $scope.fileUpload.fileList.length, files: null, CNPJPayingSource: $scope.newProduct.CNPJPayingSource })

            $scope.newProduct = {
                title: ""
            };

            $scope.productSelected = null;

            $scope.step--;
        }


        $scope.cnpjPayingFormater = function () {

            if (!$scope.newProduct.CNPJPayingSource)
                return;

            if ($scope.newProduct.CNPJPayingSource.replace(/[^\d]+/g, '').length >= 15) {
                $scope.newProduct.CNPJPayingSource = $scope.newProduct.CNPJPayingSource.replace(/[^\d]+/g, '').substring(0, 14);
            }

            $scope.CNPJPayingSourceChecked = checkCNPJ($scope.newProduct.CNPJPayingSource);

            var vr = $scope.newProduct.CNPJPayingSource.replace(/[^\d]+/g, '');
            tam = vr.length + 1;
            if (tam == 3)
                $scope.newProduct.CNPJPayingSource = vr.substr(0, 2) + '.';
            if (tam == 6)
                $scope.newProduct.CNPJPayingSource = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.';
            if (tam == 10)
                $scope.newProduct.CNPJPayingSource = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8, 4);
            if (tam == 15)
                $scope.newProduct.CNPJPayingSource = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8, 4) + '-' + vr.substr(12, 2);
        }

        function checkCNPJ(cnpj) {

            cnpj = cnpj.replace(/[^\d]+/g, '');

            if (cnpj == '') return null;

            if (cnpj.length != 14)
                return null;

            // Elimina CNPJs invalidos conhecidos
            if (cnpj == "00000000000000" ||
                cnpj == "11111111111111" ||
                cnpj == "22222222222222" ||
                cnpj == "33333333333333" ||
                cnpj == "44444444444444" ||
                cnpj == "55555555555555" ||
                cnpj == "66666666666666" ||
                cnpj == "77777777777777" ||
                cnpj == "88888888888888" ||
                cnpj == "99999999999999")
                return false;

            // Valida DVs
            tamanho = cnpj.length - 2
            numeros = cnpj.substring(0, tamanho);
            digitos = cnpj.substring(tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(0))
                return false;

            tamanho = tamanho + 1;
            numeros = cnpj.substring(0, tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(1))
                return false;

            return true;

        }

        //procedural script
        loadPage();

    }]);
}());