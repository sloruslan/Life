using Life.Server.Core.Contracts.Domain;

namespace Life.Server.Core.Contracts.Managers
{
    public interface IFirstGeneration
    {
        public static abstract T[,] StartGeneration<T>(int perHorizontal, int perVertical, int density) where T : ICellIsLife, new();
    }
}
