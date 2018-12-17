
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
}

function exportCPL(frm) {
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
            url: baseApp + '/ReportCarsMovement/ExportCPL',
            type: 'GET',
            contentType: 'application/x-www-form-urlencoded',
            data: $(frm).serialize(),
            success: function (response, status, xhr) {
                downLoadFile(response, xhr);
            },
            error: function (xhr, status, error) {
            },
            complete: function () {
                $(warpper).css('display', 'none');
            }
        });
    }
}


function exportFPL(frm) {
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
            url: baseApp + '/ReportCarsMovement/ExportFPL',
            type: 'GET',
            contentType: 'application/x-www-form-urlencoded',
            data: $(frm).serialize(),
            success: function (response, status, xhr) {
                downLoadFile(response, xhr);
            },
            complete: function () {
                $(warpper).css('display', 'none');
            }
        });
    }
}

function downLoadFile(response, xhr) {
    var filename = "";
    var disposition = xhr.getResponseHeader('Content-Disposition');
    if (disposition && disposition.indexOf('attachment') !== -1) {
        var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
        var matches = filenameRegex.exec(disposition);
        if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
    }

    var type = xhr.getResponseHeader('Content-Type');
    var blob = new Blob([response], { type: type });

    if (typeof window.navigator.msSaveBlob !== 'undefined') {
        // IE workaround for "HTML7007: One or more blob URLs were revoked by closing 
        // the blob for which they were created.These URLs will no longer resolve as the data backing the URL has been freed."
        window.navigator.msSaveBlob(blob, filename);
    } else {
        var URL = window.URL || window.webkitURL;
        var downloadUrl = URL.createObjectURL(blob);

        if (filename) {
            // use HTML5 a[download] attribute to specify filename
            var a = document.createElement("a");
            // safari doesn't support this yet
            if (typeof a.download === 'undefined') {
                window.location = downloadUrl;
            } else {
                a.href = downloadUrl;
                a.download = filename;
                document.body.appendChild(a);
                a.click();
            }
        } else {
            window.location = downloadUrl;
        }

        setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100); // cleanup
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
