using Life.Server.Core.Contracts.Managers;
using Life.Server.Core.Contracts.Domain;

namespace Life.Server.Infrastructure.Managers
{
    public class GameLoop<T> : IGameLoop<T> where T : ICellIsLife, new ()
    {
        public static Dictionary<string, T[,]> Cells = new Dictionary<string, T[,]>();
        //public static T[,] Cells;

        public static T[,] StartGeneration(int perHorizontal, int perVertical, int density)
        {
            T[,] res = new T[perHorizontal, perVertical];

            Random random = new Random();
            for (int x = 0; x < perHorizontal; x++)
            {
                for (int y = 0; y < perVertical; y++)
                {
                    res[x, y] = new T() { IsLife = random.Next(density) == 0 };
                }
            }

            return res;
        }

        public static T[,] NextGeneration(T[,] current)
        {
            var perHorizontal = current.GetLength(0);
            var perVertical = current.GetLength(1);

            var newFiled = new T[perHorizontal, perVertical];

            for (int x = 0; x < perHorizontal; x++)
            {
                for (int y = 0; y < perVertical; y++)
                {
                    var countSosedi = NeighboursCount(current, x, y, perHorizontal, perVertical);
                    var currentCell = current[x, y];
                    var isLife = currentCell.IsLife;

                    if (isLife)
                    {
                        if ((countSosedi == 2) | (countSosedi == 3))
                        {
                            newFiled[x, y] = currentCell;
                        }
                        else newFiled[x, y] = new T();
                    }
                    else
                    {
                        if (countSosedi == 3)
                        {
                            newFiled[x, y] = new T() { IsLife = true };
                        }
                        else newFiled[x, y] = currentCell;
                    }

                }

            }
            return newFiled;
        }

        public static int NeighboursCount(T[,] cells, int x, int y, int cols, int rows)
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
    }
}
