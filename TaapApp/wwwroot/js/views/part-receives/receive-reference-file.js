var frmSummary = '#frm-summary';
var warpper = 'div.warpper';
var baseApp = $('#baseApp').attr('href');
var tableOptions = {
    "scrollX": true
};


$(document).ready(function () {
    $('#buyoff').DataTable(tableOptions);
});


function onUploadFile(frm) {
    $(warpper).css('display', 'block');
    var fileUpload = $(frm).find('input[type=file]').get(0);
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }

    $.ajax({
        url: baseApp + "/ReceiveReferences/ReceiveBuyOffFile",
        type: "POST",
        contentType: false,
        processData: false,
        data: data,
        success: function (result, status, xhr) {
            // $('#successModal').modal('show');

            $(frm).find('input[type=file]').val('');

            renderTable(result);
        },
        error: function (xhr, status, error) {
            $('#dangerModal').modal('show');
            $('#dangerModal').find('.modal-body').html('<p>' + error + '</p>');
        },
        complete: function () {
            $(warpper).css('display', 'none');
        }
    });

}

function onUploadReferenceFile(frm) {
    $(warpper).css('display', 'block');
    var fileUpload = $(frm).find('input[type=file]').get(0);
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }

    $.ajax({
        url: baseApp + "/ReceiveReferences/ReceiveReferenceFile",
        type: "POST",
        contentType: false,
        processData: false,
        data: data,
        success: function (result, status, xhr) {
            $('#successModal').modal('show');

            $(frm).find('input[type=file]').val('');

            $('strong[name=isMatch]').text(result['isMatch'].format());
            $('strong[name=notMatch]').text(result['notMatch'].format());
            $('strong[name=totalCount]').text(result['totalCount'].format());
        },
        error: function (xhr, status, error) {
            $('#dangerModal').modal('show');
            $('#dangerModal').find('.modal-body').html('<p>' + error + '</p>');
        },
        complete: function () {
            $(warpper).css('display', 'none');
        }
    });
}

function search(frm) {
    var obj = $(frm).serializeObject();
    //if (obj.receiveNo == '') {
    //    $(frm).find('[required]').focus();
    //    return false;
    //}
    $(warpper).css('display', 'block');

    var query = $(frm).serialize();

    $.ajax({
        url: baseApp + "/ReceiveReferences/Search",
        type: "GET",
        contentType: false,
        processData: false,
        data: query,
        success: function (result) {
            renderTable(result);
        },
        error: function (xhr, status, error) {
            alert(error);
        },
        complete: function () {
            $(warpper).css('display', 'none');
        }
    });
}

function renderTable(result) {
    var tr = '';
    var vdono = result[0].vdono;
    var totalCount = result.length;
    var inProcess = 0;
    var isUpdate = 0;
    var notUpdate = 0;
    $.each(result, function (i, e) {
        var buyoffdate = moment(e.date);
        var statusDesc = '';
        if (e.status == 0) {
            // In Process
            inProcess += 1;
            statusDesc = '<span class="badge badge-info">' + e.statusDesc + '</span>';

        } else if (e.status == 1) {
            // Is Update
            isUpdate += 1;
            statusDesc = '<span class="badge badge-success">' + e.statusDesc + '</span>';

        } else if (e.status == 2) {
            // Not Update
            notUpdate += 1;
            statusDesc = '<span class="badge badge-warning">' + e.statusDesc + '</span>';
        } else {
            statusDesc = "<span>" + e.status + "</span>"
        }

        tr += '<tr>' +
            '<td>' + (++i) + '</td>' +
            '<td class="text-center">' + statusDesc + '</td>' +
            '<td>' + e.receiveNo + '</td>' +
            '<td>' + e.commissionNo + '</td>' +
            '<td>' + e.setNo + '</td>' +
            '<td>' + e.vdono + '</td>' +
            '<td>' + buyoffdate.format('L') + '</td>' +
            '<td>' +
            '<a href="' + baseApp + '/ReceiveReferences/Edit/' + e.vdoid + '">Edit</a> |' +
            '<a href="' + baseApp + '/ReceiveReferences/Details/' + e.vdoid + '">Details</a> |' +
            '</td>' +
            '</tr>';


           // '<td>' + e.transferNoCkd + '</td>' +
           // '<td>' + e.transferNoLoc + '</td>' +

    })

    var table = $('#buyoff');

    if ($.fn.dataTable.isDataTable('#buyoff')) {
        table.DataTable().destroy();
    }

    $('#buyoff tbody').html(tr);
    table.DataTable(tableOptions);

    $(frmSummary).find('strong[name=vdono]').text(vdono);
    $(frmSummary).find('strong[name=inProcess]').text(inProcess.format());
    $(frmSummary).find('strong[name=isMatch]').text(isUpdate.format());
    $(frmSummary).find('strong[name=notMatch]').text(notUpdate.format());
    $(frmSummary).find('strong[name=totalCount]').text(totalCount.format());
}