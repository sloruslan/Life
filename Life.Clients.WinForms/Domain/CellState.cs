namespace Life.Clients.WinForms.Domain
{
    public class CellStateField
    {
        private CellState[,] _cellStates;
        public CellState[,] CellsState { get => _cellStates; set => _cellStates = value; }

        private int _width = 0;
        private int _height = 0;

        public CellStateField(int width, int height)
        {
            _width = width;
            _height = height;

            _cellStates = new CellState[width, height];
        }

        public void CopyFrom(byte[] cells)
        {
            if (cells == null) return;

            Parallel.For(0, _width, x =>
            {
                Parallel.For(0, _height, y =>
                {
                    _cellStates[x, y] = new CellState() { State = cells[x + y * _width] };
                });
            });
        }

        

        public Brush this[int x, int y]
        {
            get => _cellStates[x, y].Brush;
        }
    }
    public class CellState
    {
        private byte _state;
        public byte State
        {
            get => _state;
            set
            {
                _state = value;
                Brush = GetColor(value);
            }
        }

        public Brush Brush { get; protected set; }

        public static Brush GetColor(byte state)
        {
            switch (state)
            {
                case 0: default: return Brushes.Black;
                case 1: return Brushes.Green;
                case 2: return Brushes.Blue;
                case 3: return Brushes.Red;
                case 4: return Brushes.Magenta;
                case 5: return Brushes.Yellow;
                case 6: return Brushes.Orange;
            }
        }
    }
}
