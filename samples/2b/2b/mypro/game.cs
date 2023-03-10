using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using d3d = Microsoft.DirectX.Direct3D;
using dkey = Microsoft.DirectX.DirectInput;
using Microsoft.DirectX.DirectDraw;//******

namespace mypro
{

    class game :Form
    {
        Device dev;
       
       dkey.Device klavye = null;
       Surface arka_plan = null;
        Surface metin = null;
       Rectangle rect;
       Rectangle[] kareler = new Rectangle[4];
       int arkaPlanres_x;
       int resm_hareket = 0;
       int karak_harek = 10; 


        public game()
        {
            
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }
        
        void arkaplan()
        {
            SurfaceDescription yuzey_tanimi = new SurfaceDescription();  
                         
            yuzey_tanimi.SurfaceCaps.OffScreenPlain = true;
            arka_plan = new Surface("2B.bmp", yuzey_tanimi, dev);
            rect.Size = new Size(800, 600);
       
        }
    


        Surface b_yuzey=null; //görüntülenen bolge
         Surface i_yuzey=null; 
         ColorKey col_key;
        Surface karak_yuzey; 
        void algila() {
      
            SurfaceDescription yuzey_tanimi = new SurfaceDescription();
            yuzey_tanimi.SurfaceCaps.PrimarySurface=true;
            yuzey_tanimi.SurfaceCaps.Flip = true;
            //birincil yüzeye getir.
            yuzey_tanimi.SurfaceCaps.Complex = true;
            //Tampon tanımlanacak
            yuzey_tanimi.BackBufferCount = 1;//1 adet tampon yeralmakta
            b_yuzey = new Surface(yuzey_tanimi, dev);
        
            yuzey_tanimi.Clear();
            yuzey_tanimi.SurfaceCaps.BackBuffer = true;
                       
            i_yuzey = b_yuzey.GetAttachedSurface(yuzey_tanimi.SurfaceCaps);
          
            yuzey_tanimi.Clear();
 
          
           
           col_key.ColorSpaceHighValue = Color.Blue.ToArgb();  
           col_key.ColorSpaceLowValue = Color.Blue.ToArgb();
         

            karak_yuzey = new Surface("deneme.bmp", yuzey_tanimi, dev);
            
            karak_yuzey.SetColorKey(ColorKeyFlags.SourceDraw, col_key);
          
            yuzey_tanimi.Clear();

            //metin alanı tanımlandı
            SurfaceDescription yzy = new SurfaceDescription();
            metin = new Surface("deneme2.bmp", yzy, dev);
            metin.SetColorKey(ColorKeyFlags.SourceDraw, col_key);
            metin.ForeColor = Color.White;
            metin.DrawText(10, 0, "Onu Farkettim", true);          
                  


   
            yuzey_tanimi.Clear();
        }

   
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
               cizimler();


                klavyemiz();
               this.Invalidate();
                    }

        
        public void cizimler()
        {

            try
            {
                i_yuzey.ColorFill(Color.Black);//arkaplan rengi
                

                rect.X = arkaPlanres_x;
                rect.Y = 0;
             i_yuzey.DrawFast(0, 100, arka_plan, rect, DrawFastFlags.DoNotWait);
            
          
               i_yuzey.DrawFast(karak_harek, 240, karak_yuzey, kareler[resm_hareket], DrawFastFlags.SourceColorKey);
                //DrawFast() metodu sayesinde belirli dörtgen bir alanın çizimi sağlanmaktadır.


               i_yuzey.DrawFast(50, 50, metin, DrawFastFlags.SourceColorKey);
               //SourceColorKey parametresi belirtilmez ise renk çıkmaya devam edecektir
               //çizim sırası önemli aynen layer mantığındaki gibi

                b_yuzey.Flip(i_yuzey, FlipFlags.DoNotWait);
                
             

            }
            catch(Exception a)
            {
                Console.WriteLine(a.StackTrace);
            }

        }

        private void Init()
        {
         

           //directdraw için Device sınıfı nesnesi tanımlanmaktadır
            dev = new Device();
            dev.SetCooperativeLevel(this, CooperativeLevelFlags.FullscreenExclusiveAllowModex);//çözünürlük ve renk derinliği belirtmek için

            //tam ekran(1024*768, 32 bit renk)
            dev.SetDisplayMode(1024, 768, 32, 0, true);
            
            karelercizilsin();
            arkaplan();
            
            klavye = new dkey.Device(dkey.SystemGuid.Keyboard);
            klavye.Acquire();
            algila();
          
        }



        public void karelercizilsin()
        {
            int x;
            for (x = 0; x < 4; x++)
                kareler[x] = new Rectangle(140 * x, 0, 140, 250); //resmin yukseklik 250px
         
        }	

      

     
    
         void klavyemiz() { 
            dkey.KeyboardState keys = klavye.GetCurrentKeyboardState();
            
              if (keys[dkey.Key.Right]) {
                  resm_hareket = resm_hareket + 1;
                  karak_harek += 5;
                  arkaPlanres_x += 10; // arkaplan resmi kacar pixel hareket edecek
                  if (resm_hareket > 3)//sayi 3 u gecince basa donsunki ilk resimden devam edelim(karakter)
                      resm_hareket = 0;
                  
                  if (karak_harek > 700) //karakter ekran dışına çıkınca basa donsun
                      karak_harek = 10;

                  if (arkaPlanres_x > 800)//arkaplan resmini baslangıç konumuna getir 
                      arkaPlanres_x = 100;


                  
                
              }
              if (keys[dkey.Key.Escape])
              {
                  this.Dispose();
                  Application.Exit();

              }
            }



        static void Main()
        {
            using (game frm = new game())
            {
                frm.Show();
                frm.Init();
                Application.Run(frm);
            }
        }

                             



    }
}
