$(document).ready(function () {
    $('table .edit').on('click', function () {
        var id = $(this).data("id");

        $.ajax({
            type: 'POST',
            url: '/Home/Transaction/Edit',
            data: { "id": id },
            success: function (response) {
                $('#EditRecord #id').val(response.oId);
                $('#EditRecord #transId').val(response.transId);
                $('#EditRecord #date').val(response.date);
                $('#EditRecord #amount').val(response.amount);
                $('#EditRecord #name').val(response.name);
                $('#EditRecord #source').val(response.source);
                $('#EditRecord #type').val(response.type);
                $('#EditRecord #transactionType').val(response.transactionType);

                console.log(response.source)
                var type = response.transactionType;
                var s1 = document.getElementById("source");
                var s2 = document.getElementById("type");
                s1.innerHTML = "";
                s2.innerHTML = "";
                if (type == "Withdraw") {

                    var optionArray = ['Medical', 'Grocery', 'Shopping', 'Travel', 'Education', 'Other'];

                    for (var option in optionArray) {
                        var newOption = document.createElement("option");
                        if (optionArray[option] == response.source) {
                            newOption.selected = response.source;
                        }
                        newOption.value = optionArray[option];
                        newOption.innerHTML = optionArray[option];
                        s1.options.add(newOption);
                    }
                    if (s1.value == "Shopping") {
                        var options = ['Cloth', 'Jwelery'];
                    }
                    else if (s1.value == "Medical") {
                        var options = ['Checkup', 'Medicine', 'Operation'];
                    }
                    else if (s1.value == "Education") {
                        var options = ['Tution fee', 'Books'];
                    }
                    else if (s1.value == "Travel") {
                        var options = ['Flight', 'Hotel', 'Visa'];
                    }
                    else if (s1.value == "Other") {
                        var options = ['Passport fee', 'Hotel', 'Visa'];
                    }
                    for (var option in options) {
                        var newOption = document.createElement("option");
                        if (optionArray[option] == response.type) {
                            newOption.selected = response.type;
                        }
                        newOption.value = options[option];
                        newOption.innerHTML = options[option];
                        s2.options.add(newOption);
                    }

                }
                else if (type == "Deposit") {

                    var optionArray = ['Income', 'Gift', 'Lottery', 'Business', 'Other'];


                    for (var option in optionArray) {
                        var newOption = document.createElement("option");
                        if (optionArray[option] == response.source) {
                            newOption.selected = response.source;
                        }
                        newOption.value = optionArray[option];
                        newOption.innerHTML = optionArray[option];
                        s1.options.add(newOption);
                    }
                    if (s1.value == "Gift") {
                        var options = ['Personal', 'Organizational'];
                    }
                    else if (s1.value == "Business") {
                        var options = ['Profit', 'Donation'];
                    }
                    else if (s1.value == "Income") {
                        var options = ['Monthly', 'Yearly'];
                    }
                    for (var option in options) {
                        var newOption = document.createElement("option");
                        if (optionArray[option] == response.type) {
                            newOption.selected = response.type;
                        }
                        newOption.value = options[option];
                        newOption.innerHTML = options[option];
                        s2.options.add(newOption);
                    }
                }
            }
        })
    });
});

    function Delete(url) {
        swal({
            title: "Are you sure you want to delete?",
            text: "You will not be able to restore !",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, Delete it",
            closeOnconfirm: true
        }, function () {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr["success"](data.message);
                        window.location.reload();
                    }
                    else {
                        toastr["error"](data.message);
                    }
                }
            });
        });
}


function HandleChange(s1, s2, s3) {
    var type = document.getElementById(s3)
    var s1 = document.getElementById(s1);
    var s2 = document.getElementById(s2);

    s2.innerHTML = "";
    if (type.value == "Deposit") {
        if (s1.value == "Gift") {
            var optionArray = ['Personal', 'Organizational'];
        }
        else if (s1.value == "Business") {
            var optionArray = ['Profit', 'Donation'];
        }
        else if (s1.value == "Income") {
            var optionArray = ['Monthly', 'Yearly'];
        }
        for (var option in optionArray) {
            var newOption = document.createElement("option");

            newOption.value = optionArray[option];
            newOption.innerHTML = optionArray[option];
            s2.options.add(newOption);
        }
    }
    else if (type.value == "Withdraw") {
        if (s1.value == "Shopping") {
            var optionArray = ['Cloth', 'Jwelery'];
        }
        else if (s1.value == "Medical") {
            var optionArray = ['Checkup', 'Medicine', 'Operation'];
        }
        else if (s1.value == "Education") {
            var optionArray = ['Tution fee', 'Books'];
        }
        else if (s1.value == "Travel") {
            var optionArray = ['Flight', 'Hotel', 'Visa'];
        }
        else if (s1.value == "Other") {
            var optionArray = ['Passport fee', 'Hotel', 'Visa'];
        }



        for (var option in optionArray) {
            var newOption = document.createElement("option");

            newOption.value = optionArray[option];
            newOption.innerHTML = optionArray[option];
            s2.options.add(newOption);
        }
    }
}
   
