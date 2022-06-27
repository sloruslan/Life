public interface IGameLife<T> 
{
    public T FirstGeneration(T inputCells);
    public T NextGeneration(T inputCells);
}
