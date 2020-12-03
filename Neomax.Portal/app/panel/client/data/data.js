(function () {

    angular.module("app").controller('dataController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', 'userControl', '$uibModal', 'validationMessages', 'fileDownloadUpload', function ($scope, $state, $http, CONFIG, $stateParams, userControl, $uibModal, validationMessages, fileDownloadUpload) {

        var _apiUrl = CONFIG.apiRootUrl;
        var steps = 6;
        $scope.step = 1;

        $scope.fileUpload = {
            fileList: []
        }

        $scope.validationMessages = validationMessages;

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

        //Dropdown Multiselect (with Checkboxs)
        $scope.dropdownSettings = {
            checkBoxes: true,
            scrollable: true,
            buttonClasses: 'buttonVisibleColumn btn btn-default'
        };

        //Dropdown Multiselect -- EvaluatingBody
        $scope.dropdownDayTextsStatus = {
            checkAll: 'Selecionar Todos',
            uncheckAll: 'Apagar Todos',
            buttonDefaultText: 'Dias da Semana',
            dynamicButtonTextSuffix: 'Selecionado(s)'
        };

        //Dropdown Multiselect -- EvaluatingBody
        $scope.dropdownTimeTextsStatus = {
            checkAll: 'Selecionar Todos',
            uncheckAll: 'Apagar Todos',
            buttonDefaultText: 'Horários',
            dynamicButtonTextSuffix: 'Selecionado(s)'
        };

        $scope.daysSelecteds = [];

        $scope.timesSelecteds = [];

        $scope.client = {
            banks: [],
            telephones: [],
            contactDays: [],
            ContactTimes: [],
            documents: []
        };

        $scope.newBankErrors = {};
        $scope.newTelephoneErrors = {};

        $scope.newTelephone = {}

        //Load Page
        function loadPage() {

            setProgressBar();

            $http.get(_apiUrl + '/clients/telephoneTypes')
                .then(function successCallback(response) {
                    $scope.telephoneTypes = response.data.resultData;
                })

            $http.get(_apiUrl + '/clients/genderTypes')
                .then(function successCallback(response) {
                    $scope.genderTypes = response.data.resultData;
                })

            $http.get(_apiUrl + '/clients/noteTypes')
                .then(function successCallback(response) {
                    $scope.noteTypes = response.data.resultData;
                })

            $http.get(_apiUrl + '/clients/annualBillingTypes')
                .then(function successCallback(response) {
                    $scope.annualBillingTypes = response.data.resultData;
                })

            $http.get(_apiUrl + '/clients/companyNatureTypes')
                .then(function successCallback(response) {
                    $scope.companyNatureTypes = response.data.resultData;
                })

            $http.get(_apiUrl + '/clients/days')
                .then(function successCallback(response) {
                    $scope.days = response.data.resultData;
                    $scope.daysList = $scope.days.map(function (item) { return { id: item.parameter, label: item.name } });
                })

            $http.get(_apiUrl + '/clients/times')
                .then(function successCallback(response) {
                    $scope.times = response.data.resultData;
                    $scope.timesList = $scope.times.map(function (item) { return { id: item.parameter, label: item.name } });
                })

            $http.get(_apiUrl + '/user/loggedUser')
                .then(function successCallback(response) {
                    console.log(response);
                    $scope.user = response.data.resultData;
                    $scope.client = $scope.user.clientDto;

                    $scope.client.username = $scope.user.username;
                    $scope.client.nickName = $scope.user.nickname;
                    $scope.client.email = $scope.user.email;
                    $scope.client.name = $scope.user.name;
                    $scope.client.CompanyNatureType = $scope.user.clientDto.natureBackground;
                    $scope.client.TypeNoteEmited = $scope.user.clientDto.typeNoteEmited;
                    $scope.client.documents = [];

                    $scope.client.listDocumentsBase64.map(function (item) {

                        var initialDataPatch = item.mimeType == "image/png" ? "data:image/png;base64," : (item.mimeType == "application/pdf" ? "data:application/pdf;base64," : "data:image/jpeg;base64,/9j/");

                        item.fileData = initialDataPatch + "" + item.imageBase64;
                    })

                    console.log($scope.user);

                    $scope.hasPhoto = false;

                    if ($scope.user.photo) {

                        var initialDataPatch = $scope.user.photo.mimeType == "image/png" ? "data:image/png;base64," : "data:image/jpg;base64,/9j/";

                        $scope.conteudoArquivoFotoPerfil = initialDataPatch + $scope.user.photo.imageBase64;

                        $scope.hasPhoto = true;
                    }

                    $scope.client.telephones = [];

                    $scope.client.listTelephones.map(function (item) {
                        $scope.client.telephones.push({
                            contactName: item.contactName,
                            telephoneType: item.telephoneType,
                            telephoneTypeObj: $scope.telephoneTypes[$scope.telephoneTypes.findIndex(x => x.parameter == item.telephoneType)],
                            number: item.number,
                        });
                    })

                    $scope.client.listContactDay.map(function (item) {
                        $scope.daysSelecteds.push(item);
                    })

                    $scope.client.listContactTime.map(function (item) {
                        $scope.timesSelecteds.push(item);
                    })

                    $scope.client.banks = [];

                    $scope.client.listBanks.map(function (item) {
                        $scope.client.banks.push({
                            account: item.account,
                            bank: item.bank,
                            agency: item.agency
                        });
                    })

                })
        }


        $scope.downloadFile = function (idFile) {
            fileDownloadUpload.downloadFileToUrl(_apiUrl + '/user/anexo/baixar/' + idFile);
        };

        $scope.deleteDoc = function (idFile, index) {
            $http.delete(_apiUrl + '/user/anexo/excluir/' + $scope.user.id + "/" + idFile)
                .success(function successCallback(response) {
                    $scope.client.listDocumentsBase64.splice(index, 1);
                })
        }

        $scope.deletePhoto = function () {
            $http.delete(_apiUrl + '/user/foto/excluir/' + $scope.user.id)
                .success(function successCallback(response) {
                    $scope.user.photo = [];
                    $scope.conteudoArquivoFotoPerfil = null;
                    $scope.hasPhoto = false;
                })
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
                bank: $scope.newBank.bank,
                agency: $scope.newBank.agency
            });

            $scope.newBank = {
                account: null,
                bank: null,
                agency: null
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


        //Management phone
        $scope.addPhone = function () {
            $scope.newTelephoneErrors.typeRequired = false;
            $scope.newTelephoneErrors.numberRequired = false;
            $scope.newTelephoneErrors.nameRequired = false;

            if (!$scope.newTelephone.telephoneType) {
                $scope.newTelephoneErrors.typeRequired = true;
            }

            if (!$scope.newTelephone.number) {
                $scope.newTelephoneErrors.numberRequired = true;
            }

            if ($scope.newTelephoneErrors.typeRequired || $scope.newTelephoneErrors.numberRequired || $scope.newTelephoneErrors.nameRequired) {
                return;
            }

            $scope.client.telephones.push({
                contactName: $scope.newTelephone.contactName,
                telephoneType: $scope.newTelephone.telephoneType.parameter,
                telephoneTypeObj: $scope.newTelephone.telephoneType,
                number: $scope.newTelephone.number,
            });

            $scope.newTelephone = {
                contactName: null,
                telephoneType: null,
                telephoneTypeObj: null,
                number: null,
            }
        }

        $scope.editPhone = function (phone, index) {

            var editPhone = {};

            console.log(phone);

            angular.copy(phone, editPhone);

            $scope.newTelephone.contactName = editPhone.contactName;

            $scope.newTelephone.telephoneType = $scope.telephoneTypes[editPhone.telephoneType - 1];

            $scope.newTelephone.number = editPhone.number;

            $scope.client.telephones.splice(index, 1);
        }

        $scope.next = function () {

            if ($scope.step == 5) {

                $scope.client.ContactDays = $scope.daysSelecteds.map(function (item) { return item.id })
                $scope.client.ContactTimes = $scope.timesSelecteds.map(function (item) { return item.id })

                console.log($scope.fileUpload.fileList != null);
                console.log($scope.fileUpload.fileList != []);
                console.log($scope.fileUpload.fileList);


                if ($scope.fileUpload.fileList != null && $scope.fileUpload.fileList.length > 0) {

                    for (var index = 0; index < $scope.fileUpload.fileList.length; index++) {

                        console.log($scope.fileUpload.fileList[index]);

                        $scope.client.documents[index] = {
                            imageBase64: "",
                            mimeType: $scope.fileUpload.fileList[index].type,
                            fileName: $scope.fileUpload.fileList[index].name
                        }

                        const reader = new FileReader();
                        reader.readAsDataURL($scope.fileUpload.fileList[index]);
                        reader.onload = () => $scope.conteudoArquivoFoto = reader.result;
                        reader.onerror = error => reject(error);
                        reader.onloadend = () => {
                            var i = 0;
                            for (achou = false; achou == false;) {
                                if ($scope.client.documents[i].imageBase64 == "")
                                    achou = true;
                                else
                                    i++
                            }

                            $scope.client.documents[i].imageBase64 = $scope.conteudoArquivoFoto;

                            console.log(i);
                            console.log($scope.fileUpload.fileList.length - 1);
                            if (i == $scope.fileUpload.fileList.length - 1) {
                                $http.put(_apiUrl + '/clients', $scope.client)
                                    .success(function successCallback(response) {
                                        $state.go("panel.home");
                                    })
                            }
                        }
                    }
                }
                else {
                    $http.put(_apiUrl + '/clients', $scope.client)
                        .success(function successCallback(response) {
                            $state.go("panel.home");
                        })
                }
            }

            else
                if ($scope.step == 1 && $scope.form1.$valid)
                    $scope.step++;
                else
                    $scope.step++;
        }

        /* Save form 1 */
        $scope.saveForm1 = function () {
            if ($scope.form1.$valid && $scope.client.telephones.length > 0) {
                $scope.step = 2;
            }
        }

        /* Save form 2 */
        $scope.saveForm2 = function () {
            if ($scope.form2.$valid) {
                $scope.step = 3;
            };
        }

        /* Save forms 3 */
        $scope.saveForm3 = function () {
            //if ($scope.form3.$valid) {

            //}

            $scope.step = 4;
        }

        /* Save forms 4 */
        $scope.saveForm4 = function () {
            //if ($scope.form4.$valid) {
            //};

            $scope.step = 5;

        }

        $scope.saveForm5 = function () {
            //if ($scope.form4.$valid) {
            //};

            // $scope.step = 5;

        }

        $scope.back = function () {

            if ($scope.step == 1) {
                //back to menu
                $state.go('panel.home');
            }
            else {
                $scope.step--;
            }
        }

        //procedural script
        loadPage();

    }]);
}());