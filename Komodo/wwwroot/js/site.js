// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.ticketsIndex').DataTable({
        stripeClasses: []
    });
    //InitDonut();
    //InitBarChart();
});

function ApexDonut(Url) {
    var pieData = [];
    $.post(Url).then(function (res) {
        for (let i = 0; i < res.length; i++) {
            pieData.push(parseInt(res[i]));
        }
    }).then(() => {
        var donutChart = {
            chart: {
                height: 'auto',
                type: 'pie',
                toolbar: {
                    show: false,
                }
            },
            //series: [10, 20, 30, 40, 50],
            series: pieData,
            colors: ['#e7515a', '#009688', '#e2a03f', '#5c1ac3', 'rgb(74, 109, 167)'],
            labels: ["Admins", "Project Managers", "Developers", "Submitters", "New Users"],

            //responsive: [{
            //    breakpoint: 480,
            //    options: {
            //        chart: {
            //            width: 250
            //        },
            //        legend: {
            //            position: 'bottom'
            //        }
            //    }
            //}]
        }
        var donut = new ApexCharts(
            document.querySelector("#donut-chart"),
            donutChart
        );
        donut.render();
    });
}

function ChartJsDonut(Url) {
    var pieData = [];
    $.post(Url).then(function (res) {
        for (let i = 0; i < res.length; i++) {
            pieData.push(parseInt(res[i]));
        }
    }).then(() => {
        // For a pie chart
        var ctx = document.getElementById('myChart').getContext('2d');
        var myPieChart = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: ['Admin', 'Project Manager', 'Developer', 'Submitter', 'New User'],
                datasets: [{
                    data: pieData,
                    backgroundColor: ['#e7515a', '#009688', '#e2a03f', '#5c1ac3', 'rgb(74, 109, 167)']
                }],
            }
        });
    });
}

function InitBarChart(Url) {
    $.post(Url).then(function (res) {
        let bar1 = [];
        let bar2 = [];
        let devs = [];
        for (let i in res) {
            bar1.push(res[i]["numAssigned"]);
            bar2.push(res[i]["numClosed"]);
            devs.push(res[i]["name"]);
        }
        var sCol = {
            chart: {
                height: 350,
                type: 'bar',
                toolbar: {
                    show: false,
                }
            },
            plotOptions: {
                bar: {
                    horizontal: false,
                    columnWidth: '55%',
                    endingShape: 'rounded'
                },
            },
            dataLabels: {
                enabled: false
            },
            stroke: {
                show: true,
                width: 2,
                colors: ['transparent']
            },
            series: [{
                name: 'Assigned',
                //data: [44, 55, 57, 56, 61, 58, 63, 60, 66]
                data: bar1
            }, {
                name: 'Closed',
                //data: [76, 85, 101, 98, 87, 105, 91, 114, 94]
                data: bar2
            }],
            xaxis: {
                //categories: ['Dev1', 'Dev2', 'Dev3', 'Dev4', 'Dev5', 'Dev6', 'Dev7', 'Dev8', 'Dev9'],
                categories: devs,
            },
            yaxis: {
                title: {
                    text: '(#tickets)'
                }
            },
            fill: {
                opacity: 1,
                //colors: ['#009688', '#5c1ac3']
                //colors:['#F00', '#00F']
            },
            tooltip: {
                y: {
                    formatter: function (val) {
                        return val + " tickets"
                    }
                }
            }
        }

        var chart = new ApexCharts(
            document.querySelector("#bar-chart"),
            sCol
        );

        chart.render();
    });
}