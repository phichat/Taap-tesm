
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

    if (obj.partNo == '') {
        $(frm).find('[required]').focus();
        return false;
    }

    var valid = $(frm).validate();
    valid.element('#dateFrom')
    valid.element('#dateTo')

    if (valid.element('#dateFrom') && valid.element('#dateTo')) {

        $(warpper).css('display', 'block');

        $.ajax({
            url: baseApp + '/ReportPartsMovement/Search',
            type: 'GET',
            contentType: 'application/json',
            dataType: 'json',
            data: $(frm).serialize(),
            success: function (result) {

                var tr = '';
                $.each(result, function (i, e) {
                    var dateToP = moment(e.dateToProduction);
                    var dateBuy = moment(e.dateBuyOff);
                    var dateFG = moment(e.dateFG);

                    tr += '<tr>' +
                        '<td>' + (++i) + '</td>' +
                        '<td>' + dateToP.format('L') + '</td>' +
                        '<td>' + e.transferNo + '</td>' +
                        '<td>' + e.setNo + '</td>' +
                        '<td>' + e.receiveNo + '</td>' +
                        '<td>' + e.model + '</td>' +
                        '<td>' + e.partNo + '</td>' +
                        '<td>' + e.partDescription + '</td>' +
                        '<td>' + e.qty + '</td>' +
                        '<td>' + e.qpv + '</td>' +
                        '<td>' + e.commissionNo + '</td>' +
                        '<td>' + dateFG.format('L') + '</td>' +
                        '<td>' + dateBuy.format('L') + '</td>' +
                        '<td>' + e.vdoNo + '</td>' +
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
    console.log(obj.dateFrom, obj.dateTo)
    if (obj.partNo == '') {
        $(frm).find('[required]').focus();
        return false;
    }

    var valid = $(frm).validate();
    valid.element('#dateFrom')
    valid.element('#dateTo')

    if (valid.element('#dateFrom') && valid.element('#dateTo')) {
        var reportName = 'ReportName=PartsMovement';
        var query = $(frm).serialize();
        var reportType = 'ReportType=pdf';
        var param = reportName + '&' + reportType + '&' + query;
        window.open(baseReport + '/Reports/Reports.aspx?' + param);
    }
}

function exportExcel(frm) {

    var obj = $(frm).serializeObject();

    if (obj.partNo == '') {
        $(frm).find('[required]').focus();
        return false;
    }

    var valid = $(frm).validate();
    valid.element('#dateFrom')
    valid.element('#dateTo')

    if (valid.element('#dateFrom') && valid.element('#dateTo')) {
        var reportName = 'ReportName=PartsMovement';
        var query = $(frm).serialize();
        var reportType = 'ReportType=excel';
        var param = reportName + '&' + reportType + '&' + query;
        window.open(baseReport + '/Reports/Reports.aspx?' + param);
    }
}