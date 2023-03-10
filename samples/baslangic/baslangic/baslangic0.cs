using System;

using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace baslangic
{
    class baslangic0 : Form
    {
        private Device device = null;
        // Grafik burada algılanacak
        public void grafik_algila()
        {
            try
            {


                // parametre nesnesi tanımlanıyor
                PresentParameters parametre = new PresentParameters();

                parametre.Windowed = true;//pencere modunda olacak (konsole)

                parametre.SwapEffect = SwapEffect.Discard;
                //Antialiasing kullanılacaksa mutlaka
                //SwapEffect.Discard. yapılmalı yani tamponda önceki çizimi tutulmamalı
                // device nesnesi oluşturulur
                device = new Device(0, DeviceType.Hardware, this,
                   CreateFlags.SoftwareVertexProcessing, parametre);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                device.Clear(ClearFlags.Target, Color.Aqua, 0.0f, 19);
                //hedef pencere temizlensin ,renk, Z-buffer 
                //ekrana arka plan rengi verildi
                device.BeginScene();//Direct3D  birşeyler ciziceğimizi soledik
                //konulmazsa  kod cizim esnasında hata verir


                CustomVertex.TransformedColored[] vert = new CustomVertex.TransformedColored[3];
                //4. pürüssüzleştirmek için kullanılır
                vert[0].Position = new Vector4(150f, 100f, 0f, 1f);  //tek bir üçgen çizilmekte
                vert[0].Color = Color.Red.ToArgb();
                vert[1].Position = new Vector4(300, 100f, 0f, 1f);
                vert[1].Color = Color.Green.ToArgb();
                vert[2].Position = new Vector4(250f, 300f, 0f, 1f);
                vert[2].Color = Color.Yellow.ToArgb();


                device.VertexFormat = CustomVertex.TransformedColored.Format;
                device.DrawUserPrimitives(PrimitiveType.TriangleList, 1, vert);
                //farklı üçgenlerden oluşan bir liste gelmektedir
                //eğer 4 üçgen çizmek isteseydik 12 adet vertice tanımlamamız gerekecekti
                //TriangleStrip  de kullanılabilirdi ve bu daha hızlı çalşırdı ancak bu
                //durumda üçgenlerin birbirine bağlantılı olması gerekirdi

                device.EndScene();//artık çizilmicek
                device.Present();
                this.Invalidate();//repaint edilecek
                //ancak bu titremeye ve boş bir ekran gelmesine sebep olmakta
            }
            catch (DirectXException hata)
            {

                MessageBox.Show("Hata: " + hata.Message);

            }

        }

        public baslangic0()
        {
            grafik_algila();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            //bu kod sayesinde paint işlemlerinin override void OnPaint metodunda olmasını 
            //sağlayabiliriz bu kodu kaldırırsak paint içerisindeki
            //invalidate metodunuda yorum yapmak gerekecek

           
            foreach (AdapterInformation a in Manager.Adapters)
            {
                MessageBox.Show(a.Information.Description);
            }

        }

        static void Main()
        {
            using (baslangic0 frm = new baslangic0())
            {
                frm.SetBounds(0,0,400,400);
                frm.Text = "İlk -Örnek";
                frm.Show();
                Application.Run(frm);
            }
        }

    }
}
