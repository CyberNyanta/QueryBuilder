

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
            Notify();
            var s = function () {
                UpdateProjectList(updateurl);

            }
            var rr = s();
        }
    });
}

function GetUrlFromHrefByName(selector) {
    return $(selector);
}
function SetEventForItems(selector, list, event) {
    for (var index = 0; index <= list.length; index++) {
        if (!!list[index]) {
            event(selector, list[index].href);
        }
    }
    // ModalPostDialogDelete(iconDel, actionDelUrl);
}
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
                        debugger;
                        var iconDel = ".IconModalDelete";
                        var iconEdit = ".IconModalEdit";
                        var list = GetUrlFromHrefByName(iconDel);
                        SetEventForItems(iconDel, list, ModalPostDialogDelete);
                        var actionEditUrl = GetUrlFromHrefByName(iconEdit);

                        //ModalPostDialogUpdate(iconEdit, actionEditUrl);
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
                        NotifyAndUpdate(url, updateurl)
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
            console.log(result);
            $("#ListProject").remove();
            $("#proj").append('<div id="ListProject"></div>');
            $("#ListProject").append(result);
            $("#hd-1").addClass(".hide");
        }
    })
};

function Notify() {
    $("#notify").append('<div class="alert alert-success">Success<button type="button" class="close" data-dismiss="alert" aria-hidden="true">x</button></div>');
    $(".dialog").remove();
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
function ModalPostDialogUpdate(selector, url) {

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
                        AjaxPostWithDialog(url);
                    }
                }
            }
            );
    });

};