
$(document).ready(function () {
    localStorage.removeItem('access_token');
})

$('#signIn').validate({
    submitHandler: function (form) {
        signIn(form);
    }
});


function signIn(frm) {

    var obj = $(frm).serializeObject();

    var baseApp = $('#baseApp').attr('href');

    var query = $(frm).serialize();
    $.ajax({
        url: baseApp + '/SignIn/SignIn',
        type: 'POST',
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json", 
        success: function (result) {
            if (result.access_token) {
                localStorage.setItem('access_token', JSON.stringify(result.access_token));
                window.location.href = baseApp + "/";
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    })
}