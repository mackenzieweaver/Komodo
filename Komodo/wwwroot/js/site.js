// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.ticketsIndex').DataTable();      
    InitDonut();
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
                    width: 200
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