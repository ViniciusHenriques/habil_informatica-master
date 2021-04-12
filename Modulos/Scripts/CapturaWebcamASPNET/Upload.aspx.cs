using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace CapturaWebcamASPNET
{
    public partial class Upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string Dir = Server.MapPath("uploads");
                    string FileName = String.Format("{0:ddMMyyyyhhmmss}.jpg", DateTime.Now);

                    string CompleteFileName = String.Format("{0}\\{1}", Dir, FileName);

                    using (FileStream fs = new FileStream(CompleteFileName, FileMode.Create))
                    {
                        byte[] buffer = new byte[Request.InputStream.Length];
                        Request.InputStream.Read(buffer, 0, (int)Request.InputStream.Length);
                        fs.Write(buffer, 0, buffer.Length);
                    }

                    string URLSaida = String.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.GetComponents(UriComponents.HostAndPort, UriFormat.Unescaped), "/uploads/", FileName);
                    Response.Clear();
                    Response.Write(URLSaida);
                }
                catch
                {
                    Response.Clear();
                    Response.Write("ERROR: Erro ao salvar imagem\n");
                }
                Response.End();
            }
        }
    }
}