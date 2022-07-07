using Life.Server.Core.Contracts.Domain;
using Life.Server.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Server.Infrastructure.Services
{

    public class GameLoopOptimize<T> : GameLoopBase<T>, IGameLoop<T> where T : ICellState, new()
    {
        
        public T[,] StartGeneration(int perHorizontal, int perVertical, int density)
        {
            Random random = new Random();
            T[,] cells = new T[perHorizontal, perVertical];

            for (int i = 0; i < perHorizontal; i++)
            {
                for (int j = 0; j < perVertical; j++)
                {
                    cells[i, j].State = random.Next(density) == 0 ? (byte)1 : (byte)0 ;
                }
            }

            return cells;
        }

        public T[,] NextGeneration(T[,] current)
        {
            var perHorizontal = current.GetLength(0);
            var perVertical = current.GetLength(1);

            var _xmax = perHorizontal - 1;
            var _ymax = perVertical - 1;

            var newField = new T[perHorizontal, perVertical];

            RecalcNewField(0, 0, current, newField, NeighboursCount(current, ESide.TL, perHorizontal, perVertical));
            RecalcNewField(_xmax, 0, current, newField, NeighboursCount(current, ESide.TR, perHorizontal, perVertical));
            RecalcNewField(_xmax, _ymax, current, newField, NeighboursCount(current, ESide.BR, perHorizontal, perVertical));
            RecalcNewField(0, _ymax, current, newField, NeighboursCount(current, ESide.BL, perHorizontal, perVertical));

            for (int x = 1; x < perHorizontal - 1; x++)
            {
                RecalcNewField(x, 0, current, newField, NeighboursCount(x, current, ESide.Top, perHorizontal, perVertical));
                RecalcNewField(x, _ymax, current, newField, NeighboursCount(x, current, ESide.Bottom, perHorizontal, perVertical));
            }
            for (int y = 1; y < perVertical - 1; y++)
            {
                RecalcNewField(0, y, current, newField, NeighboursCount(y, current, ESide.Left, perHorizontal, perVertical));
                RecalcNewField(_xmax, y, current, newField, NeighboursCount(y, current, ESide.Right, perHorizontal, perVertical));
            }

            for (int x = 1; x < perHorizontal - 1; x++)
            {
                for (int y = 1; y < perVertical - 1; y++)
                {
                    RecalcNewField(x, y, current, newField, NeighboursCount(current, x, y));
                }
            }

            return newField;
        }

        private void RecalcNewField(int x, int y, T[,] inputCells, T[,] newField, int countSosedi)
        {
            if ((countSosedi == 3) || (inputCells[x, y].State != 0 && countSosedi == 2))
                newField[x, y].State = 1;
            else
                newField[x, y].State = 0;
        }

        // for interface
        public int NeighboursCount(T[,] cells, int x, int y, int perHorizontal, int perVertical) { return 0; }
        public int NeighboursCount(T[,] cells, int x, int y)
        {
            var x100 = x - 1;
            var x010 = x;
            var x001 = x + 1;
            var y100 = y - 1;
            var y010 = y;
            var y001 = y + 1;

            return cells[x010, y100].State + cells[x001, y100].State + cells[x001, y010].State +
                    cells[x001, y001].State + cells[x010, y001].State + cells[x100, y001].State +
                    cells[x100, y010].State + cells[x100, y100].State;
        }



        private int NeighboursCount(int index, T[,] inputCells, ESide side, int perHorizontal, int perVertical)
        {
            var res = 0;

            var p100 = index - 1;
            var p010 = index;
            var p001 = index + 1;

            var _xmax = perHorizontal - 1;
            var _ymax = perVertical - 1;
            var _xmax1 = _xmax - 1;
            var _ymax1 = _ymax - 1;

            switch (side)
            {
                case ESide.Top:
                    res = inputCells[p001, 0].State + inputCells[p001, 1].State + inputCells[p010, 1].State + inputCells[p100, 1].State + 
                            inputCells[p100, 0].State + inputCells[p001, _ymax].State + inputCells[p010, _ymax].State + inputCells[p100, _ymax].State;
                    break;
                case ESide.Right:
                    res = inputCells[_xmax, p001].State + inputCells[_xmax1, p001].State + inputCells[_xmax1, p010].State + inputCells[_xmax1, p100].State + 
                        inputCells[_xmax, p100].State + inputCells[0, p100].State + inputCells[0, p010].State + inputCells[0, p001].State;
                    break;
                case ESide.Bottom:
                    res = inputCells[p001, _ymax].State + inputCells[p001, _ymax1].State + inputCells[p010, _ymax1].State + inputCells[p100, _ymax1].State + 
                        inputCells[p100, _ymax].State + inputCells[p100, 0].State + inputCells[p010, 0].State + inputCells[p001, 0].State;
                    break;
                case ESide.Left:
                    res = inputCells[0, p001].State + inputCells[1, p001].State + inputCells[1, p010].State + inputCells[1, p100].State + 
                        inputCells[0, p100].State + inputCells[_xmax, p100].State + inputCells[_xmax, p010].State + inputCells[_xmax, p001].State;
                    break;
            }

            return res;
        }

        private int NeighboursCount(T[,] inputCells, ESide side, int perHorizontal, int perVertical)
        {
            int res = 0;

            var _xmax = perHorizontal - 1;
            var _ymax = perVertical - 1;
            var _xmax1 = _xmax - 1;
            var _ymax1 = _ymax - 1;

            switch (side)
            {
                case ESide.TL:
                    res = inputCells[1, 0].State + inputCells[1, 1].State + inputCells[0, 1].State + inputCells[_xmax, 0].State + 
                        inputCells[_xmax, 1].State + inputCells[0, _ymax].State + inputCells[1, _ymax].State + inputCells[_xmax, _ymax].State;
                    break;
                case ESide.TR:
                    res = inputCells[_xmax1, 0].State + inputCells[_xmax1, 1].State + inputCells[_xmax, 1].State + inputCells[0, 0].State + 
                        inputCells[0, 1].State + inputCells[_xmax, _ymax].State + inputCells[_xmax1, _ymax].State + inputCells[0, _ymax].State;
                    break;
                case ESide.BR:
                    res = inputCells[_xmax1, _ymax].State + inputCells[_xmax1, _ymax1].State + inputCells[_xmax, _ymax1].State + inputCells[0, _ymax].State + 
                        inputCells[0, _ymax1].State + inputCells[_xmax, 0].State + inputCells[_xmax1, 0].State + inputCells[0, 0].State;
                    break;
                case ESide.BL:
                    res = inputCells[0, _ymax1].State + inputCells[1, _ymax1].State + inputCells[1, _ymax].State + inputCells[0, 0].State + 
                            inputCells[1, 0].State + inputCells[_xmax, _ymax].State + inputCells[_xmax, _ymax1].State + inputCells[_xmax, 0].State;
                    break;
            }
            return res;
        }

        enum ESide
        {
            Top,
            Right,
            Bottom,
            Left,
            TL,
            TR,
            BR,
            BL
        }


    }
    
}
