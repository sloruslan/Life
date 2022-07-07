using Life.Server.Core.Contracts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Server.Core.Contracts.Services
{
    public interface IGameLoop<T> : IFirstGeneration<T>, INextGeneration<T>, INeighboursCount<T>, IContvertToByteArray<T> where T : ICellState, new()
    {
    }
}
