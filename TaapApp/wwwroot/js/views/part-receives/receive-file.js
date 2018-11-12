
var frmSummary = '#frm-summary';
var frmSearch = '#frm-search';
var frmUpload = '#frm-upload';
var warpper = 'div.warpper';
var baseApp = $('#baseApp').attr('href');

var tableOptions = {
    "scrollX": true
};

$(document).ready(function () {
    $('#part_receive').DataTable(tableOptions);
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
        url: baseApp + "/PartReceives/ReceiveFile",
        type: "POST",
        contentType: false,
        processData: false,
        data: data,
        success: function (result, status, xhr) {
            //$('#successModal').modal('show');

            $(frm).find('input[type=file]').val('');

            renderTable(result);

            //$('strong[name=receiveNo]').text(result['receiveNo']);
            //$('strong[name=packingMonth]').text(result['packingMonth']);
            //$('strong[name=model]').text(result['model']);
            //$('strong[name=consignment]').text(result['consignment']);
            //$('strong[name=commissionNo]').text(result['commissionFrom'] + ' - ' + result['commissionTo'] + ' (' + result['countCommission'] + ')');
            //$('strong[name=shop]').text(result['shop']);

            //$('strong[name=totalCount]').text(result['totalCount'].format());
            //$('strong[name=totalQty]').text(result['totalQty'].format());
        },
        error: function (xhr, status, error) {
            console.log(xhr);
            
            $('#dangerModal').modal('show');
            $('#dangerModal').find('.modal-body').html('<p>' + error + '</p>');
        },
        complete: function () {
            $(warpper).css('display', 'none');
        }
    });
}

function renderTable(result) {
    var tr = '';
    var receiveNo = '';
    var totalCount = 0;
    var totalQty = 0;
    $.each(result, function (i, e) {
        receiveNo = e.receiveNo;
        ++totalCount;
        totalQty += e.qty;
        var datetoP = moment(e.dateToProduction);

        tr += '<tr>' +
            '<td>' + (++i) + '</td>' +
            '<td>' + e.customEntryNo + '</td>' +
            '<td>' + e.invoiceNo + '</td>' +
            '<td>' + datetoP.format('L') + '</td>' +
            '<td>' + e.partNo + '</td>' +
            '<td>' + e.partDescription + '</td>' +
            '<td>' + e.qty + '</td>' +
            '<td>' + e.amount + '</td>' +
            '<td>' + e.qpv + '</td>' +
            '<td>' + e.um + '</td>' +
            '<td>' + e.partType + '</td>' +
            '<td>' +
            '<a href="' + baseApp + '/PartReceives/Edit/' + e.receiveId + '">Edit</a> |' +
            '<a href="' + baseApp + '/PartReceives/Details/' + e.receiveId + '">Details</a> |' +
            '<a href="' + baseApp + '/PartReceives/Delete/' + e.receiveId + '">Delete</a>' +
            '</td>' +
            '</tr>'
    });

    var table = $('#part_receive');

    if ($.fn.dataTable.isDataTable('#part_receive')) {
        table.DataTable().destroy();
    }

    $('#part_receive tbody').html(tr);
    table.DataTable(tableOptions);

    var comfrom = parseInt(result[0].commissionFrom);
    var comto = parseInt(result[0].commissionTo)
    var countCom = 1 + (comto - comfrom);

    $(frmSummary).find('strong[name=receiveNo]').text(result[0].receiveNo);
    $(frmSummary).find('strong[name=packingMonth]').text(result[0].packingMonth);
    $(frmSummary).find('strong[name=model]').text(result[0].model);
    $(frmSummary).find('strong[name=consignment]').text(result[0].consignment);
    $(frmSummary).find('strong[name=commissionNo]').text(result[0].commissionFrom + ' - ' + result[0].commissionTo + ' (' + countCom + ')');
    $(frmSummary).find('strong[name=shop]').text(result[0].shop);

    $(frmSummary).find('strong[name=totalCount]').text(totalCount.format());
    $(frmSummary).find('strong[name=totalQty]').text(totalQty.format());
}

function search(frm) {
    var obj = $(frm).serializeObject();
    if (obj.receiveNo == '') {
        $(frm).find('[required]').focus();
        return false;
    }
    var query = $(frm).serialize();

    $(warpper).css('display', 'block');

    $.ajax({
        url: baseApp + "/PartReceives/Search",
        type: "GET",
        contentType: false,
        processData: false,
        data: query,
        success: function (result) {
            renderTable(result);
        },
        error: function (xhr, status, error) {
            console.log()
            alert(error);
        },
        complete: function () {
            $(warpper).css('display', 'none');
        }
    });
}

function toggleFrmDelete(frm) {
    $(frm).toggle();
}