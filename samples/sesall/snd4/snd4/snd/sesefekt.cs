using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.DirectX;
using d3ses=Microsoft.DirectX.DirectSound;



namespace snd
{
    class sesefekt: System.Windows.Forms.Form
   {
        
     
        private Button button1;
        private Label label1;
        private Label label2;
        private TrackBar trackBar2;
        private Label label3;
        private TrackBar trackBar3;
        private CheckBox checkBox1;
        private TrackBar trackBar1;
      
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(35, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "MonoSes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(301, 146);
            this.trackBar1.Maximum = 0;
            this.trackBar1.Minimum = -10000;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 45);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.TickFrequency = 500;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(206, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ses:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(206, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Frekans:";
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(301, 12);
            this.trackBar2.Maximum = 300;
            this.trackBar2.Minimum = 1;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(104, 45);
            this.trackBar2.TabIndex = 4;
            this.trackBar2.TickFrequency = 10;
            this.trackBar2.Value = 100;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(206, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Hoparlör(sag-sol):";
            // 
            // trackBar3
            // 
            this.trackBar3.LargeChange = 500;
            this.trackBar3.Location = new System.Drawing.Point(301, 79);
            this.trackBar3.Maximum = 10000;
            this.trackBar3.Minimum = -10000;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new System.Drawing.Size(104, 45);
            this.trackBar3.TabIndex = 6;
            this.trackBar3.TickFrequency = 1000;
            this.trackBar3.Scroll += new System.EventHandler(this.trackBar3_Scroll);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.checkBox1.Location = new System.Drawing.Point(51, 40);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(59, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Normali";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // sesefekt
            // 
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(470, 188);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.trackBar3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.button1);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.Name = "sesefekt";
            this.Text = "Pan, Volume, Frequency ";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



  
        d3ses.Device device_ses;
        d3ses.Buffer buf;
        void ses() {
            device_ses = new d3ses.Device();
            buf = new d3ses.Buffer("b.wav", device_ses);
            device_ses.SetCooperativeLevel(this, d3ses.CooperativeLevel.Normal);

        }
     private void button1_Click_2(object sender, EventArgs e)
        {
            ses();
            buf.Play(0, d3ses.BufferPlayFlags.Default);
            buf.Pan = trackBar1.Value;
            buf.Frequency = trackBar2.Value * buf.Format.SamplesPerSecond / 200;
            buf.Pan = trackBar3.Value;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            buf.Volume = trackBar1.Value;//maximum sesle dosya kaydedilmeli
       }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            this.Text = buf.Format.SamplesPerSecond + "";
             buf.Frequency = trackBar2.Value * buf.Format.SamplesPerSecond / 200;
           
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            buf.Pan = trackBar3.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            buf.Frequency = buf.Format.SamplesPerSecond;
        }
        static void Main()
        {
            using (sesefekt frm = new sesefekt())
            {
                frm.Show();
                frm.InitializeComponent();
                Application.Run(frm);
            }
        }
        
    }
}
