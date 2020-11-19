// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.ticketsIndex').DataTable({
        stripeClasses: []
    });      
    InitDonut();
    InitBarChart();    
});    

function InitDonut() {
    var donutChart = {
        chart: {
            height: 350,
            type: 'pie',
            toolbar: {
                show: false,
            }
        },

        series: [10, 20, 30, 40],
        labels: ["Admin", "Project Manager", "Developer", "Submitter"],

        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 250
                },
                legend: {
                    position: 'bottom'
                }
            }
        }]
    }

    var donut = new ApexCharts(
        document.querySelector("#donut-chart"),
        donutChart
    );

    donut.render();
}

function InitBarChart() {
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
            data: [44, 55, 57, 56, 61, 58, 63, 60, 66, 44, 55, 57, 56, 61, 58, 63, 60, 66]
        }, {
            name: 'Closed',
                data: [76, 85, 101, 98, 87, 105, 91, 114, 94, 44, 55, 57, 56, 61, 58, 63, 60, 66]
        }],
        xaxis: {
            categories: ['Dev1', 'Dev2', 'Dev3', 'Dev4', 'Dev5', 'Dev6', 'Dev7', 'Dev8', 'Dev9', 'Dev1', 'Dev2', 'Dev3', 'Dev4', 'Dev5', 'Dev6', 'Dev7', 'Dev8', 'Dev9'],
        },
        yaxis: {
            title: {
                text: '(#tickets)'
            }
        },
        fill: {
            opacity: 1

        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return "$ " + val + " thousands"
                }
            }
        }
    }

    var chart = new ApexCharts(
        document.querySelector("#bar-chart"),
        sCol
    );

    chart.render();
}