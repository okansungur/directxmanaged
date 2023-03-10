using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace baslangic2
{
    public partial class Form1 : Form
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {


                device.Clear(ClearFlags.Target, Color.Silver, 1.0f, 0);
                Kamera();
                device.BeginScene();
                CustomVertex.PositionColored[] vert = new CustomVertex.PositionColored[3];
                vert[0].Position = new Vector3(0.0f, 1.0f, 1.0f);
                vert[0].Color = System.Drawing.Color.Tomato.ToArgb();
                vert[1].Position = new Vector3(-1.0f, -1.0f, 1.0f);
                vert[1].Color = System.Drawing.Color.YellowGreen.ToArgb();
               vert[2].Position = new Vector3(1.0f, -1.0f, 1.0f);
               vert[2].Color = System.Drawing.Color.Red.ToArgb();
          
                device.VertexFormat = CustomVertex.PositionColored.Format;
                        
                device.DrawUserPrimitives(PrimitiveType.TriangleList, 1, vert);
                device.EndScene();
                device.Present();
                this.Invalidate();

            }
            catch (Exception hata)
            {

                MessageBox.Show(hata.Message);

            }

        }


        float aci;//nesnenin dönmesi için tanımladık
        private void Kamera()
        {
            //boyle bir ortamda Direct3D renkleri algilamasi isik yardimiyla olmaktadir
            //aksi takdirde ekranda objemiz siyah görüntülenecektir
           device.RenderState.Lighting = false;//isiklarin açilmasi gerekli

            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4,
                 this.Width / this.Height, 1.0f, 100.0f);
            //aspect ratio tvnin gorus alanini temsil etmekte orn tvnin genisligini 
            //yuksekligine boldugumuzde genelde (1.85) direct3D içinde ayni durum sözkonusu
            //znearplane parametresi piramitin tepesini zfarplane parametresi
            //ise piramitin tabanini göstermektedir
            //(notlara bakiniz)
            device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, 4.0f), new Vector3(),
                 new Vector3(0, 1, 0));
           
            //Adım 1
            //device.Transform.World = Matrix.RotationZ((System.Environment.TickCount / 450.0f) / (float)Math.PI);
            
            //System.Environment.TickCount =15 ms süreyi temsil etmektedir
            //GetTickCount() metodu Win32 API kullanmaktadır
            //Bu yüzden 15ms yerine farklı yollarda deneyebiliriz örneğin:

            //Adım 2
            device.Transform.World = Matrix.RotationZ((aci / 450.0f) / (float)Math.PI);
            aci += 0.1f;
            //daha yavaş bir şekilde dönüş gerçekleşmekte ve Dikkat edilirse sadece 
            //Z eksenine göre bir dönüş sağlanmakta

            //Adım 3
            //farklı eksenlerede bu dönüşü yaymayı sağlayabilirsek:
            
            device.Transform.World = Matrix.RotationAxis(new Vector3(aci / ((float)Math.PI * 2.0f),
    aci / ((float)Math.PI * 4.0f), aci / ((float)Math.PI * 6.0f)),
    aci / (float)Math.PI);
            //Matrix.RotationAxis() bize farklı eksenlerde dönüşü mümkün kılmaktadır
            //Dönüşler esnesında üçgenin bazı bölümlerinin kaybolduğu görülmektedir
            //"back face culling" Direct3D üçgeni render ederken bir yüzeyinin kamera 
            //tarafından görülmediği için çizmemektedir buna ayrılma,kopma(back face culling) denilmektedir
            //bu çizilen bölümleri
            //device.RenderState.CullMode = Cull.Clockwise;
            //saat yönü çizilmesin
         //   device.RenderState.CullMode = Cull.CounterClockwise;
            //saat yönünün tersi çizilmesin
            device.RenderState.CullMode = Cull.None;
            //Artık "arka yüzey ayrılması" sonlandırılmış olacak
        }


        private Device device = null;
        public void InitializeGraphics()
        {
            PresentParameters parametre = new PresentParameters();
            parametre.Windowed = true;
            parametre.SwapEffect = SwapEffect.Discard;
            device = new Device(0, DeviceType.Hardware, this,
            CreateFlags.SoftwareVertexProcessing, parametre);
        }
        public Form1()
        {
            InitializeGraphics();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            this.Text = "Nesnenin eksen etrafıda döndürülmesi";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        
        }
    }
}