using Life.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Core.Managers
{
    public static class CalcGeneration
    {
        public static Cell[,] StartGeneration(int size, int rows, int cols, int plotnost)
        {
            /*size = (int)sizeUpDown.Value;

            rows = pictureBox1.Height / size;
            cols = pictureBox1.Width / size;
            */
            Cell[,] res = new Cell[cols, rows];

            Random random = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    res[x, y] = new Cell() { IsLife = (random.Next(plotnost) == 0) };
                }
            }

            return res;

        }

        public static int SosediCount(Cell[,] cells, int x, int y, int rows, int cols)
        {
            var res = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = x + i;
                    var row = y + j;

                    if ((col < 0) || (row < 0) || (col >= cols) || (row >= rows) || ((col == x) && (row == y)))
                    {
                        continue;
                    }

                    if (cells[col, row].IsLife) res++;
                }
            }
            return res;
        }

        public static Cell[,] NextGeneration(Cell[,] current, int rows, int cols)
        {
            var newFiled = new Cell[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var countSosedi = SosediCount(current, x, y, rows, cols);
                    var currentCell = current[x, y];
                    var isLife = currentCell.IsLife;

                    if (isLife)
                    {
                        if ((countSosedi == 2) | (countSosedi == 3))
                        {
                            newFiled[x, y] = currentCell;
                        }
                        else newFiled[x, y] = new Cell();
                    }
                    else
                    {
                        if (countSosedi == 3)
                        {
                            newFiled[x, y] = new Cell() { IsLife = true };
                        }
                        else newFiled[x, y] = currentCell;
                    }

                }

            }
            return newFiled;
        }
    }
}
