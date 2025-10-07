let loginApp = ((loginApp) => {
    loginApp.Init = () => {
        $("#Username").focus();
        $('#message').hide();
        let form = $('#form')
            , formData = $.data(form[0])
            , settings = formData.validator.settings
            , oldErrorPlacement = settings.errorPlacement
            , oldSuccess = settings.success;

        settings.errorPlacement = (label, element) => {
            oldErrorPlacement(label, element);
            label.parents('.form-group').addClass('input-validation-error ');
            label.addClass('text-danger');
        };
        settings.success = (label) => {
            label.parents('.form-group').removeClass('input-validation-error').addClass('was-validated');
            oldSuccess(label);
        };
    };

    return loginApp;
})({});