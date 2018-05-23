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
    public partial class WinWindow : Form
    {
        public WinWindow()
        {
            InitializeComponent();
        }

        public int g, b;

        private void okButton_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.pvPGame = false;
            form1.pvAIGame = false;
            form1.aivAIGame = false;
            this.Close();
        }

        private void WinWindow_Load(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();

            if (g > b)
            {
                playerName.Text = form1.greenPlayerName + "wins";
            }
            else if (g == b)
            {
                playerName.Text = "Draw";
            }
            else
            {
                playerName.Text = form1.brownPlayerName + "wins";
            }

            scoreLabel.Text = form1.greenFieldWinWindow.ToString() + ":" + form1.brownFieldWinWindow.ToString();
        }
    }
}
