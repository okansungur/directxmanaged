using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
namespace chapt3_B
{
    public partial class Form1 : Form
    {
       private Device device = null;
        VertexBuffer vertexBuffer = null;
       public void grafik_algila()
        {
            try
            {
               PresentParameters parametre = new PresentParameters();
                parametre.Windowed = true;
      
                parametre.SwapEffect = SwapEffect.Discard; 
              
                device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, parametre);
                device.RenderState.Lighting = false;
                this.device_olusumu(device, null);

            }
            catch (DirectXException ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        public void device_olusumu(object sender, EventArgs e)
        {
            Device dev = (Device)sender;
            dev.RenderState.Lighting = false;
            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionNormalColored), 18, dev, 0, CustomVertex.PositionNormalColored.Format, Pool.Default);
            vertexBuffer.Created += new System.EventHandler(this.vertex_buffer_olusumu);
            this.vertex_buffer_olusumu(vertexBuffer, null);
            this.OnResetDevice(device, null);
        }
        public void OnResetDevice(object sender, EventArgs e)
        {
            Device dev = (Device)sender;
                      
            dev.RenderState.CullMode = Cull.CounterClockwise;

           
         

        }
        public void vertex_buffer_olusumu(object sender, EventArgs e)
        {

    
            VertexBuffer vb = (VertexBuffer)sender;

            int renk = Color.Maroon.ToArgb();
            CustomVertex.PositionNormalColored[] verts = new CustomVertex.PositionNormalColored[18];

            // küp üst yüzey
         
            verts[0].X = -5.0f; verts[0].Y = 5.0f; verts[0].Z = -5.0f; verts[0].Color = renk;
            verts[1].X = -5.0f; verts[1].Y = 5.0f; verts[1].Z = 5.0f; verts[1].Color = renk;
            verts[2].X = 5.0f; verts[2].Y = 5.0f; verts[2].Z = -5.0f; verts[2].Color = renk;
            verts[3].X = 5.0f; verts[3].Y = 5.0f; verts[3].Z = 5.0f; verts[3].Color = renk;

            // 1.kenar
            verts[4].X = -5.0f; verts[4].Y = -5.0f; verts[4].Z = -5.0f; verts[4].Color = renk;
            verts[5].X = -5.0f; verts[5].Y = 5.0f; verts[5].Z = -5.0f; verts[5].Color = renk;
            verts[6].X = 5.0f; verts[6].Y = -5.0f; verts[6].Z = -5.0f; verts[6].Color = renk;
            verts[7].X = 5.0f; verts[7].Y = 5.0f; verts[7].Z = -5.0f; verts[7].Color = renk;

            // 2.
             verts[8].X = 5.0f; verts[8].Y = -5.0f; verts[8].Z = 5.0f; verts[8].Color = renk;
             verts[9].X = 5.0f; verts[9].Y = 5.0f; verts[9].Z = 5.0f; verts[9].Color = renk;

            // 3.
             verts[10].X = -5.0f; verts[10].Y = -5.0f; verts[10].Z = 5.0f; verts[10].Color = renk;
             verts[11].X = -5.0f; verts[11].Y = 5.0f; verts[11].Z = 5.0f; verts[11].Color = renk;

            // 4.
             verts[12].X = -5.0f; verts[12].Y = -5.0f; verts[12].Z = -5.0f; verts[12].Color = renk;
             verts[13].X = -5.0f; verts[13].Y = 5.0f; verts[13].Z = -5.0f; verts[13].Color = renk;

            // alt yüzey
             verts[14].X = 5.0f; verts[14].Y = -5.0f; verts[14].Z = -5.0f; verts[14].Color = renk;
             verts[15].X = 5.0f; verts[15].Y = -5.0f; verts[15].Z = 5.0f; verts[15].Color = renk;
             verts[16].X = -5.0f; verts[16].Y = -5.0f; verts[16].Z = -5.0f; verts[16].Color = renk;
             verts[17].X = -5.0f; verts[17].Y = -5.0f; verts[17].Z = 5.0f; verts[17].Color = renk;

      
   vb.SetData(verts, 0, LockFlags.None);



           

        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            render();
        }


        float aci;
        private void Kamera()
        {
            aci += 0.005f;
            device.Transform.World = Matrix.RotationY(aci);

                 device.Transform.View =
                Matrix.LookAtLH(
                new Vector3(0.0f, 20.0f, -40.0f),
                new Vector3(0.0f, 1.0f, 0.0f),
                new Vector3(0.0f, 1.0f, 0.0f));

            device.Transform.Projection =
                Matrix.PerspectiveFovLH((float)Math.PI / 4, 1.0f, 1.0f, 100.0f);
        }

        void render() {
            device.Clear(ClearFlags.Target, System.Drawing.Color.Aqua, 1.0f, 0);
          
            device.BeginScene();



            device.SetStreamSource(0, vertexBuffer, 0);
            device.VertexFormat = CustomVertex.PositionNormalColored.Format;

            
           
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);//üst yüzey(2 üçgen var!!)
           //alt ve üst yüzeylerde toplam 8 adet köşegen tanımlandı
            //kalan 4 üçgende ise toplam 10 adet köşegen tanımlanmakta
            //ve bunlar ekranda gösterilmektedir
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 4, 8);
          
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 14, 2);
            device.RenderState.Lighting = false;

            Kamera();
            device.EndScene();
            device.Present();
            this.Invalidate();
        
        }
        public Form1()
        {
            grafik_algila();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}