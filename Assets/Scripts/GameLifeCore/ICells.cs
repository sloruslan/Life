public class CellsBase
{
    public virtual CellBase[,] Array { get; set; }
    
    public virtual bool this[int x, int y]
    {
        get => Array[x, y].IsLife;
        set => Array[x, y].IsLife = value;
    }

    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    public CellsBase(int width, int height)
    {
        Width = width;
        Height = height;
        Array = new CellBase[width, height];

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                Array[i, j] = new CellBase();
    }

    public static CellsBase GetEmptyFromParams(CellsBase cellsBase)
    {
        return new CellsBase(cellsBase.Width, cellsBase.Height);
    }
}
