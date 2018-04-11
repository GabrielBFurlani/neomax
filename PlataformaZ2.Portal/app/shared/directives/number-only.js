(function () {
    "use strict";

    angular.module('app').directive('numberOnly', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                var negativo = /\-/.test(attrs.numberOnly);
                var decimal = /\.|\,/.test(attrs.numberOnly) ? /\.|\,/.exec(attrs.numberOnly)[0] : null;

                var regExp = '^';
                regExp += negativo ? '[\\-]{0,1}' : '';
                regExp += '[\\d]+';
                if (decimal != null) {
                    regExp += '[\\' + decimal + '][\\d]+|';
                    if (negativo) {
                        regExp += '[\\-]{0,1}'
                    }
                    regExp += '[\\d]+'
                }
                regExp += '';
                regExp = new RegExp(regExp);

                ngModel.$parsers.unshift(function (input) {
                    if (input === undefined) return null;
                    if (input === null) return;

                    input = input.toString().replace(/\./, decimal);
                    if (input == '-') return input;
                    if (decimal !== null && input.charAt(input.length - 1) == decimal) return input;

                    input = regExp.test(input) ? regExp.exec(input)[0] : null;

                    var viewVal = null;

                    if (input !== null) {
                        input = decimal != null ? parseFloat(input.replace(/\,/, '.')) : parseInt(input);
                    }

                    viewVal = isNaN(input) || input === null ? '' : input;

                    ngModel.$setViewValue(decimal != null ? viewVal.toString().replace(/\./, decimal) : viewVal.toString());
                    ngModel.$render();

                    return isNaN(input) ? null : input;
                });

                ngModel.$formatters.unshift(function (value) {
                    if (value !== undefined && value !== null) {
                        return decimal != null ? value.toString().replace(/\./, decimal) : value.toString();
                    }
                });
            }
        }
    });

}());