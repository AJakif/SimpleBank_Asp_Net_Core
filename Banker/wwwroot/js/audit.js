var dataTable; //variable

$(document).ready(function () {
    loadDataTable(); //function
}); //it works when document in ready


function loadDataTable() {

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Home/Audit/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "userId" },
            { "data": "name"},
            { "data": "amount" },
            { "data": "transId" },
            { "data": "date" },
            { "data": "transactionType" },
            { "data": "source" },
            { "data": "type" },
            { "data": "logType" },
        ],
        "language": {
            "emptyTable": "No record found."
        },
        "width": "100%"
    });
}

