function ShowMessage(message, messagetype) {
    var cssclass;
    var messagetype2;

    switch (messagetype) {
        case 'Success':
            cssclass = 'alert-success';
            messagetype2 = 'Sucesso';
            break;
        case 'Error':
            cssclass = 'alert-danger';
            messagetype2 = 'Erro';
            break;
        case 'Warning':
            cssclass = 'alert-warning';
            messagetype2 = 'Atenção';
            break;
        default:
            cssclass = 'alert-info';
            messagetype2 = 'Informação';
    }
    $('#alert_container').append('<div id="alert_div" style="margin: 0 0.5%; -webkit-box-shadow: 3px 4px 6px #999;" class="alert fade in ' + cssclass + '"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>' + messagetype2 + ' !</strong> <span>' + message + '</span></div>');

}

