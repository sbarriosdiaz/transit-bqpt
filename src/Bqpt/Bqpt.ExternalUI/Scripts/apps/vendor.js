let vendor = ((vendor) => {
    const urls = {
        bidPartsPartialUrl: 'bids/getBidParts/',
        initAddBidUi: 'bids/initAddBid/',
        initUpdateBidUi: 'bids/initUpdateBid/',
        initDeleteBidUi: 'bids/initDeleteBid/',
        DeleteBidUi: 'bids/DeleteBid/',
        initDeleteAttachment:'bids/DeleteAttachment',
        addBidQuoteFile: 'bids/addBidQuoteFile/',
        initUploadVendorBidQuoteAttachmentUi: 'bids/initUploadVendorBidQuoteAttachment/',
    }

    vendor.OpenAddBid = (DomainKey) => {
        helperApp.BlockUI('js-pnl-helper');
        axios.get(`${appUrl}${urls.initAddBidUi}${DomainKey}`).then(function (response) {
            helperApp.DynamicModal('lg', 'show', 'Add Bid', response.data);

            $('input[type=date]').attr({
                min: moment().add(1, 'days').format('YYYY-MM-DD'),
                max: moment().add(1, 'years').format('YYYY-MM-DD')
            });

            $('.usd_input').change(function () {
                this.value = parseFloat(this.value).toFixed(2);
            });
            helperApp.RefreshDOM();
            helperApp.UnBlockUI('js-pnl-helper');
        });
    };

    vendor.AddBid = () => {
        $('#form').submit((e) => {
            e.preventDefault();
            e.stopImmediatePropagation();

            if ($('#form').valid()) {
                let ajaxOptions = {
                    beforeSend: () => {
                        helperApp.BlockUI('js-pnl-helper');
                    },
                    success: (data) => {
                        if (!!data && data.length > 4 && data.substring(0, 5).toLowerCase() === 'error') {
                            helperApp.ShowErrorNotification(results);
                            $('#js-pnl-validationresultmessage').removeClass('d-none').html(`<div class="alert alert-warning">${data}</div>`);
                            helperApp.UnBlockUI('js-pnl-helper');
                        }
                        else {
                            if ($('#js-btn-invoice-uploader').hasClass('d-none')) $('#js-btn-invoice-uploader').removeClass('d-none');

                            helperApp.UnBlockUI('js-pnl-helper');
                            helperApp.ShowSuccessNotification(`SUCCESS: Added ${data} Bid`);
                            helperApp.DynamicModal('lg', 'hide', '', '');
                            $('#js-pnl-parts').load(`${appUrl}${urls.bidPartsPartialUrl}${$('#DomainKey').val()}`, () => {
                                helperApp.InitDtSimple('dt-vendorbids', '0', 'asc');
                            });
                        }
                    },
                    error: (xhr, textStatus, errorThrown) => {
                        console.log(errorThrown);
                        helperApp.ShowErrorNotification("Error when saving the data! Web Team Was Notified!");
                    }
                };

                $('#form').ajaxSubmit(ajaxOptions);
            }
            else {
                helperApp.ShowErrorNotification("Some fields required, complete and then submit...");
            }
        });
    }

    vendor.OpenUpdateBid = (DomainKey) => {
        helperApp.BlockUI('js-pnl-helper');
        axios.get(`${appUrl}${urls.initUpdateBidUi}${DomainKey}`).then(function (response) {
            helperApp.DynamicModal('lg', 'show', 'Update Bid', response.data);

            $('input[type=date]').attr({
                min: moment().add(1, 'days').format('YYYY-MM-DD'),
                max: moment().add(1, 'years').format('YYYY-MM-DD')
            });

            $('.usd_input').change(function () {
                this.value = parseFloat(this.value).toFixed(2);
            });
            helperApp.RefreshDOM();
            helperApp.UnBlockUI('js-pnl-helper');
        });
    };

    vendor.UpdateBid = () => {
        $('#form').submit((e) => {
            e.preventDefault();
            e.stopImmediatePropagation();

            if ($('#form').valid()) {
                let ajaxOptions = {
                    beforeSend: () => {
                        helperApp.BlockUI('js-pnl-helper');
                    },
                    success: (data) => {
                        if (!!data && data.length > 4 && data.substring(0, 5).toLowerCase() === 'error') {
                            helperApp.ShowErrorNotification(results);
                            $('#js-pnl-validationresultmessage').removeClass('d-none').html(`<div class="alert alert-warning">${data}</div>`);
                            helperApp.UnBlockUI('js-pnl-helper');
                        }
                        else {
                            helperApp.UnBlockUI('js-pnl-helper');
                            helperApp.ShowSuccessNotification(`SUCCESS: Updated ${data} Bid`);
                            helperApp.DynamicModal('lg', 'hide', '', '');
                            $('#js-pnl-parts').load(`${appUrl}${urls.bidPartsPartialUrl}${$('#DomainKey').val()}`, () => {
                                helperApp.InitDtSimple('dt-vendorbids', '0', 'asc');
                            });
                        }
                    },
                    error: (xhr, textStatus, errorThrown) => {
                        console.log(errorThrown);
                        helperApp.ShowErrorNotification("Error when saving the data! Web Team Was Notified!");
                    }
                };

                $('#form').ajaxSubmit(ajaxOptions);
            }
            else {
                helperApp.ShowErrorNotification("Some fields required, complete and then submit...");
            }
        });
    }

    vendor.OpenDeleteBid = (DomainKey) => {
        helperApp.BlockUI('js-pnl-helper');
        axios.get(`${appUrl}${urls.initDeleteBidUi}${DomainKey}`).then(function (response) {
            helperApp.DynamicModal('lg', 'show', 'Delete Bid', response.data);
            helperApp.RefreshDOM();
            helperApp.UnBlockUI('js-pnl-helper');
        });
    };

    vendor.DeleteBid = () => {
        $('#form').submit(function (e) {
            e.preventDefault();
            e.stopImmediatePropagation();

            if ($('#form').valid()) {
                var bidQuoteDomainKey = $('#BidQuoteDomainKey').val();
                var data = $('#form').serialize();
                $.ajax({
                    beforeSend: () => {
                        helperApp.BlockUI('js-pnl-helper');
                    },
                    type: "POST",
                    url: `${appUrl}${urls.DeleteBidUi}`,
                    data: data,
                    success: (results) => {
                        if (!!results && results.length > 4 && results.substring(0, 5).toLowerCase() === 'error') {
                            helperApp.ShowErrorNotification(results);
                            $('#js-pnl-validationresultmessage').removeClass('d-none').html(`<div class="alert alert-warning">${results}</div>`);
                            
                            helperApp.UnBlockUI('js-pnl-helper');
                        }
                        else {
                            helperApp.UnBlockUI('js-pnl-helper');
                            helperApp.ShowSuccessNotification(`SUCCESS: Deleted ${results} Bid`);
                            $('#divBidPartsPartial').load(`${appUrl}${urls.bidPartsPartialUrl}${bidQuoteDomainKey}`, () => {
                                helperApp.InitDtSimple('dt-vendorbids', '0', 'asc');
                                
                            });
                            $('#js-dynamic-modal').modal('toggle');
                            location.reload();
                        }
                    },
                    error: (xhr, textStatus, errorThrown) => {
                        console.log("Error");
                    }
                })
                

            }
            else {
                helperApp.ShowErrorNotification("Some fields required, complete and then submit...");
                
            }
        });
    }

    //*************************************************************************
    //* pop up the modal for uploading an invoice                             *
    //*************************************************************************
    vendor.loadUploadInvoiceUi = (bidQuoteId) => {
        helperApp.UnBlockUI('js-pnl-helper');
        axios.get(`${appUrl}${urls.initUploadVendorBidQuoteAttachmentUi}${bidQuoteId}?token=${helperApp.Guid()}`)
            .then((response) => {
                helperApp.DynamicModal('lg', 'show', 'Upload / Delete File', response.data);
                vendor.onDigitalFileUploadChange();
                helperApp.RefreshDOM();
            });
    }

    //*************************************************************************
    //* onchange                                                              *
    //*************************************************************************
    vendor.onDigitalFileUploadChange = () => {
        $('#File').bind('change', function () {
            // 15000000 15 MB
            let checkForExt = Boolean(helperApp.HasExtension('File', ['.pdf', '.PDF','.Pdf','PDf','.pDF','.pdF']));
            let size = this.files[0].size;
            let allowedSize = 15000000;
            if (size >= allowedSize || checkForExt === false) {
                // not allowed to upload
                $('#js-pnl-file-validation-error').removeClass('d-none');
                $('#js-btn-submitfile').prop('disabled', true).attr('type', 'button');
            } else {
                $('#js-btn-submitfile').removeClass('d-none').prop('disabled', false).attr('type', 'submit');
                $('#js-pnl-file-validation-error').addClass('d-none');
            }
        });
    };

    //*************************************************************************
    //* Save                                                                  *
    //*************************************************************************
    vendor.saveInvoiceFile = () => {
        $('#form-attachment').submit((e) => {
            e.preventDefault();
            e.stopImmediatePropagation();

            if ($('#form-attachment').valid()) {
                let ajaxOptions = {
                    beforeSend: () => {
                        helperApp.BlockUI('js-pnl-attachment-manager');
                    },
                    success: (data) => {
                        if (data === 'error') {
                            helperApp.ShowErrorNotification("Error when saving the data! Web Team Was Notified!");
                            helperApp.UnBlockUI('js-pnl-attachment-manager');
                        } else {
                            helperApp.ShowSuccessNotification("File successfully saved");
                            helperApp.UnBlockUI('js-pnl-attachment-manager');
                            $('#js-pnl-attachments').html(data);
                            $('#js-btn-submitfile').addClass('d-none').prop('disabled', true).attr('type', 'button');
                            $('#form-attachment').trigger('reset');
                        }
                    },
                    error: (xhr, textStatus, errorThrown) => {
                        console.log(errorThrown);
                        helperApp.UnBlockUI('js-pnl-attachment-manager');
                        helperApp.ShowErrorNotification("Error when saving the data! Web Team Was Notified!");
                    }
                };

                $('#form-attachment').ajaxSubmit(ajaxOptions);
            }
            else {
                helperApp.ShowErrorNotification("Some fields required, complete and then submit...");
                helperApp.UnBlockUI('js-pnl-attachment-manager');
            }
        });
    }

    //*************************************************************************
    //* Delete                                                                *
    //*************************************************************************
    vendor.deleteInvoice = (attachmentId, bidQuoteId) => {
        let payload = {
            bidQuoteId: bidQuoteId,
            AttachmentId: attachmentId
        };

        swal({
            title: 'Are you sure you want to delete this file?',
            text: 'This Action will delete the file from our system. Do you still want to CONTINUE?',
            icon: "error",
            confirmButtonClass: 'btn-danger',
            buttons: true,
            dangerMode: true
        }).then((isConfirm) => {
            if (isConfirm) {
                helperApp.BlockUI('js-pnl-attachment-manager');

                const url = `${appUrl}${urls.initDeleteAttachment}`;

                axios.post(url, payload).then((response) => {
                    $('#js-pnl-attachments').html(response.data);
                    helperApp.UnBlockUI('js-pnl-attachment-manager');
                });
            }
        });
    };



    return vendor;
})({});