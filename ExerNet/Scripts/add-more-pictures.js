var count=0;
function addInput() {
	// read table
	var table = document.getElementById("inputs");		
	
	// create new input
	var input = document.createElement("input");
	input.type = "file";
	input.name = "Images";
	input.id = count;
	input.className = "form-control";
	var button = document.createElement("input");
	button.type = "button";
	button.value = "Remove";
	button.className = "btn btn-info";
	button.name = count;

	button.onclick = function del() {
		var table = document.getElementById("inputs");
		var index=this.name;	
		var i = -1;
		while(table.rows[++i].cells[1].innerHTML!=index);
		table.deleteRow(i);
	}
	var container = document.createElement("div");
	container.className = "form-group input-group";
	var picImgSpan = document.createElement("span");
	picImgSpan.className = "input-group-addon";
	var picImg = document.createElement("div");
	picImg.className = "glyphicon glyphicon-picture";
	picImgSpan.appendChild(picImg);
	var butSpan = document.createElement("span");
	butSpan.className = "input-group-btn";
	butSpan.appendChild(button);

	container.appendChild(picImgSpan);
	container.appendChild(input);
	container.appendChild(butSpan);
	//insert input in table
	var tr = table.insertRow(table.rows.length);
	var cell = tr.insertCell(0);
	cell.appendChild(container);
	//cell.appendChild(picImgSpan);
	//cell.appendChild(input);
	//cell.appendChild(butSpan);
	cell = tr.insertCell(1);
	cell.hidden=true;
	cell.innerHTML = count;
	count++;
}