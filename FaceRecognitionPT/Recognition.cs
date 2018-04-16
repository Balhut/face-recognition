using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace FaceRecognitionPT
{
    public partial class Recognition : Form
    {
        public Recognition()
        {
            InitializeComponent();
        }
 
        VideoCapture cam;
        CascadeClassifier classifier;
        private void Form1_Load(object sender, EventArgs e)
        {
            cam = new VideoCapture(0);
            classifier = new CascadeClassifier("haarcascade_frontalface_default.xml");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            using (Image<Bgr, byte> img = cam.QueryFrame().ToImage<Bgr, Byte>())
            {
                if (img != null)
                {
                    Image<Gray, byte> greyScaleGoruntu = img.Convert<Gray, byte>();
                    Rectangle[] rectangles = classifier.DetectMultiScale(greyScaleGoruntu, 1.4, 1, new Size(100, 100), new Size(800, 800));
                    foreach (var face in rectangles)
                    {
                        img.Draw(face, new Bgr(0, 255, 0), 3);
                        //CvInvoke.PutText(img, "test", new System.Drawing.Point(10, 80), FontFace.HersheyComplex, 1.0, new Bgr(0, 255, 0).MCvScalar);
                        textBox1.Text = face.Width.ToString();
                        textBox2.Text = face.Height.ToString();
                    }
                    pictureBox1.Image = img.ToBitmap();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            pictureBox1.Image = null;
        }
    }
}
