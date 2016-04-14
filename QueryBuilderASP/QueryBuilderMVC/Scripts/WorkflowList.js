$(function () {

    $(".portlet")
      .addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
        .find(".portlet-header")
        .addClass("ui-widget-header ui-corner-all")
        .prepend("<span class='ui-icon ui-icon-minusthick portlet-toggle'></span>");

    $(".nonSortable")
      .addClass(" ui-widget-content ui-helper-clearfix ui-corner-all")
        .find(".portlet-header")
        .addClass("ui-widget-header ui-corner-all")

    $(".viewDialogDelete")
        .prepend("<span class='iconHeaderDelete iconHeader portlet-2'></span>");

    $(".portlet")
    .find(".viewDialogEdit")
        .prepend("<span class='iconHeaderEdit iconHeader portlet-3'></span>");

    $(".UpdateConnection")
        .prepend("<span class='iconHeaderEdit iconHeader portlet-3'></span>");

    $(".UpdateConnectionItem")
        .prepend("<span class='iconHeaderEditItem iconHeader portlet-3'></span>");

    $(".DeleteConnection")
        .prepend("<span class='iconHeaderDelete iconHeader portlet-2'></span>");
    $(".DeleteConnectionItem")
        .prepend("<span class='iconHeaderDeleteItem iconHeader portlet-2'></span>");
    $(".CreateConnection")
        .prepend("<span class='iconHeaderCreate iconHeader portlet-2'></span>");

    $(".ERModel")
        .prepend("<span class='iconHeaderERModelItem iconHeader portlet-2'></span>");

    $(".portlet-toggle").click(function () {
        var icon = $(this);
        icon.toggleClass("ui-icon-minusthick ui-icon-plusthick");
        icon.closest(".portlet").find(".portlet-content").toggle();
    });
});