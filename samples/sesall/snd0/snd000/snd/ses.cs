using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using d3d = Microsoft.DirectX.Direct3D;
using dkey = Microsoft.DirectX.DirectInput;
using dses=Microsoft.DirectX.DirectSound;
namespace snd
{

    public class ses : System.Windows.Forms.Form
    {
        dses.Device device_ses;
        dses.Buffer buf;        
        d3d.Device device = null;
        dkey.Device klavye = null;
        d3d.Mesh nesne;
        public ses()
        {
            this.Text = "Press P or Z";
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
    

            d3d.PresentParameters param = new d3d.PresentParameters();
            param.SwapEffect = d3d.SwapEffect.Discard;
            param.Windowed = true;
            param.EnableAutoDepthStencil = true;
            param.AutoDepthStencilFormat = d3d.DepthFormat.D16;

            device = new d3d.Device(0, d3d.DeviceType.Hardware, this, Microsoft.DirectX.Direct3D.CreateFlags.SoftwareVertexProcessing, param);
            device.DeviceReset += new System.EventHandler(this.OnResetDevice);

            OnResetDevice(device, null);

            klavye = new dkey.Device(dkey.SystemGuid.Keyboard);
            klavye.Acquire();//Klavye erisimi saglansin   


         
          

        }


        public void OnResetDevice(object sender, EventArgs e)
        {
            device = (d3d.Device)sender;
            device.Transform.Projection =
            Matrix.PerspectiveFovLH((float)Math.PI / 4, 1,  0.1f, 1800.0f);
            device.RenderState.Lighting = false;
            nesne = d3d.Mesh.TextFromFont(device, new Font("Arial", 20), "Sound", 1, 1);
            
            
            device_ses = new dses.Device();
            device_ses.SetCooperativeLevel(this, dses.CooperativeLevel.Normal);
            buf = new dses.Buffer("c.wav", device_ses);  

        }




        private void Render()
        {
            device.Clear(d3d.ClearFlags.ZBuffer | d3d.ClearFlags.Target,
                             Color.FromArgb(50, 90, 150), 1.0f, 0);


            device.BeginScene();

            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI /4,
                  800 / 600, 2.5f, 200.0f);
            device.Transform.World = Matrix.Identity;
            device.Transform.View = Matrix.LookAtLH(new Vector3(0.5f, 0, -20), new Vector3(0, 0, 0),
                     new Vector3(0, 0, 1));

            klavyemiz();
            nesne.DrawSubset(0);
            device.EndScene();
            device.Present();
        }

       void klavyemiz()
        {
            dkey.KeyboardState keys = klavye.GetCurrentKeyboardState();
            if (keys[dkey.Key.P])
            {
                device_ses.SetCooperativeLevel(this, dses.CooperativeLevel.WritePrimary);
            }
            if (keys[dkey.Key.Z])
            {
                device_ses.SetCooperativeLevel(this, dses.CooperativeLevel.Normal);
                buf.Restore();
              
               sescal();
            }
        }
      void sescal()
        {
             try{
                buf.Play(0, dses.BufferPlayFlags.Default);//
             }
             catch (Exception eee){
               Console.WriteLine(eee.Message);
             }
        }
           



 
    }
}
