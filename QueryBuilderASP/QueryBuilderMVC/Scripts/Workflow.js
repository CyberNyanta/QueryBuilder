function InvitationUser(userName, userId) {
    $("#UserName").val(userName);
    $("#invitedUserId").val(userId);
};



function NotifyAndUpdate(url, updateurl) {
    console.log($('form').serialize());
    console.log("url: " + url);
    $.ajax({
        url: url,
        type: "POST",
        data: $('form').serialize(),
        datatype: "json",
        success:
        function (result) {
            console.log(result);
                if ( result == "Success") {
                    Notify();
                    
                }
                else {
                    $("#dialogContent").html(result);
                }
            
        }
    });
}

function Notify() {
    $("#notify").append('<div class="alert alert-success">Success<button type="button" class="close" data-dismiss="alert" aria-hidden="true">x</button></div>');
    $(".dialog").remove();
};


function ModalPostDialogCreateWithNotifyAndUpdate(selector, url, updateurl, callback) {

    $(selector).on("click", function (e) {
        console.log(selector+" "+url+" "+updateurl+" "+callback)
        e.preventDefault();
        $("<div id='dialogContent'></div>")
            .addClass("dialog")
            .appendTo("body")
            .load(this.href)
            .dialog({
                title: $(this).attr("data-dialog-title"),
                close: function () { $(this).remove() },
                modal: true,
                buttons: {
                    "Create": function () {
                        NotifyAndUpdate(url, updateurl);
                        setTimeout(function() {
                            callback(updateurl);
                        }, 500);

                    }
                }
            }
            );
    });

};
function ModalPostDialogDeleteWithNotifyAndUpdate(selector, url, updateurl, callback) {

    $(selector).on("click", function (e) {
        e.preventDefault();

        $("<div id='dialogContent'></div>")
            .addClass("dialog")
            .appendTo("body")
            .load(this.href)
            .dialog({
                title: $(this).attr("data-dialog-title"),
                close: function () { $(this).remove() },
                modal: true,
                buttons: {
                    "Delete": function () {
                        NotifyAndUpdate(url, updateurl);
                        setTimeout(function () {
                            callback(updateurl);
                        }, 500);
                    }
                }
            }
            );
    });

};

function ModalPostDialogInviteWithNotifyAndUpdate(selector, url, updateurl, callback) {

    $(selector).on("click", function (e) {
        e.preventDefault();
        $("<div id='dialogContent'></div>")
            .addClass("dialog")
            .appendTo("body")
            .load(this.href)
            .dialog({
                title: $(this).attr("data-dialog-title"),
                close: function () { $(this).remove() },
                modal: true,
                buttons: {
                    "Invite": function () {
                        NotifyAndUpdate(url, updateurl);
                        setTimeout(function () {
                            callback(updateurl);
                        }, 500);

                    }
                }
            }
            );
    });

};

function ModalPostDialogUpdateWithNotifyAndUpdate(selector, url, updateurl, callback) {

    $(selector).on("click", function (e) {
        e.preventDefault();

        $("<div id='dialogContent'></div>")
            .addClass("dialog")
            .appendTo("body")
            .load(this.href)
            .dialog({
                title: $(this).attr("data-dialog-title"),
                close: function () { $(this).remove() },
                modal: true,
                buttons: {
                    "Update": function () {
                        NotifyAndUpdate(url, updateurl);
                        setTimeout(function () {
                            callback(updateurl);
                        }, 500);
                    }
                }
            }
            );
    });

};


function UpdateProjectList(url) {
    console.log("UpdateProjectList ");

    $.ajax({
        url: url,
        type: "POST",
        datatype: "json",
        success:
        function (result) {
            $("#ListProject").remove();
            $("#proj").append('<div id="ListProject"></div>');
            $("#ListProject").append(result);
        }
    })
};

function UpdateConnectionList(url) {
    console.log("UpdateConnectionList");

    $.ajax({
        url: url,
        type: "POST",
        datatype: "json",
        success:
        function (result) {
            $("#ListConnection").remove();
            $("#ContainerListConnection").append('<div id="ListConnection"></div>');
            $("#ListConnection").append(result);
        }
    })
};

function UpdateQueryList(url) {
	console.log("UpdateQueryList");

	$.ajax({
		url: url,
		type: "POST",
		datatype: "json",
		success:
        function (result) {
        	$("#ListQuery").remove();
        	$("#ContainerListQuery").append('<div id="ListQuery"></div>');
        	$("#ListQuery").append(result);
        }
	})
};