using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using d3d = Microsoft.DirectX.Direct3D;
using dkey = Microsoft.DirectX.DirectInput;
namespace font1
{
    class myfont : Form
    {
       Font fontumuz;
        d3d.Font font_;
        d3d.Device device = null;
       dkey.Device klavye = null;
       d3d.Mesh mesh_yaz = null;
       d3d.Material mesh_yaz_Material;
        public myfont()
        {
            this.ClientSize = new Size(400, 400);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
                     
            Render();
            e.Graphics.DrawEllipse(Pens.LightBlue, 50, 50, 100, 100);
            Invalidate();
        }

        
        static void Main()
        {
            using (myfont frm = new myfont())
            {
                frm.Show();
                frm.Init();
                Application.Run(frm);
            }
        }
        private void Init()
        {
            this.Text = "Metin Yazdırma";

            d3d.PresentParameters param = new d3d.PresentParameters();
            param.SwapEffect = d3d.SwapEffect.Discard;
            param.Windowed = true;
            param.EnableAutoDepthStencil = true;
            param.AutoDepthStencilFormat = d3d.DepthFormat.D16;

            device = new d3d.Device(0, d3d.DeviceType.Hardware, this,Microsoft.DirectX.Direct3D.CreateFlags.SoftwareVertexProcessing, param);
            device.DeviceReset += new System.EventHandler(this.OnResetDevice);
 
            OnResetDevice(device, null);

            klavye = new dkey.Device(dkey.SystemGuid.Keyboard);
            klavye.Acquire();
         
             fontumuz = new Font("Arial", 18.0f, FontStyle.Italic);
            mesh_yaz = d3d.Mesh.TextFromFont(device, fontumuz, "OKAN(3D)",
                0.001f, 0.4f);

        }
     
 
        public void OnResetDevice(object sender, EventArgs e)
        {
            device = (d3d.Device)sender;
device.Transform.Projection=Matrix.PerspectiveFovLH((float)Math.PI /5 , 1.0f,1.0f, 500.0f);

 device.Lights[0].Type = d3d.LightType.Directional;
 device.Lights[0].Diffuse = Color.Yellow;
 device.Lights[0].Direction = new Vector3(0, 0, 3);
 device.Lights[0].Enabled = true;
}
     private void Render()
        {
            device.Clear(d3d.ClearFlags.ZBuffer | d3d.ClearFlags.Target,
                             Color.FromArgb(0, 0, 129, 179), 1.0f, 0);

         device.BeginScene();
         device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, -10.0f),
  new Vector3(2,0,2), new Vector3(0, 1, 0));
           klavyemiz();
           //***************Font 2D
           font_ = new d3d.Font(device, fontumuz);
           font_.DrawText(null, "Merhaba (2D)",new Rectangle(0, 0,
           400, 400),
           d3d.DrawTextFormat.None , Color.GreenYellow);
           //***************3D 
            mesh_yaz_Material = new Microsoft.DirectX.Direct3D.Material();
            mesh_yaz_Material.Diffuse = Color.Red;
            device.Material = mesh_yaz_Material;
            mesh_yaz.DrawSubset(0);
            //***************Font
   
          device.EndScene();
          device.Present();
        }
         void klavyemiz() { 
            dkey.KeyboardState keys = klavye.GetCurrentKeyboardState();
            
              if (keys[dkey.Key.P]) {
                 this.Text = "font***";
                 fontumuz = new System.Drawing.Font("Impact", 14.0f, FontStyle.Italic);     
              }
            }
        
 
   
                                     


    
    }
}
