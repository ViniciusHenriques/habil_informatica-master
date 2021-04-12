function mascara_cat(campo) {
    if (campo.value.length > 14) {
        campo.value = '';
    }
    else {
        if (campo.value.length == 2) {
            campo.value += '.';
        }
        if (campo.value.length == 5) {
            campo.value += '.';
        }
        if (campo.value.length == 8) {
            campo.value += '.';
        }
        if (campo.value.length == 11) {
            campo.value += '.';
        }

    }
}


function checa_value_cat(campo) {
    var stringvalida = "0123456789.";
    var flagerro = 1;
    for (i = 0; i < campo.value.length; i++) {
        for (j = 0; j < stringvalida.length; j++) {
            if (stringvalida.charAt(j) == campo.value.charAt(i).toUpperCase()) {
                flagerro = 0; break;
            }
            else {
                flagerro = 1;
            }
        }
        if (flagerro == 1) {
            charinvalido = campo.value.charAt(i);
            campo.value = campo.value.substring(0, campo.value.length - 1);
            var msg = "Campo Categoria o caracter '" + charinvalido + "'";
            alert(msg);
            campo.select();
            campo.focus();
        }
    }
}
