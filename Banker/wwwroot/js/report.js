$(document).ready(function () {
    const d = new Date();
    let month = d.getMonth();
    monthly(month);
});

function HandleChange() {
    var s2 = document.getElementById("option2");
    if (s2.checked == true) {
        yearly();
    }
    else {
        var month = new Date().getMonth();
        monthly(month);
    }
}

function MonthDepositChange(v) {
    const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    let V = parseInt(v);
    let a = 1;
    var month = V + a;
    /* deposit pie*/
    $.ajax({
        /* ajax start*/
        url: '/Home/Report/monthlyDeposit/' + month,
        type: 'GET',
        dataType: 'JSON',
        success: function (json) {

            var amount = []
            var remark = []


            let e = eval(json);
            var len = e.length;

            for (var i = 0; i < len; i++) {
                amount.push(e[i].Total);
                remark.push(e[i].Remark);
            }
            var A = []

            for (var i = 0; i < len; i++) {
                A.push({
                    name: remark[i],
                    y: amount[i]
                })
            }
            /*Highcharts using here.. PIE CHART*/
            Highcharts.chart('container', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: `All Deposit of <b>${monthNames[v]}</b>`
                },
                tooltip: {
                    pointFormat: 'Ratio: <b>{point.percentage:.1f}%</b> <br /> {series.name}: <b>{point.y:.1f}$</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                        }
                    }
                },
                series: [{
                    name: 'Total Amount',
                    colorByPoint: true,
                    data: A
                }]
            });

        },
        Error: function (json) {
            console.log(json)
        }
        /* ajax end*/
    })
}

function MonthWithdrawChange(v) {
    const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    /* withdraw pie*/
    let V = parseInt(v);
    let a = 1;
    var month = V + a;
    $.ajax({
        /* ajax start*/
        url: '/Home/Report/monthlyWithdraw/' + month,
        type: 'GET',
        dataType: 'JSON',
        success: function (json) {

            var amount = []
            var remark = []


            let e = eval(json);
            var len = e.length;

            for (var i = 0; i < len; i++) {
                amount.push(e[i].Total);
                remark.push(e[i].Remark);
            }
            var A = []

            for (var i = 0; i < len; i++) {
                A.push({
                    name: remark[i],
                    y: amount[i]
                })
            }
            /*Highcharts using here.. PIE CHART*/
            Highcharts.chart('container1', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: `All Withdraw of <b>${monthNames[v]}</b>`
                },
                tooltip: {
                    pointFormat: 'Ratio: <b>{point.percentage:.1f}%</b> <br /> {series.name}: <b>{point.y:.1f}$</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                        }
                    }
                },
                series: [{
                    name: 'Total Amount',
                    colorByPoint: true,
                    data: A
                }]
            });

        },
        Error: function (json) {
            console.log(json)
        }
        /* ajax end*/
    })
}

