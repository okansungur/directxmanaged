using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace dokular
{
    public class doku : Form
    {
        Device device;
        CustomVertex.PositionTextured[] kosegenler;
        Texture[] dokular = new Texture[5];
        //resimleri icerisinde tutacak doku dizisi tanımlandı
        Mesh mesh;
        public doku()
        {
            InitializeComponent();
            grafik_algila();
            Kamera();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }
        public void grafik_algila()
        {

            PresentParameters parametre = new PresentParameters();
            parametre.Windowed = true;
            parametre.SwapEffect = SwapEffect.Discard;

            parametre.EnableAutoDepthStencil = true;
            parametre.AutoDepthStencilFormat = DepthFormat.D16;
            device = new Device(0, DeviceType.Hardware, this, CreateFlags.HardwareVertexProcessing, parametre);
            device.RenderState.Lighting = false;
            mesh = Mesh.Teapot(device);  //resmin önünde çaydanlık tanımlanıyor
        }
        private void Kamera()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, 1, 1.3f, 1500f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, 40), new Vector3(0, 0, 0), new Vector3(0, 1, 0)) * Matrix.Scaling(2, 2, 2);
        }
        protected override void OnPaint(PaintEventArgs eee)
        {
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            device.BeginScene();
            device.VertexFormat = CustomVertex.PositionTextured.Format;
            ucgenciz();
            device.DrawUserPrimitives(PrimitiveType.TriangleList, 2, kosegenler);
            mesh.DrawSubset(0);
            doku_degistir();
            device.SetTexture(0, dokular[i]);
            device.EndScene();
            device.Present();
            this.Invalidate();
        }
        void ucgenciz()
        {
            kosegenler = new CustomVertex.PositionTextured[6];
            kosegenler[0].Position = new Vector3(15f, 15f, 0f);
            kosegenler[0].Tu = 0;
            kosegenler[0].Tv = 0;
            kosegenler[1].Position = new Vector3(-15f, -15f, 0f);
            kosegenler[1].Tu = 1;
            kosegenler[1].Tv = 1;
            kosegenler[2].Position = new Vector3(15f, -15f, 0f);
            kosegenler[2].Tu = 0;
            kosegenler[2].Tv = 1;
            kosegenler[3].Position = new Vector3(-15f, -15f, 0f);
            kosegenler[3].Tu = 1;
            kosegenler[3].Tv = 1;
            kosegenler[4].Position = new Vector3(15.0f, 15.0f, 0f);
            kosegenler[4].Tu = 0;
            kosegenler[4].Tv = 0;
            kosegenler[5].Position = new Vector3(-15f, 15f, 0f);
            kosegenler[5].Tu = 1;
            kosegenler[5].Tv = 0;
        }
        int i;
        void doku_degistir()
        { //resimler değişecek
            i = (System.Environment.TickCount / 500) % 5;
              dokular[i] = TextureLoader.FromFile(device, "a" + (i + 1) + ".jpg");
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // sinif
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "doku";
            this.Text = "doku1";
            this.ResumeLayout(false);
        }
        static void Main()
        {
            using (doku orn = new doku())
            {
                orn.Show();
                Application.Run(orn);
            }
        }
    }


}
