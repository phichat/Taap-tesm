
var warpper = 'div.warpper';

var baseApp = $('#baseApp').attr('href');
var tableOptions = {
    "scrollX": true
};


$(document).ready(function () {
    $('table').DataTable(tableOptions);
});


function search(frm) {
    var obj = $(frm).serializeObject();
    if (obj.receiveNo == '') {
        $(frm).find('[required]').focus();
        return false;
    }

    $(warpper).css('display', 'block');

    $.ajax({
        url: baseApp + '/ReportStockAvailable/Search',
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        data: $(frm).serialize(),
        success: function (result) {

            var tr = '';
            $.each(result, function (i, e) {
                var dateToP = moment(e.dateToProduction);

                tr += '<tr>' +
                    '<td>' + (++i) + '</td>' +
                    '<td>' + dateToP.format('L') + '</td>' +
                    '<td>' + e.receiveNo + '</td>' +
                    '<td>' + e.customEntryNo + '</td>' +
                    '<td>' + e.invoiceNo + '</td>' +
                    '<td>' + e.partNo + '</td>' +
                    '<td>' + e.partDescription + '</td>' +
                    '<td>' + e.qty + '</td>' +
                    '<td>' + e.qpv + '</td>' +
                    '<td>' + e.amount + '</td>' +
                    '<td>' + e.um + '</td>' +
                    '</tr>'
            })

                if ($.fn.dataTable.isDataTable('table')) {
                    $('table').DataTable().destroy();
                }

                $('table tbody').html(tr);
                $('table').DataTable(tableOptions);
        },
        error: function (xhr, status, error) {
            alert(error);
        },
        complete: function () {
            $(warpper).css('display', 'none');
        }
    })
}

var baseReport = $('#baseReport').attr('href');
function exportPDF(frm) {
    var obj = $(frm).serializeObject();
    if (obj.receiveNo == '') {
        $(frm).find('[required]').focus();
        return false;
    }
    var reportName = 'ReportName=StockAvailable';
    var ReceiveNo = $(frm).serialize();
    var reportType = 'ReportType=pdf';
    var param = reportName + '&' + reportType + '&' + ReceiveNo;
    window.open(baseReport + '/Reports/Reports.aspx?' + param);
}

function exportExcel(frm) {
    var obj = $(frm).serializeObject();
    if (obj.receiveNo == '') {
        $(frm).find('[required]').focus();
        return false;
    }
    var reportName = 'ReportName=StockAvailable';
    var ReceiveNo = $(frm).serialize();
    var reportType = 'ReportType=excel';
    var param = reportName + '&' + reportType + '&' + ReceiveNo;
    window.open(baseReport + '/Reports/Reports.aspx?' + param);
}