using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi
{
    public partial class NameWindow : Form
    {
        public string greenPlayerName;
        public string brownPlayerName;
        public bool isOKClicked = false;
        public bool isCancelClicked = false;
        public NameWindow()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            greenPlayerName = greenTextBox.Text;
            brownPlayerName = brownTextBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            greenPlayerName = "";
            brownPlayerName = "";
            this.Close();
        }
    }
}
