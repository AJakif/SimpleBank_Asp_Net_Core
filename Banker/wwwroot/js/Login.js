var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "Name", "width": "25%" },
            { "data": "DateTime","width": "25%" }


        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to restore the Content!",
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
                    dataTable.ajax.reload();
                }
                else {
                    toastr["error"](data.message);
                }
            }
        });
    });
}