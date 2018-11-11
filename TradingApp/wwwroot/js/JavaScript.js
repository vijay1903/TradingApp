function selectRange() {
    var options = document.getElementById("chart_range").children;
    var selected = getCookie("range");
    if (getCookie("range") == undefined || getCookie("range") == "") {
        selected = "1d";
    }
    for (var i = 0; i < options.length; i++) {
        if (options[i].value == selected) {
            options[i].selected = true;
        } else {
            options[i].selected = false;
        }
    }       
}
selectRange();
function changeChart(symbol) {
    var selected = $("#chart_range").val();
    setCookie("range", selected, 1);
    setCookie("symbol", symbol, 1);
    location.href = '/Home/Index?symbol=' + getCookie("symbol") + '&range=' + selected;
}