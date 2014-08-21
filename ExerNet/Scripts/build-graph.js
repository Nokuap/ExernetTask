$("#buildGraph").click(function (e) {
    //$('canvas').remove();
    e.preventDefault();
    var expr = Parser.parse($("#expression").val());
    var cosPoints = [];

    for (var i = parseFloat($("#from").val()) ; i < $("#to").val() ; i += parseFloat($("#step").val())) {
        cosPoints.push([i, expr.evaluate({ x: i })]);
    }
    var plot1 = $.jqplot('chart1', [cosPoints], {
        series: [{ showMarker: false }]
    }).replot();
});
