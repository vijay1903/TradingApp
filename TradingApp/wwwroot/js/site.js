// Write your JavaScript code.


// id- canvas Id
// xlabels - array of labels for X-Axis
// ylabels - array of labels for Y-Axis
// ydatas - array of data for Y-Axis
// cdataavgs - array of average values of Y-Axis Data
function createChart(id, xlabels, ylabels, ydatas, cdataavgs) {
    var ctx = document.getElementById(id).getContext('2d');
    var ydatasets = [];
    var yannotations = [];
    for (var i = 0; i < ylabels.length; i++) {
        ydatasets.push({
            label: ylabels[i],
            data: ydatas[i].split(","),
            type: (ylabels[i]!= 'Volumes')?'line':'bar',
            borderColor: 'rgba(0, 103, 71, 1)',
            backgroundColor: 'rgba(0, 103, 71, 0.1)',
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
            labels: xlabels.split(","),
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
