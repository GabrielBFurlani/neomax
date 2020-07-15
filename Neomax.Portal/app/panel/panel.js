(function () {

    angular.module("app").controller('panelController', ['$scope', '$state', '$http', 'CONFIG', 'userControl', function ($scope, $state, $http, CONFIG, userControl) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.isMenuOpen = false;

        function loadPage() {

            //gets the logged user data (user session)
            $scope.session = userControl.userSession;

            //checks if the logged user has photo
            if ($scope.session.photo) {
                $scope.hasPhoto = true;
            }
            else {
                $scope.hasPhoto = false;
            }

            console.log($scope.session);

            //checks if the logged user has "admin" profile
            if ($scope.session.client == null) {
                $scope.isAdmin = true;
            }
            else {
                $scope.isAdmin = false;
            }
        }

        $scope.wasMenubarToggled = function () {
            $scope.isMenuOpen = !$scope.isMenuOpen;
        }

        $scope.editUser = function () {
            $state.go('panel.user.userArea.edit', { idUser: $scope.session.idUser });
        };

        $scope.changePassword = function () {
            $state.go('panel.user.userArea.internalPasswordChange');
        }

        $scope.logOut = function () {
            //call the user service to logout
            userControl.logout();
        };

        loadPage();

    }]);
}());