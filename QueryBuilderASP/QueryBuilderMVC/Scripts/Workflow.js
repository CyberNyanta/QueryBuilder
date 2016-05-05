

function InvitationUser(userName, userId) {
    $("#UserName").val(userName);
    $("#invitedUserId").val(userId);
};

function ModalPostDialogDelete(selector, url) {

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
                        AjaxPostWithDialog(url);
                    }
                }
            }
            );
    });

};

function ModalPostDialogInvite(selector, url) {

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
                        //AjaxPostWithNotify(url);
                        AjaxPostWithDialog(url);
                    }
                }
            }
            );
    });

};

function ModalPostDialogCreate(selector, url) {

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
                    "Create": function () {
                        AjaxPostWithDialog(url);
                    }
                }
            }
            );
    });

};

function NotifyAndUpdate(url, updateurl) {
    $.ajax({
        url: url,
        type: "POST",
        data: $('form').serialize(),
        datatype: "json",
        success:
        function (result) {
                if ( result == "Success") {
                    UpdateProjectList(updateurl)
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


function ModalPostDialogCreateWithNotifyAndUpdate(selector, url, updateurl) {

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
                    "Create": function () {
                        NotifyAndUpdate(url, updateurl);

                    }
                }
            }
            );
    });

};
function ModalPostDialogDeleteWithNotifyAndUpdate(selector, url, updateurl) {

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
                    }
                }
            }
            );
    });

};

function UpdateProjectList(url) {
    $.ajax({
        url: url,
        type: "POST",
        datatype: "json",
        success:
        function (result) {
            $("#ListProject").remove();
            $("#proj").append('<div id="ListProject"></div>');
            $("#ListProject").append(result);
            //$("#hd-1").addClass(".hide");
        }
    })
};



function AjaxPostWithDialog(url) {
    $.ajax({
        url: url,
        type: "POST",
        data: $('form').serialize(),
        datatype: "json",
        success: function (result) {
            $("#dialogContent").html(result);
        }
    });
};
function AjaxPostWithNotify(url) {
    $.ajax({
        url: url,
        type: "POST",
        data: $('form').serialize(),
        datatype: "json",
        success: function (result) {
            Notify();
        }
    });
};
function ModalPostDialogUpdate(selector, url, updateurl) {

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
                    }
                }
            }
            );
    });

};