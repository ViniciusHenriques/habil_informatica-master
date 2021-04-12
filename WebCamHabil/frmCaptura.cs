using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.DirectShow;


namespace WebCamHabil
{
    public partial class frmCaptura : Form
    {
        private VideoCaptureDevice videoSource;
        public frmCaptura()
        {
            InitializeComponent();

            var videoSources = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoSources != null && videoSources.Count > 0 )
            {
                videoSource = new VideoCaptureDevice(videoSources[0].MonikerString);
                videoSource.NewFrame += videoSource_NewFrame;
            }

            txtCaminho.Text = ConfigurationManager.AppSettings["ArquivoDaCapturaFisica"].ToString();
        }

        private void videoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            if (pbCamera.Image != null)
            {
                pbCamera.Image.Dispose();
            }

            pbCamera.Image = (Bitmap)eventArgs.Frame.Clone();

        }

        

        private void frmCaptura_Load(object sender, EventArgs e)
        {

        }

        private void btnIniciarDesligar_Click(object sender, EventArgs e)
        {
            if (videoSource.IsRunning)
            {
                videoSource.Stop();
                pbCamera.Image = null; 
                btnIniciarDesligar.Text = "Desligar";
            }
            else
            {
                videoSource.Start();
                btnIniciarDesligar.Text = "Ligar";
            }
        }

        private void btnCapturar_Click(object sender, EventArgs e)
        {
            if (pbCamera.Image!= null)
            {
                try
                {
                    videoSource.NewFrame -= videoSource_NewFrame;
                    pbCamera.Image.Save(ConfigurationManager.AppSettings["ArquivoDaCapturaFisica"].ToString(), System.Drawing.Imaging.ImageFormat.Png);
                }
                finally 
                {
                    videoSource.NewFrame += videoSource_NewFrame;

                    if (videoSource.IsRunning)
                    {
                        videoSource.Stop();
                    }

                    Dispose();
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (videoSource.IsRunning)
            {
                videoSource.Stop();
            }
            base.OnFormClosing(e);
        }

        private void frmCaptura_Activated(object sender, EventArgs e)
        {
            //btnIniciarDesligar_Click(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //btnCapturar_Click(sender, e);
        }
    }
}