function yearly() {
    document.getElementById("report").innerHTML = `<div class="row">
            <div class="col-6 border">
                <figure class="highcharts-figure">
                    <div id="container2"></div>
                    <p class="highcharts-description">

                    </p>
                </figure>
            </div>
            <div class="col-6 border">
                <figure class="highcharts-figure">
                    <div id="container3"></div>
                    <p class="highcharts-description">

                    </p>
                </figure>
            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="col-6 border">
                <figure class="highcharts-figure">
                    <div id="linechart1"></div>
                    <p class="highcharts-description">

                    </p>
                </figure>
            </div>
            <div class="col-6 border">
                <figure class="highcharts-figure">
                    <div id="linechart2"></div>
                    <p class="highcharts-description">

                    </p>
                </figure>
            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="col-6 border">
                <figure class="highcharts-figure">
                    <div id="dril1"></div>
                    <p class="highcharts-description">

                    </p>
                </figure>
            </div>
            <div class="col-6 border">
                <figure class="highcharts-figure">
                    <div id="dril2"></div>
                    <p class="highcharts-description">

                    </p>
                </figure>
            </div>
        </div>`

    /* withdraw bar*/
    $.ajax({
        /* ajax start*/
        url: '/Home/Report/yearlyWithdraw',
        type: 'GET',
        dataType: 'JSON',
        success: function (yearList) {
            var A = []

            for (var i = 0; i < yearList.length; i++) {
                var total = [];
                let year;
                year = yearList[i].year;
                for (var j = 0; j < yearList[i].list.length; j++) {

                    total.push(yearList[i].list[j].total)
                }
                A.push({
                    name: year,
                    data: total
                })

            }
            /*Highcharts using here.. PIE CHART*/
            Highcharts.chart('container3', {
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Last 5 years total <b>Withdraw<b/>'
                },
                xAxis: {
                    categories: ['Education', 'Grocery', 'Medical', 'Other', 'Shopping', 'Travel'],
                    title: {
                        text: null
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Population (millions)',
                        align: 'high'
                    },
                    labels: {
                        overflow: 'justify'
                    }
                },
                tooltip: {
                    valueSuffix: '$'
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    x: +10,
                    y: 80,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
                    shadow: true
                },
                credits: {
                    enabled: false
                },
                series: A
            });

        },
        Error: function (yearList) {
            console.log(yearList)
        }
        /* ajax end*/
    })

    /* deposit bar*/
    $.ajax({
        /* ajax start*/
        url: '/Home/Report/yearlyDeposit',
        type: 'GET',
        dataType: 'JSON',
        success: function (yearList) {
            var A = []

            for (var i = 0; i < yearList.length; i++) {
                var total = [];
                let year;
                year = yearList[i].year;
                for (var j = 0; j < yearList[i].list.length; j++) {

                    total.push(yearList[i].list[j].total)
                }
                A.push({
                    name: year,
                    data: total
                })

            }
            /*Highcharts using here.. PIE CHART*/
            Highcharts.chart('container2', {
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Last 5 years total <b>Deposit<b/>'
                },
                xAxis: {
                    categories: ['Business', 'Gift', 'Income', 'Lottery', 'Other'],
                    title: {
                        text: null
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Population (millions)',
                        align: 'high'
                    },
                    labels: {
                        overflow: 'justify'
                    }
                },
                tooltip: {
                    valueSuffix: '$'
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    x: +10,
                    y: 80,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
                    shadow: true
                },
                credits: {
                    enabled: false
                },
                series: A
            });

        },
        Error: function (yearList) {
            console.log(yearList)
        }
        /* ajax end*/
    })


    /* deposit line*/
    $.ajax({
        /* ajax start*/
        url: '/Home/Report/yearlyCatDeposit',
        type: 'GET',
        dataType: 'JSON',
        success: function (remarkList) {
            var A = []

            for (var i = 0; i < remarkList.length; i++) {
                var total = [];
                let remark = remarkList[i].remark;
                for (var j = 0; j < remarkList[i].list.length; j++) {

                    total.push(remarkList[i].list[j].total)
                }
                A.push({
                    name: remark,
                    data: total
                })

            }
            var sYear = parseInt(remarkList[0].list[0].year);
            var fYear = parseInt(remarkList[0].list[4].year);

            /*Highcharts using here.. Line CHART*/
            Highcharts.chart('linechart1', {

                title: {
                    text: `Yearly deposit, ${sYear} - ${fYear}`
                },

                yAxis: {
                    title: {
                        text: 'Total deposit amount'
                    }
                },

                xAxis: {
                    accessibility: {
                        rangeDescription: 'Range: ${sYear} to ${fYear}'
                    }
                },

                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },

                plotOptions: {
                    series: {
                        label: {
                            connectorAllowed: false
                        },
                        pointStart: sYear
                    }
                },

                series: A,

                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom'
                            }
                        }
                    }]
                }

            });

        },
        Error: function (remarkList) {
            console.log(remarkList)
        }
        /* ajax end*/
    })

    /* Withdraw line*/
    $.ajax({
        /* ajax start*/
        url: '/Home/Report/yearlyCatWithdraw',
        type: 'GET',
        dataType: 'JSON',
        success: function (remarkList) {
            var A = []

            for (var i = 0; i < remarkList.length; i++) {
                var total = [];
                let remark = remarkList[i].remark;
                for (var j = 0; j < remarkList[i].list.length; j++) {

                    total.push(remarkList[i].list[j].total)
                }
                A.push({
                    name: remark,
                    data: total
                })

            }

            var sYear = parseInt(remarkList[0].list[0].year);
            var fYear = parseInt(remarkList[0].list[4].year);
            /*Highcharts using here.. Line CHART*/
            Highcharts.chart('linechart2', {

                title: {
                    text: `Yearly withdraw, ${sYear} - ${fYear}`
                },

                yAxis: {
                    title: {
                        text: 'Total withdraw amount'
                    }
                },

                xAxis: {
                    accessibility: {
                        rangeDescription: 'Range: ${sYear} to ${fYear}'
                    }
                },

                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },

                plotOptions: {
                    series: {
                        label: {
                            connectorAllowed: false
                        },
                        pointStart: sYear
                    }
                },

                series: A,

                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom'
                            }
                        }
                    }]
                }

            });

        },
        Error: function (remarkList) {
            console.log(remarkList)
        }
        /* ajax end*/
    })


    /* Yearly deposit drildown pie*/
    $.ajax({
        /* ajax start*/
        url: '/Home/Report/monthlyYearlyDeposit',
        type: 'GET',
        dataType: 'JSON',
        success: function (List) {
            var A = []
            List.secondList.map(function (a) {
                A.push({
                    name: a.year,
                    y: a.yTotal,
                    drilldown: a.year
                })
            })

            var B = []

            List.firstList.map(function (a) {
                let data = []
                a.list.map(function (b) {
                    data.push([b.month, b.mTotal])
                })
                B.push({
                    name: a.year,
                    id: a.year,
                    data: data
                })
            })

            /*Highcharts using here.. drill down PIE CHART*/
            Highcharts.chart('dril1', {
                chart: {
                    type: 'pie'
                },
                title: {
                    text: 'Total <b>deposit</b> of last 5 years'
                },

                accessibility: {
                    announceNewData: {
                        enabled: true
                    },
                    point: {
                        valueSuffix: '%'
                    }
                },

                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            format: '{point.name}: {point.percentage:.1f}%'
                        }
                    }
                },

                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.percentage:.2f}%</b> of total<br/><span style="color:{point.color}">{point.name} total</span>: <b>{point.y:.1f} $</b><br/>'
                },

                series: [
                    {
                        name: "Years",
                        colorByPoint: true,
                        data: A
                    }
                ],
                drilldown: {
                    series: B
                }
            });

        },
        Error: function (List) {
            console.log(List)
        }
        /* ajax end*/
    })

    /* Yearly deposit drildown pie*/
    $.ajax({
        /* ajax start*/
        url: '/Home/Report/monthlyYearlyWithdraw',
        type: 'GET',
        dataType: 'JSON',
        success: function (List) {
            var A = []
            List.secondList.map(function (a) {
                A.push({
                    name: a.year,
                    y: a.yTotal,
                    drilldown: a.year
                })
            })

            var B = []

            List.firstList.map(function (a) {
                let data = []
                a.list.map(function (b) {
                    data.push([b.month, b.mTotal])
                })
                B.push({
                    name: a.year,
                    id: a.year,
                    data: data
                })
            })

            /*Highcharts using here.. drill down PIE CHART*/
            Highcharts.chart('dril2', {
                chart: {
                    type: 'pie'
                },
                title: {
                    text: 'Total <b>withdraw</b> of last 5 years'
                },

                accessibility: {
                    announceNewData: {
                        enabled: true
                    },
                    point: {
                        valueSuffix: '%'
                    }
                },

                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            format: '{point.name}: {point.percentage:.1f}%'
                        }
                    }
                },

                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.percentage:.2f}%</b> of total<br/><span style="color:{point.color}">{point.name} total</span>: <b>{point.y:.1f} $</b><br/>'
                },

                series: [
                    {
                        name: "Years",
                        colorByPoint: true,
                        data: A
                    }
                ],
                drilldown: {
                    series: B
                }
            });

        },
        Error: function (json) {
            console.log(List)
        }
        /* ajax end*/
    })
}


