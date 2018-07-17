$(() => {
    $("#amount").on("input", function () {
        $(".btn").prop("disabled", function () {
            const amount = $("#amount").val();
            return amount === null || amount < 1;
        });
    });

});