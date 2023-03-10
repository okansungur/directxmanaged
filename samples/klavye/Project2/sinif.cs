using System;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;

// kisa isimler
using Direct3D = Microsoft.DirectX.Direct3D;
using DirectInput = Microsoft.DirectX.DirectInput;

namespace Project2
{
    public class sinif : System.Windows.Forms.Form
    {
          
           


        Direct3D.Device device = null;
        DirectInput.Device dev_klav = null;
        Mesh nesne = null;
        Point mouse_son_poz = new Point();
        Point mouse_simd_poz = new Point();
        bool mouse_basilimi = false;
        float hareket_hiz = 25.0f;
        DateTime simdi, son_t;
        float kalan_zaman;
        
        Vector3 eye_vector = new Vector3(5.0f, 5.0f, -5.0f);  //  konum(kamera) 
        Vector3 vlook = new Vector3(-0.5f, -0.5f, 0.5f); // yönelim 1. vektör(bakis açisi)
        Vector3 v_up = new Vector3(0.0f, 1.0f, 0.0f);   // yönelim 2. vektör(nesne konumu)
        Vector3 v_right = new Vector3(1.0f, 0.0f, 0.0f);//mouse basili iken  donus miktari (x,y,z)
        public sinif()
        {
            this.ClientSize = new Size(640, 480);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            simdi = DateTime.Now;
            TimeSpan kalan_zamanSpan = simdi.Subtract(son_t);
            kalan_zaman = (float)kalan_zamanSpan.Milliseconds * 0.001f;
            son_t = simdi;
            this.Render();
            this.Invalidate();
        }
        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            mouse_basilimi = true;
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            mouse_basilimi = false;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        static void Main()
        {
            using (sinif frm = new sinif())
            {
                frm.Show();
                frm.Init();
                Application.Run(frm);
            }
        }
        private void Init()
        {

            PresentParameters param = new PresentParameters();
            param.SwapEffect = SwapEffect.Discard;
            param.Windowed = true;
            param.EnableAutoDepthStencil = true;
            param.AutoDepthStencilFormat = DepthFormat.D16;
            
            device = new Direct3D.Device(0, Direct3D.DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, param);
            device.DeviceReset += new System.EventHandler(this.OnResetDevice);

            OnResetDevice(device, null);
            //**DirectInput ile klavyeden veri alinmasi
            dev_klav = new DirectInput.Device(SystemGuid.Keyboard);
            dev_klav.Acquire();//Klavye erişimi sağlansın
            

        }

