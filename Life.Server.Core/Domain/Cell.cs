using Life.Server.Core.Contracts.Domain;

namespace Life.Server.Core.Domain
{
    public class Cell : ICellState
    {
        private byte _state;
        public byte State 
        { 
            get
            { 
                return _state; 
            }
            set
            {
                _state = value;
                if (_state != 0)
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
            State = 0;
            Age = 0;
        }
    }
}
