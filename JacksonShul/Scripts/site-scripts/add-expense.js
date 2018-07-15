$(() => {
    const d = new Date();
    const month = d.getMonth() + 1;
    const day = d.getDate();
    const date = (month < 10 ? '0' : '') + month + '/' +
        day + '/' +
        (day < 10 ? '0' : '') + d.getFullYear();    
    $("#date").val(date);
    console.log($("#date").val());

    $(".form-control").on('input', function () {
        $(".btn").prop('disabled', function () {
            const name = $("#name").val();
            const cost = $("#cost").val();
            return !name || !cost;
        });
    })
})