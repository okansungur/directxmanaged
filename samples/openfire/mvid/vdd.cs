using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using d3d = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using d_klvy = Microsoft.DirectX.DirectInput;

namespace ates_et
{
    public class ates : System.Windows.Forms.Form
    {
         d3d.Device device;
         Material materyal;
         d_klvy.Device keyb;
         Texture doku;
         ArrayList liste = new ArrayList();
         public Vector3 konum;


        public ates()
        {
                
            this.Size = new System.Drawing.Size(500, 500);
            this.Text = "Ateş";
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }
       public void grafik_algila()
        {
            PresentParameters parametre = new PresentParameters();
            parametre.Windowed = true;
            parametre.SwapEffect = SwapEffect.Discard;
            parametre.AutoDepthStencilFormat = DepthFormat.D16;
            parametre.EnableAutoDepthStencil = true;
            device = new d3d.Device(0, d3d.DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, parametre);
            device.RenderState.PointSpriteEnable = true; 
            device.RenderState.PointScaleEnable = true;
            device.RenderState.PointScaleA =220;  //boyutlar ayarlanmakta
            device.RenderState.PointScaleB = 220;
            device.RenderState.PointScaleC = 220;
           
            keyb = new d_klvy.Device(SystemGuid.Keyboard);
            keyb.SetCooperativeLevel(this, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            keyb.Acquire();

           materyal = new Material();
           materyal.Ambient = Color.Red;
            doku = TextureLoader.FromFile(device, "a.jpg");

        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            klavye();
           device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            device.BeginScene();
           device.RenderState.Ambient = Color.OrangeRed;
  
            atesleme();
            device.EndScene();
            device.Present();
            this.Invalidate();
        }

        private void kamera()
        {
            
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, (float)this.Width / (float)this.Height, 0.3f, 500f);
             device.Transform.View = Matrix.LookAtLH(new Vector3(0, -1f, 0.2f), new Vector3(0, 0, 0), new Vector3(0, 0, 1))  ;
           
        }


     
          private void klavye()
        {
            KeyboardState keys = keyb.GetCurrentKeyboardState();

            

            if (keys[Key.Space])
            {
               
                konum = new Vector3(8,2,1); ;
               
                liste.Add(konum);
            }
        }
        Vector3 spt;
  private void atesleme()
        {
  

            for (int i = 0; i < liste.Count; i++)
            {
                 spt = (Vector3)liste[i];
                Vector3 gecici = new Vector3();
                gecici.Y += 1;
                
                spt += gecici * (0.05f); //hizi
            //    this.Text = spt.Y + "";
                if (spt.Y > 10) {
                    spt.Y = 4;
                    liste.Clear();
                    this.Text = liste.Count+"";
                 
                    break;
                
                }
                this.Text = liste.Count + "";
                //belirli bir noktadan sonra boş yere kaynakların tükenmemesi için 
                //Arraylist nesnesi olan listenişn içinin boşaltılması gerekmektedir.

                liste[i] = spt;
        
       
            }

            if (liste.Count > 0)
            {
                device.SetTexture(0, doku);
                device.Material = materyal;
                device.VertexFormat = VertexFormats.Position  ;
               // Bu formata gore vertex tanımlanmalı parametreler konum şeklinde gönderilmektedir

                device.Transform.World = Matrix.Translation(-8, -2, -1);
                for (int i = 0; i < liste.Count; i++)
                {

                    device.DrawUserPrimitives(PrimitiveType.PointList,1 , liste[i]);
                }
            }
            
        }

 

       
            
        static void Main()
        {



            using (ates orn = new ates())
            {
                orn.grafik_algila();
                orn.kamera();





                Application.Run(orn);
            }
        }
    }
}
