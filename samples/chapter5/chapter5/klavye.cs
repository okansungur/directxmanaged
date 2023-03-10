using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

using Microsoft.DirectX.Direct3D;

using keyboard=Microsoft.DirectX.DirectInput;//Klavyeyi kullanmak için
namespace chapter5
{
    class klavye:Form
    {
        Device device;
        Mesh nesne;

        public klavye()
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
       Material[] meshmateryal; 
       Texture[] mesh_doku; 
        ExtendedMaterial[] materyaller = null; 

        public void mesh_yukle() {
            if (mesh_doku == null && materyaller.Length>0)
            {
                                
                mesh_doku = new Texture[materyaller.Length];
                meshmateryal = new Material[materyaller.Length];

                for (int i = 0; i < materyaller.Length; i++)
                {
                    meshmateryal[i] = materyaller[i].Material3D;
                    
                    meshmateryal[i].Ambient = meshmateryal[i].Ambient;
                
                   try
                    {
                        if (materyaller[i].TextureFilename != null && materyaller[i].TextureFilename!=String.Empty)
                            mesh_doku[i] = TextureLoader.FromFile(device, materyaller[i].TextureFilename);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.StackTrace);
                    }
                }

            }

        
        }

        private void Render()
        {
            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target,
                                Color.Pink, 1.0f, 0);
                       
            device.BeginScene();
            kamera();
            
        
           for (int i = 0; i < meshmateryal.Length; i++)
           {
        
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
        

            device.RenderState.Lighting = false;//isik olmasin
            nesne = Mesh.FromFile("ew.x", MeshFlags.Managed, device, out materyaller);//materyalde alacak
      }

        float sag_sol = -8.0f;
        float ileri_geri = 245.0f;
        float asa_yukari = -20.0f;
        
        public void kamera()
        {
            //Geometry.DegreeToRadian(45.0f)
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4,
                    800 / 600, 0.5f, 950.0f);
            
            
            device.Transform.View = Matrix.LookAtLH(new Vector3(10.0f, 0, 0), new Vector3(0, 0, 0),
                     new Vector3(0, 0, 1));

            device.Transform.World = Matrix.Translation(sag_sol, ileri_geri, asa_yukari) * Matrix.RotationZ((float)Math.PI / 2);//görüntüyü düzelttik z yönünde döndürerek
           //buradaki yön degerleri  yukarda verilen görünüm değerlerine göre düzenlenmiştir
            //sag_sol,ileri_geri gibi değişkenlerin yerleri değişebilmektedir.Konuyu daha 
            //iyi anlayabilmek için değerleri değiştirerek denemeler yapmalısınız
            // this.Text = sag_sol + ":" + ileri_geri + ":" + asa_yukari;//koordinatlar
        }
          static void Main()
       {
           klavye frm = new klavye();
           frm.Text = "klavye";
           frm.Show();
           Application.Run(frm);
       }


        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();//escape tuşuyla sınıf yok edilsin

            }

            if (e.KeyCode == Keys.A)
            {
                sag_sol -= 0.9f;//sol

            }

            if (e.KeyCode == Keys.D)
            {
                sag_sol += 0.9f;//sağ

            }

            if (e.KeyCode == Keys.Down)
            {
                ileri_geri += 0.9f;//geri

            }
            if (e.KeyCode == Keys.Up)
            {
                ileri_geri -= 0.9f;//ileri

            }
            if (e.KeyCode == Keys.W)
            {
                asa_yukari -= 0.9f;//yukari


            }
            if (e.KeyCode == Keys.S)
            {
                asa_yukari += 0.9f;//asaği

            }
 

        }


    }
}
