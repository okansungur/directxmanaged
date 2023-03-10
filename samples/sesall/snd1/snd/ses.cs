using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using d3d=Microsoft.DirectX.Direct3D;
using dkey=Microsoft.DirectX.DirectInput;
using Microsoft.DirectX.DirectSound;
namespace snd
{
  
        public class ses : System.Windows.Forms.Form
    {
       Microsoft.DirectX.DirectSound.Device device_ses = null;
      SecondaryBuffer sound = null;
       d3d.Device device = null;
       dkey.Device klavye = null;
       d3d.Mesh nesne;
        public ses()
        {
            this.ClientSize = new Size(640, 480);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            
            
            Render();
            Invalidate();
        }

        
        static void Main()
        {
            using (ses frm = new ses())
            {
                frm.Show();
                frm.Init();
                Application.Run(frm);
            }
        }
        private void Init()
        {
            this.Text = "Press \"P\"";

            d3d.PresentParameters param = new d3d.PresentParameters();
            param.SwapEffect = d3d.SwapEffect.Discard;
            param.Windowed = true;
            param.EnableAutoDepthStencil = true;
            param.AutoDepthStencilFormat = d3d.DepthFormat.D16;

            device = new d3d.Device(0, d3d.DeviceType.Hardware, this,Microsoft.DirectX.Direct3D.CreateFlags.SoftwareVertexProcessing, param);
            device.DeviceReset += new System.EventHandler(this.OnResetDevice);

            OnResetDevice(device, null);

            klavye = new dkey.Device(dkey.SystemGuid.Keyboard);
            klavye.Acquire();//Klavye erişimi sağlansın   

        }

       
        public void OnResetDevice(object sender, EventArgs e)
        {
            device = (d3d.Device)sender;
            device.Transform.Projection =
             Matrix.PerspectiveFovLH((float)Math.PI / 4, (float)this.ClientSize.Width / this.ClientSize.Height,
                 0.1f, 800.0f);
            device.RenderState.Lighting = false;
            try
            {


                nesne = d3d.Mesh.Torus(device, 2.0f, 4.0f, 5, 3);
            
            }
            catch { }
            
           
            
        }

  
        

        private void Render()
        {
            device.Clear(d3d.ClearFlags.ZBuffer | d3d.ClearFlags.Target,
                             Color.FromArgb(0, 200, 129, 179), 1.0f, 0);

          
            device.BeginScene();
            
             device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 2,
                   800 / 600, 0.5f, 100.0f);
             device.Transform.World = Matrix.Identity;
             device.Transform.View = Matrix.LookAtLH(new Vector3(5.0f, 0, 0), new Vector3(0, 0, 0),
                      new Vector3(0, 0, 1));

             klavyemiz();
             nesne.DrawSubset(0);
            device.EndScene();
            device.Present();
        }


            void klavyemiz() { 
            dkey.KeyboardState keys = klavye.GetCurrentKeyboardState();
            //klavyeden basilan tuslari  tuslar "keys" degiskenine ataniyor
              if (keys[dkey.Key.P]) {
                 this.Text = "ses geldimi?";
                 sesler();
              }
            }
        
            void sesler() {
                device_ses = new Microsoft.DirectX.DirectSound.Device();
                device_ses.SetCooperativeLevel(this, CooperativeLevel.Normal);
                sound = new SecondaryBuffer("notify.wav", device_ses);
                sound.Play(0, BufferPlayFlags.Default);//
                System.Threading.Thread.Sleep(2000);//2sn sonra durdurulsun
                sound.Stop();
            }



          private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // sinif
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "ses";
            this.Text = "Press \"P\"";

            this.ResumeLayout(false);

        }
    }
}
