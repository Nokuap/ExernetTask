var BlockNumber = 2;  //Infinate Scroll starts from second block
var inProgress = false;
$(window).scroll(function ()
{
    if ($(window).scrollTop() == $(document).height() - $(window).height() && !inProgress) {
        inProgress = true;
       
        var element = document.getElementById("commentslist");
        if (element != null) {
            $("div#loadmoreajaxloader").show();
                $.post("http://localhost:54084/Task/InfiniteComments", { "BlockNumber": BlockNumber, "TaskId": document.getElementById("TaskId").innerHTML },
                function (data) {
                    BlockNumber = BlockNumber + 1;
                    $("#commentslist").append(data);
                    $("div#loadmoreajaxloader").hide();
                    inProgress = false;
                });
            }
        }
});