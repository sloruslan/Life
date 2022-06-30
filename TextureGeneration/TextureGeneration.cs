namespace TextureGenerationMethods
{
    public class TextureGeneration
    {
        public static unsafe byte[] GetTextureData(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat)
        {
            int dstWidth = srcWidth * pixelPerCell * pixelFormat;
            int dstHeight = srcHeight * pixelPerCell;
            byte[] dstData = new byte[dstWidth * dstHeight];
            int[] dstPtr = new int[pixelPerCell];
            int dstPtrX = pixelPerCell * pixelFormat;

            for (int srcY = 0, dstY = 0; srcY < srcHeight; srcY++, dstY += pixelPerCell)
            {
                for (int srcX = 0, dstX = 0; srcX < srcWidth; srcX++, dstX += dstPtrX)
                {
                    for (int indexPtr = 0; indexPtr < pixelPerCell; indexPtr++)
                    {
                        dstPtr[indexPtr] = dstY * dstWidth + indexPtr * dstWidth + dstX;
                    }

                    for (int indexPixelX = 0; indexPixelX < pixelPerCell; indexPixelX++)
                    {
                        for (int indexPixelY = 0; indexPixelY < pixelPerCell; indexPixelY++)
                        {
                            int indexPixelPart = indexPixelY * pixelFormat;

                            for (int indexColor = 0; indexColor < pixelFormat; indexColor++)
                            {
                                dstData[dstPtr[indexPixelX] + indexPixelPart + indexColor] = srcData[srcY * srcWidth + srcX];
                            }
                        }
                    }
                }
            }

            return dstData;
        }


        public static unsafe byte[] GetTextureDataGreen(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat)
        {
            int dstWidth = srcWidth * pixelPerCell * pixelFormat;
            int dstHeight = srcHeight * pixelPerCell;
            byte[] dstData = new byte[dstWidth * dstHeight];
            int[] dstPtr = new int[pixelPerCell];
            int dstPtrX = pixelPerCell * pixelFormat;

            for (int srcY = 0, dstY = 0; srcY < srcHeight; srcY++, dstY += pixelPerCell)
            {
                for (int srcX = 0, dstX = 0; srcX < srcWidth; srcX++, dstX += dstPtrX)
                {
                    for (int indexPtr = 0; indexPtr < pixelPerCell; indexPtr++)
                    {
                        dstPtr[indexPtr] = dstY * dstWidth + indexPtr * dstWidth + dstX;
                    }

                    for (int indexPixelX = 0; indexPixelX < pixelPerCell; indexPixelX++)
                    {
                        for (int indexPixelY = 0; indexPixelY < pixelPerCell; indexPixelY++)
                        {
                            dstData[dstPtr[indexPixelX] + indexPixelY * pixelFormat + 1] = (byte)(srcData[srcY * srcWidth + srcX] * 255);
                        }
                    }
                }
            }

            return dstData;
        }

        private static unsafe byte[] GetTextureDataReserved(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat)
        {
            int dstWidth = srcWidth * pixelPerCell * pixelFormat;
            int dstHeight = srcHeight * pixelPerCell;
            byte[] dstData = new byte[dstWidth * dstHeight];
            int[] dstPtr = new int[pixelPerCell];
            int dstPtrX = pixelPerCell * pixelFormat;

            for (int srcY = 0, dstY = 0; srcY < srcHeight; srcY++, dstY += pixelPerCell)
            {
                for (int srcX = 0, dstX = 0; srcX < srcWidth; srcX++, dstX += dstPtrX)
                {
                    for (int indexPtr = 0; indexPtr < pixelPerCell; indexPtr++)
                    {
                        dstPtr[indexPtr] = dstY * dstWidth + indexPtr * dstWidth + dstX;
                    }

                    for (int indexPixelX = 0; indexPixelX < pixelPerCell; indexPixelX++)
                    {
                        for (int indexPixelY = 0; indexPixelY < pixelPerCell; indexPixelY++)
                        {
                            int indexPixelPart = indexPixelY * pixelFormat;

                            for (int indexColor = 0; indexColor < pixelFormat; indexColor++)
                            {
                                dstData[dstPtr[indexPixelX] + indexPixelPart + indexColor] = srcData[srcY * srcWidth + srcX];
                            }
                        }
                    }
                }
            }

            return dstData;
        }
    }
}