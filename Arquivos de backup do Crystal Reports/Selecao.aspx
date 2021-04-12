<%@ Page Title="" Language="C#" MasterPageFile="~/MasterRelacional.Master" AutoEventWireup="true" CodeBehind="Selecao.aspx.cs" Inherits="HabilInformatica.Selecao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cttCorpo" runat="server">
    <br/>    
    <style type="text/css">
        .centerHeaderText{
            text-align: center!important;
        }
        	
        .noprint{display:none;}
    </style>
    <link type="text/css" href="Content/EstiloDefinidoPorParametro.css" rel="stylesheet" />
    <div class="container-fluid" style="background-color:white;border:none;text-align:center;">
        <div class="row CorPadraoFundo" >
            <div class="col-md-3 col-xs-1 " ></div>
            <div class="col-md-6 col-xs-10"   style="background-color:white;border:none;text-align:center;padding:0!important;border-radius:4px">
                <div class="panel panel-default"  >
<%--                    <div class="panel-heading">
                        <asp:Label ID="Label1" Text=":::... Módulos Liberados ...:::" runat="server"></asp:Label>
                    </div>--%>
                    <div class="panel-footing" style="text-align:center; " >
                         <%--table table-bordered table-striped--%>
                        <asp:GridView ID="grdConsulta" runat="server" AutoPostBack=true  CssClass="table table-hover table-striped" 
                            GridLines="None" AutoGenerateColumns="False" Font-Size="10pt"  Font-Bold ="true" Font-Names="Verdana"
                            OnSelectedIndexChanged="grdConsulta_SelectedIndexChanged"  OnRowDataBound ="grdConsulta_RowDataBound">
                            <Columns>
                                    <asp:CommandField HeaderText="Entrar" ShowSelectButton="True" 
                                        ItemStyle-Height ="15px" ItemStyle-Width ="50px" ButtonType="Image"  SelectImageUrl ="~/Images/Acessar.svg" 
                                        ControlStyle-Width ="20px" ControlStyle-Height ="20px" HeaderStyle-Width ="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="centerHeaderText" />

                                <asp:BoundField DataField="DescricaoModulo" HeaderText="Gestão/Módulo" HeaderStyle-Width ="40%" />
                                <asp:BoundField DataField="CodigoModulo" HeaderText="ID" HeaderStyle-Width ="5%" HeaderStyle-HorizontalAlign ="Right" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"/>
                            </Columns>
                            <RowStyle CssClass="cursor-pointer" />

                        </asp:GridView>                   
                    </div>
                </div>
                <br />
                <asp:Label ID="Label2" Text="Veja quais são os módulos liberados para sua Empresa." runat="server"></asp:Label>

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cttRodape" runat="server">
</asp:Content>
