
$("#buildGraph").click(function (e) {
    e.preventDefault();
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

$(".btn-xs").click(
    function () {
        $("#graphic").toggle(function () {

            var cosPoints = [];
            cosPoints.push([0, 0]);
            var plot1 = $.jqplot('chart1', [cosPoints], {
                series: [{ showMarker: false }]
            }).replot();
        });

    });

