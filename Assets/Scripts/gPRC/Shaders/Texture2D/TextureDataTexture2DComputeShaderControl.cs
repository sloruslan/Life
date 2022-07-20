using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TextureDataTexture2DComputeShaderControl : MonoBehaviour
{
    public ComputeShader computeShader;
    private RenderTexture _renderTexture;
    private ComputeBuffer _inBuffer;

    private bool _isInit = false;

    public unsafe void CreateShader(RenderTexture renderTexture, byte[] srcData, int srcWidth, int pixelPerCell)
    {
        _renderTexture = renderTexture;
        uint[] dstDataConvert = new uint[srcData.Length / 4];
        uint[] dstDataOut = new uint[srcData.Length * 4];

        fixed (void* srcPtr = &srcData[0])
        {
            GCHandle gCHandle = GCHandle.Alloc(dstDataConvert, GCHandleType.Pinned);
            IntPtr dstPtr = gCHandle.AddrOfPinnedObject();
            UnsafeUtility.MemCpy((byte*)(void*)dstPtr, (byte*)srcPtr, srcData.Length);
            gCHandle.Free();
        }

        int testKernel = computeShader.FindKernel("Test");

        if (!_isInit)
        {
            _inBuffer = new ComputeBuffer(dstDataConvert.Length, 4, ComputeBufferType.Structured);
            computeShader.SetTexture(testKernel, "_OutBuffer", renderTexture);

            computeShader.SetInt("srcWidth", srcWidth);
            computeShader.SetInt("pixelPerCell", pixelPerCell);
            computeShader.SetInt("dstWidth", srcWidth * pixelPerCell);

            _isInit = true;
        }

        _inBuffer.SetData(dstDataConvert);
        computeShader.SetBuffer(0, "_InBuffer", _inBuffer);

        uint x, y, z;
        computeShader.GetKernelThreadGroupSizes(testKernel, out x, out y, out z);

        var groupCount = Mathf.CeilToInt((float)dstDataConvert.Length / x);
        var bufferSize = groupCount * x;

        computeShader.Dispatch(testKernel, groupCount, 1, 1);
    }

    private void OnDisable()
    {
        //inBuffer.Release();
        //inBuffer.Dispose();
    }
}
