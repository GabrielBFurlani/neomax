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
                .success(function (httpResultModel) {
                    if (httpResultModel.operationSuccess) {

                        // "Change Password" token is valid and user's data was returned
                        $scope.user = httpResultModel.data;
                    }
                    else {
                        //Redirects to login page
                        $state.go('login');
                    }
                })
                .error(function (httpResultModel) {
                    //Redirects to login page
                    $state.go('login');
                });            
        }

        $scope.ok = function (data) {
            $http.post(_apiUrl + '/user/externalPasswordChange', $scope.changeInfo)
                .success(function (httpResultModel) {
                    if (httpResultModel.operationSuccess) {                        
                        $state.go('login');                        
                    }
                })
                .error(function (httpResultModel) {
                });
        };

        loadPage();
    }]);
}());