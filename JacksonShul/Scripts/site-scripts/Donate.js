
$(function (event) {
    var style = {
        border: '1px solid black',
        'font-size': '18px',
        padding: '1px',
        width: '257px'
    };
    setIfieldStyle('card-number', style);
    setIfieldStyle('cvv', style);
    setAccount("ifields_YehoshuaLevinDev_Test2_45fba2f272044e", "ylevin", "1.4.2");


    document.getElementById('submit-btn').addEventListener('click', function (e) {
        e.preventDefault();
        document.getElementById('transaction-status').innerHTML = 'Processing Transaction...';
        var submitBtn = this;
        $("#name").val($("#nameR").val());
        $("#credit").val(true);  
        submitBtn.disabled = true;
        getTokens(function () {
            document.getElementById('transaction-status').innerHTML = '';
            document.getElementById('card-token').innerHTML = document.querySelector("[data-ifields-id='card-number-token']").value;
            document.getElementById('cvv-token').innerHTML = document.querySelector("[data-ifields-id='cvv-token']").value;           
            submitBtn.disabled = false;
            document.getElementById('payment-form').submit();
        },
            function () {
                document.getElementById('transaction-status').innerHTML = '';
                document.getElementById('card-token').innerHTML = '';
                document.getElementById('cvv-token').innerHTML = '';
                submitBtn.disabled = false;
            },
            30000
        );
    }); 

    $('#payment-form').submit(function () {
        const card = $('#payment-method').is(":checked");
        if (!card) {
                $("#name").val($("#nameH").val());
                $("#credit").val(false);                
        } 
        return true;
    });
    $('#payment-method').on("click", function () {
        const card = $('#payment-method').is(":checked");
        if (!card) {
            $("#cc").hide();
            $("#cc").fadeOut();
            $("#cc").attr("style", "display:none");
            $("#check").show();
            $("#check").fadeIn();
            $("#check").removeAttr("style");
            $("#credit").val(false);
        } else {
            $("#check").hide();
            $("#check").fadeOut();
            $("#check").attr("style", "display:none");
            $("#cc").show();
            $("#cc").fadeIn();
            $("#cc").removeAttr("style");
            $("#credit").val(true);
        }
    })
    

    $("#count").on('keyup change', function (e) {
        if ($('#one-time:checked').length > 0) {            
            $(this).val("");
        } 
    });

    $("#one-time").on("click", function (e) {
        $("#count").val("");
    });

    $("#month").on('keyup', setExpiration);
    $("#year").on('keyup', setExpiration);

    function setExpiration() {
        console.log("exp");
        $("#exp").val($("#month").val() + $("#year").val());
        console.log("exp2");
        const hi = $("#exp").val();
        console.log(hi);
    }

    $(".fake").on("click", setRButtonValidity)
    $("#amount").on('keyup change', setRButtonValidity);
    $("#count").on('keyup change', setRButtonValidity);
    $("#nameR").on('keyup', setRButtonValidity);
    $("#street").on('keyup', setRButtonValidity);
    $("#zipcode").on('keyup', setRButtonValidity);
    $("#month").on('keyup change', setRButtonValidity);
    $("#year").on('keyup change', setRButtonValidity);

    function setRButtonValidity() {
        $("#submit-btn").prop('disabled', !isRFormValid());
    }

    function isRFormValid() {
        const amount = $("#amount").val();
        const name = $("#nameR").val();
        const street = $("#zipcode").val();
        const month = $("#month").val();
        const year = $("#year").val();
        const radio = ($('.fake:checked').length > 0);
        const count = $("#count").val();
        const repeat = ($('#repeat:checked').length > 0);
        const ok = count > 1 && repeat || (!count && !repeat);
        return amount > 0 && name && street && month.length == 2 && month < 13 && year.length == 2 && year > 17 && radio && ok;
    }

    $(".fake").on("click", setHButtonValidity);
    $("#amount").on('keyup change', setHButtonValidity);
    $("#count").on('keyup change', setHButtonValidity);
    $("#nameH").on('keyup', setHButtonValidity);
    $("#account").on('keyup', setHButtonValidity);
    $("#routing").on('keyup', setHButtonValidity);

    function setHButtonValidity() {
        $("#check-btn").prop('disabled', !isHFormValid());
    }

    function isHFormValid() {
        const amount = $("#amount").val();
        const name = $("#nameH").val();
        const account = $("#account").val();
        const routing = $("#routing").val();
        const radio = ($('.fake:checked').length > 0);
        const count = $("#count").val();
        const repeat = ($('#repeat:checked').length > 0);
        const ok = count > 1 && repeat || (!count && !repeat);
        return amount > 0 && name && account && routing && radio && ok;
    }

});