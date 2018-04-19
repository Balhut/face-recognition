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
    public partial class Training : Form
    {
        public Training()
        {
            InitializeComponent();
        }

        VideoCapture camera;
        CascadeClassifier cclassifier;
        private void Training_Load(object sender, EventArgs e)
        {
            camera = new VideoCapture(0);
            cclassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            using (Image<Bgr, byte> img = camera.QueryFrame().ToImage<Bgr, Byte>())
            {
                if (img != null)
                {
                    Image<Gray, byte> greyScaleGoruntu = img.Convert<Gray, byte>();
                    Rectangle[] rectangles = cclassifier.DetectMultiScale(greyScaleGoruntu, 1.4, 1, new Size(100, 100), new Size(800, 800));
                    foreach (var face in rectangles)
                    {
                        img.Draw(face, new Bgr(0, 255, 0), 3);
                    }
                    pictureBox1.Image = img.ToBitmap();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            pictureBox1.Image = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
