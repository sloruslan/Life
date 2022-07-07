using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Server.Infrastructure.Services
{
    public class MyCalculator
    {
        public Guid Id { get; set; }

        private int _count = 0;

        public MyCalculator(Guid guid)
        {
            Id = guid;
        }
        public int Calculate()
        {
            while (_count != 1000000)
            {
                Thread.Sleep(10);
                _count++;
            }
            return _count;
        }
    }
}
