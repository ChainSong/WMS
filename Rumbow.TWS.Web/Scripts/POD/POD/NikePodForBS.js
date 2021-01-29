$(document).ready(function () {
    $('#ShipperName').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Pod/Pod/GetUserShipper",
                type: "POST",
                dataType: "json",
                data: { name: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Text, data: item }
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#ShipperID').val(ui.item.data.Value);
            $('#ShipperName').val(ui.item.data.Text);
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#ShipperID').val('');
        }
    });

})