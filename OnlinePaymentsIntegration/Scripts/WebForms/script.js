function CallMe(src, dest) {
    var ctrl = document.getElementById(src);
    // call server side method
    PageMethods.GetContactName(ctrl.value, CallSuccess, CallFailed, dest);
}

// set the destination textbox value with the ContactName
function CallSuccess(res, destCtrl) {
    var dest = document.getElementById(destCtrl);
    dest.text = res;
}

// alert message on some failure
function CallFailed(res, destCtrl) {
    alert(res.get_message());
}