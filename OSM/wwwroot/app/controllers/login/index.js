var loginController = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {
        $('#frmLogin').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                userName: {
                    required: true
                },
                password: {
                    required: true
                }
            }
        });
        $('#btnLogin').on('click', function (e) {
            if ($('#frmLogin').valid()) {
                e.preventDefault();
                var user = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                login(user, password);
            }
        });
    }

    var login = function (user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                Email: user,
                Password: pass
            },
            dataType: 'json',
            url: '/Admin/Login/Authen',
            success: function (res) {
                if (res.Success) {
                    window.location.href = "/Admin/Home/Index";
                }
                else {
                    osm.notify('Your username or password is wrong!', 'error');
                }
            }
        })
    }
}