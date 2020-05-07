$(function () {
    $("#btnLogout").click(function () {
        $.ajax({
            url: '/admin/auth/logout'
            //cache: false
            //success: function (msg) {
            //    window.onload = function () {
            //        swal("", "Logout account success!", "success").then(function () {
            //            window.location.href = "/Trangchu/lll";
            //        });
            //    };
            //}
        });
    });
});