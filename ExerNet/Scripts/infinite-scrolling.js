var BlockNumber = 2;  //Infinate Scroll starts from second block
var NoMoreData = false;
var inProgress = false;
$(window).scroll(function ()
{
    if ($(window).scrollTop() == $(document).height() - $(window).height() && !NoMoreData && !inProgress) {
        inProgress = true;
        $("div#loadmoreajaxloader").show();
        var element = document.getElementById("postswrapper");
            if (element != null) {
                $.post("http://localhost:54084/User/InfiniteScroll", { "BlockNumber": BlockNumber, "UserName": element.firstElementChild.id },
                function (data) {

                    BlockNumber = BlockNumber + 1;
                    NoMoreData = data.NoMoreData;
                    $("#postswrapper").append(data.HTMLString);
                    $("div#loadmoreajaxloader").hide();
                    inProgress = false;
                });
            }
        }
});