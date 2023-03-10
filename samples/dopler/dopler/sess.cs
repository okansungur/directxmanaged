using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.DirectX;
using dsesbuf = Microsoft.DirectX.DirectSound;


namespace snd
{
    class sess : Form
    {
        private Timer timer1;
        private IContainer components;
        private Button button1;
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // button1
            // 
            this.Text = "3D-SES";
            this.button1.Location = new System.Drawing.Point(77, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Helikopter";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // sess
            // 
            this.ClientSize = new System.Drawing.Size(216, 39);
            this.Controls.Add(this.button1);
            this.Name = "sess";
            this.Load += new System.EventHandler(this.sesefekti_Load);
            this.ResumeLayout(false);

        }


        dsesbuf.Device device;
        dsesbuf.Buffer buffer;
        dsesbuf.Buffer3D buffer3D;
        dsesbuf.Buffer birincil;






        void ses_hazirlansin()
        {

            device = new dsesbuf.Device();
            //3 boyutlu ortam için 
            dsesbuf.BufferDescription desc = new dsesbuf.BufferDescription();
            desc.Control3D = true;//steryo formatýnda olmayacak ve 3-D kontrolü saðlanacak
            buffer = new dsesbuf.Buffer("hell.wav", desc, device);

            buffer3D = new dsesbuf.Buffer3D(buffer);//Bu sýnýf 3 boyutlu ortamda konum yönelim ve çevresel faktörlerle ilgili metod ve özellikler içerir


            buffer3D.Mode = dsesbuf.Mode3D.HeadRelative;
            //Mode özelliði 3boyutlu ses özelliklerini belirtmek için kullanýlýr

            //3 bölümde incelenir ve varsayýlan deðeri "Mode3D.Normal" dir
            //bu özellikte "listener" (0,0,0) konumunda yeralýr.
            //"Mode3D.HeadRelative" de ise ses kaynaðý ve dinleyicinin konumu dikkate alýnýr
            //"Mode3D.Disable" de ise 3 boyutlu ses özellikleri iptal edilir

            dsesbuf.Buffer birincil = p_buffer();
            dsesbuf.Listener3D listener = new dsesbuf.Listener3D(birincil);

            listener.Deferred = true;
            // konum ve hýz belirtilir
            listener.Position = new Vector3();
            listener.Velocity = new Vector3();
            // Yönelim
            dsesbuf.Listener3DOrientation yonelim = listener.Orientation;
            yonelim.Front = new Vector3(0, 0, 1);//ileriye bakmakta
            yonelim.Top = new Vector3(0, 1, 0);//dik durmakta
            listener.Orientation = yonelim;


            // Bu metod çaðrilincaya kadar deðiþiklikler uygulanmaz Bu olmasaydý anýnda deðiþiklikler uygulanacaktý
            listener.CommitDeferredSettings();


            // bütün 3d parametreler cevreye ve listenerea uygulanır
            dsesbuf.Listener3DSettings ayarlar = listener.AllParameters;

           ayarlar.DistanceFactor = 1;

            // varsayýlan deðer 1 dir
            ayarlar.DopplerFactor = 10;
          
            //uzakliğa göre sesin azalması veya artması belirlenebilir
            ayarlar.RolloffFactor = 1;

            // güncellenen veriler listener nesnesine tekrar atanmıştır
            //Ardından bu bilgilere göre güncelleme yapıldıktan sonra
            //etkisi izlenebilir
            listener.AllParameters = ayarlar;

            listener.CommitDeferredSettings();

            device.SetCooperativeLevel(this, dsesbuf.CooperativeLevel.Priority);

        }
        public dsesbuf.Buffer p_buffer()
        {

            dsesbuf.BufferDescription buf_desc = new dsesbuf.BufferDescription();


            buf_desc.PrimaryBuffer = true;
            buf_desc.Control3D = true;


            birincil = new dsesbuf.Buffer(buf_desc, device);


            return birincil;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            buffer.Play(0, dsesbuf.BufferPlayFlags.Looping);
        }

        private void sesefekti_Load(object sender, EventArgs e)
        {
            ses_hazirlansin();
        }

        public static void Main()
        {
            sess ses = new sess();

            ses.InitializeComponent();


            Application.Run(ses);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int t = Environment.TickCount;
            buffer3D.Position = new Vector3(
              (float)Math.Sin(t / 1000.0),
              0,
              (float)Math.Cos(t / 1000.0)
           );
            //konum ve hýz zamana göre deðiþtirilmektedir ve bu sayede helikopter etrafýmýzda dönmektedir.
            //Bu kodu oyuna uyarlarken nesnenin konumuna göre gerekli olan deðiþiklikleri
            //yapmalýyýz.
            this.Text = "" + t;
            buffer3D.Velocity = new Vector3(
              (float)Math.Cos(t / 1000.0),
              0,
              -(float)Math.Sin(t / 1000.0)
           );

        }
    }



}
