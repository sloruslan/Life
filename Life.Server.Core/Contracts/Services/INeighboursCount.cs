using Life.Server.Core.Contracts.Domain;

namespace Life.Server.Core.Contracts.Services
{
    public interface INeighboursCount<T> where T : ICellState, new()
    {
        public int NeighboursCount(T[,] cells, int x, int y, int perHorizontal, int perVertical);
    }
}
