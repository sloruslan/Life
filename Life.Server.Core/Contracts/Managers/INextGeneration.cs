using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Life.Server.Core.Contracts.Domain;
using Life.Server.Core.Domain;

namespace Life.Server.Core.Contracts.Managers
{
    public interface INextGeneration<T> where T : ICellState, new()
    {
        public static abstract T[,] NextGeneration(T[,] current); 
    }
}
