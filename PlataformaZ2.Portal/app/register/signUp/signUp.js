(function () {

    angular.module("app").controller('signUpController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', 'userControl', '$uibModal', function ($scope, $state, $http, CONFIG, $stateParams, userControl, $uibModal) {

        var _apiUrl = CONFIG.apiRootUrl;
        var id = $stateParams.idUser;
        var steps = 6;
        $scope.step = 1;
        $scope.days = CONFIG.weekdaysEnum;
        $scope.times = CONFIG.timeEnum;
        $scope.companyNatureTypes = CONFIG.companyNatureEnum;
        $scope.annualBillingTypes = CONFIG.annualBillingEnum;
        $scope.noteTypes = CONFIG.issuedNoteEnum;
        $scope.telephoneTypes = CONFIG.telephoneTypes;
        $scope.genderTypes = CONFIG.genderTypes;

        $scope.fileUpload = {
            fileList: []
        }

        $scope.stepsNames = ["1 - Dados Pessoais", "2 - Seus Documentos e Filiações", "3 - Residência", "4 - Informações Complementares",
            "5 - Foto do Perfil", "6 - Comprovante de Anexos"];

        //test email
        var regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        //test nome
        var regexName = /^((\b[A-zÀ-ú']{2,40}\b)\s*){2,}$/;

        function setProgressBar() {
            var percent = parseFloat(100 / steps) * $scope.step - 1;
            percent = percent.toFixed();
            $(".progress-bar")
                .css("width", percent + "%")
        }

        $scope.client = {
            banks: []
        };

        $scope.newBankErrors = {};

        //Load Page
        function loadPage() {

            setProgressBar();

            /*  
              //get user data
              $http.get(_apiUrl + '/user/userArea/' + id)
                  .then(function successCallback(response) {
  
                      $scope.user = response.data.resultData;
  
                      //checks if the user has photo
                      if ($scope.user.photo) {
                          $scope.hasPhoto = true;
                      }
                      else {
                          $scope.hasPhoto = false;
                      }
                  });   */
        }

        $scope.cnpjPayingFormater = function () {

            if (!$scope.client.cnpjPaying)
                return;

            if ($scope.client.cnpjPaying.replace(/[^\d]+/g, '').length >= 15) {
                $scope.client.cnpjPaying = $scope.client.cnpjPaying.replace(/[^\d]+/g, '').substring(0, 14);
            }

            $scope.cnpjPayingChecked = checkCNPJ($scope.client.cnpjPaying);

            var vr = $scope.client.cnpjPaying.replace(/[^\d]+/g, '');
            tam = vr.length + 1;
            if (tam == 3)
                $scope.client.cnpjPaying = vr.substr(0, 2) + '.';
            if (tam == 6)
                $scope.client.cnpjPaying = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.';
            if (tam == 10)
                $scope.client.cnpjPaying = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8, 4);
            if (tam == 15)
                $scope.client.cnpjPaying = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8, 4) + '-' + vr.substr(12, 2);
        }

        $scope.cpfCnpjFormater = function () {

            if ($scope.client.cpfcnpj.replace(/[^\d]+/g, '').length >= 15) {
                $scope.client.cpfcnpj = $scope.client.cpfcnpj.replace(/[^\d]+/g, '').substring(0, 14);
            }

            //cpf
            if ($scope.client.cpfcnpj && $scope.client.cpfcnpj.length <= 11) {

                $scope.cpfcnpjChecked = checkCPF($scope.client.cpfcnpj);

                var vr = $scope.client.cpfcnpj.replace(/[^\d]+/g, '');
                tam = vr.length + 1;
                if (tam == 4)
                    $scope.client.cpfcnpj = vr.substr(0, 3) + '.';
                if (tam == 7)
                    $scope.client.cpfcnpj = vr.substr(0, 3) + '.' + vr.substr(3, 3) + '.';
                if (tam == 10)
                    $scope.client.cpfcnpj = vr.substr(0, 3) + '.' + vr.substr(3, 3) + '.' + vr.substr(6, 3) + '-' + vr.substr(9, 4);
                if (tam == 15)
                    $scope.client.cpfcnpj = vr.substr(0, 3) + '.' + vr.substr(3, 3) + '.' + vr.substr(6, 3) + '-' + vr.substr(9, 4);
            }
            //cnpj
            else {
                $scope.cpfcnpjChecked = checkCNPJ($scope.client.cpfcnpj);

                var vr = $scope.client.cpfcnpj.replace(/[^\d]+/g, '');
                tam = vr.length + 1;
                if (tam == 3)
                    $scope.client.cpfcnpj = vr.substr(0, 2) + '.';
                if (tam == 6)
                    $scope.client.cpfcnpj = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.';
                if (tam == 10)
                    $scope.client.cpfcnpj = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8, 4);
                if (tam == 15)
                    $scope.client.cpfcnpj = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8, 4) + '-' + vr.substr(12, 2);
            }
        }

        function checkCPF(strCPF) {
            var Soma;
            var Resto;
            Soma = 0;
            if (strCPF == "00000000000") return false;

            for (i = 1; i <= 9; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
            Resto = (Soma * 10) % 11;

            if ((Resto == 10) || (Resto == 11)) Resto = 0;
            if (Resto != parseInt(strCPF.substring(9, 10))) return false;

            Soma = 0;
            for (i = 1; i <= 10; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
            Resto = (Soma * 10) % 11;

            if ((Resto == 10) || (Resto == 11)) Resto = 0;
            if (Resto != parseInt(strCPF.substring(10, 11))) return false;
            return true;
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

        //Management bank
        $scope.addBank = function () {
            $scope.newBankErrors.bankRequired = false;
            $scope.newBankErrors.agencyRequired = false;
            $scope.newBankErrors.accountRequired = false;

            if (!$scope.newBank.bank) {
                $scope.newBankErrors.bankRequired = true;
            }

            if (!$scope.newBank.agency) {
                $scope.newBankErrors.agencyRequired = true;
            }

            if ($scope.newBank.parameter == 4 && !$scope.newBank.account) {
                $scope.newBankErrors.accountRequired = true;
            }

            if ($scope.newBankErrors.bankRequired || $scope.newBankErrors.agencyRequired || $scope.newBankErrors.accountRequired) {
                return;
            }

            $scope.client.banks.push({
                account: $scope.newBank.account,
                bank: $scope.newBank.bank.parameter,
                agency: $scope.newBank.agency
            });

            $scope.newBank = {
                account: null,
                bank: null,
                agency: null,
            }
        }

        $scope.editBank = function (bank, index) {

            if ($scope.newBank.bank && $scope.newBank.account && $scope.newBank.agency) {
                $scope.addBank();
            }

            var editBank = {};

            angular.copy(bank, editBank);

            $scope.newBank.account = editBank.account;

            $scope.newBank.bank = editBank.bank;

            $scope.newBank.agency = editBank.agency;

            $scope.client.banks.splice(index, 1);
        }

        $scope.next = function () {

            if ($scope.step == 4)
                $state.go('login');
            else
                $scope.step++;
        }

        /* Save form 1 */
        $scope.saveForm1 = function () {
            //if ($scope.form1.$valid && $scope.cidadao.banks.length > 0) {
            //    loadIssueBody();
            //}

            $scope.step = 2;
        }

        /* Save form 2 */
        $scope.saveForm2 = function () {
            //if ($scope.form2.$valid) {

            //};
            $scope.step = 3;
        }

        /* Save forms 3 */
        $scope.saveForm3 = function () {
            //if ($scope.form3.$valid) {
            //    $scope.step = 4;
            //    loadEducationLevels();
            //    loadEducationUnits();
            //    loadDisabilityTypes();
            //    loadLocomotionEquipaments();
            //}

            $scope.step = 4;
        }

        /* Save forms 4 */
        $scope.saveForm4 = function () {
            //if ($scope.form4.$valid) {
            //};

            $scope.step = 5;

        }

        $scope.back = function () {

            if ($scope.step == 1) {
                //back to menu
                $state.go('login');
            }
            else {
                $scope.step--;
            }
        }

        //procedural script
        loadPage();

    }]);
}());