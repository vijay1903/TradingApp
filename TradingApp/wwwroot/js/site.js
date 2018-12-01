// Write your JavaScript code.

//Cookie function
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}


// id- canvas Id
// xlabels - array of labels for X-Axis
// ylabels - array of labels for Y-Axis
// ydatas - array of data for Y-Axis
// cdataavgs - array of average values of Y-Axis Data
var dynamicColors = function () {
    var r = Math.floor(Math.random() * 255);
    var g = Math.floor(Math.random() * 255);
    var b = Math.floor(Math.random() * 255);
    return "rgba(" + r + "," + g + "," + b +",";
};

function createChart(id, xlabels, ylabels, ydatas, cdataavgs, notAll) {
    console.log(id, xlabels, ylabels, ydatas, cdataavgs);
    var ctx = document.getElementById(id).getContext('2d');
    var ydatasets = [];
    var yannotations = [];
    var borderColors = [];
    var bgColors = [];
    //for (var i = 0; i < ylabels.length; i++) {
    //    var color = dynamicColors();
    //    borderColors.push(color + " 1)");
    //    bgColors.push(color + " 0.2)");
    //}
    var borderColors = ['rgba(255,99,132,1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
        'rgba(75, 192, 192, 1)',
        'rgba(153, 102, 255, 1)',
        'rgba(255, 159, 64, 1)',
        'rgba(255, 10, 64, 1)',
        'rgba(10, 159, 64, 1)',
        'rgba(255, 159, 10, 1)'];
    var bgColors = ['rgba(255, 99, 132, 0.2)',
        'rgba(54, 162, 235, 0.2)',
        'rgba(255, 206, 86, 0.2)',
        'rgba(75, 192, 192, 0.2)',
        'rgba(153, 102, 255, 0.2)',
        'rgba(255, 159, 64, 0.2)',
        'rgba(255, 10, 64, 0.2)',
        'rgba(10, 159, 64, 0.2)',
        'rgba(255, 159, 10, 0.2)'];
    for (var i = 0; i < ylabels.length; i++) {
        ydatasets.push({
            label: ylabels[i],
            fill: false,
            data: ydatas[i].split(","),
            hidden: (!((ylabels[i] == 'Close') || (ylabels[i] == 'Volume')) && notAll),
            type: (ylabels[i] != 'Volume') ? 'line' : 'bar',
            borderColor: borderColors[i],
            backgroundColor: bgColors[i],
            lineTension: 0
        });
        yannotations.push({
            type: 'line',
            mode: 'horizontal',
            scaleID: 'H',
            value: cdataavgs[i],
            borderColor: 'green',
            borderWidth: 1,
            label: {
                backgroundColor: "green",
                content: "Mean: $" + cdataavgs[i],
                enabled: true
            }
        });
    }
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: xlabels[0].split(","),
            datasets: ydatasets
        },
        options: {
            responsive: false,
            scales: {
                yAxes: [{
                    type: 'linear',
                    position: 'left',
                }]
            },
            legend: {
                position: 'right'
            },
            annotation: {
                drawTime: 'afterDatasetsDraw',
                annotations: yannotations
            }
        }
    });
}


