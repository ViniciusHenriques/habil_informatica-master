    <%@ Page Title="" Language="C#" MasterPageFile="~/MasterRelacional.Master" AutoEventWireup="true" CodeBehind="CadPessoa_Contato.aspx.cs" Inherits="SoftHabilInformatica.Pages.Pessoas.CadPessoa_Contato" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="~/Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jsCep.js" type="text/javascript"></script>
    <script src="../../Scripts/funcoes.js" type="text/javascript"></script>
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <link type="text/css" href="../../Content/Style.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js" ></script>

    <script type="text/javascript">
       
        $(document).ready(function () {
            $('.js-example-basic-single').select2({});

        });

        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_endRequest(OnEndRequest);
        function OnEndRequest(sender, args) {
            $('.js-example-basic-single').select2({});  

        }
       function VerificaTamanhoArquivo() {

            var fi = document.getElementById('<%= arquivo.ClientID %>'); 
            var maxFileSize = 10480760 // 4MB -> 4 * 1024 * 1024
                               
            if (fi.files.length > 0) {
                for (var i = 0; i <= fi.files.length - 1; i++) {
                
                    var fsize = fi.files.item(i).size;

                    if (fsize > maxFileSize) {

                        alert("Tamanho do Arquivo deve ser no maximo 10MB");

                        fi.value = null;
                    }
                    else {
                        $get('<%=btnEscolherFoto.ClientID%>').click(); 
                    }
                }                
           }
        }
    </script>
    <style>

        @media screen and (max-width: 1000px) {
            .ClassFotoContato{
                height:220px!important;
				width:100%!important;
				
            }
            .FotoContato{
                width:50%!important;
            }			
        }
    </style>
    <div id="divNavTeste" style="padding-left:30px;padding-top:30px;padding-right:30px;font-size:small;" >
        <div class="panel panel-primary">
            <div class="panel-heading">Cadastro de Pessoas</div>

            <div class="panel-body"  style="padding-left:30px;padding-top:30px;padding-right:30px;font-size:small;">

                <div class="container-fluid">
                    <div class="row" style="background-color:white;border:none;">
                        <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                            <label for="usr" style ="margin-top:1px;">Pessoa (<asp:Label ID="txtCodPessoa" runat="server"/>)</label>
                            <asp:Label ID="txtRazSocial" runat="server" CssClass="form-control" />
                        </div>
                    </div>                            
                </div>

                <div class="panel panel-default">
                    <div class="panel-heading">Contato da Pessoa
                        <div class="messagealert" id="alert_container"></div>
                        <asp:Panel ID="pnlMensagem" Visible ="false" style="margin-left:10px;margin-right:10px;" runat="server" >
                            <div id="divmensa" style ="background-color:red; border-radius: 15px;">
                                <asp:Label ID="lblMensagem" runat="server" ForeColor ="Black"   ></asp:Label>
                                <asp:Button ID="BtnOkExcluir" runat="server" Text="X" CssClass="btn btn-danger" OnClick ="BtnOkExcluir_Click" />
                            </div> 
                        </asp:Panel>
                    </div>
        
                    <div class="panel-body">
                        <div class="container-fluid">

                            <div class="col-md-9" style="padding:0!important;">
                                <div class="col-md-8" style="margin-bottom:15px">
                                    <asp:LinkButton ID="btnCanContato" runat="server" Text="Excluir" CssClass="btn btn-default" UseSubmitBehavior="false" Visible="true" OnClick="btnCanContato_Click"> 
										<span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-default"></span>  Cancelar
                                    </asp:LinkButton>
                                
                                    <asp:LinkButton ID="btnSlvContato" runat="server" Text="Salvar" CssClass="btn btn-success" UseSubmitBehavior="false" Visible="true" OnClick="btnSlvContato_Click"> 
										<span aria-hidden="true" title="Salvar" class="glyphicon glyphicon-save"></span>  Salvar
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnExcContato" runat="server" Text="Excluir" CssClass="btn btn-danger" UseSubmitBehavior="false" Visible="false" OnClick="btnExcContato_Click"> 
										<span aria-hidden="true" title="Excluir" class="glyphicon glyphicon-remove"></span>  Excluir
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnTiraFoto" runat="server" Text="Tirar Foto" CssClass="btn btn-success" data-toggle="modal" data-target="#modalWebCam" > 
										<span aria-hidden="true" title="Tirar Foto" class="glyphicon glyphicon-camera"></span> Tirar Foto
                                    </asp:LinkButton>
									
                                    <asp:LinkButton ID="btnEscolherFoto" runat="server" Text="Escolher Foto" CssClass="btn btn-success" onclick="btnEscolherFoto_Click" Style="display:none" AutoPostBack="true"> 
										<span aria-hidden="true" title="Escolher Foto" class="glyphicon glyphicon-refresh"></span>
                                    </asp:LinkButton>

                                </div>
                                <div class="col-md-4" style="margin-bottom:15px">
                                    <asp:FileUpload ID="arquivo" runat="server"  CssClass="form-control" Text=""  onchange="Javascript: VerificaTamanhoArquivo();" />
                                </div>

                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-2" style="font-size:x-small;margin:0!important">
                                            <label for="usr" style ="margin-top:1px;">Item</label>
                                            <asp:TextBox ID="txtCttItem" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>                            
                                        <div class="col-md-5" style="font-size:x-small;margin:0!important">
                                            <label for="usr" style ="margin-top:1px;">Tipo de Contato</label>
                                            <asp:DropDownList ID="ddlTipoContato" runat="server" AutoPostBack="true" CssClass="form-control" >
                                        
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-5" style="font-size:x-small;margin:0!important">
                                            <label for="usr" style ="margin-top:1px;">Inscrição da Pessoa</label>
                                            <asp:DropDownList ID="DdlTipoInscricao" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="DdlTipoInscricao_SelectedIndexChanged" Width="100%"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-12" style="font-size:x-small;margin:0!important">
                                            <label for="usr" style ="margin-top:1px;">Nome do Contato</label>
                                            <asp:TextBox ID="txtNomeContato" runat="server" CssClass="form-control" Width="100%" TabIndex="1" MaxLength="100"/>
                                        </div>
                                        <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">                                     
                                            <label for="usr" style ="margin-top:1px;">País</label>
                                            <asp:DropDownList ID="ddlPaises" runat="server" CssClass="form-control js-example-basic-single" OnSelectedIndexChanged="ddlPaises_SelectedIndexChanged" AutoPostBack="true" Width="100%"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-8" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                            <label for="usr" style ="margin-top:1px;">Fone Principal</label>
                                            <div class="input-group " >
                                                <div class="input-group-addon" style="padding-right:20px">
                                                    <img src="../../Images/WhatsApp.png" width="20" height="20" title="Marcar se o Telefone for WhatsApp" style="float:left;margin-right:5px" />
                                                    <div style="margin-top:4px;"><asp:CheckBox runat="server" ID="chkWhatsApp"  /></div>
                                                </div>
                                                <span class="input-group-addon"><asp:label runat="server" ID="lblFone1"></asp:label></span>                                      
                                                    <asp:TextBox ID="txtfone1"
                                                        name="txtfone1"
                                                        CssClass="form-control"
                                                        runat="server"
                                                        TabIndex="2"  MaxLength="20"
                                                        OnTextChanged="txtfone1_TextChanged" PlaceHolder="(DD) 99999-9999" AutoPostBack="true" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="DdlTipoInscricao" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="col-md-3 " >
										<div class="ClassFotoContato" style="width:100%!important">

                                            <asp:LinkButton runat="server" ID="btnRemoverFoto" Style="color:white;padding:5px; font-size:30px;background-color:rgba(0,0,0,0.5);position: absolute;" OnClick="btnRemoverFoto_Click">
                                                <span class="glyphicon glyphicon-trash"></span>
                                            </asp:LinkButton>

                                            <asp:Image ID="imaFoto" ImageUrl=""  runat="server" BorderStyle ="Solid"  BorderWidth ="2" ImageAlign ="Right" Width="100%" Height="220px" CssClass="FotoContato" />
                                        </div>
                                        
                                    </div>
                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Fone Secundário</label>
                                    
                                        <div class="input-group ">
                                            <span class="input-group-addon"><asp:label runat="server" ID="lblFone2"></asp:label></span>
                                            <asp:TextBox ID="txtfone2"
                                                name="txtfone1"
                                                CssClass="form-control"
                                                runat="server"
                                                TabIndex="3" MaxLength="20"
                                                OnTextChanged="txtfone2_TextChanged" PlaceHolder="(DD) 99999-9999" AutoPostBack="true"/>
                                        </div>
                                    </div>                               

                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Fone Recado</label>
                                        <div class="input-group ">
                                            <span class="input-group-addon"><asp:label runat="server" ID="lblFone3"></asp:label></span>
                                            <asp:TextBox ID="txtfone3"
                                                name="txtfone1"
                                                CssClass="form-control"
                                                runat="server"
                                                TabIndex="4"  MaxLength="20"
                                                OnTextChanged="txtfone3_TextChanged" PlaceHolder="(DD) 99999-9999" AutoPostBack="true"/>
                                        </div>
                                    </div>
                                
                                    <div class="col-md-8" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">E-Mail Principal</label>
                                        <asp:TextBox ID="txtmail1"
                                            name="txtmailnfe"
                                            CssClass="form-control"
                                            runat="server"
                                            MaxLength="100" TabIndex="5" 
                                            OnTextChanged="txtmail1_TextChanged" />
                                    </div>
                                    <div class="col-md-4" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">Senha Email</label>
                                        <asp:TextBox ID="txtMailSenha"
                                            name="txtMailSenha"
                                            CssClass="form-control"
                                            runat="server"
                                            MaxLength="100" TabIndex="6" 
                                            TextMode="Password" />
                                    </div>
                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">E-Mail NF-e</label>
                                        <asp:TextBox ID="txtmailnfe"
                                            name="txtmailnfe"
                                            CssClass="form-control"
                                            runat="server"
                                            MaxLength="100" TabIndex="7" 
                                            OnTextChanged="txtmailnfe_TextChanged" />
                                    </div>
                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">E-Mail NFS-e</label>
                                        <asp:TextBox ID="txtmailNFSe"
                                            name="txtmailnfe"
                                            CssClass="form-control"
                                            runat="server" TabIndex="8" 
                                            MaxLength="100"
                                            OnTextChanged="txtmailNFSe_TextChanged" />
                                    </div>

                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">E-Mail Secundário</label>
                                        <asp:TextBox ID="txtmail2"
                                            name="txtmailnfe"
                                            CssClass="form-control"
                                            runat="server" TabIndex="9" 
                                            MaxLength="100"
                                            OnTextChanged="txtmail2_TextChanged" />
                                    </div>
                                    <div class="col-md-6" style="background-color:white;border:none;text-align:left;font-size:x-small;">
                                        <label for="usr" style ="margin-top:1px;">E-Mail Recado</label>
                                        <asp:TextBox ID="txtmail3"
                                            name="txtmailnfe" TabIndex="10" 
                                            CssClass="form-control"
                                            runat="server"
                                            MaxLength="100"
                                            OnTextChanged="txtmail3_TextChanged" />
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlPaises" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="txtfone1" EventName="TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="txtfone2" EventName="TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="txtfone3" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
 
    <div class="modal fade" id="modalWebCam" tabindex="-1" role="dialog" aria-labelledby="myLargeModal" >
        <div class="modal-dialog" role="document" style="width:60%!important;margin-top:10px!important">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Tire sua foto</h4>
                </div>
                <div class="modal-body" style="text-align:center">
                     
                    <video id="vid" autoplay muted playsinline></video>
                    <asp:textbox runat="server"  type="text" id="base_img" name="base_img" Style="display:none"></asp:textbox>

                    <p id="caminhoImagem" class="caminho-imagem"><a href="" target="_blank"></a></p>
                        <div style="margin-top:15px">
                            <asp:Linkbutton class="btn btn-default" runat="server" onclick="btnRefresh_Click" ID="btnRefresh" style="height:35px;font-size:18px;display:none">
                                <span aria-hidden="true" title="Tirar Foto" class="glyphicon glyphicon-refresh"></span>
                            </asp:Linkbutton>
                            
                            <input type="button" class="btn btn-success"  value="Capturar" onclick="snap();" />

                        </div>

                        <script type="text/javascript">

                            //navigator.mediaDevices.getUserMedia({
                            //    video: { width: '100%', height: 700 }

                            //}).then(stream => {
                            //    document.getElementById("vid").srcObject = stream;
                            //});
                            function errorMessage(message, e) {
                                ShowMessage(message, 'Info');
                                $get('<%=btnTiraFoto.ClientID%>').style.display = 'none';
                                //alert(message);
                            }
                            //environment ou user
                            //if (location.protocol === 'https:') {
                                navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;
                                if (navigator.getUserMedia) {
                                    navigator.getUserMedia({
                                        video: true,
                                            
                                    }, function (stream) {
                                        document.getElementById("vid").srcObject = stream;
                                        var mediaStreamTrack = stream.getVideoTracks()[0];
                                        if (typeof mediaStreamTrack != "undefined") {
                                            mediaStreamTrack.onended = function () {//for Chrome.
                                                errorMessage('Sua webcam está ocupada')
                                            }
                                        } else errorMessage('Permissão negada!');
                                    }, function (e) {
                                        var message;
                                        switch (e.name) {
                                            case 'NotFoundError':
                                            case 'DevicesNotFoundError':
                                                message = 'Por favor, configure sua webcam primeiro.';
                                                break;
                                            case 'SourceUnavailableError':
                                                message = 'Sua webcam está ocupada';
                                                break;
                                            case 'PermissionDeniedError':
                                            case 'SecurityError':
                                                message = 'Permissão negada!';
                                                break;
                                            default: errorMessage('Acesso a Webcam rejeitado!!', e);
                                                return;
                                        }
                                        errorMessage(message);
                                    });
                                } else errorMessage('Navegador incompatível!');
                                
                            //} else errorMessage('Use o protocolo https para abrir esta página.')
                            

                            function snap() {
                                var video = document.querySelector("#vid");
	
	                            //Criando um canvas que vai guardar a imagem temporariamente
	                            var canvas = document.createElement('canvas');
	                            canvas.width = video.videoWidth;
	                            canvas.height = video.videoHeight;
	                            var ctx = canvas.getContext('2d');
	
	                            //Desenhando e convertendo as dimensões
	                            ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
	
	                            //Criando o JPG
                                var dataURI = canvas.toDataURL('image/jpeg'); //O resultado é um BASE64 de uma imagem.
                                //sendSnapShot(dataURI); //

                                document.getElementById("<%=base_img.ClientID%>").value = dataURI;
                                $get('<%=imaFoto.ClientID%>').src = dataURI;
                                $get('<%=btnRefresh.ClientID%>').click(); 
                                
                            }
                        </script>

                </div>

            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cttRodape" runat="server">
</asp:Content>

