$(() => {
    $("#firstname").on('keyup', setButtonValidity);
    $("#lastname").on('keyup', setButtonValidity);
    $("#cell").on('keyup', setButtonValidity);
    $("#email").on('keyup', setButtonValidity);
    $("#password").on('keyup', setButtonValidity);
    $("#password2").on('keyup', setButtonValidity);

    function setButtonValidity() {
        $("#submit-button").prop('disabled', !isFormValid());
    }

    function isFormValid() {
        const firstname = $("#firstname").val();
        const lastname = $("#lastname").val();
        const cell = $("#cell").val();
        const email = isValidEmail($("#email").val());
        const password = $("#password").val();
        const password2 = $("#password2").val();
        const same = isSame(password, password2);
        return firstname && lastname && cell.length >= 10 && email && password.length >= 6 && same;
    }

    function isSame(a, b) {
        if (a === b) {
            return true;
        } return false;
    }

    function isValidEmail(email) {
        var re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }




    $(".name").on("blur", function () {
        const div = this.closest('div');
        const invalid = $(div).children(".invalid-feedback");
        const val = $(this).val();
        if (val) {
            $(invalid).hide();
        } else {
            $(invalid).show();
        }
    });

    $("#cell").on("blur", function () {
        const val = $(this).val();
        if (val.length >= 10) {
            $("#invalid-cell").hide();
        } else {
            $("#invalid-cell").show();
        }
    });

    $("#email").on("blur", function () {
        const val = $(this).val();
        if (isValidEmail(val)) {
            $("#invalid-email").hide();
        } else {
            $("#invalid-email").show();
        }
    });

    $("#password").on("blur", function () {
        const val = $(this).val();
        if (val.length >= 6) {
            $("#invalid-password").hide();
        } else {
            $("#invalid-password").show();
        }
    });

    $("#password2").on("blur", function () {
        const val = $(this).val();
        if (isSame($("#password").val(), val)) {
            $("#invalid-copy").hide();
        } else {
            $("#invalid-copy").show();
        }
    });
});

