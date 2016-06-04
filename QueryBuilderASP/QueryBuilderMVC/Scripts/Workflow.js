function InvitationUser(userName, userId) {
    $("#UserName").val(userName);
    $("#invitedUserId").val(userId);
};

function LoadGrid(idProject, actionGetGridModel, actionGetData, actionListHistory, userAuthorized) {
    var s = formatQueryString();

    $.ajax({
        type: "POST",
        url: actionGetGridModel + "?query=" + s + "&idCurrentProject=" + idProject,
        data: "",
        dataType: "json",
        success: function(result) {
            var modelList = [];
            if (typeof result == "string"){
                $("#gridWrapper").hide();
                $("#nodata_jqg").text(result);
                $("#nodata_jqg").show();
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    modelList.push({
                        name: result[i].Name,
                        index: result[i].Index,
                        sortable: result[i].Sortable,
                        align: result[i].Align
                    });
                }

                $("#jqg").jqGrid("GridUnload");

                $("#jqg").jqGrid({
                    jsonReader: {
                        cell: "",
                        id: "0"
                    },
                    url: actionGetData,
                    colModel: modelList,
                    datatype: "json",
                    rowNum: 10,
                    rowList: [10, 20, 30, 50, 100],
                    pager: "#jpager",
                    loadonce: true,
                    autowidth: true,
                    scrollerbar: true,
                    gridComplete: function() {
                        var recs = parseInt($("#jqg").getGridParam("records"),10);
                        if (isNaN(recs) || recs === 0) {
                            $("#gridWrapper").hide();
                            $("#nodata_jqg").show();
                        }
                        else {
                            $("#gridWrapper").show();
                            $("#nodata_jqg").hide();
                        }
                    }
                });

                $("#jqg").jqGrid("navGrid", "#jpager", {
                    search: true,
                    searchtext: "Search",
                    refresh: false,
                    add: false,
                    del: false,
                    edit: false,
                    view: true,
                    viewtext: "View",
                    viewtitle: "Selected record"
                });

                if (userAuthorized) {
                    UpdateHistoryList(actionListHistory);
                }
            };

        },
        error: function(x, e)
        {
            alert(x.readyState + " "+ x.status +" "+ e.msg);
        }
    });
};

function fillParameters(projectId) {
    var s = formatQueryString();
    $(".sqlQueryForSaveToFile").val(s);
    $(".idCurrentProjectForSaveToFile").val(projectId);
}

function formatQueryString() {
    var query = $("#sql-code").val();

    query = formatQueryStringByKeyword(query, "From");
    query = formatQueryStringByKeyword(query, "Where");
    query = formatQueryStringByKeyword(query, "Group By");
    query = formatQueryStringByKeyword(query, "Order By");

    return query;
}

function formatQueryStringByKeyword(query, keyword) {
    var arr = query.split(keyword);
    query = "";
    if (arr.length > 1) {
    	for (var i = 0; i < arr.length; i++) {
    		query += arr[i];
    		if (i !== arr.length - 1) {
    			query += " " + keyword;
    		}
    	}
    } else {
    	query = arr;
    }
    return query;        
}

function NotifyAndUpdate(url, updateurl) {
    $.ajax({
        url: url,
        type: "POST",
        data: $('form').serialize(),
        datatype: "json",
        success:
        function (result) {
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
    //$("#notify").append('<div class="alert alert-success">Success<button type="button" class="close" data-dismiss="alert" aria-hidden="true">x</button></div>');
    runEffect();
    $(".dialog").remove();
};


function ModalPostDialogCreateWithNotifyAndUpdate(selector, url, updateurl, callback) {

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
            .addClass("ui-dialog")
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
	});
};

function UpdateHistoryList(url) {
	$.ajax({
		url: url,
		type: "POST",
		datatype: "json",
		success:
        function (result) {
        	$("#ListHistory").remove();
        	console.log(result);
        	$("#ContainerHistory").append('<div id="ListHistory"></div>');
        	$("#ListHistory").append(result);
        }
	});
};


    // run the currently selected effect
    function runEffect() {
        // get effect type from
        var selectedEffect = 'blind';

        // most effect types need no options passed by default
        var options = {};
        // some effects have required parameters
        if (selectedEffect === "scale") {
            options = { percent: 100 };
        } else if (selectedEffect === "size") {
            options = { to: { width: 280, height: 185 } };
        }

        // run the effect
        $("#notify").show(selectedEffect, options, 600, callback);
    };

    //callback function to bring a hidden box back
    function callback() {
        setTimeout(function () {
            $("#notify:visible").removeAttr("style").fadeOut();
        }, 4000);
    };

