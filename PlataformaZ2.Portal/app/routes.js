(function () {
    'use strict';

    angular.module("app").config(function ($stateProvider, $urlRouterProvider) {

        /*default route*/
        $urlRouterProvider.otherwise("/login");

        /*routes mappings*/
        $stateProvider

            /* View - Level 1 (Login)*/
            .state('login', {
                url: "/login",
                templateUrl: "app/login/login.html",
                controller: "loginController"
            })

            /* View - Level 1 (External Password Change)*/
            .state('externalPasswordChange', {
                url: "/externalPasswordChange/:idUser/:changePasswordToken",
                templateUrl: "app/externalPasswordChange/externalPasswordChange.html",
                controller: "externalPasswordChangeController"
            })

                /* View - Level 1 (Panel) */
                .state('panel', {
                    url: "/panel",
                    templateUrl: "app/panel/panel.html",
                    controller: "panelController"
                })

                /* nested views - Level 2 (home) */
                .state('panel.home', {
                    url: "/home",
                    templateUrl: "app/panel/home/home.html",
                    controller: "homeController"
                })

                /* nested views - Level 2 (user) */
                .state('panel.user', {
                    url: "/user",
                    templateUrl: "app/panel/user/user.html"
                })

                    /* nested views - Level 3 (user management) */
                    .state('panel.user.management', {
                        url: "/management",
                        templateUrl: "app/panel/user/management/management.html"
                    })

                        /* nested views - Level 4  */
                        .state('panel.user.management.list', {
                            url: "/list",
                            templateUrl: "app/panel/user/management/list/list.html",
                            controller: "userManagementListController"
                        })
                        .state('panel.user.management.edit', {
                            url: "/edit/:idUser",
                            templateUrl: "app/panel/user/management/edit/edit.html",
                            controller: "userManagementEditController"
                        })

                /* nested views - Level 3 (user area) */
                .state('panel.user.userArea', {
                    url: "/userArea",
                    templateUrl: "app/panel/user/userArea/userArea.html"
                })

                    /* nested views - Level 4  */
                    .state('panel.user.userArea.edit', {
                        url: "/edit/:idUser",
                        templateUrl: "app/panel/user/userArea/edit/edit.html",
                        controller: "userAreaEditController"
                    })
                    .state('panel.user.userArea.internalPasswordChange', {
                        url: "/internalPasswordChange",
                        templateUrl: "app/panel/user/userArea/internalPasswordChange/internalPasswordChange.html",
                        controller: "internalPasswordChangeController"
                    })

    });
}());