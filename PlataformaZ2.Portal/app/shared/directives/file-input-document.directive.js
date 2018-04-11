(function () {
    "use strict";

    angular.module("app").directive('fileInputDocument', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                $(element).fileinput({
                    browseLabel: '<span style=\"margin-left: 5px;\">Escolha o Arquivo</span>',
                    browseIcon: '<span class="fa fa-folder-open"></span>',
                    removeLabel: "<span style=\"margin-left: 5px;\">Excluir</span>",
                    removeIcon: '<span class="fa fa-trash"></span>',
                    showPreview: false,
                    showUpload: false,
                    msgValidationError: 'Excede tamanho máximo permitido',
                    msgNoFilesSelected: 'Tipo de arquivo inválido',
                    allowedFileExtensions: ["xls","xlsx","csv"]
                });
            }
        };
    });

}());