

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
            UpdateProjectList(updateurl)
            Notify();
        }
    });
}

function Notify() {
    $("#notify").append('<div class="alert alert-success">Success<button type="button" class="close" data-dismiss="alert" aria-hidden="true">x</button></div>');
    $(".dialog").remove();
};


//function GetUrlFromHrefByName(selector) {
//    return $(selector);
//}
//function SetEventForItems(selector, list, callback) {
//    for (var index = 0; index <= list.length; index++) {
//        if (!!list[index]) {
//            callback(selector, list[index].href);

//        }
//    }

//}

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
                        //debugger;
                        //var iconDel = ".IconModalDelete";
                        //var iconEdit = ".IconModalEdit";
                        //var list = GetUrlFromHrefByName(iconEdit);
                        //SetEventForItems(iconEdit, list, ModalPostDialogUpdate);
                        //var actionEditUrl = GetUrlFromHrefByName(iconEdit);

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
            $("#hd-1").addClass(".hide");
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