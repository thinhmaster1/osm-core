﻿var BillController = function () {
    var cachedObj = {
        products: [],
        colors: [],
        sizes: [],
        paymentMethods: [],
        billStatuses: []
    }
    this.initialize = function () {
        $.when(loadBillStatus(),
            loadPaymentMethod(),
            loadColors(),
            loadSizes(),
            loadProducts())
            .done(function () {
                loadData();
            });

        registerEvents();
    }
    var statusActivities = document.getElementById("ddlStatus");
    statusActivities.addEventListener("change", function () {
        loadData(true);
    });
    function registerEvents() {
        $('#txtFromDate, #txtToDate').datepicker({
            autoclose: true,
            format: 'dd/mm/yyyy'
        });
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtCustomerName: { required: true },
                txtCustomerAddress: { required: true },
                txtCustomerMobile: { required: true },
                ddlBillStatus: { required: true }
            }
        });
        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });
        $("#btn-search").on('click', function () {
            loadData();
        });

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-detail').modal('show');
        });
        $("#ddl-show-page").on('change', function () {
            osm.configs.pageSize = $(this).val();
            osm.configs.pageIndex = 1;
            loadData(true);
        });

        $('body').on('click', '.btn-view', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Bill/GetById",
                data: { id: that },
                beforeSend: function () {
                    osm.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#hidCustomerId').val(data.CustomerId);
                    $('#hidDC').val(data.DateCreated);
                    $('#hidStatus').val(data.Status);
                    $('#txtCustomerName').val(data.CustomerName);
                    $('#txtCustomerAddress').val(data.CustomerAddress);
                    $('#txtCustomerMobile').val(data.CustomerMobile);
                    $('#txtCustomerMessage').val(data.CustomerMessage);
                    $('#ddlPaymentMethod').val(data.PaymentMethod);
                    $('#ddlBillStatus').val(data.BillStatus);

                    var billDetails = data.BillDetails;
                    var totalPrice = 0;
                    if (data.BillDetails != null && data.BillDetails.length > 0) {
                        var render = '';
                        var templateDetails = $('#template-table-bill-details').html();

                        $.each(billDetails, function (i, item) {
                            var products = getProductOptions(item.ProductId);
                            var colors = getColorOptions(item.ColorId);
                            var sizes = getSizeOptions(item.SizeId);
                            totalPrice += item.Price * item.Quantity;
                            render += Mustache.render(templateDetails,
                                {
                                    Id: item.Id,
                                    Products: products,
                                    Colors: colors,
                                    Sizes: sizes,
                                    Quantity: item.Quantity
                                });
                        });
                        $('#tbl-bill-details').html(render);
                    }
                    $('#totalPrice').text(osm.formatNumber(totalPrice,0));
                    var pending = document.getElementById('btnPending');
                    var confirm = document.getElementById('btnConfirm');
                    var cancel = document.getElementById('btnCancel');
                    var save = document.getElementById('btnSave');
                    var returned = document.getElementById('btnReturn');
                    var refuse = document.getElementById('btnRefuse');
                    if (data.BillStatus == 0) {
                        pending.disabled = false;
                        cancel.disabled = false;
                        save.disabled = false;
                        returned.disabled = true;
                        refuse.disabled = true;
                        confirm.disabled = true;
                    }
                    else if (data.BillStatus == 1 ) {
                        pending.disabled = true;
                        save.disabled = true;
                        cancel.disabled = true;
                        returned.disabled = true;
                        refuse.disabled = true;
                        confirm.disabled = false;
                    }
                    else if (data.BillStatus == 2 && data.Status == 1) {
                        pending.disabled = true;
                        save.disabled = true;
                        cancel.disabled = true;
                        returned.disabled = false;
                        refuse.disabled = false;
                        confirm.disabled = true;
                    }
                    else {
                        pending.disabled = true;
                        save.disabled = true;
                        cancel.disabled = true;
                        returned.disabled = true;
                        refuse.disabled = true;
                        confirm.disabled = true;
                    }
                    $('#modal-detail').modal('show');
                    osm.stopLoading();
                },
                error: function (e) {
                    osm.notify('Has an error in progress', 'error');
                    osm.stopLoading();
                }
            });
        });

        $('#btnSave').on('click', function () {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidId').val();
                var customerId = $('#hidCustomerId').val();
                var dateCreated = $('#hidDC').val();
                var status = $('#hidStatus').val();
                var customerName = $('#txtCustomerName').val();
                var customerAddress = $('#txtCustomerAddress').val();
                var customerMobile = $('#txtCustomerMobile').val();
                var customerMessage = $('#txtCustomerMessage').val();
                var paymentMethod = $('#ddlPaymentMethod').val();
                var billStatus = $('#ddlBillStatus').val();
                //bill detail

                var billDetails = [];
                $.each($('#tbl-bill-details tr'), function (i, item) {
                    billDetails.push({
                        Id: $(item).data('id'),
                        ProductId: $(item).find('select.ddlProductId').first().val(),
                        Quantity: $(item).find('input.txtQuantity').first().val(),
                        ColorId: $(item).find('select.ddlColorId').first().val(),
                        SizeId: $(item).find('select.ddlSizeId').first().val(),
                        BillId: id
                    });
                });

                $.ajax({
                    type: "POST",
                    url: "/Admin/Bill/SaveEntity",
                    data: {
                        Id: id,
                        CustomerId: customerId,
                        DateCreated: dateCreated,
                        BillStatus: billStatus,
                        CustomerAddress: customerAddress,
                        CustomerId: customerId,
                        CustomerMessage: customerMessage,
                        CustomerMobile: customerMobile,
                        CustomerName: customerName,
                        PaymentMethod: paymentMethod,
                        Status: status,
                        BillDetails: billDetails
                    },
                    dataType: "json",
                    beforeSend: function () {
                        osm.startLoading();
                    },
                    success: function (response) {
                        osm.notify('Save order successful', 'success');
                        $('#modal-detail').modal('hide');
                        resetFormMaintainance();

                        osm.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        osm.notify('Has an error in progress', 'error');
                        osm.stopLoading();
                    }
                });
                return false;
            }
        });
        $('#btnPending').on('click', function () {
            $.ajax({
                url: '/Admin/Bill/UpdateStatus',
                type: 'post',
                data: { billId: $('#hidId').val(), billStatus: 1, status: 1 },
                dataType: 'json',
                success: function (data) {
                    osm.notify('The order is shipping.', 'success');
                    $('#modal-detail').modal('hide');
                    resetFormMaintainance();
                    loadData(true);
                }


            });
        });
        $('#btnConfirm').on('click', function () {
            $.ajax({
                url: '/Admin/Bill/UpdateStatus',
                type: 'post',
                data: { billId: $('#hidId').val(), billStatus: 4, status: 1 },
                dataType: 'json',
                success: function () {
                    osm.notify('The order is paid.', 'success');
                    $('#modal-detail').modal('hide');
                    resetFormMaintainance();
                    loadData(true);
                }
            });
        });
        $('#btnCancel').on('click', function () {
            e.preventDefault();
            $.ajax({
                url: '/Admin/Bill/UpdateStatus',
                type: 'post',
                data: { billId: $('#hidId').val(), billStatus: 3, status: 0 },
                dataType: 'json',
                success: function () {
                    osm.notify('The order is canceled.', 'success');
                    $('#modal-detail').modal('hide');
                    resetFormMaintainance();
                    loadData(true);
                }
            });
        });
        $('#btnAddDetail').on('click', function () {
            var template = $('#template-table-bill-details').html();
            var products = getProductOptions(null);
            var colors = getColorOptions(null);
            var sizes = getSizeOptions(null);
            var render = Mustache.render(template,
                {
                    Id: 0,
                    Products: products,
                    Colors: colors,
                    Sizes: sizes,
                    Quantity: 0,
                    Total: 0
                });
            $('#tbl-bill-details').append(render);
        });
        $('#btnReturn').on('click', function () {
            $.ajax({
                url: '/Admin/Bill/UpdateStatus',
                type: 'post',
                data: { billId: $('#hidId').val(), billStatus: 2, status: 0 },
                dataType: 'json',
                success: function () {
                    osm.notify('The order is refund.', 'success');
                    $('#modal-detail').modal('hide');
                    resetFormMaintainance();
                    loadData(true);
                }
            });
        });
        $('#btnRefuse').on('click', function () {
            $.ajax({
                url: '/Admin/Bill/UpdateStatus',
                type: 'post',
                data: { billId: $('#hidId').val(), billStatus: 4, status: 0 },
                dataType: 'json',
                success: function () {
                    osm.notify('The return is refused.', 'success');
                    $('#modal-detail').modal('hide');
                    resetFormMaintainance();
                    loadData(true);
                }
            });
        });
        $('body').on('click', '.btn-delete-detail', function () {
            $(this).parent().parent().remove();
        });

        $("#btnExport").on('click', function () {
            var that = $('#hidId').val();
            $.ajax({
                type: "POST",
                url: "/Admin/Bill/ExportExcel",
                data: { billId: that },
                beforeSend: function () {
                    osm.startLoading();
                },
                success: function (response) {
                    window.location.href = response;

                    osm.stopLoading();
                }
            });
        });
    };

    function loadBillStatus() {
        return $.ajax({
            type: "GET",
            url: "/admin/bill/GetBillStatus",
            dataType: "json",
            success: function (response) {
                cachedObj.billStatuses = response;
                var render = "";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Value + "'>" + item.Name + "</option>";
                });
                $('#ddlBillStatus').html(render);
            }
        });
    }

    function loadPaymentMethod() {
        return $.ajax({
            type: "GET",
            url: "/admin/bill/GetPaymentMethod",
            dataType: "json",
            success: function (response) {
                cachedObj.paymentMethods = response;
                var render = "";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Value + "'>" + item.Name + "</option>";
                });
                $('#ddlPaymentMethod').html(render);
            }
        });
    }

    function loadProducts() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Product/GetAll",
            dataType: "json",
            success: function (response) {
                cachedObj.products = response;
            },
            error: function () {
                osm.notify('Has an error in progress', 'error');
            }
        });
    }

    function loadColors() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetColors",
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function () {
                osm.notify('Has an error in progress', 'error');
            }
        });
    }

    function loadSizes() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetSizes",
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function () {
                osm.notify('Has an error in progress', 'error');
            }
        });
    }

    function getProductOptions(selectedId) {
        var products = "<select class='form-control ddlProductId'>";
        $.each(cachedObj.products, function (i, product) {
            if (selectedId === product.Id)
                products += '<option value="' + product.Id + '" selected="select">' + product.Name + '</option>';
            else
                products += '<option value="' + product.Id + '">' + product.Name + '</option>';
        });
        products += "</select>";
        return products;
    }

    function getColorOptions(selectedId) {
        var colors = "<select class='form-control ddlColorId'>";
        $.each(cachedObj.colors, function (i, color) {
            if (selectedId === color.Id)
                colors += '<option value="' + color.Id + '" selected="select">' + color.Name + '</option>';
            else
                colors += '<option value="' + color.Id + '">' + color.Name + '</option>';
        });
        colors += "</select>";
        return colors;
    }

    function getSizeOptions(selectedId) {
        var sizes = "<select class='form-control ddlSizeId'>";
        $.each(cachedObj.sizes, function (i, size) {
            if (selectedId === size.Id)
                sizes += '<option value="' + size.Id + '" selected="select">' + size.Name + '</option>';
            else
                sizes += '<option value="' + size.Id + '">' + size.Name + '</option>';
        });
        sizes += "</select>";
        return sizes;
    }
    function resetFormMaintainance() {
        $('#hidId').val(0);
        $('#hidCustomerId').val(0);
        $('#hidDC').val(new Date());
        $('#txtCustomerName').val('');

        $('#txtCustomerAddress').val('');
        $('#txtCustomerMobile').val('');
        $('#txtCustomerMessage').val('');
        $('#ddlPaymentMethod').val('');
        $('#ddlCustomerId').val('');
        $('#ddlBillStatus').val('');
        $('#tbl-bill-details').html('');
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/bill/GetAllPaging",
            data: {
                startDate: $('#txtFromDate').val(),
                endDate: $('#txtToDate').val(),
                keyword: $('#txtSearchKeyword').val(),
                status: $('#ddlStatus').val(),
                page: osm.configs.pageIndex,
                pageSize: osm.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                osm.startLoading();
            },
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            CustomerName: item.CustomerName,
                            Id: item.Id,
                            PaymentMethod: getPaymentMethodName(item.PaymentMethod),
                            DateCreated: osm.dateTimeFormatJson(item.DateCreated),
                            BillStatus: getBillStatusName(item.BillStatus),
                            Status: item.Status == 1 ? "On Going" : "Finished"
                        });
                    });
                    $("#lbl-total-records").text(response.RowCount);
                    if (render != undefined) {
                        $('#tbl-content').html(render);
                    }
                    wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);
                }
                else {
                    $("#lbl-total-records").text('0');
                    $('#tbl-content').html('');
                }
                osm.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };
    function getPaymentMethodName(paymentMethod) {
        var method = $.grep(cachedObj.paymentMethods, function (element, index) {
            return element.Value == paymentMethod;
        });
        if (method.length > 0)
            return method[0].Name;
        else return '';
    }
    function getBillStatusName(status) {
        var status = $.grep(cachedObj.billStatuses, function (element, index) {
            return element.Value == status;
        });
        if (status.length > 0)
            return status[0].Name;
        else return '';
    }
    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / osm.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: 'First',
            prev: 'Prev',
            next: 'Next',
            last: 'Last',
            onPageClick: function (event, p) {
                osm.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
}