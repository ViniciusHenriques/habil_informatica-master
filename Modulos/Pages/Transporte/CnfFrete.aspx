 <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="CnfFrete.aspx.cs" Inherits="SoftHabilInformatica.Pages.Transporte.CnfFrete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <link type="text/css" href="../../Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jsMensagemAlert.js"></script>
    <script>
        function browseResult2(e) {
             var verificadorArquivosIncosistentes = false;
            for (let i = 0; i < arquivos.length; i++) {
                var arquivo = arquivos[i];
                if (arquivo.size == 0) {
                   verificadorArquivosIncosistentes  = true;
                   break;                        
                }
            }
            return verificadorArquivosIncosistentes
        }
        function browseResult(e)
        {
            try
            {
                var fi = document.getElementById('<%= fileselector.ClientID %>');

                var maxFileSize = 10480760 // 4MB -> 4 * 1024 * 1024

                if (fi.files.length > 0) {

                    var fsize = 0;
                    for (var i = 0; i < fi.files.length; i++) {

                        fsize += fi.files.item(i).size;

                    }
                    if (fsize > maxFileSize) {

                        alert("Tamanho total dos arquivos deve ser no máximo 10MB");

                        fi.value = null;
                    }
                }
            }
            catch (e)
            {
                alert(e);
            }
        }

     
    </script>
    <body>
        <div id="divNavTeste" style="padding-left:20px;padding-top:20px;padding-right:20px" >
            <div class="panel panel-primary" >
                <div  class="panel-heading">Importação de XML para Excel
                    <div class="messagealert" id="alert_container"></div>
                </div>            
                <div class="panel-body">
                    <div class="row">   
                        <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small; padding-top:2px" >
                            <asp:LinkButton runat="server" id="btnSelecionarPasta"  CssClass="btn btn-default" ToolTip="Escolha um diretório para buscar os arquivos">
                                <asp:FileUpload runat="server" id="fileselector" type="file" onchange="browseResult(event)" webkitdirectory directory multiple="false" style="margin:0"  />
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnAdd" runat="server"  CssClass="btn btn-success" TabIndex="0" AutoPostBack="true" OnClick="btnAdd_Click" height="34" ToolTip="Adicionar arquivos selecionados"> 
                                <span aria-hidden="true" class="glyphicon glyphicon-plus"></span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="BtnLerXML" runat="server" CssClass="btn btn-success" TabIndex="0" OnClick="BtnLerXML_Click" AutoPostBack="true" title="Transforme seus XMLs em uma Planilha excel"> 
                                <span aria-hidden="true" class="glyphicon glyphicon-download-alt"></span>  Gerar planilha
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnLimpar" runat="server" CssClass="btn btn-default" TabIndex="0" OnClick="btnLimpar_Click" AutoPostBack="true"> 
                                <span aria-hidden="true" class="glyphicon glyphicon-remove-circle"></span>  Limpar
                            </asp:LinkButton>     
                        </div>
                        <div class="col-md-12" style="background-color:white;border:none;text-align:left;font-size:x-small; padding-top:5px" >
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:GridView ID="grdXMLs" runat="server"
                                            CssClass="table  table-hover table-striped"
                                            GridLines="None" OnSelectedIndexChanged="grdXMLs_SelectedIndexChanged" 
                                            Font-Size="8pt"  >
                                        <PagerStyle HorizontalAlign = "left"   CssClass = "GridPager" />
                                        <Columns>   
                                            <asp:CommandField HeaderText="Remover" ShowSelectButton="True" 
                                                ButtonType="Image"  SelectImageUrl ="~/Images/excluir.png" 
                                                ControlStyle-Width ="20px" ControlStyle-Height ="20px" />
                                        </Columns>
                                        <RowStyle CssClass="cursor-pointer" />
                                    </asp:GridView>
                                </ContentTemplate> 
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </body>
</asp:Content>
