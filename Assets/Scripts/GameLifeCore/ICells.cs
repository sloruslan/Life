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

    public CellsBase(byte[] bytes)
    {
        Array = new byte[bytes.Length];
        ArrayColor = new byte[bytes.Length * 3];

        for (int i = 0; i < bytes.Length; i++)
        {
            Array[i] = bytes[i];
            ArrayColor[i * 3 + 1] = (byte)(bytes[i] == 1 ? 255 : 0);
        }
    }
}
