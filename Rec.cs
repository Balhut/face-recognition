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
        List<string> Recognation = new List<string>();
        bool alreadyExist = false;
        int zapis = 1;








    string name, names;
        int j, labelsIndex, recognized;

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Recognation.ToArray().Length; i++)
            {
                File.WriteAllText(Application.StartupPath + "listaobecnosci.txt", Recognation.ToArray()[i] + ";");
            }
            MessageBox.Show("Lista zgrana do pliku listaobecnosci.txt", "Lista zgrana!", MessageBoxButtons.OK);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Recognation.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (zapis==1)
            { zapis = 0; }
            else { zapis = 1; }
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
                result = currentImg.Copy(f.rect).Convert<Gray, byte>().Resize(200, 200, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                currentImg.Draw(f.rect, new Bgr(Color.OrangeRed), 2);


                if (imagesT.ToArray().Length != 0)
                {
                    MCvTermCriteria termCrit = new MCvTermCriteria(recognized, 0.001);
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(imagesT.ToArray(), labels.ToArray(), 3000, ref termCrit);
                    name = recognizer.Recognize(result);
                    currentImg.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.GreenYellow));
                    alreadyExist = Recognation.Contains(name);
                    if (!alreadyExist && name != "" && zapis==1) {
                        Recognation.Add(name);
                        listBox1.Items.Clear();
                        listBox1.Items.AddRange(Recognation.ToArray());
                    }
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
