using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
namespace chapter4
{
    class meshler:  Form
    {
        Device device;

        Mesh nesne;

        public meshler()
        {
            grafik_algila();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Render();
            this.Invalidate();
        }

        public void grafik_algila()
        {

            PresentParameters parametre = new PresentParameters();
           parametre.SwapEffect = SwapEffect.Discard;
            parametre.Windowed = true;
            parametre.EnableAutoDepthStencil = true;
            parametre.AutoDepthStencilFormat = DepthFormat.D16;
         

            device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, parametre);
            device.DeviceReset += new System.EventHandler(this.OnResetDevice);
            OnResetDevice(device, null);
       }


        private void Render()
        {
            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target,
                                Color.Maroon, 1.0f, 0);

            
            device.BeginScene();
            kamera("ucak");
            //kamera("Pyramid");
           //kamera("tiger");//Kameraya çizilen nesnenin tiger olduğunu
            //söylüyoruz ona göre bakış açısı değişecek
           // kamera("hazir_nesne");
            nesne.DrawSubset(0);
            nesne.DrawSubset(1);//ucak birden fazla alt nesneye sahip
            //olduğu için bu satırın yorumunu kaldıracağız
            device.EndScene();
            device.Present();
           
        }


        private void OnResetDevice(object sender, EventArgs e)
        {

            device = (Device)sender;
            //Başka nesneler tanımlanacaksa farklı device nesneleri oluşturulacak

            device.RenderState.Lighting = false;//ışık olmasın
            nesne = Mesh.FromFile("airplane 2.x", MeshFlags.Managed, device);

//materyalde alacak     
            
            // nesne = Mesh.FromFile("Pyramid01.x", MeshFlags.Managed, device);//materyalde alacak
           // nesne = Mesh.FromFile("tiger.x", MeshFlags.Managed, device);//materyalde alacak
            //nesne = Mesh.Teapot(device);
           // nesne = Mesh.Sphere(device, 2, 2, 9);
            //hazır nesneleride ekran üzerinde çizdirme şansına sahibiz
            //Tek yapmamız gereken metoda parametre olarak device nesnesini göndermeliyiz
            //ve eğer parametre gerekiyorsa (örneğin küre nesnesinde  çap,dilim...) bunlara uygun
            //değerleri de belirtmemiz gerekmektedir
        
        }



        public void kamera(string gelennesne)
        {
            
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4,
                    800 / 600, 0.0f, 3500.0f);
            if (gelennesne == "Pyramid")
            {
                device.Transform.View = Matrix.LookAtLH(new Vector3(0.0f, 150f, 0.0f), new Vector3(0, 20, 0),
                     new Vector3(0, 0, 1));
            }
            if (gelennesne == "ucak")
            {
                device.Transform.View = Matrix.LookAtLH(new Vector3(15f, 10, 0.0f), new Vector3(0, 0, 0),
                     new Vector3(0, 1, 0));
            }
            if (gelennesne == "tiger")
            {
                device.Transform.View = Matrix.LookAtLH(new Vector3(5f, 0, 0.0f), new Vector3(0, 0, 0),
                     new Vector3(0, 1, 0));
            }

            if (gelennesne == "hazir_nesne")
            {
                device.Transform.View = Matrix.LookAtLH(new Vector3(5f, 4, 0.5f), new Vector3(0, 1, 0),
                     new Vector3(0, 1, 0));
            }


        }
          static void Main()
       {
           meshler frm = new meshler();

           frm.Text = "Yorum satırını kaldırmayı unutmayın";
        
                frm.Show();

                Application.Run(frm);

                

        
            }
        }

    }

