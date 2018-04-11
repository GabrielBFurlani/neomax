(function () {

    angular.module("app").controller('internalPasswordChangeController', ['$scope', '$state', '$http', 'CONFIG', '$stateParams', 'userControl', function ($scope, $state, $http, CONFIG, $stateParams, userControl) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.changeInfo = {
            idUser: userControl.userSession.idUser,
            newPassword: '',
            changePasswordToken: null
        };

        $scope.reEnterPassword = '';

        $scope.ok = function (data) {

            $http.post(_apiUrl + '/user/userArea/internalPasswordChange', $scope.changeInfo)
                .success(function (httpResultModel) {
                    if (httpResultModel.operationSuccess) {                        
                        $state.go('panel.home');                      
                    }
                })
                .error(function (httpResultModel) {
                });
        };

    }]);
}());