$(() => {
    $("#story").on("input", function () {
        $(".btn").prop("disabled", function () {
            const story = $("#story").val();
            return !story;
        });
    });

});