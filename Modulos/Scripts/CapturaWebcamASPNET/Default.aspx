<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CapturaWebcamASPNET.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="camutils/webcam.js"></script>
</head>
<body>
    <form id="form1" runat="server">
	    <!-- Configura algumas opções -->
	    <script type="text/javascript" language="JavaScript">
	        webcam.set_api_url('Upload.aspx');//Página de destino do arquivo capturado
	        webcam.set_quality(90); // Qualidade do JPG (1 - 100)
	        webcam.set_shutter_sound(true); // Toca o som de câmera (o arquivo shutter.mp3, que vem com os "utilitários" da câmera, deve estar no diretório raíz do site)
	    </script>

        <table style="width:100%;" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:320px;height:240px;text-align:center;" valign="top">
                    <script type="text/javascript" language="JavaScript">
                        document.write(webcam.get_html(320, 240));
	                </script>
                    <input type="button" value="Configurar..." onclick="webcam.configure();" />
                    <input type="button" value="Capturar" onclick="take_snapshot();" />
                    <input type="button" value="Reset" onclick="webcam.reset();" />
                </td>
                <td style="width:320px;height:240px;" valign="top">
	                <!-- Desenha o HTML do Flash que faz a interface com a Webcam na resolução 320x240 -->
                    <div id="upload_results">
    
                    </div>
                </td>
            </tr>
        </table>
	
        <script type="text/javascript">
            webcam.set_hook('onComplete', 'my_completion_handler');

            function take_snapshot() {
                // Captura a imagem e submete ao servidor
                document.getElementById('upload_results').innerHTML = '<h1>Realizando Upload da Foto...</h1>';
                webcam.snap();
            }

            function my_completion_handler(msg) {
                // Extrai a URL da imagem do retorno de Upload.aspx
                if (msg.match(/(http\:\/\/\S+)/)) {
                    var image_url = RegExp.$1;
                    // show JPEG image in page
                    document.getElementById('upload_results').innerHTML =
					    '<img src="' + image_url + '">' +
                        '<h1>Upload Com Sucesso!</h1>' +
					    '<h3>URL do JPG: ' + image_url + '</h3>';

                    // reset camera for another shot
                    webcam.reset();
                }
                else alert("ASPNET Error: " + msg);
            }
        </script>

    </form>
</body>
</html>
