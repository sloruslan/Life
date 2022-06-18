public interface IGameLife<T> where T : CellsBase
{
    public T FirstGeneration(T inputCells);
    public T NextGeneration(T inputCells);
}
