function NumbersOnly(input) {
    var regex = /^[a-zA-Z]+[ a-zA-Z-_]*$/gi;
    input.value = input.value.replace(regex, "");
}

function lettersOnly(input) {
    var regex = /[^a-z]/gi;
    input.value = input.value.replace(regex, "");
}