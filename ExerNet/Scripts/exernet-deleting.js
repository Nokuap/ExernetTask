function deletingComment(CommentId) {
        $.ajax({
            url: "http://localhost:54084/Task/DeleteComment",
            dataType: "json",
            type: "POST",
            data: { id: CommentId },
            success: function (data) {
                $("#comment_" + data).remove();
            },
            error: function(){
                alert("cannot delete this comment");
            }
        });
    };
function deleteTask(id) {
    $.ajax({
        url: "http://localhost:54084/Task/DeleteTask",
        dataType: "json",
        type: "POST",
        data: { id: id },
        success: function (data) {
            $("#element_" + data).remove();
        },
        error: function(){
            alert("cannot delete this task");
        }
    });
};
function deleteFormula(formulaId)
    {
        $.ajax({
            url: "http://localhost:54084/Task/DeleteFormula",
            dataType: "json",
            type: "POST",
            data: { FormulaId: formulaId },
            success: function (data) {
                $("#oldFormulas_" + data).remove();
            }
        });
    };
function deleteUser(username) {
    $.ajax({
        url: "http://localhost:54084/User/Delete",
        dataType: "json",
        type: "POST",
        data: { id: username },
        success: function (data) {
            $("#user_" + data).remove();
        },
        error: function (e) {
            alert("cannot delete this user");
        }
    });
};
function deleteImage(imageId) {
	$.ajax({
		url: "http://localhost:54084/Task/DeleteImage",
		dataType: "json",
		type: "POST",
		data: { ImageId: imageId },
		success: function (data) {
			$("#image_" + data).remove();
		},
		error: function (e) {
			alert("cannot delete this image");
		}
	});
}