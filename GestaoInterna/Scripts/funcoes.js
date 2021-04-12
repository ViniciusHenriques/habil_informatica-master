
function PermiteSomenteNumeros(event) {
    var charCode = (event.which) ? event.which : event.keyCode

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    else
        return true;
}

function MensagemTelaBootStrap(mensagem) {
    <div class="alert alert-success">
      <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
      <strong> Sucesso! </strong>
      + mensagem + 
    </div>
}

<script type="text/javascript" src="Scripts/jquery-2.1.4.min.js"></script>
<script type="text/javascript"> 
$(document).ready(function(){
    $(".flip").click(function(){
        $(".panel").slideToggle("slow");
    });
});
</script>





//function tabenter(event, campo) {
//    var tecla = event.keyDown ? event.KeyDown : event.which ? event.which : event.charCode;
//    if (tecla == 13) {
//        campo.focus();
//    }
//}



//function moveValor(campo, valor) {
//    document.getElementById(campo).value = valor;
//}


//function verificaOpcao(valor) {
//    if (valor == "ICMS/IPI e Outros") {
//        document.getElementById("cidadeiss").disabled = true;
//        document.getElementById("estadoicms").disabled = false;
//    } else if (valor == "ISS") {
//        document.getElementById("cidadeiss").disabled = false;
//        document.getElementById("estadoicms").disabled = true;
//    }
//}

//function FechaDivDialog() {
//    alert('teste');
////    parent.document.getElementById('myCadSisModulo').style.display = 'none';
//}

//function Enviar1()
//{
//    document.getElementById("teste").innerHTML = 'aeeee';
//    parent.document.getElementById('ConsultaModulo').style.display = 'none';
//}

//function getIFrameControl() {
//    alert(document.frames["iframe1"].document.forms[0].elements["txtCodigo"].value);
//}



//$(document).ready(function () {
//    $(document).ready(function () {
//        $('.modal fade').click(function (ev) {
//            ev.preventDefault();
//            $("#myCadSisModulo").hide();
//            $(".window").hide();
//        });
//    });
//});


function addEvent(obj, evType, fn) {
    if (typeof obj == "string") {
        if (null == (obj = document.getElementById(obj))) {
            throw new Error("Cannot add event listener: HTML Element not found.");
        }
    }
    if (obj.attachEvent) {
        return obj.attachEvent(("on" + evType), fn);
    } else if (obj.addEventListener) {
        return obj.addEventListener(evType, fn, true);
    } else {
        throw new Error("Your browser doesn't support event listeners.");
    }
}

function iniciarMudancaDeEnterPorTab() {
    var i, j, form, element;
    for (i = 0; i < document.forms.length; i++) {
        form = document.forms[i];
        for (j = 0; j < form.elements.length; j++) {
            element = form.elements[j];
            if ((element.tagName.toLowerCase() == "input")
                  && (element.getAttribute("type").toLowerCase() == "submit")) {
                form.onsubmit = function () {
                    return false;
                };
                element.onclick = function () {
                    if (this.form) {
                        this.form.submit();
                    }
                };
            } else {
                element.onkeypress = mudarEnterPorTab;
            }
        }
    }
}

function mudarEnterPorTab(e) {
    if (typeof e == "undefined") {
        var e = window.event;
    }
    var keyCode = e.keyCode ? e.keyCode : (e.wich ? e.wich : false);
    if (keyCode == 13) {
        if (this.form) {
            var form = this.form, i, element;
            // se o tabindex do campo for maior que zero, irá obrigatoriamente
            // procurar o campo com o próximo tabindex
            if (this.tabIndex > 0) {
                var indexToFind = (this.tabIndex + 1);
                for (i = 0; i < form.elements.length; i++) {
                    element = form.elements[i];
                    if (element.tabIndex == indexToFind) {
                        element.focus();
                        break;
                    }
                }
            }
                // se o tabindex do campo for igual a zero, irá procurar o campo com tabindex
                // igual a 1. Caso não encontre, colocará o foco no próximo campo do formulário.
            else {
                for (i = 0; i < form.elements.length; i++) {
                    element = form.elements[i];
                    if (element.tabIndex == 1) {
                        element.focus();
                        return false;
                    }
                }
                // se não encontrou pelo tabIndex, procura o próximo elemento da lista
                for (i = 0; i < form.elements.length; i++) {
                    if (form.elements[i] == this) {
                        if (++i < form.elements.length) {
                            form.elements[i].focus();
                        }
                        break;
                    }
                }
            }
        }
        return false;
    }
}

// quando terminar o carregamento da página, executa a "iniciarMudancaDeEnterPorTab"
addEvent(window, "load", iniciarMudancaDeEnterPorTab);

