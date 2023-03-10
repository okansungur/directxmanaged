using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
namespace chapter2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Direct3D  Manager sınıfı sayesinde donanımla;  yani ekran kartının özellikleri ve  desteklediği çözünürlüklerle ilgilibilgi elde edebiliriz.
        private void button1_Click(object sender, EventArgs e)
        {
    listBox1.Items.Add(Manager.Adapters[0].Information.DriverName + ":" + Manager.Adapters[0].Information.DriverVersion);
    listBox1.Items.Add(Manager.Adapters[0].CurrentDisplayMode.Width + ":" + Manager.Adapters[0].CurrentDisplayMode.Height + "" + Manager.Adapters[0].CurrentDisplayMode.Format);
    DisplayModeCollection col= Manager.Adapters[0].SupportedDisplayModes;
     while(col.MoveNext())
      listBox1.Items.Add(col.Current.ToString());
     }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = listBox1.SelectedItem + "\n Özellikler: \r\n\r\n" + Manager.GetDeviceCaps
           (0, DeviceType.Hardware).ToString().Replace("\n", "\r\n");

        }
    }
}