        Direct3D.Material[] meshnesneler;
        Texture[] meshTextures;
        ExtendedMaterial[] materials = null;
        public void OnResetDevice(object sender, EventArgs e)
        {
            device = (Direct3D.Device)sender;
            device.Transform.Projection =
             Matrix.PerspectiveFovLH((float)Math.PI / 4, (float)this.ClientSize.Width / this.ClientSize.Height,
                 0.1f, 100.0f);
            device.RenderState.Lighting = false;
            try
            {
                System.IO.Directory.SetCurrentDirectory(
                   System.Windows.Forms.Application.StartupPath + @"\..\..\");
                nesne = Mesh.FromFile("myhouse4.x", MeshFlags.Managed, device, out materials);//materyalde alacak
            }
            catch { }
            
            if (meshTextures == null)
            {

                meshTextures = new Texture[materials.Length];
                meshnesneler = new Direct3D.Material[materials.Length];
                for (int i = 0; i < materials.Length; i++)
                {
                    meshnesneler[i] = materials[i].Material3D;

                    try
                    {
                        meshTextures[i] = TextureLoader.FromFile(device, materials[i].TextureFilename);

                    }
                    catch (Exception ex) { }
                    
                }
            }
        }

       private void veriler_alinsin()
        {

           
            mouse_simd_poz.X = Cursor.Position.X;//mouse konumu
            mouse_simd_poz.Y = Cursor.Position.Y;
            
            
            
            

            Matrix mat_don;

            if (mouse_basilimi)
            {
                int x_fark = (mouse_simd_poz.X - mouse_son_poz.X);
                int y_fark = (mouse_simd_poz.Y - mouse_son_poz.Y);

                if (y_fark != 0)
                {
                    mat_don = Matrix.RotationAxis(v_right, Geometry.DegreeToRadian((float)y_fark / 3.0f));
                    //istege bagli bir eksen etrafinda
                    //dönen bir matris olusturmak için Matrix.RotationAxis() metodu kullanilir
                    vlook = Vector3.TransformCoordinate(vlook, mat_don);
                    //belirli bir matrise göre vektör yerdegistirecek 
                    v_up = Vector3.TransformCoordinate(v_up, mat_don);
                }
                if (x_fark != 0)
                {
                    Vector3 vTemp = new Vector3(0.0f, 1.0f, 0.0f);
                    //mouse saga cekilince veya sola cekilince hangi sag_sol konumuna göre dönsün
                    //örn ew.x ile çalisirken bu kodu degistirmeliyiz.
                    mat_don = Matrix.RotationAxis(vTemp, Geometry.DegreeToRadian((float)x_fark / 3.0f));
                    vlook = Vector3.TransformCoordinate(vlook, mat_don);
                    //belirli bir matrise göre vektör yerdegistirecek 
                     v_up = Vector3.TransformCoordinate(v_up, mat_don);
                }
            }

            mouse_son_poz.X = mouse_simd_poz.X;
            mouse_son_poz.Y = mouse_simd_poz.Y;



            Vector3 g_look = new Vector3();
            Vector3 g_sag = new Vector3();
            g_look = vlook;
            g_sag = v_right;

            KeyboardState keys = dev_klav.GetCurrentKeyboardState();
            //klavyeden basilan tuslari  tuslar "keys" degiskenine ataniyor


            if (keys[Key.Up])
            {
                eye_vector -= g_look * -hareket_hiz * kalan_zaman;
              
            }
            this.Text = eye_vector.X + ":" + eye_vector.Y + ":" + eye_vector.Z;
            if (keys[Key.X])
            {
                device.Transform.World = Matrix.Translation(eye_vector);
            }
            if (keys[Key.Down])
                eye_vector += (g_look * -hareket_hiz) * kalan_zaman;


            if (keys[Key.Left])
                eye_vector -= (g_sag * hareket_hiz) * kalan_zaman;


            if (keys[Key.Right])
                eye_vector += (g_sag * hareket_hiz) * kalan_zaman;


            if (keys[Key.Home])
                eye_vector.Y += hareket_hiz * kalan_zaman;


            if (keys[Key.End])
                eye_vector.Y -= hareket_hiz * kalan_zaman;

            if (keys[Key.Q])
            {


            }

        }
        //Mouse ve klavye yardimiyla 3 boyutlu ortamda 
        //yerdegistirme ve dönme islemlerini yapabilmek için asagidaki matrisi
        //tanimliyoruz.Kameramiz tanimlamis oldugumuz matrise göre ayarlanacak
        //
        private void fr_guncelle()
        {
           Matrix ms = Matrix.Identity;//Birim matris tanımlandı
           vlook = Vector3.Normalize(vlook);
           v_right = Vector3.Cross(v_up, vlook);
           //Vector3.Cross() metodu iki vektöre dikey olan 3. vektörü simgeler
            //(orthogonal nedir????)
            v_right = Vector3.Normalize(v_right);
            v_up = Vector3.Cross(vlook, v_right);
            Vector3.Normalize(v_up);

            ms.M11 = v_right.X;
            ms.M12 = v_up.X;
            ms.M13 = vlook.X;
            ms.M14 = 0.0f;

            ms.M21 = v_right.Y;
            ms.M22 = v_up.Y;
            ms.M23 = vlook.Y;
            ms.M24 = 0.0f;

            ms.M31 = v_right.Z;
            ms.M32 = v_up.Z;
            ms.M33 = vlook.Z;
            ms.M34 = 0.0f;

            ms.M41 = -Vector3.Dot(eye_vector, v_right);
            ms.M42 = -Vector3.Dot(eye_vector, v_up);
            ms.M43 = -Vector3.Dot(eye_vector, vlook);
            ms.M44 = 2.0f;
            //Vector3.Dot() metodu :
            //Geometrik olarak x vektörünün y vektörü üzerindeki birim uzunlugunu verir
            //Burada da saga veya sola dönmemiz gerektigine bu hesaba göre karar verilir

            device.Transform.World = ms;

        }

        private void Render()
        {
            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target,
                             Color.FromArgb(0, 200, 129, 179), 1.0f, 0);

            veriler_alinsin();
            device.BeginScene();
            fr_guncelle();


            for (int i = 0; i < meshnesneler.Length; i++)
            {

                device.Material = meshnesneler[i];
                device.SetTexture(0, meshTextures[i]);
                //Doku ve çizimler      
                nesne.DrawSubset(i);
            }
            device.EndScene();
            device.Present();
        }

          private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // sinif
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "sinif";
            this.Text = "house";
            this.Load += new System.EventHandler(this.sinif_Load);
            this.ResumeLayout(false);

        }
        private void sinif_Load(object sender, EventArgs e)
        {

        }

    }
}
