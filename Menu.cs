using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognitionPT
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form buf = new Rec();
            buf.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form buf = new Training();
            buf.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Projekt Rozpoznawanie twarzy i śledzenie ruchu", "INFO", MessageBoxButtons.OK);
        }
    }
}
