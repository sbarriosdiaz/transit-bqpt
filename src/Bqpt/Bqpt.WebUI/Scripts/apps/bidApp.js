let bidApp = ((bidApp) => {

    const urls = {
        initBidImportUi: `Bids/InitBidImport/`,
        ImportUi: `Bids/InitBidImport/`,
        initBidDeleteUi: `Bids/InitBidDelete/`,
        initBidStartUi: `Bids/InitBidStart/`,
        initBidEndUi: `Bids/InitBidEnd/`,
        initBidCloseUi: `Bids/InitBidClose/`,
        initBidUpdateUi: `Bids/InitBidUpdate/`,
        selectBidderUi: `Bids/SelectBidder/`,
        updatePOQuantityUi: `Bids/UpdatePurchaseOrderQuantity/`,
        initBidVendorNoEmailUi: `Bids/GetVendorNoEmails/`,
        initSyncVendorInfoUi: 'Bids/SyncVendorInfo/',
        initGeneratePoUI: 'Bids/GenerateXmlHeaders/'
    };

    // === Bid Import ===
    bidApp.OpenImportBidModal = (assetWorksBidQuoteId) => {
        helperApp.BlockUI('js-pnl-helper');
        axios.get(`${appUrl}${urls.initBidImportUi}${assetWorksBidQuoteId}`)
            .then((response) => {
                helperApp.DynamicModal('lg', 'show', 'Import Bid', response.data);
                helperApp.RefreshDOM();
                helperApp.UnBlockUI('js-pnl-helper');
            });
    };

    // === Sync Vendors ===
    bidApp.OpenSyncVendorInfo = () => {
        helperApp.BlockUI('js-pnl-helper');
        axios.post(appUrl + urls.initSyncVendorInfoUi)
            .then((response) => {
                if (response.data.redirectUrl) {
                    window.location.href = response.data.redirectUrl;
                } else {
                    helperApp.RefreshDOM();
                    helperApp.ShowSuccessNotification("Vendors successfully updated");
                    helperApp.UnBlockUI('js-pnl-helper');
                }
            })
            .catch((error) => {
                console.error(error);
                helperApp.ShowErrorNotification("ERROR: An error occurred while syncing vendor info.");
                helperApp.UnBlockUI('js-pnl-helper');
            });
    };

    // === Delete Bid ===
    bidApp.OpenDeleteBidModal = (assetWorksBidQuoteId) => {
        helperApp.BlockUI('js-pnl-helper');
        axios.get(`${appUrl}${urls.initBidDeleteUi}${assetWorksBidQuoteId}`)
            .then((response) => {
                helperApp.DynamicModal('lg', 'show', 'Delete Bid', response.data);
                helperApp.RefreshDOM();
                helperApp.UnBlockUI('js-pnl-helper');
            });
    };

    // === Start Bid ===
    bidApp.OpenStartBidModal = (assetWorksDomainKey) => {
        helperApp.BlockUI('js-pnl-helper');
        axios.get(`${appUrl}${urls.initBidStartUi}${assetWorksDomainKey}`)
            .then((response) => {
                helperApp.DynamicModal('lg', 'show', 'Start Bid', response.data);
                helperApp.RefreshDOM();
                helperApp.UnBlockUI('js-pnl-helper');
            });
    };

    // === Update Bid ===
    bidApp.OpenUpdateBidModal = (assetWorksDomainKey) => {
        helperApp.BlockUI('js-pnl-helper');
        axios.get(`${appUrl}${urls.initBidUpdateUi}${assetWorksDomainKey}`)
            .then((response) => {
                helperApp.DynamicModal('lg', 'show', 'Edit Bid', response.data);
                helperApp.RefreshDOM();
                helperApp.UnBlockUI('js-pnl-helper');
            });
    };

    // === End Bid ===
    bidApp.OpenEndBidModal = (assetWorksBidQuoteId) => {
        helperApp.BlockUI('js-pnl-helper');
        axios.get(`${appUrl}${urls.initBidEndUi}${assetWorksBidQuoteId}`)
            .then((response) => {
                helperApp.DynamicModal('lg', 'show', 'End Bid', response.data);
                helperApp.RefreshDOM();
                helperApp.UnBlockUI('js-pnl-helper');
            });
    };

    // === Close Bid ===
    bidApp.OpenCloseBidModal = (assetWorksBidQuoteId) => {
        helperApp.BlockUI('js-pnl-helper');
        axios.get(`${appUrl}${urls.initBidCloseUi}${assetWorksBidQuoteId}`)
            .then((response) => {
                helperApp.DynamicModal('lg', 'show', 'Close Bid', response.data);
                helperApp.RefreshDOM();
                helperApp.UnBlockUI('js-pnl-helper');
            });
    };

    // === Vendor No Email ===
    bidApp.OpenVendorNoEmailModal = (assetWorksBidQuoteId) => {
        helperApp.BlockUI('js-pnl-helper');
        axios.get(`${appUrl}${urls.initBidVendorNoEmailUi}${assetWorksBidQuoteId}`)
            .then((response) => {
                helperApp.DynamicModal('lg', 'show', 'Vendor No Email', response.data);
                helperApp.RefreshDOM();
                helperApp.UnBlockUI('js-pnl-helper');
            });
    };

    // === Select Vendor ===
    bidApp.SelectVendor = (domainKey, bidQuotePartId) => {
        let bidQuotePart = "js-tr_" + bidQuotePartId + "_";
        document.querySelectorAll('[id^=' + bidQuotePart + ']').forEach(function (item) {
            item.classList.remove("bg-selected-bidder");
        });

        const vm = { BidQuotePartId: bidQuotePartId, DomainKey: domainKey };

        axios.post(`${appUrl}${urls.selectBidderUi}`, vm)
            .then((results) => {
                if (!!results && results.length > 4 && results.substring(0, 5).toLowerCase() === 'error') {
                    helperApp.ShowErrorNotification(results);
                    $('#js-pnl-validationresultmessage').removeClass('d-none')
                        .html(`<div class="alert alert-warning">${results}</div>`);
                    helperApp.UnBlockUI('js-pnl-helper');
                } else {
                    if (results.data == "True") {
                        $(`#js-tr_${bidQuotePartId}_${domainKey}`).addClass('bg-selected-bidder');
                        helperApp.ShowSuccessNotification("SUCCESS: Selected Bid");
                    } else {
                        helperApp.ShowSuccessNotification("SUCCESS: Un-Selected Bid");
                    }
                }
            })
            .catch((error) => {
                console.log(error);
                helperApp.ShowErrorNotification("ERROR: An error happened when saving your data.");
            });
    };

    // === Update PO Quantity ===
    bidApp.UpdatePOQuantity = (domainKey, purchaseOrderQuantity) => {
        const vm = { DomainKey: domainKey, PurchaseOrderQuantity: purchaseOrderQuantity };
        axios.post(`${appUrl}${urls.updatePOQuantityUi}`, vm)
            .then((response) => {
                if (response.data === 'validation_error') {
                    helperApp.ShowErrorNotification("WARNING: Validation Errors were found in your form, check fields and submit again.");
                } else {
                    helperApp.ShowSuccessNotification("SUCCESS: Updated Purchase Order Quantity");
                }
            })
            .catch((error) => {
                console.log(error);
                helperApp.ShowErrorNotification("ERROR: An error happened when saving your data.");
            });
    };

    // === Toggle Vendors ===
    bidApp.ToggleVendors = (show) => {
        if (show == 1) {
            document.querySelectorAll('[id^=Vendor_Collapse_]').forEach((item) => item.classList.add("show"));
            $(`#js-btn-toggle`).val(0).html('Hide Vendors');
        } else {
            document.querySelectorAll('[id^=Vendor_Collapse_]').forEach((item) => item.classList.remove("show"));
            document.querySelectorAll('[id^=Contact_Collapse_]').forEach((item) => item.classList.remove("show"));
            $(`#js-btn-toggle`).val(1).html('Show Vendors');
        }
    };

    // === Generate PO: Open Modal ===
    bidApp.OpenModalGeneratePO = (domainKey) => {
        $('#generatePoModal').data('domainKey', domainKey);
        const $btn = $("#btn-generate-xml");
        $btn.prop("disabled", false).text("Generate PO");
        $('#generatePoModal').modal('show');
    };

    // === Generate PO: Confirm (Redirección + TempData notifications) ===
    bidApp.ConfirmGeneratePO = () => {
        const domainKey = $('#generatePoModal').data('domainKey');
        const url = `${appUrl}${urls.initGeneratePoUI}?domainKey=${domainKey}`;
        const $btn = $("#btn-generate-xml");

        // Deshabilitar botón y mostrar texto generando
        $btn.prop("disabled", true).text("Generating...");

        // Cerrar modal
        $('#generatePoModal').modal('hide');

        // Redirigir al servidor para manejar notificaciones TempData
        window.location.href = url;
    };

    // === Reset button state cuando modal se oculta (por seguridad) ===
    $('#generatePoModal').on('hidden.bs.modal', function () {
        const $btn = $("#btn-generate-xml");
        $btn.prop("disabled", false).text("Generate PO");
        $("#btn-generate-xml").focus();
    });

    return bidApp;

})({});
