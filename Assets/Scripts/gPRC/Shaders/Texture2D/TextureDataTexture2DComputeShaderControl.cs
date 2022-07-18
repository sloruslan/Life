using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TextureDataTexture2DComputeShaderControl : MonoBehaviour
{
    public ComputeShader computeShader;

    public unsafe uint[] TextureDataGeneration(Texture2D texture2D, byte[] srcData, int srcWidth, int pixelPerCell)
    {
        uint[] dstDataConvert = new uint[srcData.Length / 4];
        uint[] dstDataOut = new uint[srcData.Length * 4];

       // RenderTexture renderTexture = new RenderTexture()

        fixed (void* srcPtr = &srcData[0])
        {
            GCHandle gCHandle = GCHandle.Alloc(dstDataConvert, GCHandleType.Pinned);
            IntPtr dstPtr = gCHandle.AddrOfPinnedObject();
            UnsafeUtility.MemCpy((byte*)(void*)dstPtr, (byte*)srcPtr, srcData.Length);
            gCHandle.Free();
        }

        ComputeBuffer inBuffer = new ComputeBuffer(dstDataConvert.Length, 4, ComputeBufferType.Structured);
        inBuffer.SetData(dstDataConvert);

        ComputeBuffer outBuffer = new ComputeBuffer(dstDataOut.Length, 4, ComputeBufferType.Structured);
        computeShader.SetBuffer(0, "_InBuffer", inBuffer);
        computeShader.SetBuffer(0, "_OutBuffer", outBuffer);

        computeShader.SetInt("srcWidth", srcWidth);
        computeShader.SetInt("pixelPerCell", pixelPerCell);
        computeShader.SetInt("dstWidth", srcWidth * pixelPerCell);


        int testKernel = computeShader.FindKernel("Test");
        uint x, y, z;
        computeShader.GetKernelThreadGroupSizes(testKernel, out x, out y, out z);

        var groupCount = Mathf.CeilToInt((float)dstDataConvert.Length / x);
        var bufferSize = groupCount * x;

        computeShader.Dispatch(testKernel, groupCount, 1, 1);

        outBuffer.GetData(dstDataOut);

        inBuffer.Release();
        outBuffer.Release();
        inBuffer.Dispose();
        outBuffer.Dispose();

        return dstDataOut;
    }
}
