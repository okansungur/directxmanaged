using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
namespace chapt2A
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
              //  MessageBox.Show((int)Usage.None+"");
                vertexBuffer = new VertexBuffer(typeof(CustomVertex.TransformedColored), 3, device, 0, CustomVertex.TransformedColored.Format, Pool.Default);
                //pencere resize edildiğinde device nesnesi otomatik
                //olarak resetlenecektir 
                //Managed yapılması durumunda ise veriler  hafızada
                //tutulabileceklerdir
  CustomVertex.TransformedColored[] verts = new CustomVertex.TransformedColored[3];
                verts[0].X = 150;
                verts[0].Y = 50;
                verts[0].Z = 0.5f;
                verts[0].Rhw = 1;
                verts[0].Color = Color.Aqua.ToArgb();
                verts[1].X = 250;
                verts[1].Y = 250;
                verts[1].Z = 0.5f;
                verts[1].Rhw = 1;
                verts[1].Color = Color.Beige.ToArgb();
                verts[2].X = 50;
                verts[2].Y = 250;
                verts[2].Z = 0.5f;
                verts[2].Rhw = 1;
                verts[2].Color = Color.HotPink.ToArgb();
                vertexBuffer.SetData(verts, 0, LockFlags.None);
           
            }
            catch (DirectXException ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }
      
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
             device.Clear(ClearFlags.Target, System.Drawing.Color.Blue, 1.0f, 0);
            device.BeginScene();

            device.SetStreamSource(0, vertexBuffer, 0);
            device.VertexFormat = CustomVertex.TransformedColored.Format;
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
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