using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
namespace helikopter
{
    class helikopter:Form
    {

        public helikopter(){
            yukle();
        }
         string yol;//.x dosyasinin yolu
         AnimationRootFrame kok;//frameler için kap
         Device device;
        

         //isme göre çagrilan mesh parçasi
        public d3d_frame frame_cagir(string gelen)
        {
            return (d3d_frame)Frame.Find(this.kok.FrameHierarchy, gelen);
        }

        // .x dosyasından hiyerarsi elde edilsin
        public void yukle()
        {
            
            kok = Mesh.LoadHierarchyFromFile(this.yol,
              MeshFlags.Managed,
              this.device,
              new hiyerarsi(this.yol), 
              null); 
        }
         float p_aci = 0.0F;
         Vector3 poz = new Vector3(0, 0, 0);
        //  frame hiyerarşisi render edilsin
        public void Render()
        {
            this.frame_cagir("obj3").Hareket = Matrix.RotationY(this.p_aci);    
            this.RenderFrame(this.kok.FrameHierarchy,Matrix.Identity);     
        }

   public void RenderFrame(Frame frame, Matrix x)
        {
       
            if (frame.FrameSibling != null)
            {
                this.RenderFrame(frame.FrameSibling, x);
            }
        Matrix mat =frame.TransformationMatrix *((d3d_frame)frame).Hareket *x;

       
            if (frame.FrameFirstChild != null)
            {
                this.RenderFrame(frame.FrameFirstChild, mat);
            }
           //Material bilgisi
           if (frame.MeshContainer != null)
            {       
                this.device.Transform.World = mat;
                ExtendedMaterial[] em = frame.MeshContainer.GetMaterials();
                for (int i = 0; i < em.Length; i++)
                {
                    this.device.Material = em[i].Material3D;
                    frame.MeshContainer.MeshData.Mesh.DrawSubset(i);
                }
            }
        }


        public helikopter(string d_yol, Device device1)
        {
            yol = d_yol;
            device = device1;
        }

 
 

        public float Aci
        {
            get
            {
                return this.p_aci;
            }

            set
            {
                this.p_aci = value;
            }
        }



       public Vector3 Konum
        {
            get
            {
                return this.poz;
            }

            set
            {
                this.poz = value;
            }
        }


        
        private void helikopter_Load(object sender, EventArgs e)
        {

        }

    }      
    }

