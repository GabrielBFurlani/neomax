(function () {

    angular.module("app").controller('loginController', ['$scope', '$state', '$http', 'CONFIG', '$uibModal', '$cookies', 'userControl', function ($scope, $state, $http, CONFIG, $uibModal, $cookies, userControl) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.credentials = {
            username: '',
            password: ''
        };

        function loadPage() {
            
            //check if the user is logged and the browser has not been closed (there are cookies)
            if ($cookies.get('username')) {
                //continue without log-in (just update session)
                userControl.recoverUserSession(userControl.userSession.username);
            }
        }
        
        $scope.access = function () {

            //call the user service to login
            //- get the session and save it in the browser local storage
            //- save data to cookies
            userControl.login($scope.credentials);
        };
        
        $scope.forgotPass = function () {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/login/modals/forgot-password.html',
                controller: 'forgotPasswordController'
            });
        };

        //Procedural scripts
        loadPage();

    }]);
}());