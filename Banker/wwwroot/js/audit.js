$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: '/Home/Audit/GetAll',
        datatype: 'JSON',
        success: function (response) {

            let auditData = response.data;
            loadTableData(auditData);
        }
    });
});

function loadTableData(auditData) {
    const tableBody = document.getElementById('tableData');
    let dataHtml = '';
    var result = 0;
    for (var prop in auditData) {
        if (auditData.hasOwnProperty(prop)) {
            result++;
        }
    }
    console.log("length =", result);
    if (result == 0) {
        dataHtml += `<tr>
                            <td colspan="9" style="text-align:center">No data</td>
                        </tr>`;
        tableBody.innerHTML = dataHtml;
        $('#dtBasicExample').DataTable();
    }
    else {
        for (let audit of auditData) {

            dataHtml += `<tr>
                           <td class="text-center">${audit.userId}</td>
                           <td class="text-center">${audit.name}</td>
                           <td class="text-center">${audit.amount}</td>
                           <td class="text-center">${audit.transId}</td>
                           <td class="text-center">${audit.date}</td>
                           <td class="text-center">${audit.transactionType}</td>
                           <td class="text-center">${audit.source}</td>
                           <td class="text-center">${audit.type}</td>
                           <td class="text-center">${audit.logType}</td>
                         </tr>`;


            tableBody.innerHTML = dataHtml;
        }
    }

    $('#dtBasicExample').DataTable();
    $('.dataTables_length').addClass('bs-select');
    
}

function HandleChangeTType(type) {

    $.ajax({
        type: 'GET',
        url: '/Home/Audit/TType/' + type,
        datatype: 'JSON',
        success: function (response) {

            let auditData = response.data;
            loadTableData(auditData);
        }
    });

}

function HandleChangeType(type) {

    $.ajax({
        type: 'GET',
        url: '/Home/Audit/LogType/' + type,
        datatype: 'JSON',
        success: function (response) {

            let auditData = response.data;
            loadTableData(auditData);
        }
    });

}

function HandleDate(e) {

    var date = e.target.value;
    $.ajax({
        type: 'GET',
        url: '/Home/Audit/Date/' + date,
        datatype: 'JSON',
        success: function (response) {

            let auditData = response.data;
            loadTableData(auditData);
        }
    });
}

function reset() {
    window.location.reload();
}