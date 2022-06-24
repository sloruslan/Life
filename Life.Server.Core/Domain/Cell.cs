using Life.Server.Core.Contracts.Domain;

namespace Life.Server.Core.Domain
{
    public class Cells
    {
        private int _w;
        private int _h;

        public Cell[,] CellsArray { get; set; }

        public byte[] State { get; set; }

        public Cell this [int x, int y]
        {
            get => CellsArray[x, y];
            set
            {
                CellsArray[x, y] = value;
                State[x + y * _w] = value.IsLife ? (byte)1 : (byte)0;
            }
        }

        public Cells(int w, int h)
        {
            _w = w;
            _h = h;

            CellsArray = new Cell[w, h];
            State = new byte[w * h];
        }

        public Cells(Cell[,] cellsArray) : this(cellsArray.GetLength(0), cellsArray.GetLength(1))
        {
            for (int x = 0; x < _w; x++)
                for (int y = 0; y < _h; y++)
                {
                    this[x, y] = cellsArray[x, y];
                }
        }
    }

    public class Cell : ICellIsLife
    {
        private bool _isLife;
        public bool IsLife 
        { 
            get
            { 
                return _isLife; 
            }
            set
            {
                _isLife = value;
                if (_isLife)
                {
                    Age = 1;
                }
            }
        }

        private int _age;
        public int Age
        {
            get
            {
                return _age;
            }

            set
            {
                _age = value;
            }
        }

        public Cell()
        {
            IsLife = false;
            Age = 0;
        }
    }
}
