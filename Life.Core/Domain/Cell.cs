using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Domain
{
    public class Cell
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

                if (Age > 0) Color = Brushes.Green;
                else if (Age == 0) Color = Brushes.Black;
            }
        }

        public Brush Color { get; set; }

        public Cell()
        {
            IsLife = false;
            Age = 0;
        }
    }
}
