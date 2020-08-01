function ConfirmDelete(id,isDeleted) {
    var confSpan = 'ConfirmationDelete_' + id;
    var delSpan = 'deleteSpan_' + id;

    if (isDeleted) {
        $('#' + delSpan).hide();
        $('#' + confSpan).show();
    } else {
        $('#' + delSpan).show();
        $('#' + confSpan).hide();
    }
}