using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using D3D = Microsoft.DirectX.Direct3D;

namespace fxler
{
    public class myfx : Form
    {
      
        D3D.Device device;
        Mesh mes;
        Effect efekt;
        Matrix view;
        Matrix projeksiyon;

        

        public myfx()
        {
            this.Size = new System.Drawing.Size(400, 400);
            this.Text = "Effect File (Vertex-Shader)";
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

        public void grafik_algila()
        {
            PresentParameters parametre = new PresentParameters();
            parametre.Windowed = true;
            parametre.SwapEffect = SwapEffect.Discard;
            parametre.AutoDepthStencilFormat = DepthFormat.D16;
            parametre.EnableAutoDepthStencil = true;

            Caps caps = D3D.Manager.GetDeviceCaps(0, D3D.DeviceType.Hardware);
            D3D.DeviceType type = D3D.DeviceType.Reference;
            CreateFlags flag = CreateFlags.SoftwareVertexProcessing;
            if ((caps.VertexShaderVersion >= new Version(2, 0)) && (caps.PixelShaderVersion >= new Version(2, 0)))
            {
                type = D3D.DeviceType.Hardware;
                if (caps.DeviceCaps.SupportsHardwareTransformAndLight)
                {
                    flag = CreateFlags.HardwareVertexProcessing;
                    if (caps.DeviceCaps.SupportsPureDevice)
                    {
                        flag |= CreateFlags.PureDevice;
                    }
                }
            }

            device = new D3D.Device(0, type, this, flag, parametre);
            efekt = D3D.Effect.FromFile(device, "b.fx", null, ShaderFlags.None, null);
            mes = Mesh.Teapot(device);

            projeksiyon = Matrix.PerspectiveFovLH((float)Math.PI / 6, 1, 1f, 50f);
            view = Matrix.LookAtLH(new Vector3(4, 6, 5), new Vector3(0, -1, 0), new Vector3(0, 1, 1));
            device.Transform.Projection = projeksiyon;
            device.Transform.View = view;
            //efekt dosyasına parametre olarak gonderilecek

        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.FloralWhite, 1.0f, 0);
            device.BeginScene();
              
            efekt.Technique = "teknik";
            efekt.SetValue("ilkfx", view * projeksiyon);
            int gecis_sayisi = efekt.Begin(0);
           
            for (int i = 0; i < gecis_sayisi; i++)
            {
                efekt.BeginPass(i);
                mes.DrawSubset(0);
                efekt.EndPass();
            }
            efekt.End();
                    
            device.EndScene();
            device.Present();
            this.Invalidate();
        }
        
            
        protected override void Dispose(bool disposing)
        {
         
            base.Dispose(disposing);
        }
          

        static void Main()
        {
            using (myfx orn = new myfx())
            {
                orn.grafik_algila();
                Application.Run(orn);
            }
        }
    }
}
