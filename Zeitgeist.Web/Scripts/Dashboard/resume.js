var caminar = [];
var send = function (url, type, data, callback) {
    if (data === undefined || data === null) {
        return $.ajax({
            url: url,
            type: type,
            dataType: 'json',
            success: callback
        });
    }
    else {
        return $.ajax({
            url: url,
            type: type,
            //data: { dataSave: data },
            data: data,
            dataType: 'json',
            success: callback
        });
    }
};
$(function () {
    send('/Dashboard/GetResume', 'POST', null, datos);
});

function datos(data) {
    for (var i = 0, size = data.length; i < size; i++) {
        caminar.push([new Date(data[i].Item1), data[i].Item2]);
    }

    chart("Pasos", caminar);
};
function chart(name, dato) {
    $("#sales-charts").css({ 'width': '90%', 'min-height': '350px' });
    var my_chart = $.plot("#sales-charts",
    [{ label: name, data: dato }],
    {
        series: {
            lines: { show: true },
            points: { show: true }
        },
        xaxis: {
            mode: "time",
            timeformat: "%Y/%m/%d",
            tickSize: [1, "day"]
            //,ticks:4
            //,ticks: dato[0]
        },

        //yaxis: {
        //    ticks: 10,
        //    min: 0,
        //    max: 2,
        //    tickDecimals: 3
        //},
        grid: {

            hoverable: true,
            backgroundColor: { colors: ["#fff", "#fff"] },
            borderWidth: 1,
            borderColor: '#555',
            shadowSize: 1
        },
        tooltip: true,
        tooltipOpts: {

            content: '<b>%x</b><br/>N° Pasos: %y',
            shifts: {
                x: -60,
                y: 25
            }
        }

    });
};