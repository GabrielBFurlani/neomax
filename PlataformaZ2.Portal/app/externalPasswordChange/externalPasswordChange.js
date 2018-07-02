(function () {

    angular.module("app").controller('externalPasswordChangeController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', 'userControl', function ($scope, $state, $http, CONFIG, $stateParams, userControl) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.changeInfo = {
            idUser: $stateParams.idUser,
            newPassword: '',
            changePasswordToken: $stateParams.changePasswordToken
        };

        $scope.reEnterPassword = '';
        

        function loadPage() {

            //check if the link is valid (the user exists and the token is not expired)
            $http.post(_apiUrl + '/user/passwordLinkValidation', $scope.changeInfo)
                .then(function successCallback(response) {

                    // "Change Password" token is valid and user's data was returned
                    $scope.user = response.data.resultData; 

                }, function errorCallback(response) {

                    //Redirects to login page
                    $state.go('login');
                })          
        }

        //Button: OK
        $scope.ok = function () {
            $http.post(_apiUrl + '/user/externalPasswordChange', $scope.changeInfo)
                .then(function successCallback(response) {

                    //Redirects to login page
                    $state.go('login');  
                })
        };

        loadPage();

    }]);
}());