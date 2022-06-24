using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Shared.Domain
{
    public class CellsTransfer
    {
        public byte[] CellsState { get; set; } 

        public byte this[int x, int y]
        {
            get => CellsState[x + y * _w];
            set => CellsState[x + y * _w] = value;
        }

        private int _w;
        private int _h;

        public CellsTransfer(int w, int h)
        {
            _w = w;
            _h = h;

            CellsState = new byte[w * h];
        }
    }
}