function monthly(m) {

    document.getElementById("report").innerHTML = `<div class="row" >
                                                            <div class= "col-6 border" id="deposit" >
                                                                <figure class="highcharts-figure" >
                                                                    <div id="container"></div>
                                                                    <p class="highcharts-description">
                                                                        <select class="custom-select col-4" id="inputGroupSelect01" onchange="MonthDepositChange(this.value)">
                                                                            <option selected>Choose...</option>
                                                                            <option value="0">January</option>
                                                                            <option value="1">February</option>
                                                                            <option value="2">March</option>
                                                                            <option value="3">April</option>
                                                                            <option value="4">May</option>
                                                                            <option value="5">June</option>
                                                                            <option value="6">July</option>
                                                                            <option value="7">August</option>
                                                                            <option value="8">September</option>
                                                                            <option value="9">October</option>
                                                                            <option value="10">November</option>
                                                                            <option value="11">December</option>
                                                                        </select>
                                                                       
                                                                    </p>
                                                                </figure>
                                                            </div >
                                                            <div class="col-6 border">
                                                                <figure class="highcharts-figure" id="withdraw">
                                                                    <div id="container1"></div>
                                                                    <p class="highcharts-description">
                                                                       <select class="custom-select col-4" id="inputGroupSelect01" onchange="MonthWithdrawChange(this.value)">
                                                                            <option selected>Choose...</option>
                                                                            <option value="0">January</option>
                                                                            <option value="1">February</option>
                                                                            <option value="2">March</option>
                                                                            <option value="3">April</option>
                                                                            <option value="4">May</option>
                                                                            <option value="5">June</option>
                                                                            <option value="6">July</option>
                                                                            <option value="7">August</option>
                                                                            <option value="8">September</option>
                                                                            <option value="9">October</option>
                                                                            <option value="10">November</option>
                                                                            <option value="11">December</option>
                                                                        </select>
                                                                    </p>
                                                                </figure>
                                                            </div>
                                                        </div >`

    
    /* deposit pie*/
    MonthDepositChange(m)

    /* withdraw pie*/
    MonthWithdrawChange(m)
}