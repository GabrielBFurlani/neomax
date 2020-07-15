(function () {
    'use strict';

    angular.module("app", ['ui.router', 'ui.bootstrap', 'angularValidator', 'ui.mask', 'ui.utils.masks', 'ngAnimate', 'ngSanitize', 'ngToast', 'blockUI', 'ngImgCrop', 'ngStorage', 'ngCookies', 'ui.select']);

    angular.module('app').constant('CONFIG', {
        apiRootUrl: 'http://localhost:59000/api',
        apiViaCepUrl: 'https://viacep.com.br/ws',
        weekdaysEnum: {
            0: "Domingo",
            1: "Segunda-Feira",
            2: "Terça-Feira",
            3: "Quarta-Feira",
            4: "Quinta-Feira",
            5: "Sexta-Feira",
            6: "Sábado"
        },
        timeEnum: {
            0: "09:00h até as 10:00h",
            1: "10:00h até as 11:00h",
            2: "11:00h até as 12:00h",
            3: "12:00h até as 13:00h",
            4: "13:00h até as 14:00h",
            5: "14:00h até as 15:00h",
            6: "15:00h até as 16:00h",
            7: "16:00h até as 17:00h",
            8: "17:00h até as 18:00h",
        },
        annualBillingEnum: {
            0: "Até R$ 1 milhão",
            1: "De R$ 1 milhão até R$ 2 milhões",
            2: "De R$ 2 milhão até R$ 4 milhões",
            3: "Mais que R$ 4 milhões"
        },
        issuedNoteEnum: {
            0: "Nota de Produto",
            1: "Nota de Serviço",
            2: "Nota de Produto e Serviço"
        },
        companyNatureEnum: {
            0: "Empresário Individual (EI)",
            1: "Microempresa (ME)",
            2: "Microempresa Individual (MEU)",
            3: "Empresário Individual de Responsabilidade Limitada (EIRELI)",
            4: "Empresa de Pequeno Porte (EPP)",
            5: "Empresa de Responsabilidade Limitada (LTDA)",
            6: "Sociedade Anônima"
        },
        genderTypes: {
            0: "Masculino",
            1: "Feminino",
            2: "Não Informado/Indiferente",
        },
        telephoneTypes: {
            0: "Fixo",
            1: "Celular",
            2: "Comercial",
        },
        yesNoStatus: {
            false: "Não",
            true: "Sim",
            null: "Não Utilizado"
        },
        processStatus: {
            0: "Aguardando Aprovação",
            1: "Necessita de Revisão",
            2: "Aprovado",
            3: "reprovado"
        }
    });

    angular.module('app').config(function ($httpProvider) {
        $httpProvider.interceptors.push('appHttpInterceptor');
    });

    angular.module('app').config(['ngToastProvider', function (ngToastProvider) {
        ngToastProvider.configure({
            animation: 'slide'
        });
    }]);

    angular.module('app').config(function (blockUIConfig) {
        blockUIConfig.template = '<div style="width: 100%; height: 100%; opacity: .5; background-color: #777;"></div><div class="uil-cube-css" style="-webkit-transform:scale(0.25)"><div></div><div></div><div></div><div></div></div>';
    });

    angular.module('app').run(function ($rootScope, $location, userControl) {
        $rootScope.userControl = userControl;

        $rootScope.$on('$locationChangeStart', function (event, next, current) {

            //check if the request is "external password change"
            /*  var urlRequested = next.toString();
              var indexFound = urlRequested.search("externalPasswordChange");
  
              // redirect to login page if not logged-in AND not trying to change password externally     
              if (!userControl.userSession && indexFound < 0) {
                  $location.path('/login');
              }*/
        });
    });

}());