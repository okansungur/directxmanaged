using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace baslangic4
{
    public partial class Form1 : Form
    {
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            try
            {
               device.Clear(ClearFlags.Target, System.Drawing.Color.CornflowerBlue, 1.0f, 0);
               Kamera();
                device.BeginScene();//Direct3D  birseyler cizicegimizi soledik

                CustomVertex.PositionNormalColored[] verts = new CustomVertex.PositionNormalColored[3];
                verts[0].Position = new Vector3(0.0f, 1.0f, 1.0f);
                verts[0].Normal = new Vector3(0.0f, 0.0f, -1.0f);
                verts[0].Color = System.Drawing.Color.Blue.ToArgb();//herbir ucgenin rengini değiştirebiliriz
                verts[1].Position = new Vector3(-1.0f, -1.0f, 1.0f);
                verts[1].Normal = new Vector3(0.0f, 0.0f, -1.0f);
                Color red = Color.Red;
                verts[1].Color = red.ToArgb();
                verts[2].Position = new Vector3(1.0f, -1.0f, 1.0f);
                verts[2].Normal = new Vector3(0.0f, 0.0f, -1.0f);
                Color yesilrenk=Color.Green;
                verts[2].Color = yesilrenk.ToArgb();//int veri türüne  donusturme
                device.VertexFormat = CustomVertex.PositionNormalColored.Format;


                device.DrawUserPrimitives(PrimitiveType.TriangleStrip,3, verts);
                device.Lights[0].Type = LightType.Point;
                device.Lights[0].Position = new Vector3(0,0,3);//(0,0,0) noktasından aydınlatma yapabiliriz
                //burada ışığı yukarıdan tutuyoruz
                device.Lights[0].Diffuse = System.Drawing.Color.White;//dağılan ışık
                device.Lights[0].Attenuation0 = 0.1f;  //ışık zayıflatıcı 
                device.Lights[0].Range =2.0f;//menzil
                  
                device.Lights[0].Enabled = true;
           

       

                device.EndScene();
                device.Present();
                this.Invalidate();
            }
            catch (DirectXException ex)
            {
                MessageBox.Show( ex.Message);
            }
        }
        float aci;//nesnenin dönmesi için tanimladik
        private void Kamera()
        {
            device.RenderState.Lighting = true;//varsayılan deger budur
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4,
             this.Width / this.Height, 1.0f, 100.0f);
             device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, 5.0f), new Vector3(),
              new Vector3(0, 1, 0));

            device.Transform.World = Matrix.RotationZ((aci / 450.0f) / (float)Math.PI);
            aci += 0.1f;
             device.Transform.World = Matrix.RotationAxis(new Vector3(aci / ((float)Math.PI * 2.0f),
    aci / ((float)Math.PI * 4.0f), aci / ((float)Math.PI * 6.0f)),
    aci / (float)Math.PI);
          
            device.RenderState.CullMode = Cull.None;
           }
         Device device = null;
        public void grafik_algila()
        {
            PresentParameters parametre = new PresentParameters();
            parametre.Windowed = true;
            parametre.SwapEffect = SwapEffect.Discard;
            device = new Device(0, DeviceType.Hardware, this,
            CreateFlags.SoftwareVertexProcessing, parametre);
        }
        public Form1()
        {
            grafik_algila();
            device.DeviceResizing += new CancelEventHandler(this.CancelResize);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }
        private void CancelResize(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}