var count = 0;
function addFormulaUrl() {
    // read table
    var container = document.getElementById("formulas_container");

    // create new input
    var column = document.createElement("div");
    column.name = "Formulas.FormulaUrl";
    column.id = "formula_"+count;
    column.className = "col-md-6 row";
    var left = document.createElement("div");
    left.className = "col-md-10";
    var right = document.createElement("div");
    right.className = "col-md-2";

    var image = document.createElement("img");
    image.className = "img-thumbnail img-responsive";
    var button = document.createElement("input");
    button.type = "button";
    button.value = "Remove";
    button.className = "btn btn-info btn-xs pull-left";
    button.value = 'X';
    button.name = count;

    var hidden = document.createElement("input");
    hidden.type = "hidden";
    hidden.name = "FormulaUrls";

    button.onclick = function del() {
        var container = document.getElementById("formulas_container");
        var index = this.name;
        $("#formula_" + index).remove();
    }
    var source = document.getElementById("equation").src;
    if (source != '' && source !=document.URL) {
        image.src = source;
        hidden.value = source;
        left.appendChild(image);
        right.appendChild(button);
        column.appendChild(left);
        column.appendChild(right);
        column.appendChild(hidden);
        container.appendChild(column);
    }
    $("#equation").attr('src','');
    $("#testbox").val('');
    count++;
}