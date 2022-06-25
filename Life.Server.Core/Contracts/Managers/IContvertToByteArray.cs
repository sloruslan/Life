using Life.Server.Core.Contracts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Server.Core.Contracts.Managers
{
    public interface IContvertToByteArray<T> where T : ICellState, new()
    {
        public static abstract Dictionary<string, T[,]> Cells { get; set; }
        public static abstract byte[] GetByteArray(T[,] cells);
    }
}
