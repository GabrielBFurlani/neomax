(function () {
    "use strict";
    angular.module('app').service('userControl', ['$http', '$cookies', '$state', '$localStorage', 'CONFIG', function ($http, $cookies, $state, $localStorage, CONFIG) {
        var _apiUrl = CONFIG.apiRootUrl;

        var service = {};
        service.userSession = undefined;

        //.................... LocalStorage reference to save data into Browser's Local Storage
        service.storage = $localStorage.$default();
        try {
            service.userSession = service.storage.userSession;
        }
        catch (err) { }
        
        //.................... Log-in into the system
        service.login = function (credentials) {
            
            $http.post(_apiUrl + '/user/login', credentials)
                .then(function successCallback(response) {

                    var session = response.data.resultData;

                    $cookies.put('username', session.username, { path: '/' });
                    $cookies.put('accessToken', session.accessToken, { path: '/' });

                    service.userSession = session;
                    service.storage.userSession = session;

                    $state.go('panel.home'); 
                });            
        }

        //.................... Recovery user session from API using data stored on cookies
        service.recoverUserSession = function (username) {
            
            if (username) {
                $http.get(_apiUrl + '/user/userSession/' + username + '/')
                    .then(function successCallback(response) {

                        var session = response.data.resultData;

                        service.userSession = session;
                        service.storage.userSession = session;

                        $state.go('panel.home');
                    });
            }            
        }
        
        //.................... Log-out from the system
        service.logout = function () {            
            $cookies.remove('username', { path: '/' });
            $cookies.remove('accessToken', { path: '/' });
            
            service.userSession = undefined;            
            delete service.storage.userSession;

            $state.go('login');
        }

    	return service;       
        
	}]);
}());