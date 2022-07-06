namespace TextureGenerationMethods
{
    public class TextureGeneration
    {
        public static unsafe byte[] GetTextureDataGreenParallel(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat)
        {
            int dstWidth = srcWidth * pixelPerCell * pixelFormat;
            int dstHeight = srcHeight * pixelPerCell;
            byte[] dstData = new byte[dstWidth * dstHeight];


            Parallel.For(0, srcHeight, (srcY, state) =>
            {
                Parallel.For(0, srcWidth, srcX =>
                {
                    int[] dstPtr = new int[pixelPerCell];
                    for (int indexPtr = 0; indexPtr < pixelPerCell; indexPtr++)
                    {
                        dstPtr[indexPtr] = srcY * pixelPerCell * dstWidth + indexPtr * dstWidth + srcX * pixelPerCell * pixelFormat;
                    }

                    for (int indexPixelX = 0; indexPixelX < pixelPerCell; indexPixelX++)
                    {
                        for (int indexPixelY = 0; indexPixelY < pixelPerCell; indexPixelY++)
                        {
                            dstData[dstPtr[indexPixelX] + indexPixelY * pixelFormat + 1] = (byte)(srcData[srcY * srcWidth + srcX] * 255);
                        }
                    }
                });
            });

            return dstData;
        }

        public static unsafe byte[] GetTextureDataParallel(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat)
        {
            int dstWidth = srcWidth * pixelPerCell * pixelFormat;
            int dstHeight = srcHeight * pixelPerCell;
            byte[] dstData = new byte[dstWidth * dstHeight];
            


            Parallel.For(0, srcHeight, (srcY, state) =>
            {
                Parallel.For(0, srcWidth, srcX =>
                {
                    int[] dstPtr = new int[pixelPerCell];
                    for (int indexPtr = 0; indexPtr < pixelPerCell; indexPtr++)
                    {
                        dstPtr[indexPtr] = srcY * pixelPerCell * dstWidth + indexPtr * dstWidth + srcX * pixelPerCell * pixelFormat;
                    }

                    for (int indexPixelX = 0; indexPixelX < pixelPerCell; indexPixelX++)
                    {
                        for (int indexPixelY = 0; indexPixelY < pixelPerCell; indexPixelY++)
                        {
                            fixed( byte* ptr = &dstData[dstPtr[indexPixelX] + indexPixelY * pixelFormat])
                            {
                                *(int*)ptr = srcData[srcY * srcWidth + srcX] << 15;
                            }
                        }
                    }
                });
            });

            return dstData;
        }






















        private static unsafe byte[] GetTextureData(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat)
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


        

        private static unsafe byte[] GetTextureDataGreenFor(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat)
        {
            int dstWidth = srcWidth * pixelPerCell * pixelFormat;
            int dstHeight = srcHeight * pixelPerCell;
            byte[] dstData = new byte[dstWidth * dstHeight];
            int[] dstPtr = new int[pixelPerCell];

            for (int srcY = 0; srcY < srcHeight; srcY++) 
            {
                for (int srcX = 0; srcX < srcWidth; srcX++)
                {
                    for (int indexPtr = 0; indexPtr < pixelPerCell; indexPtr++)
                    {
                        dstPtr[indexPtr] = srcY * pixelPerCell * dstWidth + indexPtr * dstWidth + srcX * pixelPerCell * pixelFormat;
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


        private static unsafe byte[] GetTextureDataGreenParallelFor(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat)
        {
            int dstWidth = srcWidth * pixelPerCell * pixelFormat;
            int dstHeight = srcHeight * pixelPerCell;
            byte[] dstData = new byte[dstWidth * dstHeight];
            int[] dstPtr = new int[pixelPerCell];

            for (int srcY = 0; srcY < srcHeight; srcY++)
            {
                for (int srcX = 0; srcX < srcWidth; srcX++)
                {
                    for (int indexPtr = 0; indexPtr < pixelPerCell; indexPtr++)
                    {
                        dstPtr[indexPtr] = srcY * pixelPerCell * dstWidth + indexPtr * dstWidth + srcX * pixelPerCell * pixelFormat;
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

        private static unsafe byte[] GetTextureDataGreen(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat)
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


        private static unsafe byte[] GetTextureDataGreen(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat, bool isLandspace = true)
        {
            if (!isLandspace)
            {
                var temp = srcWidth;
                srcHeight = srcWidth;
                srcWidth = temp;
            }


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
                            try
                            {
                                dstData[dstPtr[indexPixelX] + indexPixelY * pixelFormat + 1] = (byte)(srcData[srcY * srcWidth + srcX] * 255);
                            }
                            catch
                            {
                                throw new IndexOutOfRangeException(
                                    "srcData.Length: " + srcData.Length.ToString() + Environment.NewLine +
                                    "srcWidth: " + srcWidth.ToString() + Environment.NewLine +
                                    "srcHeight: " + srcHeight.ToString() + Environment.NewLine +
                                    "pixelPerCell: " + pixelPerCell.ToString() + Environment.NewLine +
                                    "pixelFormat: " + pixelFormat.ToString() + Environment.NewLine +
                                    "isLandspace: " + isLandspace.ToString() + Environment.NewLine +
                                    "dstWidth: " + dstWidth.ToString() + Environment.NewLine +
                                    "dstHeight: " + dstHeight.ToString() + Environment.NewLine +
                                    "dstPtrX: " + dstPtrX.ToString() + Environment.NewLine +
                                    "indexPixelX: " + indexPixelX.ToString() + Environment.NewLine +
                                    "indexPixelY: " + indexPixelY.ToString() + Environment.NewLine +
                                    "dstPtr[indexPixelX] + indexPixelY * pixelFormat + 1" + (dstPtr[indexPixelX] + indexPixelY * pixelFormat + 1).ToString() + Environment.NewLine 

                                    );
                            }
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