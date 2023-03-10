using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace baslangic1
{
    public partial class Form1 : Form
    {

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {


                device.Clear(ClearFlags.Target, Color.Orange, 1.0f, 0);
                Kamera(); 
                device.BeginScene();//direct3D  çizim yaptigimizi söyledik
                //CustomVertex.TransformedColored[] vert = new CustomVertex.TransformedColored[3];
                //degistirdik!!!!!
                 CustomVertex.PositionColored[] vert = new CustomVertex.PositionColored[3];
                vert[0].Position = new Vector3(0.0f, 1.0f, 1.0f);
                vert[0].Color = System.Drawing.Color.PaleGreen.ToArgb();
                vert[1].Position = new Vector3(-1, -1, 1f);
                vert[1].Color = System.Drawing.Color.SaddleBrown.ToArgb();
                vert[2].Position = new Vector3(1, -1, 1);
                vert[2].Color = System.Drawing.Color.Red.ToArgb();
                //device.VertexFormat = CustomVertex.TransformedColored.Format;
                device.VertexFormat = CustomVertex.PositionColored.Format;

                //Burada  Vector3 sinifi kullanildi artik  nesneleri uzay ortamina götürdük
                //device.VertexFormat kullanilarak direct3d ye cizilen verinin turunu 
                //degistirdigimizi soluyoruz
                device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 3, vert);
                device.EndScene();
                device.Present();
                this.Invalidate();
                
            }
            catch (DirectXException hata)
            {

                MessageBox.Show(hata.Message);

            }

        }



        private void Kamera()
        {
            //boyle bir ortamda Direct3D renkleri algilamasi isik yardimiyla olmaktadir
            //aksi takdirde ekranda objemiz siyah görüntülenecektir
            device.RenderState.Lighting = false;//henüz sahne için isik tanimlanmadi
            //varsayilan olarak Lighting özelligi Direct3D'de true oldugu
            //için simdilik isik kapatilacaktir.
 

            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4,
                 this.Width / this.Height, 1.0f, 100.0f);
            //aspect ratio tvnin gorus alanini temsil etmekte orn tvnin genisligini 
            //yuksekligine boldugumuzde genelde (1.85) direct3D içinde ayni durum sözkonusu
            //znearplane parametresi piramitin tepesini zfarplane parametresi
            //ise piramitin tabanini göstermektedir
            //(notlara bakiniz)
            device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, 8.0f), new Vector3(),
                 new Vector3(0, 1, 0));
            //Matrix.LookAtLH() metodunda ilk parametre kameranin konumu ikinci parametre
            //kameranin bakmasini istedigimiz konumu üçüncü parametre ise yukaridan bakildigindaki konumu 
            //genelde (0, 1, 0) degerini alir
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
            this.Text = "DX9 Oyun Programlama";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //bu ornek ile onceki yaptigimiz ornek arasindaki temel fark 
            //nesnelerimiz son ornekte gercek anlamda 3 boyutlu bir ortama
            //tasinmis olmasidir

        }
    }
}