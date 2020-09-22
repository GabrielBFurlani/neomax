(function () {
    "use strict";
    angular.module('app').service('fileDownloadUpload', ['$http', function ($http) {

        this.uploadFileToUrl = function (http, files, model, uploadUrl) {

            //formData object (contains the object and the file)
            var formData = new FormData();

            //attach the files (if exist)
            if (files != null) {
                for (var i = 0; i < files.length; i++) {
                    formData.append(files[i].name, files[i]);
                }
            }

            //attach the object (if exists)
            if (model != null) {
                formData.append('model', angular.toJson(model, false));
            }

            //....... Send to server using HTTP POST, with formData as BODY

            //The HTTP interceptor needs change the "Content-Type" to "multipart/form-data"
            //So, here will be add %upload% to identify this kind HTTP call (it will be erased at the interceptor)

            if (http == "post") {
                var promisse = $http.post(uploadUrl + "%updload%", formData);
            }

            if (http == "put") {
                var promisse = $http.put(uploadUrl + "%updload%", formData);
            }

            return promisse;
        }

        this.downloadFileToUrl = function (downloadUrl, file) {

            var defaultFileName = null;

            var promisse = $http.post(downloadUrl, file, { responseType: 'arraybuffer' })
                .success(function (data, status, headers) {

                    var type = headers('Content-Type');
                    var disposition = headers('Content-Disposition');

                    if (disposition) {
                        var match = disposition.match(/.*filename=\"?([^;\"]+)\"?.*/);
                        if (match[1])
                            defaultFileName = match[1];
                    }

                    if (!defaultFileName) {
                        defaultFileName = 'Arquivo';
                    }

                    defaultFileName = defaultFileName.replace(/[<>:"\/\\|?*]+/g, '_');
                    var blob = new Blob([data], { type: type });

                    saveAs(blob, defaultFileName);
                });

            return promisse;
        }
    }]);

}());