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

namespace chapter4_A
{
    class meshes : System.Windows.Forms.Form
    {

        Device device;
        Mesh nesne;

        public meshes()
        {
            grafik_algila();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

       protected override void OnPaint(PaintEventArgs e)
        {
            Render();
            Invalidate();
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
            mesh_yukle();
       }
       Material[] meshmateryal; // 
       Texture[] mesh_doku; // mesh_dokular
        ExtendedMaterial[] materyaller = null; 
         //altkumedeki doku ve materyali bu  dizinin içinde tutacağız.
        //buradaki dizinin degerleri ".x" dosyası okunduktan sonra atanır.
        public void mesh_yukle() {
            if (mesh_doku == null && materyaller.Length>0)
            {
                //Kaç tane alt nesne olduğunu sayı olarak bilemeyiz 
                //ancak gerek doku gerekse materyal özellikleri için
                //gerekli olan bilgileri bir dizi içerisinde tutup herbir alt nesneye teker teker uygulayacağız
                mesh_doku = new Texture[materyaller.Length];
                meshmateryal = new Material[materyaller.Length];
                for (int i = 0; i < materyaller.Length; i++)
                {
                    meshmateryal[i] = materyaller[i].Material3D;
                    meshmateryal[i].Ambient = meshmateryal[i].Ambient;
                    // dokuların alınacağı dosyaları ".x" dosyasının yanında yeralmalı
                    try
                    {
                  if (materyaller[i].TextureFilename != null && materyaller[i].TextureFilename != String.Empty)
                          mesh_doku[i] = TextureLoader.FromFile(device, materyaller[i].TextureFilename);
                    }
                    catch (Exception ex)
                    { MessageBox.Show(ex.StackTrace); }
                    }
                }     
        }

        private void Render()
        {
            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target,
                                Color.Pink, 1.0f, 0);
                       
            device.BeginScene();
            kamera();
            
        //**Materyal ve dokuların atanması gerçekleşiyor
           for (int i = 0; i < meshmateryal.Length; i++)
           {
               // kac tane alt nesne varsa ona göre mesh_doku ve materyal atansin
               device.Material = meshmateryal[i];
               device.SetTexture(0, mesh_doku[i]);
               nesne.DrawSubset(i);
           }
          
            device.EndScene();
            device.Present();
           
        }
     private void OnResetDevice(object sender, EventArgs e)
        {

            device = (Device)sender;
            //Baska nesneler tanimlanacaksa farkli device nesneleri olusturulacak

            device.RenderState.Lighting = false;//isik olmasin
            nesne = Mesh.FromFile("airplane 2.x", MeshFlags.Managed, device, out materyaller);//materyalde alacak
      }

        public void kamera()
        {
            
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4,
                    800 / 600, 0.0f, 3500.0f);

        
                device.Transform.View = Matrix.LookAtLH(new Vector3(15f, 0, 0.0f), new Vector3(0, 0, 0),
                     new Vector3(0, 1, 0));
   
        }
          static void Main()
       {
           meshes frm = new meshes();
           frm.Text = "Ucak";
           frm.Show();
           Application.Run(frm);
       }


    }
}
