var BlogController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
        registerControls();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: { required: true },
                txtAliasM: { required: true }
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

        $("#ddl-show-page").on('change', function () {
            osm.configs.pageSize = $(this).val();
            osm.configs.pageIndex = 1;
            loadData(true);
        });

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');

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
                    osm.notify('Upload image succesful!', 'success');

                },
                error: function () {
                    osm.notify('There was error uploading files!', 'error');
                }
            });
        });
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Blog/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    osm.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    $('#txtImageM').val(data.Image);
                    $('#txtAliasM').val(data.SeoAlias);
                    CKEDITOR.instances.txtContentM.setData(data.Content);
                    $('#ckStatusM').prop('checked', data.Status === 1);
                    $('#txtDescM').val(data.Description);
                    $('#hidDC').val(data.DateCreated);
                    $('#ckHotFlagM').prop('checked', data.HotFlag);
                    $('#ckHomeFlagM').prop('checked', data.HomeFlag);
                    $('#modal-add-edit').modal('show');
                    osm.stopLoading();

                },
                error: function () {
                    osm.notify('Has an error', 'error');
                    osm.stopLoading();
                }
            });
        });

        $('#btnSaveM').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidIdM').val();
                var name = $('#txtNameM').val();
                var seoAlias = $('#txtAliasM').val();
                var content = CKEDITOR.instances.txtContentM.getData();
                var author = $('#hidIdAuthorM').val();
                var image = $('#txtImageM').val();
                var description = $('#txtDescM').val();
                var dateCreated = $('#hidDC').val();
                var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
                var hotFlag = $('#ckHotFlagM').prop('checked');
                var homeFlag = $('#ckHomeFlagM').prop('checked');
                $.ajax({
                    type: "POST",
                    url: "/Admin/Blog/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Content: content,
                        Status: status,
                        SeoAlias: seoAlias,
                        Author: author,
                        Image: image,
                        Description: description,
                        Datecreated: dateCreated,
                        HotFlag: hotFlag,
                        HomeFlag: homeFlag
                    },
                    dataType: "json",
                    beforeSend: function () {
                        osm.startLoading();
                    },
                    success: function () {
                        osm.notify('Update page successful', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();

                        osm.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        osm.notify('Have an error in progress', 'error');
                        osm.stopLoading();
                    }
                });
                return false;
            }
            return false;
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            osm.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Blog/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        osm.startLoading();
                    },
                    success: function () {
                        osm.notify('Delete page successful', 'success');
                        osm.stopLoading();
                        loadData();
                    },
                    error: function () {
                        osm.notify('Have an error in progress', 'error');
                        osm.stopLoading();
                    }
                });
            });
        });
    };

    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        $('#txtAliasM').val('');
        CKEDITOR.instances.txtContentM.setData('');
       
        $('#txtImageM').val('');
        $('#txtDescM').val('');
        var date = new Date();
        $('#hidDC').val(date.toJSON());
        $('#ckStatusM').prop('checked', true);
        $('#ckHotFlagM').prop('checked', false);
        $('#ckHomeFlagM').prop('checked', false);
    }

    function registerControls() {
        var editorConfig = {
            filebrowserImageUploadUrl: '/Admin/Upload/UploadImageForCKEditor?type=Images'
        }
        CKEDITOR.replace('txtContentM', editorConfig);
        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };
    }

    function loadData(isPageChanged) {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: "GET",
            url: "/admin/blog/GetAllPaging",
            data: {
                keyword: $('#txt-search-keyword').val(),
                page: osm.configs.pageIndex,
                pageSize: osm.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                osm.startLoading();
            },
            success: function (response) {
                if (response.RowCount > 0) {
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            Name: item.Name,
                            Id: item.Id,
                            DateCreated: osm.dateTimeFormatJson(item.DateCreated),
                            DateModified: osm.dateTimeFormatJson(item.DateModified),
                            HotFlag: osm.getStatus(item.HotFlag),
                            HomeFlag: osm.getStatus(item.HomeFlag),
                            Status: osm.getStatus(item.Status)
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
                    $('#tbl-content').html('');
                }
                osm.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

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