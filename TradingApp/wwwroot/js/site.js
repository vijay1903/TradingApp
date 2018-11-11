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


function createChart(id, xlabels, ylabels, ydatas, cdataavgs) {
    console.log(id, xlabels, ylabels, ydatas, cdataavgs);
    var ctx = document.getElementById(id).getContext('2d');
    var ydatasets = [];
    var yannotations = [];
    var borderColors = ['rgba(0, 255, 0, 1)', 'rgba(255, 0, 0, 1)'];
    var bgColor = ['rgba(0, 255, 0, 0.1)', 'rgba(255, 0, 0, 0.1)']
    for (var i = 0; i < ylabels.length; i++) {
        //for (var j = 0; j < xlabels.length; j++) {
            ydatasets.push({
                label: ylabels[i],
                data: ydatas[i].split(","),
                type: (ylabels[i] != 'Volumes') ? 'line' : 'bar',
                borderColor: borderColors[i],
                backgroundColor: bgColor[i],
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
        //}
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
            annotation: {
                drawTime: 'afterDatasetsDraw',
                annotations: yannotations
            }
        }
    });
}


