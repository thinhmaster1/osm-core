var QuantityManagement = function () {
    var self = this;

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-quantity', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#hidId').val(that);
            loadQuantities();
            $('#modal-quantity-management').modal('show');
        });
        $('body').on('click', '.btn-delete-quantity', function (e) {
            e.preventDefault();
            $(this).closest('tr').remove();
        });

        $('#btn-add-quantity').on('click', function () {
            var template = $('#template-table-quantity').html();
            var render = Mustache.render(template, {
                Id: 0,
                Colors: '',
                Sizes: '',
                Quantity: 0
            });
            $('#table-quantity-content').append(render);
        });
        $("#btnSaveQuantity").on('click', function () {
            var quantityList = [];

            $.each($('#table-quantity-content').find('tr'), function (i, item) {
                var size = $(item).find('input.txtSize').first().val();
                var color = $(item).find('input.txtColor').first().val();
                addColor(color);
                addSize(size);
                var _color;
                var _size;
                $.ajax({
                    url: '/admin/Product/GetSize',
                    data: {
                        Name: size,
                    },
                    async: false,
                    type: 'get',
                    dataType: 'json',
                    success: function (data) {
                        console.log(data);
                        _size = data;
                    },
                    error: function () {
                        osm.notify('Has an error', 'error');
                    }
                });
                $.ajax({
                    url: '/admin/Product/GetColor',
                    data: {
                        Name: color,
                    },
                    type: 'get',
                    async: false,
                    dataType: 'json',
                    success: function (data) {
                        console.log(data);
                        _color = data;
                    },
                    error: function () {
                        osm.notify('Has an error', 'error');
                    }
                });
                quantityList.push({
                    Id: $(item).data('id'),
                    ProductId: $('#hidId').val(),
                    Quantity: $(item).find('input.txtQuantity').first().val(),
                    SizeId: _size.Id,
                    ColorId: _color.Id,
                });
            });
          
            $.ajax({
                url: '/admin/Product/SaveQuantities',
                data: {
                    productId: $('#hidId').val(),
                    quantities: quantityList
                },
                async: false,
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    $('#modal-quantity-management').modal('hide');
                    $('#table-quantity-content').html('');
                }
            });
        });
    }
    function loadQuantities() {
        $.ajax({
            url: '/admin/Product/GetQuantities',
            data: {
                productId: $('#hidId').val()
            },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                var render = '';
                var template = $('#template-table-quantity').html();
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Color: item.Color.Name,
                        Size: item.Size.Name,
                        Quantity: item.Quantity
                    });
                });
                $('#table-quantity-content').html(render);
            }
        });
    }

    function addColor(color) {
        $.ajax({
            url: '/admin/Product/AddColor',
            data: {
                Name: color,
            },
            async: false,
            type: 'post',
            dataType: 'json',
            success: function () {
                osm.notify('Added a new color!', 'success');
            },
            error: function () {
                osm.notify('Has an error', 'error');
            }
        });
    }
    function addSize(size) {
        $.ajax({
            url: '/admin/Product/AddSize',
            data: {
                Name: size,
            },
            type: 'post',
            async: false,
            dataType: 'json',
            success: function () {
                osm.notify('Added a new color!', 'success');
            },
            error: function () {
                osm.notify('Has an error', 'error');
            }
        });
    }
 
}