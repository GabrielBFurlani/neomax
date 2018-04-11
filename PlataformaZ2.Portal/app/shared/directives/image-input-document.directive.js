(function () {
    "use strict";

    angular.module("app").directive('imageInputDocument', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                $(element).fileinput({
                    browseLabel: '<span style=\"margin-left: 5px;\">Escolha o Arquivo</span>',
                    browseIcon: '<span class="fa fa-folder-open"></span>',
                    removeLabel: "<span style=\"margin-left: 5px;\">Excluir</span>",
                    removeIcon: '<span class="fa fa-trash"></span>',
                    showPreview: true,
                    showUpload: false,
                    allowedFileExtensions: ["jpg", "jpeg", "png", "gif", "bmp"],
                    maxFileCount: 2,
                    msgValidationError: 'Excede tamanho máximo permitido',
                    msgNoFilesSelected: 'Somente imagens são permitidas',
                    msgFilesTooMany: 'São permitidas, no máximo, 2 imagens',
                    msgSelected: '{n} imagens selecionadas'
                });
            }
        };
    });

}());