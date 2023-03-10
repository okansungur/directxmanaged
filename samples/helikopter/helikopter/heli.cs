using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
using System.Drawing;

namespace helikopter
{
    class heli: Form
    {
          Device device;
         helikopter hel_x;

  
        //düzelecek!!!
        
        static void Main()
        {
            heli app = new heli();
            app.SetBounds(0, 0, 800, 500);
            app.grafik_algila();
            app.Show();
            while (app.Created)
            {
                app.guncelle();
                app.Render();
                Application.DoEvents();
            }
        }
    
        private void grafik_algila()
        {
            PresentParameters param = new PresentParameters();
            param.Windowed = true;
            param.SwapEffect = SwapEffect.Discard;
            param.AutoDepthStencilFormat = DepthFormat.D16;
            param.EnableAutoDepthStencil = true;

            device = new Device(0, DeviceType.Hardware, this,
              CreateFlags.SoftwareVertexProcessing, param);

            device.RenderState.ZBufferEnable = true;
            a += 10;
            device.RenderState.Lighting = true;
device.Transform.Projection =
              Matrix.PerspectiveFovLH((float)Math.PI / 4.0F, 1.0F, 1.0F, 800.0F);
            device.Transform.View = Matrix.LookAtLH(new Vector3(-20,-10, 80),
              new Vector3(), new Vector3(0, 1, 0));


            this.hel_x = new helikopter(@"hh.x", this.device);
            this.hel_x.yukle();
            this.hel_x.Konum = new Vector3(0, 0, 0);
            
        }
        float a=0f;
        void kamera() {
            a += 0000.1f;
            device.Transform.View = Matrix.LookAtLH(new Vector3(-20, -10, 80),
              new Vector3(), new Vector3(0, 1, 0))*Matrix.RotationZ(a/100);
        
        }

        protected void guncelle()
        {
            float hiz = Environment.TickCount;
            this.Text = hiz+"";
            this.hel_x.Aci = hiz / 30.0F;
            kamera();
        }

        private void Render()
        {
            this.device.Clear(
              ClearFlags.Target | ClearFlags.ZBuffer,
              Color.CornflowerBlue, 1.0F, 0);

            device.Lights[0].Type = LightType.Spot;
            device.Lights[0].Diffuse = Color.Blue;//yansitilan renk
            device.Lights[0].Direction = new Vector3(0, 0, -1);
            device.Lights[0].Position = new Vector3(0, 0, 80);
            device.Lights[0].Falloff = 90;
            device.Lights[0].Range = 220;
            device.Lights[0].InnerConeAngle = 10;
            device.Lights[0].OuterConeAngle = 14;
            device.Lights[0].Attenuation0 = 0.01f;
            device.Lights[0].Attenuation1 = 0.011f;
            device.Lights[0].Enabled = true;

 
 




            this.device.BeginScene();

            this.hel_x.Render();

            this.device.EndScene();

          
                this.device.Present();
          
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // heli
            // 
     
            this.Name = "heli";
            this.Load += new System.EventHandler(this.heli_Load);
            this.ResumeLayout(false);

        }

        private void heli_Load(object sender, EventArgs e)
        {

        }



       
    }
    }

