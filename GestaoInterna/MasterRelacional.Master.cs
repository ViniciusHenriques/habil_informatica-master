﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DAL.Persistence;
using DAL.Model;

namespace GestaoInterna
{
    public partial class MasterRelacional : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if ((Session["UsuSis"] == null ) || (Session["CodModulo"] == null)) 
            {
                Response.Redirect("~/Default.aspx");
                return;
            }
            else
            {
                if (!IsPostBack)
                {
                    pnlAuttenticado.Visible = true;
                    cttCorpo.Visible = true;
                    lblUsuario.Text = Session["UsuSis"].ToString();
                    lblModulo.Text = "Você está Logado no Módulo: " + Session["DesModulo"].ToString();
                    if (Session["NomeEmpresa"] != null)
                        lblEmpresa.Text = "Empresa: " + Session["CodEmpresa"].ToString() + " - " + Session["NomeEmpresa"].ToString();
                }
            }

        }
    }
}