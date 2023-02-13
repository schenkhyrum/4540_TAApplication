/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 2-Dec-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Javascript file used on Enrollment Trends page. Creates the high charts to display the enrollment trend data,
    gets the enrollment data based on user input, and allows user to change lighting mode and chart type.
*/
enrollmentData = []

// get the enrollment data
function getEnrollmentData() {
    $('#getDataButton').hide();
    $('#spinner').show();

    // get user input
    var startDate = $('#startDate').datepicker('getDate');
    var endDate = $('#endDate').datepicker('getDate');
    var courseInfo = $('#courseSelect option:selected').text().split(' ');

    $.get({
        url: "GetEnrollmentData",
        data: { startDate: startDate.toISOString(), endDate: endDate.toISOString(), department: courseInfo[0], courseNumber: parseInt(courseInfo[1]) }
    }).done(function (response) {
        updateChart(response, courseInfo);

    }).always(function (response) {
        $('#spinner').hide();
        $('#getDataButton').show();
    });
}

function updateChart(response, courseInfo) {
    enrollmentData = [];

    // parse the incoming data
    for (let i = 0; i < response.length; i++) {
        var dateInfo = response[i].dateTaken.substring(0, 10).split('-');
        enrollmentData.push([Date.UTC(parseInt(dateInfo[0]), parseInt(dateInfo[1]) - 1, parseInt(dateInfo[2])), response[i].enrollmentCount]);
    }

    // update the series if data has already been retrieved for it
    var updated = false;
    for (let i = 0; i < $("#enrollmentTrends").highcharts().series.length; i++) {
        if ($("#enrollmentTrends").highcharts().series[i].name === `${courseInfo[0]} ${courseInfo[1]}`) {
            $("#enrollmentTrends").highcharts().series[i].setData(enrollmentData, true, true, false);

            updated = true;
            break;
        }
    }

    // add the series if this is the first time it is being retrieved
    if (!updated) {
        $("#enrollmentTrends").highcharts().addSeries(
            {
                name: `${courseInfo[0]} ${courseInfo[1]}`,
                data: enrollmentData,
                marker: {
                    enabled: true,
                    symbol: 'circle',
                    radius: 6
                }
            });
    }
}

// set up start and end date
$(function () {
    $("#startDate").datepicker();
    $("#startDate").datepicker("setDate", "11/15/2022");

    $("#endDate").datepicker();
    $("#endDate").datepicker("setDate", new Date());
});

// changes the type of chart to display
function changeChartType() {
    var chartType = $('#chartTypeSelect option:selected').val();
    var chart = $("#enrollmentTrends").highcharts();
    chart.update({
        chart: {
            type: chartType
        }
    });
}

function changeLightMode() {
    var chart = $("#enrollmentTrends").highcharts();

    // colors for light mode
    var backgroundColor = '#ffffff';
    var textColor = "#666666";
    var legendHidden = "#cccccc";
    var legendHover = "#000000";
    var legendItem = "#333333";
    var titleColor = '#333333';
    var yAxisLabel = "#dddddd";

    // colors for dark mode
    if ($("#toggleLight").is(':checked')) {
        backgroundColor = '#232323';
        titleColor = '#dddddd';
        textColor = '#dddddd';
        legendHidden = "#333333";
        legendHover = "#ffffff";
        legendItem = "#cccccc";

        // add classes so background is dark and text is white
        $("body").addClass("bg-dark");
        $(".changeTextColor").addClass("text-white");
    }

    // remove classes so background is white and text is dark
    else {
        $("body").removeClass("bg-dark");
        $(".changeTextColor").removeClass("text-white");
    }

    // update the relevant chart items with the correct color
    chart.update({
        chart: {
            backgroundColor: backgroundColor
        },
        title: {
            style: {
                color: titleColor
            }
        },
        legend: {
            itemHiddenStyle: {
                "color": legendHidden
            },
            itemHoverStyle: {
                "color": legendHover
            },
            itemStyle: {
                "color": legendItem
            }
        },
        xAxis: {
            labels: {
                style: {
                    "color": textColor
                }
            },
            title: {
                style: {
                    "color": textColor
                }
            }
        },
        yAxis: {
            title: {
                style: {
                    "color": textColor
                }
            },
            labels: {
                style: {
                    "color": yAxisLabel
                }
            },
        }
    });

    chart.redraw();
}

// create the chart
Highcharts.chart('enrollmentTrends', {
    chart: {
        type: 'areaspline'
    },
    tooltip: {
        // formats the tooltip to the desired format
        formatter: function () {
            var point = this;
            var d = new Date(point.x);
            d.setUTCDate(d.getUTCDate() + 1);
            return `<span style="font-size: 10px"> ${d.toDateString()}</span><br/>` +
                `<span style="color:${point.color}">●</span> <span style="font-size: 10px"> ${point.series.name}: ${point.y}</span><br/>`;
        }
    },
    title: {
        text: 'Enrollments Over Time'
    },
    xAxis: {
        type: 'datetime',
        labels: {
            formatter: function () {
                return Highcharts.dateFormat('%a %d %b', this.value);
            }
        },
        title: {
            enabled: true,
            text: 'Dates'
        }
    },
    yAxis: {
        title: {
            enabled: true,
            text: 'Students'
        }
    }
});