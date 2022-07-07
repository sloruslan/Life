using Life.Server.Core.Contracts.Domain;

namespace Life.Server.Core.Contracts.Services
{
    public interface IFirstGeneration<T> where T : ICellState, new()
    {
        public T[,] StartGeneration(int perHorizontal, int perVertical, int density);
    }
}
