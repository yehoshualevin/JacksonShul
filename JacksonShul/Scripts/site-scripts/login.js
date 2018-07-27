
$(function () {
    $("#email").on('keyup', setButtonValidity);
    $("#password").on('keyup', setButtonValidity);

    function setButtonValidity() {
        $(".btn").prop('disabled', !isFormValid());
    }

    function isFormValid() {
        const email = isValidEmail($("#email").val());
        const password = $("#password").val();
        return email && password;
    }

    function isValidEmail(email) {
        var re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }

});