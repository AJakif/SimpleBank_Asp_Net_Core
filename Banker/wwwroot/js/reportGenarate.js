$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: '/Home/Report/Monthly/Statement',
        datatype: 'JSON',
        success: function (response) {
            var depositData = response.data.depositList;
            var withdrawData = response.data.withdrawList;
            console.log("deposit =", depositData);
            console.log("withdraw =", withdrawData);
            document.getElementById('name').value = response.user.name;
            document.getElementById('balance').value = response.user.amount;
            loadTableData( depositData, withdrawData);
        }
    });
});


function loadTableData(depositData, withdrawData) {
    const tableBody = document.getElementById('tableData');
    let dataHtml = '';
    var result = 0;
    for (var prop in depositData) {
        if (depositData.hasOwnProperty(prop)) {
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
        for (let [i,deposit] of depositData.entries()) {

            dataHtml += `<tr>
                            <td >${deposit.month}</td>
                            <td class="text-right">${deposit.total}</td>
                            <td class="text-right">${withdrawData[i].total}</td>
                         </tr>`;
            tableBody.innerHTML = dataHtml;
        }
    }

}


function pdfPrint() {
    console.log("yoo")
    $(".ignoreForPdf").hide();
    window.print();
    $(".ignoreForPdf").show();
    
}