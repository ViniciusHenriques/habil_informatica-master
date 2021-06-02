function ReplaceTextArea(id) {
    var anotacao = document.getElementById(id).value;
    document.getElementById(id).value = anotacao.replaceAll("<", "[").replaceAll(">", "]");
}

jQuery('body').on('keydown', 'input', function (e) {
    var str = e.target.id;

    if (e.target.type == "text") {
        if ((str.toLowerCase().indexOf("mail") > -1) == false
            && (str.toLowerCase().indexOf("email") > -1) == false
            && (str.toLowerCase().indexOf("categoria") > -1) == false
            && (str.toLowerCase().indexOf("localizacao") > -1) == false)
        {
            var conteudo = document.getElementById(e.target.id).value;
            document.getElementById(e.target.id).value = conteudo.replaceAll("<", "[").replaceAll(">", "]").replaceAll("\t", "").replaceAll("–", "-").replaceAll("—", "-").toUpperCase();
        }
        else {
            var conteudo = document.getElementById(e.target.id).value;
            document.getElementById(e.target.id).value = conteudo.replaceAll("<", "[").replaceAll(">", "]").replaceAll("\t", "").replaceAll("–", "-").replaceAll("—", "-").toLowerCase();

        }
    }
});


jQuery('body').on('keyup', 'input', function (e) {
    var str = e.target.id;

    if (e.target.type == "text") {
        if ((str.toLowerCase().indexOf("mail") > -1) == false
            && (str.toLowerCase().indexOf("email") > -1) == false
            && (str.toLowerCase().indexOf("categoria") > -1) == false
            && (str.toLowerCase().indexOf("localizacao") > -1) == false) {
            var conteudo = document.getElementById(e.target.id).value;
            document.getElementById(e.target.id).value = conteudo.replaceAll("<", "[").replaceAll(">", "]").replaceAll("\t", "").replaceAll("–", "-").replaceAll("—", "-").toUpperCase();
        }
        else {
            var conteudo = document.getElementById(e.target.id).value;
            document.getElementById(e.target.id).value = conteudo.replaceAll("<", "[").replaceAll(">", "]").replaceAll("\t", "").replaceAll("–", "-").replaceAll("—", "-").toLowerCase();
        }
    }
});


jQuery('body').on('change', 'input', function (e) {
    
    var conteudo = document.getElementById(e.target.id).value;
    document.getElementById(e.target.id).value = conteudo.replaceAll("<", "[").replaceAll(">", "]").replaceAll("\t", "").replaceAll("–", "-").replaceAll("—", "-").trim();
    ProximoCampo(e);
});



jQuery('body').bind('paste', function (e) {
    var str = e.target.id;

    if ((str.toLowerCase().indexOf("mail") > -1) ||
        (str.toLowerCase().indexOf("email") > -1) ||
        (str.toLowerCase().indexOf("categoria") > -1) ||
        (str.toLowerCase().indexOf("localizacao") > -1)) {
        if (e.target.type == "text")
            e.preventDefault();
    }
});

jQuery('body').bind('click', function (e) {
    document.getElementById(e.target.id).select();
});

function ProximoCampo(e) {
    
    if (e.target.form) {
        var form = e.target.form;
        var i;
        var element;
        // se o tabindex do campo for maior que zero, irá obrigatoriamente
        // procurar o campo com o próximo tabindex
        if (e.target.tabIndex > 0) {
            var indexToFind = (e.target.tabIndex + 1);
            for (i = 0; i < form.elements.length; i++) {
                element = form.elements[i];
                if (element.tabIndex == indexToFind) {
                    element.focus();
                    
                    break;
                }
            }
        }
    }
}




/*
jQuery('body').bind('copy', function (e) {
    var str = e.target.id;

    if ((str.toLowerCase().indexOf("mail") > -1) || (str.toLowerCase().indexOf("email") > -1)) {
        if (e.target.type == "text")
            e.preventDefault();
    }
});
jQuery('body').on('load', 'select', function (e) {
    var result = document.getElementById(e.target.id).length;
    if (result == 2) {
        alert("");
        element.options[2].text;
    }    
});
*/
