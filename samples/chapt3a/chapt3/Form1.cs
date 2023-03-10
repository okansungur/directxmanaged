using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
namespace chapt3
{
    public partial class Form1 : Form
    {
        Device device;
         CustomVertex.PositionTextured[] vertices;
         Texture texture;
         Material material;

        public Form1()
        {
            InitializeComponent();
            grafik_algila();
            Kamera();
            ucgenciz();
            doku();
           

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

        public void grafik_algila()
        {
            PresentParameters parametre = new PresentParameters();
            parametre.Windowed = true;
            parametre.SwapEffect = SwapEffect.Discard;

            parametre.EnableAutoDepthStencil = true;//Beraber kullanılır
            parametre.AutoDepthStencilFormat = DepthFormat.D16;//16 bit
            //AutoDepthStencilFormat 
            //3 boyutlu ortamdaki nesneyi 2 boyutlu ekran ortamına 
            //getirdiğimizde derinlik kaybolur bu yüzden AutoDepthStencilFormat
            //özelliği tanımlanır.
            //Örnek olarak gölgelendirme efektlerini verebiliriz 
            //nesnelerin hangisinin daha önde olacağıda belirlenebilmektedir
            //Bu sayede Direct3D tarafından arkaplandaki üçgenler
            //render esnasında gözardı edilir
            //DepthFormat.D16 Bufferda herbir değeri tutmak için kaç
            //bit kullanılacağını gösterir.
            
         
            device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, parametre);

            device.RenderState.Lighting = false;



        }
        private void Kamera()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 0.3f, 500f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, 30), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        }



        protected override void OnPaint(PaintEventArgs pea)
        {
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.AliceBlue, 1.0f, 0);
            device.BeginScene();
            
            device.VertexFormat = CustomVertex.PositionTextured.Format;
            device.SetTexture(0, texture);
            device.DrawUserPrimitives(PrimitiveType.TriangleList, 2, vertices);
            //2 üçgen çizilecek parametreye dikkat!!!
            

            ucgenciz();
            
            device.EndScene();
            device.Present();
            this.Invalidate();
        }

        void ucgenciz() {

            vertices = new CustomVertex.PositionTextured[6];
            //2 üçgen 6 elemanlı dizi

            vertices[0].Position = new Vector3(15f, 15f, 0f);
            vertices[0].Tu = 0;
            vertices[0].Tv = 0;

            vertices[1].Position = new Vector3(-15f, -15f, 0f);
            vertices[1].Tu = 1;
            vertices[1].Tv = 1;

            vertices[2].Position = new Vector3(15f, -15f, 0f);
            vertices[2].Tu = 0;
            vertices[2].Tv = 1;
//1. üçgen 

            vertices[3].Position = new Vector3(-15.1f, -14.9f, 0f);
            vertices[3].Tu = 1;
            vertices[3].Tv = 1;
            //buradaki üçgeni biraz ayıracağız diğerinden eğer tüm
            //degerleri 15 yapsaydık resim bitişik görünecekti
            vertices[4].Position = new Vector3(14.9f, 15.1f, 0f);
            vertices[4].Tu = 0;
            vertices[4].Tv = 0;

            vertices[5].Position = new Vector3(-15.1f, 15.1f, 0f);
            vertices[5].Tu = 1;
            vertices[5].Tv = 0;
            //2. üçgen      
  
        }


        void doku() { 
        
      
             material = new Material();
 
             material.Diffuse = Color.White;
             material.Specular = Color.LightGray;
            //cismin ışık altına parlaklığını temsil eder
             material.SpecularSharpness = 10.0F;
            //değer yükseltildiğinde nesne daha parlak görünür
            //(Işık dikkate alınmalı)
 
             device.Material = material;

             texture = TextureLoader.FromFile(device, "a.jpg");//Microsoft.Direct3DX eklenecek 
      

        
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}