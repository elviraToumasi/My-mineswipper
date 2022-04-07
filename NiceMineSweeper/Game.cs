using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace NiceMineSweeper
{
    public class Game 
    {
        //general settings for each difficulty: easy/ medium / hard
        static int[][] fieldCounts = { new int[] { 10, 10 }, new int[] { 20, 20 }, new int[] { 30, 30 } };
        static int[] mineCounts = { 10, 80, 180 };

        public GameBoard board;
        public static PictureBox endGamePicture;

        public int hiddenFields; // these are the fields that are not clicked at
        public int flaggedMines; 
        private int gameTotalFieldCount; // the # of all squares in the current game. Comes from FieldCounts
        private int gameMineCount; // the # of all mines in the current game. Comes from the MineCounts
        public int totalFlagCount;

        public Game(int difficulty)//, PictureBox EndGamePicture)
        {
           // endGamePicture = EndGamePicture;

            board = new GameBoard(this ,fieldCounts[difficulty], mineCounts[difficulty]);

            gameTotalFieldCount = fieldCounts[difficulty][0] * fieldCounts[difficulty][0];
            gameMineCount = mineCounts[difficulty];

            hiddenFields = gameTotalFieldCount;
        }

        public void FieldRevealed() // each time you reveal a square, substruct it from count and check if you won
        {
            gameTotalFieldCount--;
            CheckForEndConditions();
        }

        public void FlagPlaced(bool add, bool mineFlag)
        {
            if (add)
            {
                if (mineFlag)
                {
                    flaggedMines++;
                }
                totalFlagCount++;
            }
            else
            {
                if (mineFlag)
                {
                    flaggedMines--;
                }
                totalFlagCount--;
            }
            CheckForEndConditions();
        }

        private void CheckForEndConditions() //when do you win the game?
        {
            if (gameTotalFieldCount == gameMineCount)
            {
                //won
                GameOver(true);
            }else if (flaggedMines == gameMineCount && totalFlagCount == gameMineCount)
            {
                //won again
                GameOver(true);
            }
        }

        public static void GameOver(bool won) // changed the pictures "you won!" and " you lose!" with messageboxes
        {
            GameField.endGame = true;
            if (won)
            {
                MessageBox.Show("You won!");
               // endGamePicture.BackColor = Color.LightGreen;
                //endGamePicture.BackgroundImage = Properties.Resources.youwin;
            }
            else
            {
                MessageBox.Show("You Lost!");
                //endGamePicture.BackColor = Color.PaleVioletRed;
                //endGamePicture.BackgroundImage = Properties.Resources.youloose;
            }

            //endGamePicture.Show();
        }
    }

}

