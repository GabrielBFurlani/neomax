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
                    var httpResultModel = response.data;

                    if (httpResultModel.operationSuccess) {

                        // "Change Password" token is valid and user's data was returned
                        $scope.user = httpResultModel.data;
                    }
                    else {
                        //Redirects to login page
                        $state.go('login');
                    }
                })
                .error(function (httpResultModel) { //precisa verificar erro?????
                    //Redirects to login page
                    $state.go('login');
                });            
        }

        $scope.ok = function (data) {
            $http.post(_apiUrl + '/user/externalPasswordChange', $scope.changeInfo)
                .then(function successCallback(response) {
                    var httpResultModel = response.data;

                    if (httpResultModel.operationSuccess) {  
                        //Redirects to login page
                        $state.go('login');                        
                    }
                })
        };

        loadPage();
    }]);
}());