// Write your JavaScript code.

$(document).ready(function () {

    if ($('select').length > 0) {
        $('select').selectize({
            create: false,
            sortField: 'text'
        });
    }

    var metaForm = $('meta[name="form"]').attr("content");
    var acct = localStorage.getItem('access_token')
    var decode = parseJwt(acct);
    var matchForm = true;
    
    $.each(decode.Permissions, function (i, e) {
        if (e.Form == metaForm) {
            if (!e.Viewer) {
                var ele = $('[data-permission="viewer"]');
                ele && ele.attr('disabled', true);
            }

            if (!e.Creater) {
                var ele = $('[data-permission="creater"]');
                ele && ele.attr('disabled', true);
            }

            if (!e.Editer) {
                var ele = $('[data-permission="editer"]');
                ele && ele.attr('disabled', true);
            }

            if (!e.Deleter) {
                var ele = $('[data-permission="deleter"]');
                ele && ele.attr('disabled', true);
            }

            if (!e.Printer) {
                var ele = $('[data-permission="printer"]');
                ele && ele.attr('disabled', true);
            }
            matchForm = true;
            return false;
        } else {
            matchForm = false;
        };
    });

    if (metaForm != 'any' && matchForm == false) {
        window.location.href = '/';
    }

})

$(document).on('click', '.nav a[href!="#"]', function (e) {

    var thisNav = $(this).parent().parent();
    if (thisNav.hasClass('nav-tabs') || thisNav.hasClass('nav-pills')) {
        e.preventDefault();
    } else if ($(this).attr('target') == '_top') {
        e.preventDefault();
        var target = $(e.currentTarget);
        window.location = (target.attr('href'));
    } else if ($(this).attr('target') == '_blank') {
        e.preventDefault();
        var target = $(e.currentTarget);
        window.open(target.attr('href'));
    } else {
        e.preventDefault();
        var target = $(e.currentTarget);

        if (target.data('form') == 'any') {
            window.location.href = target.attr('href');
        }

        var acct = localStorage.getItem('access_token')
        var decode = parseJwt(acct);

        $.each(decode.Permissions, function (i, e) {
            if (e.Form == target.data('form')) {
                window.location.href = target.attr('href');
            };
        });
    }
});

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

function parseJwt(jwt) {
    var base64Url = jwt.split('.')[1];
    var base64 = base64Url.replace('-', '+').replace('_', '/');
    return JSON.parse(window.atob(base64));
};
