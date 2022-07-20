using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TextureDataUIntComputeShaderControl : MonoBehaviour
{
    public ComputeShader computeShader;
    Stopwatch sw0 = new Stopwatch();
    public unsafe uint[] TextureDataGeneration(byte[] srcData, int srcWidth, int pixelPerCell)
    {
        sw0.Restart();

        uint[] dstDataConvert = new uint[srcData.Length / 4];
        uint[] dstDataOut = new uint[srcData.Length * pixelPerCell * pixelPerCell];

        fixed (void* srcPtr = &srcData[0])
        {
            GCHandle gCHandle = GCHandle.Alloc(dstDataConvert, GCHandleType.Pinned);
            IntPtr dstPtr = gCHandle.AddrOfPinnedObject();
            UnsafeUtility.MemCpy((byte*)(void*)dstPtr, (byte*)srcPtr, srcData.Length);
            gCHandle.Free();
        }

        DebugLog("MemCpy");

        ComputeBuffer inBuffer = new ComputeBuffer(dstDataConvert.Length, 4, ComputeBufferType.Structured);
        inBuffer.SetData(dstDataConvert);

        DebugLog("SetData");

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
        DebugLog("Dispatch");

        outBuffer.GetData(dstDataOut);
        DebugLog("GetData");

        inBuffer.Release();
        outBuffer.Release();
        inBuffer.Dispose();
        outBuffer.Dispose();

        DebugLog("Dispose");
        return dstDataOut;
    }

    private void DebugLog(string label)
    {
        UnityEngine.Debug.Log($"{label}: {sw0.ElapsedMilliseconds}");
    }
}
