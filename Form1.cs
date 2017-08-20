using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace LicenseDll
{
    public partial class Form1 : Form
    {
        const string Request_code_here = "Request code here.";
        const string MUTE = "MUTE";
        const string UNMUTE = "UNMUTE";
        const int RECT_SIZE = 11;
        // Битмап гифки
        Bitmap bm = null;
        // Аудио поток
        static Stream stream = WindowsFormsApplication1.Properties.Resources.music;
        SoundPlayer soundPlayer = new SoundPlayer(stream);

        public Form1()
        {
            InitializeComponent();
            soundPlayer.Play();
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text.ToUpper();
            textBox2.Text = textBox2.Text.Replace("-", "");
            cAesEx c_aes = new cAesEx();
            string num = c_aes.Ty(textBox2.Text);
            textBox1.Text = num;
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox2.Text == Request_code_here)
                textBox2.Text = "";
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Text = Request_code_here;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bm);
            Random rnd = new Random();
            Color color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 20; x++)
                    if (x == 0 || y == 0 || x == 19 || y == 9)
                    {
                        int firstX = x * RECT_SIZE + x * 2;
                        int firstY = y * RECT_SIZE + y * 2;
                        Rectangle rect = new Rectangle(
                            new Point(firstX, firstY),
                            new Size(RECT_SIZE, RECT_SIZE));
                        SolidBrush brush = new SolidBrush(color);
                        g.FillRectangle(brush, rect);
                    }
            pictureBox1.Image = bm;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == MUTE)
            {
                soundPlayer.Stop();
                button3.Text = UNMUTE;
            }
            else
            {
                soundPlayer.Play();
                button3.Text = MUTE;
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
        }
    }
}
