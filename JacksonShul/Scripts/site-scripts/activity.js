$(() => {
    $(".pledge").on("click", function () {
        $("#amount").val("");
        $("#partial").show();
        $("#complete").prop('checked', false);
        const pledgeId = $(this).data('pledge-id');
        const tr = $(this).parent().parent();
        const expenseName = tr.find('td:eq(1)').text();
        const amount = tr.find('td:eq(2)').text();
        var max = parseInt(amount.replace(/\D/g, ''), 10);
        $("#pledge-amount").text(amount);
        $("#expense-name").text(expenseName);
        $("#update").data("modal-id", pledgeId);
        $("#partial").data("max-num", max);
        $("#update").data("type", "update");
        $("#pledge-modal").modal();
    });

    $("#complete").on("change", function () {
        if (this.checked) {
            $("#partial").hide();
            $("#update").data("type", "delete");
        } else {
            $("#partial").show();
            $("#update").data("type", "update");
        }
    });

    $("#update").on("click", function () {
        const id = $(this).data('modal-id');
        if ($("#update").data('type') === "delete") {
            $.post("/home/deletepledge", { id }, function (id) {
                const tr = $(`[data-pledge-id=${id}]`).parent().parent();
                tr.remove();
                const thisPledge = $("#pledge-amount").text();
                const pledgeSum = $("#pledge-sum").text();
                const thisPledgeNum = parseInt(thisPledge.replace(/\D/g, ''), 10);
                const pledgeSumNum = parseInt(pledgeSum.replace(/\D/g, ''), 10);
                const newSum = pledgeSumNum - thisPledgeNum;
                const realSum = newSum / 100;
                const betterSum = '$' + realSum.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                $("#pledge-sum").text(betterSum);
            });
        } else {
            const amountPaid = $("#amount").val();
            $.post("/home/updatepledge", { id, amount: amountPaid }, function (result) {
                const tr = $(`[data-pledge-id=${id}]`).parent().parent();
                const num = '$' + result.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                tr.find('td:eq(2)').text(num);
                const pledgeSum = $("#pledge-sum").text();
                const sum = parseInt(pledgeSum.replace(/\D/g, ''), 10);
                const realSum = sum / 100;
                const newSum = realSum - amountPaid;
                const betterSum = '$' + newSum.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                $("#pledge-sum").text(betterSum);
            });
        }

        $("#pledge-modal").modal('hide');
    });
    $("#complete").on('change', setButtonValidity);
    $("#amount").on('input', setButtonValidity);


    function setButtonValidity() {
        $("#update").prop('disabled', !isFormValid());
    }


    function isFormValid() {
        const complete = $('#complete').is(":checked");
        const amount = $("#amount").val();
        const max = $("#partial").data('max-num');

        return complete || amount > 0 && amount < max / 100;

    }

});


