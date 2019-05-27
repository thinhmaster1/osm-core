var productCategoryController = function () {
    this.initialize = function () {
        loadTreeData();
        loadData();
        registerEvents();
    }
    function loadTreeData() {
        $.ajax({
            url: '/Admin/ProductCategory/GetAll',
            dataType: 'json',
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });

                });
                var treeArr = osm.unflattern(data);
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });
                $('#treeProductCategory').tree({
                    data: treeArr
                  
                });
            }
        });
    }
    function loadData(isPageChanged) {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            data: {
                keyword: $('#txtKeyword').val(),
                page: osm.configs.pageIndex,
                pageSize: osm.configs.pageSize
            },
            url: '/admin/productCategory/GetAllPaging',
            dataType: 'json',
            success: function (response) {
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        CreatedDate: osm.dateTimeFormatJson(item.DateCreated),
                        ModifiedDate: osm.dateTimeFormatJson(item.DateModified),
                        Status: osm.getStatus(item.Status)
                    });
                });
                $('#lblTotalRecords').text(response.RowCount);
                if (render != '') {
                    $('#tbl-content').html(render);
                }
                wrapPaging(response.RowCount, function () {
                    loadData();
                }, isPageChanged);
            },
            error: function (status) {
                console.log(status);
                osm.notify('Cannot loading data', 'error');
            }
        });
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
    function registerEvents() {
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: { required: true },
                txtOrderM: { number: true },
                txtHomeOrderM: { number: true }
            }
        });
        $('#ddlShowPage').on('change', function () {
            osm.configs.pageSize = $(this).val();
            osm.configs.pageIndex = 1;
            loadData(true);
        });
        $('#btnSearch').on('click', function () {
            loadData();
        });
        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadData();
            }
        });
        $('#btnCreate').off('click').on('click', function (e) {
            e.preventDefault();
            resetFormMaintainance();
            initTreeDropDownCategory();
            $('#modal-add-edit').modal('show');
        });
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/ProductCategory/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    osm.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    initTreeDropDownCategory(data.ParentId);
                    $('#txtDescM').val(data.Description);
                    $('#txtImageM').val(data.ThumbnailImage);
                    $('#txtSeoKeywordM').val(data.SeoKeywords);
                    $('#txtSeoDescriptionM').val(data.SeoDescription);
                    $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                    $('#txtAliasM').val(data.SeoAlias);
                    $('#ckStatusM').prop('checked', data.Status == 1);
                    $('#ckShowHomeM').prop('checked', data.HomeFlag);
                    $('#txtOrderM').val(data.SortOrder);
                    $('#txtHomeOrderM').val(data.HomeOrder);
                    $('#hidDC').val(data.DateCreated);
                    $('#modal-add-edit').modal('show');
                    osm.stopLoading();
                },
                error: function (status) {
                    osm.notify('Has an error', 'error');
                    osm.stopLoading();
                }
            });
        });
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            osm.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/ProductCategory/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        osm.startLoading();
                    },
                    success: function (response) {
                        osm.notify('Deleted success', 'success');
                        osm.stopLoading();
                        loadData();
                    },
                    error: function (status) {
                        osm.notify('Has an error in deleting progress', 'error');
                        osm.stopLoading();
                    }
                });
            });
        });
        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });
        $("#fileInputImage").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    $('#txtImageM').val(path);
                    osm.notify('Upload image successful!', 'success');
                },
                error: function () {
                    osm.notify('There was error uploading files!', 'error');
                }
            });
        });
        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = parseInt($('#hidIdM').val());
                var name = $('#txtNameM').val();
                var parentId = $('#ddlCategoryIdM').combotree('getValue');
                var description = $('#txtDescM').val();
                var image = $('#txtImageM').val();
                var order = parseInt($('#txtOrderM').val());
                var homeOrder = $('#txtHomeOrderM').val();
                var seoKeyword = $('#txtSeoKeywordM').val();
                var seoMetaDescription = $('#txtSeoDescriptionM').val();
                var seoPageTitle = $('#txtSeoPageTitleM').val();
                var seoAlias = $('#txtAliasM').val();
                var status = $('#ckStatusM').prop('checked') == true ? 1 : 0;
                var showHome = $('#ckShowHomeM').prop('checked');
                var dateCreated = $('#hidDC').val();
                $.ajax({
                    type: "POST",
                    url: "/Admin/ProductCategory/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Description: description,
                        ParentId: parentId,
                        HomeOrder: homeOrder,
                        SortOrder: order,
                        HomeFlag: showHome,
                        Image: image,
                        Status: status,
                        SeoPageTitle: seoPageTitle,
                        SeoAlias: seoAlias,
                        SeoKeywords: seoKeyword,
                        SeoDescription: seoMetaDescription,
                        DateCreated: dateCreated
                    },
                    dataType: "json",
                    beforeSend: function () {
                        osm.startLoading();
                    },
                    success: function (response) {
                        osm.notify('Update success', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();
                        osm.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        osm.notify('Has an error in update progress', 'error');
                        osm.stopLoading();
                    }
                });
            }
            return false;
        });
    }
    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');
        $('#txtDescM').val('');
        $('#txtOrderM').val('');
        $('#txtHomeOrderM').val('');
        $('#txtImageM').val('');
        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtAliasM').val('');
        $('#ckStatusM').prop('checked', true);
        $('#ckShowHomeM').prop('checked', false);
        var newDate = new Date();
        $('#hidDC').val(newDate.toJSON());
    }
    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/ProductCategory/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId ,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = osm.unflattern(data);
                $('#ddlCategoryIdM').combotree({
                    data: arr
                });
                if (selectedId != undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            }
        });
    }
}