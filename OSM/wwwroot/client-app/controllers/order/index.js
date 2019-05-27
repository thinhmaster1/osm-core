var OrderController = function () {
    this.initialize = function () {
        registerEvents();
       
    }
    function registerEvents() {
        $('#btnCancel').on('click', function () {
            var status = $('#btnCancel').data("status");
            if (status == "New") {
                var retVal = confirm("You only cancel if status of the order is still new. Do you want to cancel the order ?");
                if (retVal == true) {
                    var order = $('#btnCancel').data("id");
                    $.ajax({
                        url: '/Manage/UpdateOrderStatus',
                        type: 'post',
                        data: { orderId: order, billStatus: 3, status: 0 },
                        datatype: 'json',
                        success: function (message) {
                            alert(message);
                            location.reload();
                        }
                    });
                    return true;
                } else {
                    return false;
                }
            } else {
                alert("You cannot cancel it!!");
            }

        });
        $('#btnReturn').on('click', function () {
            var status = $('#btnReturn').data("status");
            if (status == "Completed") {
                var retVal = confirm("Do you want to return the order ?");
                if (retVal == true) {
                    var order = $('#btnReturn').data("id");
                    $.ajax({
                        url: '/Manage/UpdateOrderStatus',
                        type: 'post',
                        data: { orderId: order, billStatus: 2, status: 1 },
                        datatype: 'json',
                        success: function (message) {
                            alert(message);
                            location.reload();
                        }
                    });
                    return true;
                } else {
                    return false;
                }
            } else {
                alert("You cannot return it.");
            }
          
        });
        $('#btnComplete').on('click', function () {
            var status = $('#btnComplete').data("status");
            if (status == "Completed") {
                var retVal = confirm("Do you want to complete the order ? If you complete order, you cannot return anymore.");
                if (retVal == true) {
                    var order = $('#btnCancel').data("id");
                    $.ajax({
                        url: '/Manage/UpdateOrderStatus',
                        type: 'post',
                        data: { orderId: order, billStatus: 4, status: 0 },
                        datatype: 'json',
                        success: function (message) {
                            alert(message);
                            location.reload();
                        }
                    });
                    return true;
                } else {
                    return false;
                }
            } else {
                alert("You cannot complete it!");
            }
        });
    }
}