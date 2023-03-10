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
        Texture text;
        public void grafik_algila()
        {

            InitializeComponent();
            try
            {

                PresentParameters parametre = new PresentParameters();
                parametre.Windowed = true;
            
                parametre.SwapEffect = SwapEffect.Discard; // Discard the frames 
              
                device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, parametre);
                device.RenderState.Lighting = false;
                this.device_olusumu(device, null);
                
                text=TextureLoader.FromFile(device,  @"a.jpg");
                
            }
            catch (DirectXException ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        public void device_olusumu(object sender, EventArgs e)
        {
            InitializeComponent();
            Device dev = (Device)sender;
            dev.RenderState.Lighting = false;
            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionTextured), 36, dev, 0, CustomVertex.PositionTextured.Format, Pool.Default);
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
           CustomVertex.PositionTextured[] verts = new CustomVertex.PositionTextured[36];

verts[0] = new CustomVertex.PositionTextured(-1.0f, 1.0f, 1.0f, 0.0f, 0.0f);
verts[1] = new CustomVertex.PositionTextured(-1.0f, -1.0f, 1.0f, 0.0f, 1.0f);
verts[2] = new CustomVertex.PositionTextured(1.0f, 1.0f, 1.0f, 1.0f, 0.0f);
verts[3] = new CustomVertex.PositionTextured(-1.0f, -1.0f, 1.0f, 0.0f, 1.0f);
verts[4] = new CustomVertex.PositionTextured(1.0f, -1.0f, 1.0f, 1.0f, 1.0f);
verts[5] = new CustomVertex.PositionTextured(1.0f, 1.0f, 1.0f, 1.0f, 0.0f);
verts[6] = new CustomVertex.PositionTextured(-1.0f, 1.0f, -1.0f, 0.0f, 0.0f);
verts[7] = new CustomVertex.PositionTextured(1.0f, 1.0f, -1.0f, 1.0f, 0.0f);
verts[8] = new CustomVertex.PositionTextured(-1.0f, -1.0f, -1.0f, 0.0f, 1.0f);
verts[9] = new CustomVertex.PositionTextured(-1.0f, -1.0f, -1.0f, 0.0f, 1.0f);
verts[10] = new CustomVertex.PositionTextured(1.0f, 1.0f, -1.0f, 1.0f, 0.0f);
verts[11] = new CustomVertex.PositionTextured(1.0f, -1.0f, -1.0f, 1.0f, 1.0f);
verts[12] = new CustomVertex.PositionTextured(-1.0f, 1.0f, 1.0f, 0.0f, 0.0f);
verts[13] = new CustomVertex.PositionTextured(1.0f, 1.0f, -1.0f, 1.0f, 1.0f);
verts[14] = new CustomVertex.PositionTextured(-1.0f, 1.0f, -1.0f, 0.0f, 1.0f);
verts[15] = new CustomVertex.PositionTextured(-1.0f, 1.0f, 1.0f, 0.0f, 0.0f);
verts[16] = new CustomVertex.PositionTextured(1.0f, 1.0f, 1.0f, 1.0f, 0.0f);
verts[17] = new CustomVertex.PositionTextured(1.0f, 1.0f, -1.0f, 1.0f, 1.0f);
verts[18] = new CustomVertex.PositionTextured(-1.0f, -1.0f, 1.0f, 0.0f, 0.0f);
verts[19] = new CustomVertex.PositionTextured(-1.0f, -1.0f, -1.0f, 0.0f, 1.0f);
verts[20] = new CustomVertex.PositionTextured(1.0f, -1.0f, -1.0f, 1.0f, 1.0f);
verts[21] = new CustomVertex.PositionTextured(-1.0f, -1.0f, 1.0f, 0.0f, 0.0f);
verts[22] = new CustomVertex.PositionTextured(1.0f, -1.0f, -1.0f, 1.0f, 1.0f);
verts[23] = new CustomVertex.PositionTextured(1.0f, -1.0f, 1.0f, 1.0f, 0.0f);
verts[24] = new CustomVertex.PositionTextured(-1.0f, 1.0f, 1.0f, 0.0f, 0.0f);
verts[25] = new CustomVertex.PositionTextured(-1.0f, -1.0f, -1.0f, 1.0f, 1.0f);
verts[26] = new CustomVertex.PositionTextured(-1.0f, -1.0f, 1.0f, 1.0f, 0.0f);
verts[27] = new CustomVertex.PositionTextured(-1.0f, 1.0f, -1.0f, 0.0f, 1.0f);
verts[28] = new CustomVertex.PositionTextured(-1.0f, -1.0f, -1.0f, 1.0f, 1.0f);
verts[29] = new CustomVertex.PositionTextured(-1.0f, 1.0f, 1.0f, 0.0f, 0.0f);
verts[30] = new CustomVertex.PositionTextured(1.0f, 1.0f, 1.0f, 0.0f, 0.0f);
verts[31] = new CustomVertex.PositionTextured(1.0f, -1.0f, 1.0f, 1.0f, 0.0f);
verts[32] = new CustomVertex.PositionTextured(1.0f, -1.0f, -1.0f, 1.0f, 1.0f);
verts[33] = new CustomVertex.PositionTextured(1.0f, 1.0f, -1.0f, 0.0f, 1.0f);
verts[34] = new CustomVertex.PositionTextured(1.0f, 1.0f, 1.0f, 0.0f, 0.0f);
verts[35] = new CustomVertex.PositionTextured(1.0f, -1.0f, -1.0f, 1.0f, 1.0f);
vb.SetData(verts, 0, LockFlags.None);
         

}
       protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            render();
        }


        float aci;
        private void Kamera()
        {
            aci += 0.009f;
            device.Transform.World = Matrix.RotationYawPitchRoll(aci / (float)Math.PI, aci / (float)Math.PI * 2.0f, aci / (float)Math.PI / 4.0f);
                 device.Transform.View =
                Matrix.LookAtLH(
                new Vector3(0.0f, -4.0f, -40.0f),
                new Vector3(0.0f, 1.0f, 10.0f),
                new Vector3(10.0f, 10.0f, 10.0f));

                device.Transform.Projection =
                Matrix.PerspectiveFovLH((float)Math.PI / 38, 1.0f, 1.0f, 50.0f);
        }
        
        void render() {
            device.Clear(ClearFlags.Target, System.Drawing.Color.DarkRed, 1.0f, 0);
         
            device.BeginScene();
            device.SetStreamSource(0, vertexBuffer, 0);
          
            device.VertexFormat = CustomVertex.PositionTextured.Format;

            
            device.SetTexture(0, text);
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);
  
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

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }
    }
}