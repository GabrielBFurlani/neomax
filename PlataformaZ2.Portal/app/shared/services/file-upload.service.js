(function () {
    "use strict";
    angular.module('app').service('fileUpload', ['$http', function ($http) {

        this.uploadFileToUrl = function (files, model, uploadUrl) {

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
            var promisse = $http.post(uploadUrl + "%updload%", formData);

            return promisse;
        }
    }]);

}());