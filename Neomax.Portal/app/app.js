(function () {
    'use strict';

    angular.module("app", ['ui.router', 'ui.bootstrap', 'angularValidator', 'ui.mask', 'ui.utils.masks', 'ngAnimate', 'ngSanitize', 'ngToast', 'blockUI', 'ngImgCrop', 'ngStorage', 'ngCookies', 'ui.select']);

    angular.module('app').constant('CONFIG', {
        apiRootUrl: 'http://localhost:59000/api',
        apiViaCepUrl: 'https://viacep.com.br/ws',
        yesNoStatus: {
            false: "Não",
            true: "Sim",
            null: "Não Utilizado"
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