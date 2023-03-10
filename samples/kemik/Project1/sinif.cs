using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectDraw;
using Direct3D = Microsoft.DirectX.Direct3D;

namespace Project1
{
    public class sinif : Form
    {
        Direct3D.Device device;
        private void device_reset(object sender, EventArgs e)
        {
            Direct3D.Device dev = (Direct3D.Device)sender;
            dev.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4,
             800 / 600, 1, 800.0f);
            dev.Transform.View = Matrix.LookAtLH(new Vector3(-100, 200, -600), new Vector3(), new Vector3(0, 1, 0f));
            // Işıklar
            dev.Lights[0].Type = LightType.Directional;//ışığın türünü belirtiyoruz
            dev.Lights[0].Diffuse = Color.FloralWhite;
            dev.Lights[0].Direction = new Vector3(-150, 50, 30);//ışık kaynağının yönü
            dev.Lights[0].Enabled = true;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            sonrakiframe();
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer,
                Color.CornflowerBlue, 1.0f, 1);
            device.BeginScene();
            cizim((d3d_frame)framelerim.FrameHierarchy);
            device.EndScene();
            device.Present();
            this.Invalidate();
       }
       private void cizim(d3d_frame frame)
        {
            kap mesh = (kap)frame.MeshContainer;

            while (mesh != null)
            {
                meshCont(mesh, frame);

                mesh = (kap)mesh.NextContainer;
            }
            if (frame.FrameSibling != null)
            {
                cizim((d3d_frame)frame.FrameSibling);
            }
            if (frame.FrameFirstChild != null)
            {
                cizim((d3d_frame)frame.FrameFirstChild);
            }
        }
        private void meshCont(kap mesh, d3d_frame frame)
        {
               device.Transform.World = frame.Hareket;
                ExtendedMaterial[] mtrl = mesh.GetMaterials();
                for (int i = 0; i < mtrl.Length; i++)
                {
                    device.Material = mtrl[i].Material3D;
                    device.SetTexture(0, mesh.getDoku()[i]);
                    mesh.MeshData.Mesh.DrawSubset(i);
              }
        }
        private void sonrakiframe()
        {
           zaman = 0.0001f;
           Matrix mx = Matrix.Identity;
            device.Transform.World = mx;
           if (framelerim.AnimationController != null)
                framelerim.AnimationController.AdvanceTime(zaman, null);
           frame_guncelle((d3d_frame)framelerim.FrameHierarchy, mx);
        }
        private void frame_guncelle(d3d_frame frame, Matrix m2)
        {
            //frame sınıfına hareket edilecek alanlar gönderiliyor
            frame.Hareket = frame.TransformationMatrix *m2;  
            //nesnenin hareketi için 
            if (frame.FrameSibling != null)//ilk kardeş null değilse 
            {
                frame_guncelle((d3d_frame)frame.FrameSibling, m2);
            }
            if (frame.FrameFirstChild != null) //ilk çocuk null değilse
            {
                frame_guncelle((d3d_frame)frame.FrameFirstChild,frame.Hareket);
            }
        }
        public sinif()
        {
            this.ClientSize = new Size(800, 600);
            this.Text = "okan";
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }
        static void Main()
        {
            using (sinif frm = new sinif())
            {
               frm.grafik_algila();
                frm.Show();
                Application.Run(frm);
            }
        }
        private AnimationRootFrame framelerim;
        private float zaman;
        private void animasyon(string dosya, PresentParameters parametre)
        {
           hiyerarsi alloc = new hiyerarsi(this);
            framelerim = Mesh.LoadHierarchyFromFile(dosya, MeshFlags.Managed,
                device, alloc, null);
         }
        PresentParameters parametre;
        public void grafik_algila()
        {
           parametre = new PresentParameters();
           parametre.Windowed = true;
            parametre.SwapEffect = SwapEffect.Discard;
            parametre.AutoDepthStencilFormat = DepthFormat.D16;
            parametre.EnableAutoDepthStencil = true;
    
            Direct3D.Caps hardware = Manager.GetDeviceCaps(0, DeviceType.Hardware);
//            this.Text = hardware.MaxVertexBlendMatrices + "";
            if (hardware.MaxVertexBlendMatrices >= 4)
            {
                //ekran  kartı MaxVertexBlendMatrices özelliğini destekliyorsa
           
                device = new Direct3D.Device(0, DeviceType.Hardware, this, Direct3D.CreateFlags.HardwareVertexProcessing | Direct3D.CreateFlags.PureDevice, parametre);
           //ekran kartı destekliyorsa Hardware olarak çaliştir    
            }
            else
            {
            //ekran kartı desteklemiyorsa software olarak çaliştir

                device = new Direct3D.Device(0, DeviceType.Reference, this,
                    Direct3D.CreateFlags.SoftwareVertexProcessing, parametre);
            }
           animasyon("s.x", parametre);
            device.DeviceReset += new EventHandler(device_reset);
          device_reset(device, null);
       }
   }
}
