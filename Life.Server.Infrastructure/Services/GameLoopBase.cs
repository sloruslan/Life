using Life.Server.Core.Contracts.Domain;
using Life.Server.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Server.Infrastructure.Services
{
    public class GameLoopBase<T> : IContvertToByteArray<T> where T : ICellState, new()
    {
        
        public Dictionary<string, T[,]> Cells { get; set; } = new Dictionary<string, T[,]>();

        public byte[] GetByteArray(T[,] cells)
        {
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);
            byte[] bytesArray = new byte[width * height];

            Parallel.For(0, width, x =>
            {
                Parallel.For(0, height, y =>
                {
                    bytesArray[x + y * width] = cells[x, y].State;
                });
            });

            return bytesArray;
        }
    }
}
