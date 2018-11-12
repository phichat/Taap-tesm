
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
    if (obj.model == '' || obj.packingMonth == '') {
        $(frm).find('[required]').focus();
        return false;
    }

    var valid = $(frm).validate();
    valid.element('#dateFrom')
    valid.element('#dateTo')

    if (valid.element('#dateFrom') && valid.element('#dateTo')) {

        $(warpper).css('display', 'block');

        $.ajax({
            url: baseApp + '/ReportStockMaterial/Search',
            type: 'GET',
            contentType: 'application/json',
            dataType: 'json',
            data: $(frm).serialize(),
            success: function (result) {
               
                var tr = '';
                $.each(result, function (i, e) {
                    var recDate = moment(e.receiveDate);
                    tr += '<tr>' +
                        '<td>' + (++i) + '</td>' +
                        '<td>' + recDate.format('L') + '</td>' +
                        '<td>' + e.receiveNo + '</td>' +
                        '<td>' + e.customEntryNo + '</td>' +
                        '<td>' + e.invoiceNo + '</td>' +
                        '<td>' + e.commissionFrom + '</td>' +
                        '<td>' + e.commissionTo + '</td>' +
                        '<td>' + e.model + '</td>' +
                        '</tr>'
                })

                if ($.fn.dataTable.isDataTable('table')) {
                    $('table').DataTable().destroy();
                }

                $('table tbody').html(tr);
                table.DataTable(tableOptions);
            },
            error: function (xhr, status, error) {
                alert(error);
            },
            complete: function () {
                $(warpper).css('display', 'none');
            }
        })
    }
}


var baseReport = $('#baseReport').attr('href');
function exportPDF(frm) {
    var obj = $(frm).serializeObject();
    if (obj.shop == '') {
        $(frm).find('[required]').focus();
        return false;
    }

    var valid = $(frm).validate();
    valid.element('#dateFrom')
    valid.element('#dateTo')

    if (valid.element('#dateFrom') && valid.element('#dateTo')) {
        var reportName = 'ReportName=StockMaterial';
        var query = $(frm).serialize();
        var reportType = 'ReportType=pdf';
        var param = reportName + '&' + reportType + '&' + query;
        window.open(baseReport + '/Reports/Reports.aspx?' + param);
    }
}

function exportExcel(frm) {
    var obj = $(frm).serializeObject();
    if (obj.shop == '') {
        $(frm).find('[required]').focus();
        return false;
    }

    var valid = $(frm).validate();
    valid.element('#dateFrom')
    valid.element('#dateTo')

    if (valid.element('#dateFrom') && valid.element('#dateTo')) {
        var reportName = 'ReportName=StockMaterial';
        var query = $(frm).serialize();
        var reportType = 'ReportType=excel';
        var param = reportName + '&' + reportType + '&' + query;
        window.open(baseReport + '/Reports/Reports.aspx?' + param);
    }
}