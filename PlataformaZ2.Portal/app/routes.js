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

            /* View - Level 1 (signUp)*/
            .state('register', {
                url: "/register",
                templateUrl: "app/register/register.html",
                controller: "registerController"
            })

            /* View - Level 1 (signUp)*/
            .state('register.signUp', {
                url: "/signUp",
                templateUrl: "app/register/signUp/signUp.html",
                controller: "signUpController"
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

            /* nested views - Level 2 (process) */
            .state('panel.process', {
                url: "/process",
                templateUrl: "app/panel/user/user.html"
            })

            /* nested views - Level 3 (process admin) */
            .state('panel.process.admin', {
                url: "/admin",
                templateUrl: "app/panel/process/admin/admin.html"
            })

            /* nested views - Level 4 (process admin list)  */
            .state('panel.process.admin.list', {
                url: "/list",
                templateUrl: "app/panel/process/admin/list/list.html",
                controller: "processAdminListController"
            })

            /* nested views - Level 4 (process admin edit)  */
            .state('panel.process.admin.detail', {
                url: "/detail/:idProcess",
                templateUrl: "app/panel/process/admin/detail/detail.html",
                controller: "processAdminDetailController"
            })

            /* nested views - Level 3 (process client) */
            .state('panel.process.client', {
                url: "/client",
                templateUrl: "app/panel/process/client/client.html"
            })

            /* nested views - Level 4 (process client list)  */
            .state('panel.process.client.list', {
                url: "/list",
                templateUrl: "app/panel/process/client/list/list.html",
                controller: "processClientListController"
            })

            /* nested views - Level 4 (process client edit)  */
            .state('panel.process.client.edit', {
                url: "/edit/:idProcess",
                templateUrl: "app/panel/process/client/edit/edit.html",
                controller: "processClientEditController"
            })

            /* nested views - Level 2 (client) */
            .state('panel.client', {
                url: "/client",
                templateUrl: "app/panel/user/user.html"
            })

            /* nested views - Level 3 (client list)  */
            .state('panel.client.list', {
                url: "/list",
                templateUrl: "app/panel/client/list/list.html",
                controller: "clientListController"
            })

            /* nested views - Level 3 (client edit)  */
            .state('panel.client.detail', {
                url: "/edit/:idClient",
                templateUrl: "app/panel/client/detail/detail.html",
                controller: "clientDetailController"
            })

    });
}());