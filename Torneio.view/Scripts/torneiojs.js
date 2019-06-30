jQuery(document).ready(function ($) {
    $('#GerarNova').click(function () {
        if ($(this).is(':checked')) {
            $(".form-row.times").removeAttr("hidden");
        } else {
            $(".form-row.times").attr("hidden", "hidden");
        }
    });
});