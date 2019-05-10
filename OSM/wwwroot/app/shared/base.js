var BaseController = function () {
    this.initialize = function () {
        loadAnnouncement();
    }
};
function hasRead(that) {
    $.ajax({
        type: "POST",
        url: "/admin/announcement/MarkAsRead",
        data:{
            id: that
        },
        dataType: "json",
        beforeSend: function () {
            osm.startLoading();
        },
        success: function (response) {
            loadAnnouncement();
            osm.stopLoading();
        },
        error: function (status) {
            console.log(status);
        }
    });
}
function loadAnnouncement() {
    $.ajax({
        type: "GET",
        url: "/admin/announcement/GetAllPaging",
        data: {
            page: osm.configs.pageIndex,
            pageSize: osm.configs.pageSize
        },
        dataType: "json",
        beforeSend: function () {
            osm.startLoading();
        },
        success: function (response) {
            var template = $('#announcement-template').html();
            var render = "";
            if (response.RowCount > 0) {
                $('#announcementArea').show();
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Content: item.Content,
                        Id: item.Id,
                        Title: item.Title,
                        FullName: item.FullName,
                        Avatar: item.Avatar
                    });
                });
                render += $('#announcement-tag-template').html();
                $("#totalAnnouncement").text(response.RowCount);
                if (render != undefined) {
                    $('#annoncementList').html(render);
                }
            }
            else {
                $('#announcementArea').hide();
                $('#annoncementList').html('');
            }
            osm.stopLoading();
        },
        error: function (status) {
            console.log(status);
        }
    });
};
