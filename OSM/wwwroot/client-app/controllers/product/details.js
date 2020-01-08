var ProductDetailController = function () {
    this.initialize = function () {
        registerEvents();
        getQuantity();
    }
    var colorActivities = document.getElementById("ddlColorId");
    var sizeActivities = document.getElementById("ddlSizeId");
    colorActivities.addEventListener("change", function () {
        getQuantity();
        document.getElementById("txtQuantity").value = "1";
    });
    sizeActivities.addEventListener("change", function () {
        getQuantity();
        document.getElementById("txtQuantity").value = "1";
    });
    function getQuantity() {
        var _colorId = parseInt($('#ddlColorId').val());
        var id = parseInt($('#productId').val());
        var _sizeId = parseInt($('#ddlSizeId').val());
        $.ajax({
            url: '/Product/GetQuantity',
            type: 'get',
            dataType: 'json',
            data: {
                productId: id,
                colorId: _colorId,
                sizeId: _sizeId
            },
            success: function (data) {
                $('p#quantity').text(data + " product(s) available");
                if (parseInt(data) == 0) {
                    document.getElementById("txtQuantity").value = "0";
                    document.getElementById("btnAddToCart").disabled = true;
                    document.getElementById("span-cart").text("Out Of Stock");
                    var button = document.getElementById("btnAddToCart");
                    button.classList.remove("button.pro-add-to-cart");
                } else {
                    document.getElementById("btnAddToCart").disabled = false;
                    document.getElementById("btnAddToCart").text("Add To Cart");
                    button.classList.add("button.pro-add-to-cart");
                }
            },
            error: function () {
                console.log("Error");
            }
        });
    }
    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");
    }
    function registerEvents() {
        $('#btnAddToCart').on('click', function (e) {
            e.preventDefault();
            var id = parseInt($(this).data('id'));
            var login = $(this).data('login');
            var url = $(this).data('url');
            var colorId = parseInt($('#ddlColorId').val());
            var sizeId = parseInt($('#ddlSizeId').val());
            if (login == "False") {
                window.location.href = 'login.html?returnUrl=' + url;
                alert("You have to log in to add product to cart!");
            } else {
                alert("You have added product to cart!");
                $.ajax({
                    url: '/Cart/AddToCart',
                    type: 'post',
                    dataType: 'json',
                    data: {
                        productId: id,
                        quantity: parseInt($('#txtQuantity').val()),
                        color: colorId,
                        size: sizeId
                    },
                    success: function () {
                        loadHeaderCart();
                    }
                });
            }
        
        });
    }
}