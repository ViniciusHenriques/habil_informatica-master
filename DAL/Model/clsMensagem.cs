using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class clsMensagem 
    {
    //    public string ExibeMensagem(string strMensagem)
    //    {
    ///         return("<div id=\"divAlertBootstrap\" style=\"width: 100%; position:absolute; botton: 0; text-align:center; \" class=\"alert alert-info\">" +
    //            strMensagem + "</div>" +
   //             "<script type=\"text/javascript\">window.onload=function(){alertBootstrap();};</script>");
   //     }

     //   public string ExibeMensagem(string strMensagem)
     //   {
     //       return ("<div id='divAlertBootstrap' style='width: 100%; position:relative; botton: 0; left: 0; margin-left: 0; text-align:center; ' class='alert alert-info'>" +
  //             strMensagem + "</div>" +
   //           "<script type='text/javascript'>window.onload=function(){alertBootstrap();};</script>");
      //  }
///
      public string ExibeMensagem(string strMensagem)
        {
            return ("<div class='alert alert-info' style='padding: 10px; margin: 0 auto;  width: 600px; position:absolute; left: 30%; margin-left: -100px; text-align:center;'>" +
                     " <a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>" +
                     " <strong>Informação!</strong> " + strMensagem + 
                    " </div>");
        }

      public string ExibeMensagemSucesso(string strMensagem)
      {
          return ("<div class='alert alert-success' style='padding: 10px; margin: 0 auto;  width: 600px; position:absolute; left: 30%; margin-left: -100px; text-align:center;'>" +
                    " <a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>" +
                    " <strong>Sucesso!</strong> " + strMensagem +
                   " </div>");
      }
      public string ExibeMensagemCuidado(string strMensagem)
      {
          return ("<div class='alert alert-warning' style='padding: 10px; margin: 0 auto;  width: 600px; position:absolute; left: 30%; margin-left: -100px; text-align:center;'>" +
                    " <a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>" +
                    " <strong>Cuidado!</strong> " + strMensagem +
                   " </div>");
      }
      public string ExibeMensagemErro(string strMensagem)
      {
          return ("<div class='alert alert-danger' style='padding: 10px; margin: 0 auto;  width: 600px; position:absolute; left: 30%; margin-left: -100px; text-align:center;'>" +
                    " <a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>" +
                    " <strong>Erro!</strong> " + strMensagem +
                   " </div>");
      }
    }
}
