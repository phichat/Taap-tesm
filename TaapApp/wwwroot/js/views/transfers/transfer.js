
var warpper = 'div.warpper';
var frmTransfer = '#frm-transfer';
var frmSummary = '#frm-summary';
var tableTf ='#transfer_table';
var baseApp = $('#baseApp').attr('href');

var tableOptions = {
    "scrollX": true
};


function search(frm) {

    var obj = $(frm).serializeObject();

    var valid = $(frm).validate();
    valid.element('#dateFrom')
    valid.element('#dateTo')

    if (valid.element('#dateFrom') && valid.element('#dateTo')) {

        $(warpper).css('display', 'block');

        $.ajax({
            url: baseApp + '/Transfers/Search',
            type: 'GET',
            contentType: 'application/json',
            dataType: 'json',
            data: $(frm).serialize(),
            success: function (result) {
                renderTable(result);
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



$(document).ready(function () {
    $(tableTf).DataTable(tableOptions);
});


function renderTable(result) {
    var tr = '';
    $.each(result, function (i, e) {
        var datetoP = moment(e.dateToProduction);

        var index = (++i);

        var statusDesc = getStatus(e.status, e.statusDesc);
        
        tr += '<tr>' +
            '<td class="no">' + index + '</td>' +
            '<td class="status">' +statusDesc+ '</td>' +
            '<td class="dateToProduction">' + datetoP.format('L') + '</td>' +
            '<td class="model">' + e.model + '</td>' +
            '<td class="packingMonth">' + e.packingMonth + '</td>' +
            '<td class="consignment">' + e.consignment + '</td>' +
            '<td class="commissionNo">' + e.commissionFrom + ' - ' + e.commissionTo + '</td>' +
            '<td class="partType">' + e.partType + '</td>' +
            '<td class="tfNo">' + e.tfNo + '</td>' +
            '<td class="tfId" style="display:none;">' + e.tfId + '</td>' +
            '<td class="shop" style="display:none;">' + e.shop + '</td>' +
            '<td>' +
            '<a href="javaScript:void(0);" ' + 
            'onclick=getTransfer("'+
            index+'","'+
            e.dateToProduction+'","'+
            e.commissionFrom+'","'+
            e.commissionTo+'","'+
            e.receiveNo+'");>Edit</a>' +
            '</td>' +
            '</tr>'
    });

    var table = $(tableTf);

    if ($.fn.dataTable.isDataTable(tableTf)) {
        table.DataTable().destroy();
    }

    $(tableTf).find('tbody').html(tr);
    table.DataTable(tableOptions);

}

function getTransfer(rowNo, dateToProduction, commissionFrom, commissionTo, receiveNo) {
    var datetoP = moment(dateToProduction);
    var comfrom = parseInt(commissionFrom);
    var comto = parseInt(commissionTo)
    var countCom = 1 + (comto - comfrom);

    var tr = $(tableTf).find('tbody tr td.no').filter(function() {
        return $(this).text() == rowNo;
    }).closest("tr");

    var partType = tr.find('td.partType').text();
    var model = tr.find('td.model').text();
    var packingMonth = tr.find('td.packingMonth').text();
    var consignment = tr.find('td.consignment').text();
    var partType = tr.find('td.partType').text();
    var tfNo = tr.find('td.tfNo').text();
    var tfId = tr.find('td.tfId').text();
    var shop = tr.find('td.shop').text();
    
    $(frmTransfer).find('input[name=tfNo]').val(tfNo).focus();
    $(frmTransfer).find('input[name=dateToProduction]').val(dateToProduction);
    $(frmTransfer).find('input[name=partType]').val(partType);
    $(frmTransfer).find('input[name=receiveNo]').val(receiveNo);
    $(frmTransfer).find('input[name=tfId]').val(tfId);

    $(frmSummary).find('input[name=rowNo]').val(rowNo);
    $(frmSummary).find('strong[name=dateToProduction]').text(datetoP.format('L'));
    $(frmSummary).find('strong[name=receiveNo]').text(receiveNo);
    $(frmSummary).find('strong[name=packingMonth]').text(packingMonth);
    $(frmSummary).find('strong[name=model]').text(model);
    $(frmSummary).find('strong[name=consignment]').text(consignment);
    $(frmSummary).find('strong[name=commissionNo]').text(commissionFrom + ' - ' + commissionTo + ' (' + countCom + ')');
    $(frmSummary).find('strong[name=partType]').text(partType);
    $(frmSummary).find('strong[name=shop]').text(shop);
}

function onSave(frm) {
    var valid = $(frm).validate();
    valid.element('input[name=tfNo]');
    valid.element('input[name=dateToProduction]');
    valid.element('input[name=partType]');
    valid.element('input[name=receiveNo]');
    valid.element('input[name=tfId]');

    if (
    valid.element('input[name=tfNo]') &&
    valid.element('input[name=dateToProduction]') &&
    valid.element('input[name=partType]') &&
    valid.element('input[name=receiveNo]') &&
    valid.element('input[name=tfId]') 
    ) {

        $(warpper).css('display', 'block');

        $.ajax({
            url: baseApp + '/Transfers/CreateTransfer',
            type: 'POST',
            dataType: 'json',
            data: $(frm).serialize(),
            success: function (result) {
                alert('Save completed!');
                var rowNo = $(frmSummary).find('input[name=rowNo]').val();

                var tr = $(tableTf).find('tbody tr td.no').filter(function() {
                    return $(this).text() == rowNo;
                }).closest("tr");

                var statusDesc = getStatus(1, 'Is Update');

                tr.find('td.tfNo').text(result.tfNo);
                tr.find('td.tfId').text(result.tfId);
                tr.find('td.status').html(statusDesc);

                onReset();
                
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

function getStatus(status, statusDesc) {
    switch(status){
        case 0:
            return '<span class="badge badge-warning">' + statusDesc + '</span>';

        case 1:
            return '<span class="badge badge-success">' + statusDesc + '</span>';
    }
}

function onReset(){
    $(frmTransfer).find('input[name=tfNo]').val('');
    $(frmTransfer).find('input[name=dateToProduction]').val('');
    $(frmTransfer).find('input[name=partType]').val('');
    $(frmTransfer).find('input[name=receiveNo]').val('');
    $(frmTransfer).find('input[name=tfId]').val('');


    $(frmSummary).find('strong[name=dateToProduction]').text('');
    $(frmSummary).find('strong[name=receiveNo]').text('');
    $(frmSummary).find('strong[name=packingMonth]').text('');
    $(frmSummary).find('strong[name=model]').text('');
    $(frmSummary).find('strong[name=consignment]').text('');
    $(frmSummary).find('strong[name=commissionNo]').text('');
    $(frmSummary).find('strong[name=partType]').text('');
    $(frmSummary).find('strong[name=shop]').text('');
}