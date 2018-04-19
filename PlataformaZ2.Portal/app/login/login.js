(function () {

    angular.module("app").controller('loginController', ['$scope', '$state', '$http', 'CONFIG', '$uibModal', '$cookies', 'userControl', function ($scope, $state, $http, CONFIG, $uibModal, $cookies, userControl) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.credentials = {
            username: '',
            password: ''
        };

        //Load Page
        function loadPage() {
            
            //check if the user is logged and the browser has not been closed (there are cookies)
            if ($cookies.get('username')) {

                //continue without log-in (just update user's session)
                userControl.recoverUserSession(userControl.userSession.username);
            }
        }

        //Log-in
        $scope.access = function () {

            // Call the user service to login:
            //  - get the session and save it in the browser local storage
            //  - save data to cookies
            userControl.login($scope.credentials);
        };

        //Button: Forgot Password
        $scope.forgotPassord = function () {

            // open modal
            var modalInstance = $uibModal.open({
                templateUrl: 'app/login/modals/forgot-password.html',
                controller: 'forgotPasswordController'
            });
        };

        //Button: Sign-Up
        $scope.signUp = function () {

            //open modal
            var modalInstance = $uibModal.open({
                templateUrl: 'app/login/modals/sign-up.html',
                controller: 'signUpController'
            });

            //modal result
            modalInstance.result.then(function (signUpUser) {
                
                //get the login credentials of the new user
                $scope.credentials = {
                    username: signUpUser.username,
                    password: signUpUser.password
                };

                //log-in into the system
                $scope.access();
            });
        };

        //Procedural scripts
        loadPage();

    }]);
}());