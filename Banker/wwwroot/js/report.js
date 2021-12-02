$(document).ready(function () {
    /* document ready*/

    /* deposit pie*/
    $.ajax({
        /* ajax start*/
        url: '/Home/Report/monthlyDeposit',
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
                A.push( {
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
                        text: 'All <b>deposits</b> of current month'
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
            
        }
        /* ajax end*/
    })

    /* withdraw pie*/
    $.ajax({
        /* ajax start*/
        url: '/Home/Report/monthlyWithdraw',
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
                    text: 'All <b>withdraw</b> of current month'
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

        }
        /* ajax end*/
    })

    /* withdraw bar*/
    $.ajax({
        /* ajax start*/
        url: '/Home/Report/yearlyWithdraw',
        type: 'GET',
        dataType: 'JSON',
        success: function (yearList) {
            var A = []
            
            for (var i = 0; i < yearList.length; i++) {
                var total2 = [];
                let year;
                year = yearList[i].year;
                for (var j = 0; j < yearList[i].list.length; j ++) {
                    
                    total2.push(yearList[i].list[j].total)
                }
                A.push({
                    name:year,
                    data: total2
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
        Error: function (json) {

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
                var total2 = [];
                let year;
                year = yearList[i].year;
                for (var j = 0; j < yearList[i].list.length; j++) {

                    total2.push(yearList[i].list[j].total)
                }
                A.push({
                    name: year,
                    data: total2
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
        Error: function (json) {

        }
        /* ajax end*/
    })

    
    /* document end*/
    });


