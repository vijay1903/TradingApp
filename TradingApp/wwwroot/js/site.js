// Write your JavaScript code.
function createChart(id, clabels, cdatas, cdataavgs) {
    var ctx = document.getElementById(id).getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: clabels.split(","),
            datasets: [{
                label: 'High Prices',
                yAxisID: 'H',
                data: cdatas[0].split(","),
                type: 'line',
                borderColor: 'rgba(0, 103, 71, 1)',
                backgroundColor: 'rgba(0, 103, 71, 0.1)',
                lineTension: 0
            },
            {
                label: 'Volumes (Mn)',
                data: cdatas[1].split(","),
                borderColor: 'rgba(0, 0, 250, 1)',
                borderWidth: 1
            }]
        },
        options: {
            responsive: false,
            scales: {
                yAxes: [{
                    id: 'H',
                    type: 'linear',
                    position: 'left',
                }]
            },
            annotation: {
                drawTime: 'afterDatasetsDraw',
                annotations: [
                    {
                        id: 'highprice',
                        type: 'line',
                        mode: 'horizontal',
                        scaleID: 'H',
                        value: avgprice,
                        borderColor: 'green',
                        borderWidth: 1,
                        label: {
                            backgroundColor: "green",
                            content: "Mean: $" + cdataavgs[0],
                            enabled: true
                        }
                    },
                    {
                        id: 'volume',
                        type: 'line',
                        mode: 'horizontal',
                        scaleID: 'H',
                        value: avgvol,
                        borderColor: 'blue',
                        borderWidth: 1,
                        label: {
                            backgroundColor: "blue",
                            content: "Mean Volume: " + cdataavgs[1] + "(Mn)",
                            enabled: true
                        }
                    }]
            }
        }
    });
}
