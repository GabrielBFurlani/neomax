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

            //$state.go('panel.home'); 
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
            $state.go('register.signUp');
        };

        $scope.cpfCnpjFormater = function () {

            if ($scope.credentials.username.replace(/[^\d]+/g, '').length >= 15) {
                $scope.credentials.username = $scope.credentials.username.replace(/[^\d]+/g, '').substring(0, 14);
            }

            //cpf
            if ($scope.credentials.username && $scope.credentials.username.length <= 11) {

                var vr = $scope.credentials.username.replace(/[^\d]+/g, '');
                tam = vr.length + 1;
                if (tam == 4)
                    $scope.credentials.username = vr.substr(0, 3) + '.';
                if (tam == 7)
                    $scope.credentials.username = vr.substr(0, 3) + '.' + vr.substr(3, 3) + '.';
                if (tam == 10)
                    $scope.credentials.username = vr.substr(0, 3) + '.' + vr.substr(3, 3) + '.' + vr.substr(6, 3) + '-' + vr.substr(9, 4);
                if (tam == 15)
                    $scope.credentials.username = vr.substr(0, 3) + '.' + vr.substr(3, 3) + '.' + vr.substr(6, 3) + '-' + vr.substr(9, 4);
            }
            //cnpj
            else {
                var vr = $scope.credentials.username.replace(/[^\d]+/g, '');
                tam = vr.length + 1;
                if (tam == 3)
                    $scope.credentials.username = vr.substr(0, 2) + '.';
                if (tam == 6)
                    $scope.credentials.username = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.';
                if (tam == 10)
                    $scope.credentials.username = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8, 4);
                if (tam == 15)
                    $scope.credentials.username = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8, 4) + '-' + vr.substr(12, 2);
            }
        }

        //Procedural scripts
        loadPage();

    }]);
}());