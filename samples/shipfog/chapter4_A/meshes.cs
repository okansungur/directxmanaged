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
                    // dokuların alınacağı dosyaları ".x" dosyasının yanında yeralmalı
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
                                Color.BlanchedAlmond, 1.0f, 0);
                       
            device.BeginScene();
            isik();
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
        

            device.RenderState.Lighting = true;
            nesne = Mesh.FromFile("gemumuz.x", MeshFlags.Managed, device, out materyaller);//materyalde alacak
            device.Lights[0].Type = LightType.Directional;
            device.Lights[0].Diffuse = Color.White;
            device.Lights[0].Direction = new Vector3(0, 10, 180);

            device.Lights[0].Enabled = true;
      }


   
        void isik() {
            
            device.Lights[0].Type = LightType.Directional;
            device.Lights[0].Diffuse = Color.White;
            device.Lights[0].Direction = new Vector3(1000,-5000 , 1000);//isik kaynaginin yönü

            device.Lights[0].Enabled = true;

//sis vermek için kullandık yoğunluğu başlangıç ve bitiş noktalarını ayarlayabilmekteyiz
            device.RenderState.FogColor = Color.White;
            device.RenderState.FogDensity = 0.2f;
            device.RenderState.FogVertexMode = FogMode.Linear;
            device.RenderState.FogStart= 0;
            device.RenderState.FogEnd = 100;
            device.RenderState.FogEnable = true;

        }
        float cevir = 100;
        bool yon;
        float sagsol = 0;
        void cevirme() {
            if (yon)
            {
                cevir += 1;
                sagsol += 2;
            }

            if (yon == false) {
                cevir -= 1;
                sagsol -= 2;
            }

            if (cevir >110) {

                yon = false;
            }

            if (cevir < 50)
            {

                yon = true;
            }

        }


        public void kamera()
        {
       
           
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 2,
                    800 / 800, 10.0f, 900.0f);

          
         cevirme();
                device.Transform.View = Matrix.LookAtLH(new Vector3(sagsol, 40, cevir), new Vector3(0, 0, 0),
                     new Vector3(0, 1, 0));
   
        }
          static void Main()
       {
           meshes frm = new meshes();
           frm.Text = "Gemi";
           frm.SetBounds(0, 0, 800, 600);
           frm.Show();
           Application.Run(frm);
       }


    }
}
