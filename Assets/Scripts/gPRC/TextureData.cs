using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TextureData
{
    public static byte[] GetTextureData(byte[] data)
    {
        byte[] result = new byte[data.Length * 3];

        for (int i = 0; i < data.Length; i++)
        {
            result[i * 3 + 1] = data[i] == 1 ? (byte)255 : (byte)0;
        }

        return result;
    }

    public static byte[] GetTextureData(byte[] data, int pixelsPerCell)
    {
        byte[] result = new byte[data.Length * 3 * pixelsPerCell];

        for (int i = 0; i < data.Length; i++)
        {
            result[i * 3 + 1] = data[i] == 1 ? (byte)255 : (byte)0;
        }

        return result;
    }
}

