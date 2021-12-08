function NumbersOnly(input) {
    var regex = /^[a-zA-Z]+[ a-zA-Z-_]*$/gi;
    input.value = input.value.replace(regex, "");
}

function lettersOnly(input) {
    var regex = /[^a-z]/gi;
    input.value = input.value.replace(regex, "");
}

function HandleChange(s1, s2) {
    var s1 = document.getElementById(s1);
    var s2 = document.getElementById(s2);

    s2.innerHTML = "";
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


    function HandleDeposit(s1, s2) {
        var s1 = document.getElementById(s1);
        var s2 = document.getElementById(s2);

        s2.innerHTML = "";
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