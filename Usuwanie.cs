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
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;

namespace FaceRecognitionPT
{
    public partial class Usuwanie : Form
    {

        List<Image<Gray, byte>> imagesT = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> Pearsons = new List<string>();
        List<int> photoNumber = new List<int>();
        List<int> photoCounter = new List<int>();
        List<string> people = new List<string>();

        int j, labelsIndex, peopleIndex, recognized, people_cout = 0, selected_i;

        private void button3_Click(object sender, EventArgs e)
        {
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


            MessageBox.Show("Pomyślnie zapisano zmiany", "Pomyslnie zapisano zmiany", MessageBoxButtons.OK);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = people[selected_i];
            people.Remove(name);
            labels.Remove(name);
            int first_foto_id = photoNumber[selected_i];
            int number_photo = photoCounter[selected_i];
            //imagesT.RemoveAll(s => s. >= selected_i && s.selected_i)
            photoNumber.Remove(selected_i);
            photoCounter.Remove(selected_i);

        }

        public Usuwanie()
        {
            InitializeComponent();

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
            string[] PhotoNumber = PhotoNumberinfo.Split('#');
            string[] PhotoCounter = PhotoCounterinfo.Split('#');


            for (int a = 1; a < peopleIndex + 1; a++)
            {
                people.Add(People[a]);
                photoCounter.Add(Convert.ToInt16(PhotoCounter[a]));
                photoNumber.Add(Convert.ToInt16(PhotoNumber[a]));
            }

           // listBox1.Items.Clear();
            listBox1.Items.AddRange(people.ToArray());
           

        }


        private void button1_Click(object sender, EventArgs e)
        {
            selected_i = listBox1.SelectedIndex;
            textBox1.Text = selected_i.ToString();



        }
    }
}
