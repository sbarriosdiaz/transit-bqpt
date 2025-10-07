/////////////////////////////////////////////////////////////////////////////////////
///// Cesar L Diaz
///// Helper JS Class App v2.0.0
/////////////////////////////////////////////////////////////////////////////////////

'use strict';

var helperApp = (function (helperApp) {
    var _this = this;

    helperApp.RefreshDOM = function () {
        $('form').removeData('validator');
        $('form').removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse('form');

        $('form').each(function () {
            if ($(this).data('validator')) $(this).data('validator').settings.ignore = ".note-editor *";
        });

        helperApp.GlobalCleaner();
    };

    helperApp.HasExtension = function (e, exts) {
        var fileName = $('#' + e).val();

        return new RegExp('(' + exts.join('|').replace(/\./g, '\\.') + ')$').test(fileName);
    };

    helperApp.BlockUI = function (element) {
        $('div#' + element).block({
            message: '<h3><span class="fas fa-sync-alt fa-spin mr-1" aria-hidden="true"></span><br />Processing Request... please wait...</h3>'
        });
    };

    helperApp.HasExtension = function (e, exts) {
        var fileName = $('#' + e).val();

        return new RegExp('(' + exts.join('|').replace(/\./g, '\\.') + ')$').test(fileName);
    };

    helperApp.CheckAllowedFiles = function (e, submitPanel, errorPanel) {
        var elem = $('#' + e);
        var elemForSubmitPanel = $('#' + submitPanel);
        var elemForErrorPanel = $('#' + errorPanel);

        elem.bind('change', function () {
            // 15000000 15 MB
            var checkForExt = Boolean(helperApp.HasExtension('File', ['.pdf', '.PDF', '.Pdf', '.PDf', '.pDF', '.pdF', '.pDf', '.PdF', '.jpg', '.jpeg', '.JPG', '.JPEG', '.bmp', '.tiff', '.TIFF', '.png', '.PNG', '.BMP', '.doc', '.DOC', '.docx', '.DOCX']));
            var size = this.files[0].size;
            var allowedSize = 15000000;
            if (size >= allowedSize || checkForExt === false) {
                // not allowed to upload
                elemForSubmitPanel.addClass('d-none');
                elemForErrorPanel.removeClass('d-none');
            } else {
                elemForSubmitPanel.removeClass('d-none');
                elemForErrorPanel.addClass('d-none');
            }
        });
    };

    helperApp.UnBlockUI = function (element) {
        $('div#' + element).unblock();
    };

    helperApp.HtmlScape = function (s) {
        return s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    };

    helperApp.HtmlCleaner = function (str) {
        return str.replace(/[&\/\\#@,+()$~%.'":*?<>{}=]/g, '');
    };

    helperApp.ImagePlaceholder = function (input, node, outputElement) {
        var reader = new FileReader();

        reader.onload = function (e) {
            var imgNode = $('#' + node);
            imgNode.attr('src', e.target.result);

            $('#' + outputElement).val(e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    };

    helperApp.GlobalCleaner = function () {
        $('[data-toggle="tooltip"]').tooltip();

        $('*[data-purpose="number"').number(true, 0);
        $('*[data-purpose="currency"').number(true, 2);

        $('*[data-purpose="datepicker"').datetimepicker({
            format: 'L'
        }); // date

        $('*[data-purpose="timepicker"').datetimepicker({
            format: 'LT'
        }); // time

        jQuery.validator.setDefaults({ ignore: ":hidden:not(#summernote),.note-editable.panel-body" });

        $('input[type=text], textarea').keypress(function (e) {
            var regex = new RegExp("^[A-Za-z0-9- /@.'\]+$");
            var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);

            if (regex.test(str)) {
                return true;
            }

            e.preventDefault();
            return false;
        });
        $('input[type=text], textarea').bind('paste', function (e) {
            var regex = new RegExp("^[A-Za-z0-9- /@.'\]+$");
            var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);

            if (regex.test(str)) {
                return true;
            }

            e.preventDefault();
            return false;
        });
    };

    helperApp.RefreshUI = function () {
        setTimeout(function () {
            window.location.reload(true);
        }, 500);
    };

    helperApp.FormatCurrency = function (input) {
        var output = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });

        return output.format(input); // ‘$123,456.79’
    };

    helperApp.SmartTextArea = function (e, chars, warning) {
        var options = {
            'maxCharacterSize': chars,
            'originalStyle': 'text-info',
            'warningStyle': 'text-danger',
            'warningNumber': warning,
            'displayFormat': '#input/#max | #words words'
        };

        $('#' + e).textareaCount(options);
    };

    helperApp.Guid = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0,
                v = c === 'x' ? r : r & 0x3 | 0x8;
            return v.toString(16);
        });
    };

    helperApp.ShowSuccessNotification = function (message) {
        toastr['success'](message);
    };

    helperApp.ShowValidationNotification = function (message) {
        toastr['warning'](message);
    };

    helperApp.ShowErrorNotification = function (message) {
        toastr['error'](message);
    };

    //alert confirmation with sweet alert.
    helperApp.ShowAlertConfirmation = function (msg, yesFn, parameterYesFn) {
        swal({
            title: 'Record Status',
            text: msg,
            icon: "error",
            confirmButtonClass: 'btn-danger',
            buttons: true,
            dangerMode: true
        }).then(function (isConfirm) {
            if (isConfirm) {
                yesFn(parameterYesFn);
                swal('Success!', 'Transaction was successfully saved!', 'success');
            } else {
                swal("User Cancel the Process!");
            }
        });
    };

    helperApp.SetTextAreaCounterMaxValue = function (chars) {
        return {
            'maxCharacterSize': chars,
            'originalStyle': "text-info",
            'warningStyle': "text-danger",
            'warningNumber': 50,
            'displayFormat': "#input/#max | #words words"
        };
    };

    helperApp.FormatDate = function (dateObj, format) {
        var monthNames = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

        var currentDate = dateObj.getDate();
        var currentMonth = dateObj.getMonth();

        currentMonth = currentMonth + 1;

        var currentYear = dateObj.getFullYear();
        var currentMin = dateObj.getMinutes();
        var currentHour = dateObj.getHours();
        var currentSeconds = dateObj.getSeconds();

        if (currentMonth.toString().length === 1) currentMonth = '0' + currentMonth;

        if (currentDate.toString().length === 1) currentDate = '0' + currentDate;

        if (currentHour.toString().length === 1) currentHour = '0' + currentHour;

        if (currentMin.toString().length === 1) currentMin = '0' + currentMin;

        switch (format) {
            case 1:
                // dd-mm-yyyy
                return currentDate + '-' + currentMonth + '-' + currentYear;

            case 2:
                // yyyy-mm-dd
                return currentYear + '-' + currentMonth + '-' + currentDate;

            case 3:
                // dd/mm/yyyy
                return currentDate + '/' + currentMonth + '/' + currentYear;

            case 4:
                // MM/dd/yyyy HH:mm:ss
                return currentMonth + '/' + currentDate + '/' + currentYear + ' ' + currentHour + ':' + currentMin + ':' + currentSeconds;

            case 5:
                // MM/dd/yyyy
                return currentMonth + '/' + currentDate + '/' + currentYear;

            default:
                return currentMonth + '/' + currentDate + '/' + currentYear;
        }
    };
    helperApp.InitDtSimple = function (tbl) {
        var sortby = arguments.length <= 1 || arguments[1] === undefined ? 0 : arguments[1];
        var sortDir = arguments.length <= 2 || arguments[2] === undefined ? 'desc' : arguments[2];

        $('#' + tbl).DataTable({
            pageLength: 25,
            responsive: true,
            dom: '<"html5buttons"B>lTfgitp',
            "order": [[sortby, sortDir]],
            buttons: [{ extend: 'excel', title: 'grid-report' }, {
                extend: 'print',
                customize: function customize(win) {
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');

                    $(win.document.body).find('table').addClass('compact').css('font-size', 'inherit');
                }
            }]
        });
    };
    helperApp.RefreshDt = function (tbl) {
        var buttons = [];
        var tablix = $('#' + tbl).DataTable();

        $.each(tablix.buttons()[0].inst.s.buttons, function () {
            buttons.push(_this);
        });

        $.each(buttons, function () {
            tablix.buttons()[0].inst.remove(_this.node);
        });
        tablix.rows().remove().draw();
        tablix.destroy();
    };

    helperApp.InitDt = function (tbl, sortby) {
        var sortDir = arguments.length <= 2 || arguments[2] === undefined ? 'desc' : arguments[2];
        var search = arguments.length <= 3 || arguments[3] === undefined ? true : arguments[3];
        var state = arguments.length <= 4 || arguments[4] === undefined ? false : arguments[4];

        var tablix = $('#' + tbl).DataTable({
            dom: "Bfrtip",
            "order": [[sortby, sortDir]],
            "sAjaxDataProp": "",
            "deferRender": true,
            stateSave: state,
            lengthMenu: [[10, 25, 50, -1], ['10 rows', '25 rows', '50 rows', 'Show all']],
            buttons: ['excel', 'print', 'pageLength'],
            responsive: true
        });

        $('#dtSearch').attr('placeholder', 'search...');

        tablix.buttons().container().appendTo($('.dataTables_filter:eq(0)', tablix.table().container()));

        $('.dt-buttons').addClass('btn-group-sm float-left d-none d-md-block');
        $('.dataTables_paginate').addClass('float-right d-none d-md-block');

        if ($(window).width() < 500) {
            $('.table').removeClass('table-bordered');
        }

        if (search === false) {
            $('.dataTables_filter').addClass('d-none');
        }
    };

    helperApp.SelectRow = function () {
        $('tr').delegate('a', 'click', function () {
            $(this).closest('tr').addClass('selected-row').siblings('tr').removeClass('selected-row');
        });
    };

    helperApp.InitSummernote = function (e, mode, height) {
        var options = {};

        switch (mode) {
            case 'simple':
                options = {
                    height: height,
                    toolbar: [['style', ['bold', 'italic', 'underline', 'clear']]]
                };

                $('#' + e).summernote(options);
                break;

            case 'full':
                options = {
                    height: height,
                    toolbar: [['style', ['style']], ['font', ['bold', 'italic', 'underline', 'clear']], ['fontname', ['fontname']], ['fontsize', ['fontsize']], ['color', ['color']], ['para', ['ul', 'ol', 'paragraph']], ['height', ['height']], ['table', ['table']], ['insert', ['link', 'picture', 'hr']], ['view', ['fullscreen', 'codeview']], ['help', ['help']]]
                };

                $('#' + e).summernote(options);
                break;
            default:
        }
    };

    helperApp.DynamicModal = function () {
        var size = arguments[0]; // lg, sm, md, refer to BS Site for more information
        var action = arguments[1]; //show, hide
        var header = arguments[2];
        var content = arguments[3];

        $('#js-dynamic-modal').modal(action);
        $('#js-dynamic-modal-dialog').addClass('modal-' + size);
        $('#js-dynamic-modal-label').html(header);
        $('#js-dynamic-modal-content').html(content);
    };

    helperApp.FormMode = function (action, container) {
        switch (action) {
            case "new":
                $('#' + container).find('[data-action=save]').show();
                $('#' + container).find('[data-action=update]').hide();
                $('#' + container).find('[data-action=cancel]').hide();
                break;

            case "edit":
                $('#' + container).find('[data-action=save]').hide();
                $('#' + container).find('[data-action=update]').show();
                $('#' + container).find('[data-action=cancel]').show();
                break;

            default:
                break;
        }
    };

    return helperApp;
})({});

