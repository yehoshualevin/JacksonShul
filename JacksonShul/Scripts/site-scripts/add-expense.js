$(() => {
    const d = new Date();
    const month = d.getMonth() + 1;
    const day = d.getDate();
    const date = (month < 10 ? '0' : '') + month + '/' +
        day + '/' +
        (day < 10 ? '0' : '') + d.getFullYear();
    $("#date").val(date);


    $(".form-control").on('input', function () {
        $(".btn").prop('disabled', function () {
            const name = $("#name").val();
            return !name;
        });
    });

    $("#unavailable").on("change", function () {
        if (this.checked) {
            $("#cost").hide();
        } else {
            $("#cost").show();
        }
    });
});