(function () {
    "use strict";
    angular.module('app').factory('appHttpInterceptor', ['$q', '$cookies', 'ngToast', function ($q, $cookies, ngToast) {
        return {

            // "request" interceptor
            'request': function (config) {

                ////Search for specifics HTTP calls
                if (config.url.indexOf("%updload%") > 0) {

                    ////Call with file upload (URL with %updload%)

                    //erase the "%updload%" from URL
                    config.url = config.url.replace(/%updload%/g, '');

                    //avoid angularJs of serializing the object
                    config.transformRequest = angular.identity;

                    //set authorization field and removes the "Content-Type" to allow "multipart/form-data"
                    if ($cookies.get('accessToken')) {
                        config.headers = {
                            'Content-Type': undefined,
                            'Authorization': 'AccessToken ' + $cookies.get('accessToken')
                        }
                    }
                }
                else if (config.url.indexOf("viacep") > 0) {

                    ////Call to ViaCep API (URL with "viacep")

                    //clear authorization field and set only the "Content-Type"
                    config.headers = {
                        'Content-Type': 'application/json'
                    }
                }
                else {

                    ////Regular call: set authorization field and the "Content-Type"
                    if ($cookies.get('accessToken')) {
                        config.headers = {
                            'Content-Type': 'application/json',
                            'Authorization': 'AccessToken ' + $cookies.get('accessToken')
                        }
                    }
                }

                return config;
            },

            // "requestError" interceptor
            'requestError': function (rejection) {

                //actions to be performed (on error)
                           
                return $q.reject(rejection);
            },


            // "response" interceptor
            'response': function (response) {

                switch (response.status) {
                    //Ok                    
                    case 200:
                        {                    
                            if (response.data.resultMessage) {
                                ngToast.create({
                                    content: response.data.resultMessage,
                                    className: 'success'
                                });
                            }
                        }
                }
                return response;
            },

            // "responseError" interceptor
            'responseError': function (rejection) {
                
                switch (rejection.status) {
                    //Bad Request
                    case 400:
                        {
                            if (rejection.data.message) {
                                ngToast.create({
                                    content: rejection.data.message,
                                    className: 'danger'
                                });
                            }
                            break;
                        }

                    //Unauthorized 
                    case 401:
                        {
                            ngToast.create({
                                content: 'Você não tem permissão para acessar esse recurso. Tente fazer login novamente.',
                                className: 'danger'
                            });

                            //clear cookies and redirect to root page (to force login)
                            $cookies.remove('username', { path: '/' });
                            $cookies.remove('accessToken', { path: '/' });
                            location.href = '#';

                            break;
                        }

                    //Not Found
                    case 404:
                        {
                            ngToast.create({
                                content: 'Recurso não encontrado',
                                className: 'danger'
                            });
                            break;
                        }
                        
                    default:
                        {
                            ngToast.create({
                                content: 'Erro ocorrido',
                                className: 'danger'
                            });
                        }
                }                
                return $q.reject(rejection);
            }

        };
    }]);

}());