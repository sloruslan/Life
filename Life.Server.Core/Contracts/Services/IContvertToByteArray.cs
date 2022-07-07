using Life.Server.Core.Contracts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Server.Core.Contracts.Services
{
    public interface IContvertToByteArray<T> where T : ICellState, new()
    {
        public Dictionary<string, T[,]> Cells { get; set; }
        public byte[] GetByteArray(T[,] cells);
    }
}
