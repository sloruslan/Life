public struct CellsBase
{
    public byte[] Array;
    
    public byte this[int x]
    {
        get => Array[x];
        set
        {
            Array[x] = value;

            ArrayColor[x * 3 + 1] = (byte)(value == 1 ? 255 : 0);
        }
    }

    public byte[] ArrayColor;
    //public int Width, Height;

    public CellsBase(int width, int height)
    {
        //Width = width;
        //Height = height;
        Array = new byte[width * height];
        ArrayColor = new byte[width * height * 3];
    }

    public CellsBase(byte[] value, byte[] color)
    {
        Array = value;
        ArrayColor = color;
    }
}
