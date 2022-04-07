using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace NiceMineSweeper
{
    public class GameBoard : Panel //whats behind the scenes
    {
        public Game parentGame;
        //-1 bomb, 0 empty square, 1-8 numbers
        public int[,] fields;

        private Random randomGen = new Random(); // to generate mines radomly

        private int[] fieldCount;
        private int mineCount;

        public GameBoard(Game ParentGame, int[] FieldCount, int MineCount)
        {
            this.parentGame = ParentGame;
            fieldCount = FieldCount;
            mineCount = MineCount;
            fields = GenerateValues();
            InitializeControls();
        }

        public int[,] GenerateValues() // make the whole 
        {
            //initialize the field with size
            int[,] fields = new int[fieldCount[0], fieldCount[1]];

            //fill on the bombs
            for (int i = 0; i < mineCount; i++)
            {
                int randX = randomGen.Next(0,fieldCount[0]);
                int randY = randomGen.Next(0,fieldCount[1]);
                if (fields[randX, randY] != -1)
                {
                    fields[randX, randY] = -1;
                }
                else
                {
                    i--;
                }
            }

            fields = FillNumbers(fields);

            return fields;
        }

        public int[,] FillNumbers(int[,] mineField)
        {
            int[,] numbers = mineField;
            //iterate through all members
            for (int x = 0; x < fieldCount[0]; x++)
            {
                for (int y = 0; y < fieldCount[1]; y++)
                {
                    //if current field is mine we dont need to check surroundings
                    if (numbers[x,y] == -1)
                    {
                        continue;
                    }

                    //now we got field to check
                    int count = 0;


                    //check surroundings
                    for (int sx = x-1; sx <= x+1; sx++)
                    {

                        if (sx < 0 || sx >= fieldCount[0]) //if x out of range
                        {
                            continue;
                        }
                        for (int sy = y-1; sy <= y+1; sy++)
                        {
                            if (sy < 0 || sy >= fieldCount[1]) //if y out of range
                            {
                                continue;
                            }

                            //we checked that the coordinates are not out of range and now we will count that down
                            if (numbers[sx,sy] == -1) //is the bomb
                            {
                                count++;
                            }           
                        }
                    }
                    numbers[x, y] = count;
                }
            }
            return numbers;
        }

        public void InitializeControls()
        {
            this.Controls.Clear();
            int[,] fields = this.fields;

            for (int y = 0; y < fieldCount[1]; y++) // 10 x 10, 20 x 20 or 30 x 30
            {
                for (int x = 0; x < fieldCount[0]; x++)
                {
                    GameField gf = new GameField(this, fields[x, y], x, y);
                    gf.Location = new Point(x * gf.Width, y * gf.Height);
                    this.Controls.Add(gf);
                }
            }
        }

        public void ReavealAllFields()
        {
            foreach (GameField gf in this.Controls)
            {
                gf.state = 1;
                gf.UpdateField();
            }
        }

        public void ReavealAllMines()
        {
            foreach (GameField gf in this.Controls)
            {
                if (gf.value == -1)
                {
                    gf.state = 1;
                    gf.UpdateField();
                }
            }
        }

        public void RevealEmptyNeighbours(GameField gf) // for when clicking at empty squares
        {
            int starX = gf.x, startY = gf.y;
            List<GameField> visited = new List<GameField>(); // this could be an array of boolean too
            List<GameField> unvisited = new List<GameField>(); // our queue

            unvisited.Add(gf);

            GameField current;

            //check for all free neighbours
            while (unvisited.Count > 0) // like bfs... kind of
            {
                current = unvisited[0]; 

                for (int x = current.x - 1; x <= current.x + 1; x++)
                {
                    if (x < 0 || x >= fieldCount[0]) //if x out of range
                    {
                        continue;
                    }
                    for (int y = current.y - 1; y <= current.y + 1; y++)
                    {
                        if (y < 0 || y >= fieldCount[1]) //if y out of range
                        {
                            continue;
                        }
                        //checked that the coordinates are not out of range and now we will mark all empty squares
                        GameField neighbour = GetFieldByCoordinates(x,y);
                        if (!visited.Contains(neighbour) && !unvisited.Contains(neighbour) && neighbour.state == 0)
                        {
                            if (neighbour.value == 0)
                            {
                                unvisited.Add(neighbour);
                            }
                            else if (neighbour.value > 0)
                            {
                                visited.Add(neighbour);
                            }
                            else
                            {
                                visited.Add(neighbour);
                            }
                        }
                    }
                }
                //after we checked every neighbour we put current into visited
                visited.Add(current);
                unvisited.Remove(current);
            }
            //reveal all final
            foreach (GameField gameField in visited)
            {
                gameField.state = 1;
                gameField.UpdateField();
                parentGame.FieldRevealed();
            }
        }

        private GameField GetFieldByCoordinates(int X, int Y)
        {
            foreach (GameField gf in this.Controls)
            {
                if (gf.x == X && gf.y == Y)
                {
                    return gf;
                }
            }
            return null;
        }
         
    }
}
