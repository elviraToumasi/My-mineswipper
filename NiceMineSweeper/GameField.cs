using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace NiceMineSweeper
{
    public class GameField : PictureBox  // Here I control all the visuals (pictures etc)
    {
        private GameBoard parentBoard;

        public static bool endGame = false;
        static int width = 30, height = 30;

        public int x,y;

        //state: 0 hidden, 1 revealed, 2 flagged
        public int state = 0;
        //valu: -2 bombExplosion, -1 Bomb, 0 empty 1-8 numbers
        public int value = 0;
        
        public GameField(GameBoard ParentBoard, int Value, int X, int Y)
        {
            x = X;
            y = Y;
            endGame = false;
            parentBoard = ParentBoard;
            this.MouseDown += new MouseEventHandler(Field_Click);
            value = Value;
            this.Size = new Size(width, height);
            this.BackgroundImageLayout = ImageLayout.Zoom;
            UpdateField();
        }

        public void UpdateField()
        {
            switch (state)
            {
                case 0:
                    this.BackgroundImage = Properties.Resources.tileMineSweep;
                    break;
                case 1:
                    switch (value)
                    {
                        case -2:
                            this.BackgroundImage = Properties.Resources.tileMineSweep_mine_explode;
                            break;
                        case -1:
                            this.BackgroundImage = Properties.Resources.tileMineSweep_mine_ok;
                            break;
                        case 0:
                            this.BackgroundImage = Properties.Resources.tileMineSweep_empty;
                            break;
                        case 1:
                            this.BackgroundImage = Properties.Resources.tileMineSweep_1;
                            break;
                        case 2:
                            this.BackgroundImage = Properties.Resources.tileMineSweep_2;
                            break;
                        case 3:
                            this.BackgroundImage = Properties.Resources.tileMineSweep_3;
                            break;
                        case 4:
                            this.BackgroundImage = Properties.Resources.tileMineSweep_4;
                            break;
                        case 5:
                            this.BackgroundImage = Properties.Resources.tileMineSweep_5;
                            break;
                        case 6:
                            this.BackgroundImage = Properties.Resources.tileMineSweep_6;
                            break;
                        case 7:
                            this.BackgroundImage = Properties.Resources.tileMineSweep_7;
                            break;
                        case 8:
                            this.BackgroundImage = Properties.Resources.tileMineSweep_8;
                            break;
                        default:
                            break;
                    }
                    break;
                case 2:
                    this.BackgroundImage = Properties.Resources.tileMineSweep_flag;
                    break;
                default:
                    break;
            }
        }

        private void Field_Click(object sender, MouseEventArgs e) //when right or left clicking
        {
            if (endGame)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            { //reveal and check for game state
                if (state == 0)
                {
                    switch (value)
                    {
                        case -1://-1 is a bomb so make it explode
                            value = -2;
                            //reveal bombs
                            parentBoard.ReavealAllMines();
                            //stop fields from working
                            Game.GameOver(false);
                            break;
                        case 0:// 0 is empty so reveal all empty sqaures
                               parentBoard.RevealEmptyNeighbours(this);
                            break;
                        default:
                            if (value >= 1) // else if it's a number, reveal it
                            {
                                parentBoard.parentGame.FieldRevealed();
                            }
                            break;
                    }
                    state = 1;
                }
            }
            else if (e.Button == MouseButtons.Right) //flag it
            { 
                if (state == 0)//if empty
                {
                    //add flag and check if it was right flag
                    if (value < 0)//if it was a mine and flagging
                    {
                        parentBoard.parentGame.FlagPlaced(true, true);
                    }
                    else //if not mine and flagging
                    {
                        parentBoard.parentGame.FlagPlaced(true, false);
                    }
                    state = 2; // flag it
                }
                else if (state == 2)//if flagged
                {
                    //remove flag and check if it was right flag
                    if (value < 0)//if mine and were removing flag
                    {
                        parentBoard.parentGame.FlagPlaced(false, true);
                    }
                    else//if not mine and removing flag
                    {
                        parentBoard.parentGame.FlagPlaced(false, false);
                    }
                    state = 0; //unflag it
                }

            }
            UpdateField();
        }
    }
}
