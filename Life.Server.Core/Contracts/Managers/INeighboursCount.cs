using Life.Server.Core.Contracts.Domain;

namespace Life.Server.Core.Contracts.Managers
{
    public interface INeighboursCount<T> where T : ICellState, new()
    {
        public static abstract int NeighboursCount(T[,] cells, int x, int y, int perHorizontal, int perVertical);
    }
}
