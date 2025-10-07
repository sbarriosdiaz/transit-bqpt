'use strict';

var loginApp = (function (loginApp) {
    loginApp.Init = function () {
        $("#Username").focus();
        $('#message').hide();

        var form = $('#js-form-login'),
            formData = $.data(form[0]),
            settings = formData.validator.settings,
            oldErrorPlacement = settings.errorPlacement,
            oldSuccess = settings.success;

        settings.errorPlacement = function (label, element) {
            oldErrorPlacement(label, element);
            label.parents('.form-group').addClass('input-validation-error ');
            label.addClass('text-danger');
        };
        settings.success = function (label) {
            label.parents('.form-group').removeClass('input-validation-error').addClass('was-validated');
            oldSuccess(label);
        };
    };

    return loginApp;
})({});

