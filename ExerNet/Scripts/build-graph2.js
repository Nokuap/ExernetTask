$(document).ready(function () {
    var expr = Parser.parse($("#expression").val());
    var cosPoints = [];
    var increment = parseFloat($("#step").val());
    if (increment == 0) increment = 0.005;
    for (var i = parseFloat($("#from").val()) ; i < $("#to").val() ; i += increment) {
        cosPoints.push([i, expr.evaluate({ x: i })]);
    }
    var plot1 = $.jqplot('chart1', [cosPoints], {
        series: [{ showMarker: false }]
    }).replot();
});
