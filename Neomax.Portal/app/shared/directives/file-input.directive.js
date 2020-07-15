(function () {
    "use strict";

    angular.module("app").directive('fileInput', function () {
        return {
            restrict: 'A',
            scope: {
                maxFileSize: '@',
                showDragDrop: '@',
                uploadUrl: '@',
                showPreview: '@',
                showZoom: '@',
                showRemove: '@',
                showUpload: '@',
                allowedFileExtensions: '=',
            },
            link: function (scope, element, attrs) {
                $(element).fileinput({
                    showPreview: (scope.showPreview == 'true'),
                    showUpload: (scope.showUpload == 'true'),
                    allowedFileExtensions: scope.allowedFileExtensions,
                    maxFileSize: scope.maxFileSize,
                    dropZoneEnabled: (scope.showDragDrop == 'true'),
                    uploadUrl: scope.uploadUrl,
                    browseLabel: '<span style=\"margin-left: 5px;\">Escolha o Arquivo</span>',
                    browseIcon: '<span class="fa fa-folder-open"></span>',
                    removeLabel: '<span style="margin-left: 5px;">Excluir</span>',
                    removeIcon: '<span class="fa fa-trash"></span>',
                    dropZoneTitle: 'Arraste e solte o arquivo aqui',
                    msgValidationError: 'Arquivo inválido',
                    fileSingle: 'arquivo',
                    filePlural: 'arquivos',
                    removeTitle: 'Excluir os arquivos selecionados',
                    cancelLabel: 'Cancelar',
                    cancelTitle: 'Cancelar upload em andamento',
                    msgNo: 'Não',
                    msgNoFilesSelected: 'Nenhum arquivo selecionado',
                    msgCancelled: 'Cancelado',
                    msgSizeTooLarge: 'O arquivo "{name}" (<b>{size} KB</b>) o tamanho máximo permitido de <b>{maxSize} KB</b>.',
                    msgFilesTooLess: 'Ao menos <b>{n}</b> {files} deve ser selecionado.',
                    msgFilesTooMany: 'O número de arquivos selecionados para upload <b>({n})</b> excede o limite de <b>{m}</b>.',
                    msgFileNotFound: 'Arquivo "{name}" não encontrado!',
                    msgFileSecured: 'Restrições de segurança estão prevenindo a leitura do arquivo "{name}".',
                    msgFileNotReadable: 'O conteúdo do arquivo "{name}" não é legível.',
                    msgFilePreviewAborted: 'A visualização do arquivo "{name}" foi abortada.',
                    msgFilePreviewError: 'Um ero ocorreu durante a leitura do arquivo "{name}".',
                    msgInvalidFileType: 'O tipo do arquivo "{name}" é inválido. Apenas os tipos "{types}" são suportados.',
                    msgInvalidFileExtension: 'Extensão inválida para o arquivo "{name}". Apenas as extensões "{extensions}" são suportadas.',
                    msgUploadAborted: 'O envio do arquivo foi abortado',
                    msgLoading: 'carregando arquivo {index} de {files} &hellip;',
                    msgProgress: 'Carregando arquivo {index} de {files} - {name} - {percent}% completado.',
                    msgSelected: '{n} {files} selecionado(s)',
                    msgFoldersNotAllowed: 'Apenas arquivos são suportados! {n} pasta(s) foram ignoradas.',
                    msgImageWidthSmall: 'A largura da imagem "{name}" deve ser de pelo menos {size} px.',
                    msgImageHeightSmall: 'A altura da imagem "{name}" deve ser de pelo menos {size} px.',
                    msgImageWidthLarge: 'A largura da imagem "{name}" não pode exceder {size} px.',
                    msgImageHeightLarge: 'A altura da imagem "{name}" não pode exceder {size } px.',
                    msgImageResizeError: 'Não foi possível recuperar as dimensões da imagem para redimensionar.',
                    msgImageResizeException: 'Erro ao redimensionar imagem.<pre>{errors}</pre>',
                    dropZoneClickTitle: '<br>(ou clique para selecionar {files})',
                    previewZoomButtonTitles: {
                        prev: 'Visualizar arquivo anterior',
                        next: 'Visualizar próximo arquivo',
                        toggleheader: 'Alternar para modo de cabeçalho',
                        fullscreen: 'Alternar para o modo tela cheia',
                        borderless: 'Alternar para modo sem bordas',
                        close: 'Fechar visualização detalhada'
                    },
                    fileActionSettings: {
                        showUpload: (scope.showUpload == 'true'),
                        showDrag: (scope.showDragDrop == 'true'),
                        showRemove: (scope.showRemove == 'true'),
                        showZoom: (scope.showZoom == 'true'),
                        removeTitle: 'Remover arquivo',
                        uploadTitle: 'Enviar arquivo',
                        zoomTitle: 'Ver detalhes',
                        dragTitle: 'Mover / Reorganizar',
                        indicatorNewTitle: 'Ainda não enviado',
                        indicatorSuccessTitle: 'Enviado',
                        indicatorErrorTitle: 'Erro ao enviar',
                        indicatorLoadingTitle: 'Enviando ...'
                    }
                });
            }
        };
    });

}());