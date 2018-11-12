
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
    if (obj.model == '') {
        $(frm).find('[required]').focus();
        return false;
    }

    if (obj.packingMonth == '') {
        $(frm).find('[required]').focus();
        return false;
    }

    var valid = $(frm).validate();
    valid.element('#dateFrom')
    valid.element('#dateTo')

    if (valid.element('#dateFrom') && valid.element('#dateTo')) {

        $(warpper).css('display', 'block');

        $.ajax({
            url: baseApp + '/ReportCarsMovement/Search',
            type: 'GET',
            contentType: 'application/json',
            dataType: 'json',
            data: $(frm).serialize(),
            success: function (result) {

                var tr = '';
                $.each(result, function (i, e) {
                    var dateFG = moment(e.dateFG);
                    var datebuyOff = moment(e.dateBuyOff);
                    tr += '<tr>' +
                        '<td>' + (++i) + '</td>' +
                        '<td>' + dateFG.format('L') + '</td>' +
                        '<td>' + e.commissionNo + '</td>' +
                        '<td>' + e.model + '</td>' +
                        '<td>' + e.packingMonth + '</td>' +
                        '<td>' + e.setNo + '</td>' +
                        '<td>' + e.unit + '</td>' +
                        '<td>' + datebuyOff.format('L') + '</td>' +
                        '<td>' + e.amount + '</td>' +
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
    if (obj.receiveNo == '') {
        $(frm).find('[required]').focus();
        return false;
    }
    var reportName = 'ReportName=CarsMovement';
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
    var reportName = 'ReportName=CarsMovement';
    var ReceiveNo = $(frm).serialize();
    var reportType = 'ReportType=excel';
    var param = reportName + '&' + reportType + '&' + ReceiveNo;
    window.open(baseReport + '/Reports/Reports.aspx?' + param);
}