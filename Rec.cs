using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace FaceRecognitionPT
{
    public partial class Rec : Form
    {        
        Image<Bgr, Byte> currentImg;
        Capture capture;
        HaarCascade face;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5, 0.5);
        Image<Gray, byte> result, grayImg = null;

        List<Image<Gray, byte>> imagesT = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> Pearsons = new List<string>();

        string name, names;
        int j, labelsIndex, recognized;

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        public Rec()
        {
            InitializeComponent();
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            try
            {
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "labels.txt");
                string[] labels = Labelsinfo.Split('#');
                labelsIndex = Convert.ToInt16(labels[0]);
                recognized = labelsIndex;
                string buforFaces;

                for (int tf = 1; tf < labelsIndex + 1; tf++)
                {
                    buforFaces = "pearson" + tf + ".bmp";
                    imagesT.Add(new Image<Gray, byte>(Application.StartupPath + buforFaces));
                    this.labels.Add(labels[tf]);
                }

                //camera on
                capture = new Capture();
                capture.QueryFrame();
                Application.Idle += new EventHandler(FrameGrabber);

            }
            catch (Exception e)
            {

            }
        }

        public void FrameGrabber(object sender, EventArgs e)
        {
            Pearsons.Add("");
            currentImg = capture.QueryFrame().Resize(480, 320, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            grayImg = currentImg.Convert<Gray, Byte>();

            MCvAvgComp[][] facesDetected = grayImg.DetectHaarCascade(face, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

            foreach (MCvAvgComp f in facesDetected[0])
            {
                j++;
                result = currentImg.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                currentImg.Draw(f.rect, new Bgr(Color.OrangeRed), 2);


                if (imagesT.ToArray().Length != 0)
                {
                    MCvTermCriteria termCrit = new MCvTermCriteria(recognized, 0.001);
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(imagesT.ToArray(), labels.ToArray(), 3000, ref termCrit);
                    name = recognizer.Recognize(result);
                    currentImg.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.GreenYellow));
                }

                Pearsons[j - 1] = name;
                Pearsons.Add("");
            }

            j = 0;

            for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                names = names + Pearsons[nnn] + ", ";
            }

            pictureBox1.Image = currentImg.ToBitmap();
            names = "";
            Pearsons.Clear();
        }

    }
}
