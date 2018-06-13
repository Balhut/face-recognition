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
    public partial class Training : Form
    {
        Image<Bgr, Byte> currentImg;
        Capture capture;
        HaarCascade face;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5, 0.5);
        Image<Gray, byte> result, faceT = null, grayImg = null, facebox=null;

        List<Image<Gray, byte>> imagesT = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> Pearsons = new List<string>();
        List<int> photoNumber = new List<int>();
        List<int> photoCounter = new List<int>();
        List<string> people = new List<string>();
        string name, names;




        int j, labelsIndex, peopleIndex, recognized, people_cout=0, example_cout = 2, photo_cout = 0;

        public Training()
        {
            InitializeComponent();
            pictureBox3.Image = Image.FromFile("ex.jpg");
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            try
            {
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "labels.txt");
                string[] Labels = Labelsinfo.Split('#');
                labelsIndex = Convert.ToInt16(Labels[0]);
                recognized = labelsIndex;
                string buforFaces;

                for (int tf = 1; tf < labelsIndex + 1; tf++)
                {
                    buforFaces = "pearson" + tf + ".bmp";
                    imagesT.Add(new Image<Gray, byte>(Application.StartupPath + buforFaces));
                    labels.Add(Labels[tf]);
                }

                string Peopleinfo = File.ReadAllText(Application.StartupPath + "people.txt");
                string PhotoNumberinfo = File.ReadAllText(Application.StartupPath + "photonumber.txt");
                string PhotoCounterinfo = File.ReadAllText(Application.StartupPath + "photocounter.txt");
                string[] People = Peopleinfo.Split('#');
                peopleIndex = Convert.ToInt16(People[0]);
                people_cout = peopleIndex;
                string [] PhotoNumber = PhotoNumberinfo.Split('#');
                string[] PhotoCounter = PhotoCounterinfo.Split('#');


                for (int a = 1; a < peopleIndex + 1; a++)
                {
                    people.Add(People[a]);
                    photoCounter.Add(Convert.ToInt16(PhotoCounter[a]));
                    photoNumber.Add(Convert.ToInt16(PhotoNumber[a]));
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
                j = j + 1;
                result = currentImg.Copy(f.rect).Convert<Gray, byte>().Resize(200, 200, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

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

        private void button1_Click(object sender, EventArgs e)
        {

            if (example_cout == 6)
            {
                this.Hide();
                photo_cout = 0;
            }

         

            try
            {
            

                recognized++;

                grayImg = capture.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                MCvAvgComp[][] detectedFaces = grayImg.DetectHaarCascade(face, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));


                string tekst = ("Dodaj " + example_cout + " zdjecie.");
                label4.Text = (tekst);
                string path_ex = ("ex" + example_cout + ".jpg");
                pictureBox3.Image = Image.FromFile(path_ex);



                foreach (MCvAvgComp face in detectedFaces[0])
                {
                    faceT = currentImg.Copy(face.rect).Convert<Gray, byte>();
                    break;
                }

                faceT = result.Resize(200, 200, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                imagesT.Add(faceT);
                labels.Add(textBox1.Text);
                photo_cout++;

                if (example_cout == 2)
                {
                    //people_cout++;
                    people.Add(textBox1.Text);
                    photoNumber.Add(recognized);
                    photoCounter.Add(photo_cout);
                }
          

                if (example_cout > 2)
                {
                    photoCounter.RemoveAt(photoCounter.Count - 1);
                    photoCounter.Add(photo_cout);
                }
               

                textBox1.Enabled = false;
                example_cout++;


                facebox = result.Resize(200, 200, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                pictureBox2.Image = facebox.ToBitmap();

                File.WriteAllText(Application.StartupPath + "labels.txt", imagesT.ToArray().Length.ToString() + "#");
                File.WriteAllText(Application.StartupPath + "people.txt", people.ToArray().Length.ToString() + "#");
                File.WriteAllText(Application.StartupPath + "photocounter.txt", photoCounter.ToArray().Length.ToString() + "#");
                File.WriteAllText(Application.StartupPath + "photonumber.txt", photoNumber.ToArray().Length.ToString() + "#");


                for (int i = 1; i < imagesT.ToArray().Length + 1; i++)
                {
                    imagesT.ToArray()[i - 1].Save(Application.StartupPath + "pearson" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "labels.txt", labels.ToArray()[i - 1] + "#");
                }

            for (int i = 1; i < people.ToArray().Length + 1; i++)
                {
                   File.AppendAllText(Application.StartupPath + "people.txt", people.ToArray()[i - 1] + "#");
                   File.AppendAllText(Application.StartupPath + "photonumber.txt", photoNumber.ToArray()[i - 1] + "#");
                   File.AppendAllText(Application.StartupPath + "photocounter.txt", photoCounter.ToArray()[i - 1] + "#");
                }


                MessageBox.Show("Pomyślnie dodano osobę/zdjecie do bazy danych", "Dodawanie nowej osoby/zdjecia", MessageBoxButtons.OK);

            }
            catch (Exception e1)
            {
                MessageBox.Show("Nie wykryto żadnej osoby", "Błąd dodawania nowej osoby", MessageBoxButtons.OK);
            
            }
        }
    }
}
