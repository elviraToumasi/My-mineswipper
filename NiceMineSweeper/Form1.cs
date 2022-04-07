using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace NiceMineSweeper
{
    public partial class Form1 : Form  //form details and settings
    {
        Game game;
        public Form1()
        {
            InitializeComponent();
        }
 
        //Game Tab
        private void Easy30X30ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame(0);
        }

        private void Medium50X50ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame(1);
        }

        private void Hard100X100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame(2);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Help Tab
        private void AboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://en.wikipedia.org/wiki/Minesweeper_(video_game)");
        }

        private void HowToPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=93oSIfWN0HU");
        }

        private void ShowAllFieldsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (game != null)
            {
                game.board.ReavealAllFields();
            }
        }

        private void NewGame(int difficulty)
        {
            //Hide all pictures
            pictureBoxLogo.Hide();
            //  pictureBoxGameOver.Hide();

            int sizeX = 300 + 300 * difficulty;
            int sizeY = 300 + 300 * difficulty + 24; //add top menu size

            if (game != null) //if it is not the first game
            {
                this.Controls.Remove(game.board);
                this.Refresh();
            }

            this.ClientSize = new Size(sizeX, sizeY);
            game = new Game(difficulty); //, pictureBoxGameOver);
            game.board.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
            game.board.Top = 24;
            this.Controls.Add(game.board);    
        }
    }
}
