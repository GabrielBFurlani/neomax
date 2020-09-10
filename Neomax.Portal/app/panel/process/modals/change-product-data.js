(function () {
    "use strict";

    angular.module('app').controller('changeProductDataController', ['$scope', '$http', 'CONFIG', '$uibModalInstance', 'product', function ($scope, $http, CONFIG, $uibModalInstance, product) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.product = angular.copy(product);

        console.log($scope.product);

        //Load Page
        function loadPage() {

        }

        //Button: Sign-Up
        $scope.send = function (status) {

            $http.post(_apiUrl + '/solicitations/product/' + product.id, $scope.product)
                .then(function successCallback(response) {
                    $uibModalInstance.close();
                })

        }

        //Button: Cancel
        $scope.cancel = function (close) {

            if ($scope.openSuggestion && !close)
                $scope.openSuggestion = false;
            else
                $uibModalInstance.dismiss();
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

        loadPage();

    }]);
}());