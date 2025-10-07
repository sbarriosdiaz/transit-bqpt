/////////////////////////////////////////////////////////////////////////////////////
///// Cesar L Diaz
///// Helper JS Class App v2.0.0
/////////////////////////////////////////////////////////////////////////////////////

let helperApp = function (helperApp) {
    helperApp.RefreshDOM = () => {
        $('form').removeData('validator');
        $('form').removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse('form');

        $('form').each(function () {
            if ($(this).data('validator'))
                $(this).data('validator').settings.ignore = ".note-editor *";
        });

        helperApp.GlobalCleaner();
    };

    helperApp.HasExtension = (e, exts) => {
        let fileName = $(`#${e}`).val();

        return (new RegExp('(' + exts.join('|').replace(/\./g, '\\.') + ')$', 'i')).test(fileName);
    };

    helperApp.BlockUI = (element) => {
        $(`div#${element}`).block({
            message: '<h3><span class="fas fa-sync-alt fa-spin mr-2" aria-hidden="true"></span><br />Processing Request... please wait...</h3>'
        });
    };

    helperApp.UnBlockUI = (element) => {
        $(`div#${element}`).unblock();
    };

    helperApp.HtmlScape = (s) => {
        return s
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
    };

    helperApp.HtmlCleaner = (str) => {
        return str.replace(/[&\/\\#@,+()$~%.'":*?<>{}=]/g, '');
    };

    helperApp.ImagePlaceholder = (input, node, outputElement) => {
        let reader = new FileReader();

        reader.onload = (e) => {
            var imgNode = $(`#${node}`);
            imgNode.attr('src', e.target.result);

            $(`#${outputElement}`).val(e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    };

    helperApp.GlobalCleaner = () => {
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

        $('input[type=text], textarea').keypress((e) => {
            let regex = new RegExp("^[A-Za-z0-9- /@.'\]+$");
            let str = String.fromCharCode(!e.charCode ? e.which : e.charCode);

            if (regex.test(str)) {
                return true;
            }

            e.preventDefault();
            return false;
        });

        $('input[type=text], textarea').bind('paste', (e) => {
            let regex = new RegExp("^[A-Za-z0-9- /@.'\]+$");
            let str = String.fromCharCode(!e.charCode ? e.which : e.charCode);

            if (regex.test(str)) {
                return true;
            }

            e.preventDefault();
            return false;
        });
    };

    helperApp.RefreshUI = () => {
        setTimeout(() => { window.location.reload(true); }, 500);
    };

    helperApp.FormatCurrency = (input) => {
        let output = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });

        return output.format(input); // ‘$123,456.79’
    };

    helperApp.SmartTextArea = (e, chars, warning) => {
        let options = {
            'maxCharacterSize': chars,
            'originalStyle': 'text-info',
            'warningStyle': 'text-danger',
            'warningNumber': warning,
            'displayFormat': '#input/#max | #words words'
        };

        $(`#${e}`).textareaCount(options);
    };

    helperApp.Guid = () => {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            let r = Math.random() * 16 | 0, v = c === 'x' ? r : r & 0x3 | 0x8;
            return v.toString(16);
        });
    };

    helperApp.ShowSuccessNotification = (message) => {
        toastr['success'](message);
    };

    helperApp.ShowValidationNotification = (message) => {
        toastr['warning'](message);
    };

    helperApp.ShowErrorNotification = (message) => {
        toastr['error'](message);
    };

    //alert confirmation with sweet alert.
    helperApp.ShowAlertConfirmation = (msg, yesFn, parameterYesFn) => {
        swal({
            title: 'Record Status',
            text: msg,
            icon: "error",
            confirmButtonClass: 'btn-danger',
            buttons: true,
            dangerMode: true
        }).then((isConfirm) => {
            if (isConfirm) {
                yesFn(parameterYesFn);
                swal('Success!', 'Transaction was successfully saved!', 'success');
            } else {
                swal("User Cancel the Process!");
            }
        });
    };

    helperApp.FormatDate = (dateObj, format) => {
        let monthNames = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

        let currentDate = dateObj.getDate();
        let currentMonth = dateObj.getMonth();

        currentMonth = currentMonth + 1;

        let currentYear = dateObj.getFullYear();
        let currentMin = dateObj.getMinutes();
        let currentHour = dateObj.getHours();
        let currentSeconds = dateObj.getSeconds();

        if (currentMonth.toString().length === 1)
            currentMonth = '0' + currentMonth;

        if (currentDate.toString().length === 1)
            currentDate = '0' + currentDate;

        if (currentHour.toString().length === 1)
            currentHour = '0' + currentHour;

        if (currentMin.toString().length === 1)
            currentMin = '0' + currentMin;

        switch (format) {
            case 1: // dd-mm-yyyy
                return `${currentDate}-${currentMonth}-${currentYear}`;

            case 2: // yyyy-mm-dd
                return `${currentYear}-${currentMonth}-${currentDate}`;

            case 3: // dd/mm/yyyy
                return `${currentDate}/${currentMonth}/${currentYear}`;

            case 4: // MM/dd/yyyy HH:mm:ss
                return `${currentMonth}/${currentDate}/${currentYear} ${currentHour}:${currentMin}:${currentSeconds}`;

            case 5: // MM/dd/yyyy
                return `${currentMonth}/${currentDate}/${currentYear}`;

            default:
                return `${currentMonth}/${currentDate}/${currentYear}`;
        }
    };
    helperApp.InitDtSimple = (tbl, sortby = 0, sortDir = 'desc') => {
        $(`#${tbl}`).DataTable({
            pageLength: 25,
            responsive: true,
            dom: '<"html5buttons"B>lTfgitp',
            "order": [[sortby, sortDir]],
            buttons: [
                { extend: 'excel', title: 'grid-report' },
                {
                    extend: 'print',
                    customize: function (win) {
                        $(win.document.body).addClass('white-bg');
                        $(win.document.body).css('font-size', '10px');

                        $(win.document.body).find('table')
                            .addClass('compact')
                            .css('font-size', 'inherit');
                    }
                }
            ]
        });
    };
    helperApp.RefreshDt = (tbl) => {
        let buttons = [];
        let tablix = $(`#${tbl}`).DataTable();

        $.each(tablix.buttons()[0].inst.s.buttons,
            () => {
                buttons.push(this);
            });

        $.each(buttons,
            () => {
                tablix.buttons()[0].inst.remove(this.node);
            });
        tablix.rows().remove().draw();
        tablix.destroy();
    };

    helperApp.InitDt = (tbl, sortby, sortDir = 'desc', search = true, state = false) => {
        let tablix = $(`#${tbl}`).DataTable({
            dom: "Bfrtip",
            "order": [[sortby, sortDir]],
            "sAjaxDataProp": "",
            "deferRender": true,
            stateSave: state,
            lengthMenu: [
                [10, 25, 50, -1],
                ['10 rows', '25 rows', '50 rows', 'Show all']
            ],
            buttons: [
                'excel', 'print', 'pageLength'
            ],
            responsive: true
        });

        $('#dtSearch').attr('placeholder', 'search...');

        tablix.buttons().container()
            .appendTo($('.dataTables_filter:eq(0)', tablix.table().container()));

        // local customization to add small class to action buttons on Datatables
        $('.dt-buttons').addClass('btn-group-sm float-left d-none d-md-block');
        $('.dataTables_paginate').addClass('float-right d-none d-md-block');

        if ($(window).width() < 500) {
            $('.table').removeClass('table-bordered');
        }

        if (search === false) {
            $('.dataTables_filter').addClass('d-none');
        }
    };

    helperApp.SelectRow = () => {
        $('tr').delegate('a', 'click', function () {
            $(this)
                .closest('tr').addClass('selected-row')
                .siblings('tr').removeClass('selected-row');
        });
    };

    helperApp.InitSummernote = (e, mode, height) => {
        let options = {};

        switch (mode) {
            case 'simple':
                options = {
                    height: height,
                    toolbar: [
                        ['style', ['bold', 'italic', 'underline', 'clear']]
                    ]
                };

                $(`#${e}`).summernote(options);
                break;

            case 'full':
                options = {
                    height: height,
                    toolbar: [
                        ['style', ['style']],
                        ['font', ['bold', 'italic', 'underline', 'clear']],
                        ['fontname', ['fontname']],
                        ['fontsize', ['fontsize']],
                        ['color', ['color']],
                        ['para', ['ul', 'ol', 'paragraph']],
                        ['height', ['height']],
                        ['table', ['table']],
                        ['insert', ['link', 'picture', 'hr']],
                        ['view', ['fullscreen', 'codeview']],
                        ['help', ['help']]
                    ]
                };

                $(`#${e}`).summernote(options);
                break;
            default:
        }
    };

    helperApp.FormMode = (action, container) => {
        switch (action) {
            case "new":
                $(`#${container}`).find('[data-action=save]').show();
                $(`#${container}`).find('[data-action=update]').hide();
                $(`#${container}`).find('[data-action=cancel]').hide();
                break;

            case "edit":
                $(`#${container}`).find('[data-action=save]').hide();
                $(`#${container}`).find('[data-action=update]').show();
                $(`#${container}`).find('[data-action=cancel]').show();
                break;

            default:
                break;
        }
    };

    helperApp.DynamicModal = (...args) => {
        let size = args[0]; // lg, sm, md, refer to BS Site for more information
        let action = args[1]; //show, hide
        let header = args[2];
        let content = args[3];

        $('#js-dynamic-modal').modal(action);
        $('#js-dynamic-modal-dialog').addClass(`modal-${size}`);
        $('#js-dynamic-modal-label').html(header);
        $('#js-dynamic-modal-content').html(content);
    };

    helperApp.CheckAllowedFiles = (e, submitPanel, errorPanel) => {
        let elem = $(`#${e}`);
        let elemForSubmitPanel = $(`#${submitPanel}`);
        let elemForErrorPanel = $(`#${errorPanel}`);

        elem.bind('change', function () {
            // 15000000 15 MB
            let checkForExt = Boolean(helperApp.HasExtension('File', ['.pdf', '.PDF', '.Pdf', '.PDf', '.pDF', '.pdF', '.pDf', '.PdF', '.jpg', '.jpeg', '.JPG', '.JPEG', '.bmp', '.tiff', '.TIFF', '.png', '.PNG', '.BMP', '.doc', '.DOC', '.docx', '.DOCX']));
            let size = this.files[0].size;
            let allowedSize = 15000000;
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
    return helperApp;
}({});