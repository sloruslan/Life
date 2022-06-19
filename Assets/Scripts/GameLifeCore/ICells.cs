public struct CellsBase
{
    //public CellBase[,] Array;
    public byte[,] Array;
    
    public byte this[int x, int y]
    {
        get => Array[x, y];
        set
        {
            Array[x, y] = value;

            ArrayColor[x * 3 + y * Width * 3 + 1] = (byte)(value == 1 ? 255 : 0);
        }
    }

    private int Width;
    private int Height;

    public byte[] ArrayColor;

    public CellsBase(int width, int height)
    {
        Width = width;
        Height = height;
        Array = new byte[width, height];
        ArrayColor = new byte[width * height * 3];
    }
}