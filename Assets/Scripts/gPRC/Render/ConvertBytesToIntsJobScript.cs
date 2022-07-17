using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Burst;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;

public class ConvertBytesToIntsJobScript : MonoBehaviour
{
    bool isRun = false;
    JobHandle jobHandle;
    ConvertBytesToIntsJob job;
    Stopwatch sw0 = new Stopwatch();
    Stopwatch sw1 = new Stopwatch();

    public ComputeShader computeShader;

    private void Start()
    {
        //byte[] src = new byte[16384 * 16384];
        //byte[] src = new byte[8192 * 8192];
        byte[] src = new byte[3840 * 2160];

        /*
        for (int i = 0; i < 1024; i++)
        {
            src[i] = (byte)(i % 256);
        }
        src[src.Length - 16] = 15;
        src[src.Length - 15] = 14;
        src[src.Length - 14] = 13;
        src[src.Length - 13] = 12;
        src[src.Length - 12] = 11;
        src[src.Length - 11] = 10;
        src[src.Length - 10] = 9;
        src[src.Length - 9] = 8;

        src[src.Length - 8] = 7;
        src[src.Length - 7] = 6;
        src[src.Length - 6] = 5;
        src[src.Length - 5] = 4;
        src[src.Length - 4] = 3;
        src[src.Length - 3] = 2;
        src[src.Length - 2] = 1;
        src[src.Length - 1] = 0;

        */
        
        for (int i = 0; i < 1024; i++)
        {
            src[i] = 1;// (byte)(i % 2 == 0 ? 1 : 0);
        }

        StartShader(src);
    }

    private unsafe void StartShader(byte[] src)
    {
        int[] dstOut = new int[src.Length];
        int[] dstIn = new int[src.Length / 4];

        sw0.Restart();
        
        fixed (void* srcPtr = &src[0])
        {
            GCHandle gCHandle = GCHandle.Alloc(dstIn, GCHandleType.Pinned);
            IntPtr dstPtr = gCHandle.AddrOfPinnedObject();
            UnsafeUtility.MemCpy((byte*)(void*)dstPtr, (byte*)srcPtr, src.Length);
            gCHandle.Free();
        }

        DebugLog("MemCpy");
        
        ComputeBuffer inBuffer = new ComputeBuffer(dstIn.Length, 4, ComputeBufferType.Structured);
        DebugLog("inBuffer");
        inBuffer.SetData(dstIn);
        DebugLog("SetData");
        
        ComputeBuffer outBuffer = new ComputeBuffer(dstOut.Length, 4, ComputeBufferType.Structured);
        DebugLog("outBuffer");
        computeShader.SetBuffer(0, "_InBuffer", inBuffer);
        DebugLog("_InBuffer");
        computeShader.SetBuffer(0, "_OutBuffer", outBuffer);
        DebugLog("_OutBuffer");

        computeShader.SetInt("srcWidth", 3840);
        computeShader.SetInt("pixelPerCell", 2);
        computeShader.SetInt("pixelFormat", 4);
        computeShader.SetInt("dstWidth", 3840 * 2 * 4);

        
        int testKernel = computeShader.FindKernel("Test");
        uint x, y, z;
        computeShader.GetKernelThreadGroupSizes(testKernel, out x, out y, out z);

        var groupCount = Mathf.CeilToInt((float)dstIn.Length / x);
        var bufferSize = groupCount * x;
        DebugLog($"groupCount {groupCount}");

        computeShader.Dispatch(testKernel, groupCount, 1, 1);
        DebugLog("Dispatch");

        outBuffer.GetData(dstOut);
        sw0.Stop();
        DebugLog("computeShader");

        inBuffer.Release();
        outBuffer.Release();
        inBuffer.Dispose();
        outBuffer.Dispose();

        UnityEngine.Debug.Log("0..16: ");
        OutArray(dstOut, 1024, 16);

        UnityEngine.Debug.Log("index..last:");

        OutArrayInvers(dstOut, 1024, 16);

        // dstInN.Dispose();
    }

    private void OutArray(int[] dstOut, int count, int countOfLine)
    {
        for (int i = 0; i < count; i += countOfLine)
        {
            string outStr = string.Empty;
            for (int j = 0; j < countOfLine; j++)
            {
                outStr += dstOut[i + j].ToString("X8") + " ";
            }

            UnityEngine.Debug.Log(outStr);
        }
    }

    private void OutArrayInvers(int[] dstOut, int count, int countOfLine)
    {
        for (int i = count; i > 0; i -= countOfLine)
        {
            string outStr = string.Empty;
            for (int j = 0; j < countOfLine; j++)
            {
                outStr += dstOut[dstOut.Length  - i + j].ToString("X8") + " ";
            }

            UnityEngine.Debug.Log(outStr);
        }
    }

    private void DebugLog(string label)
    {
        UnityEngine.Debug.Log($"{label}: {sw0.ElapsedMilliseconds}");
    }

    private void StartJob()
    {
        
    }

    private unsafe void StartUnsafe(byte[] src)
    {
        int[] dstOut = new int[src.Length];


        int[] dstIn = new int[src.Length / 4];

        sw0.Restart();
        fixed (void* srcData = &src[0])
        {
            GCHandle gCHandle = GCHandle.Alloc(dstIn, GCHandleType.Pinned);
            IntPtr intPtr = gCHandle.AddrOfPinnedObject();
            UnsafeUtility.MemCpy((byte*)(void*)intPtr, (byte*)srcData, src.Length);
            gCHandle.Free();
        }

        sw0.Stop();
        sw1.Stop();
        UnityEngine.Debug.Log($"sw0: {sw0.ElapsedMilliseconds}");
        UnityEngine.Debug.Log($"sw1: {sw1.ElapsedMilliseconds}");


        sw0.Restart();
        sw1.Restart();
        //UnsafeUtility.SizeOf<T>()
        fixed (byte* srcData = &src[0])
            fixed (void* dstData = &dstIn[0])
        {
            UnsafeUtility.MemCpy((byte*)dstData, srcData, src.Length);
        }

        sw0.Stop();
        sw1.Stop();
        UnityEngine.Debug.Log($"sw0: {sw0.ElapsedMilliseconds}");
        UnityEngine.Debug.Log($"sw1: {sw1.ElapsedMilliseconds}");

        job = new ConvertBytesToIntsJob();

        sw0.Restart();
        job.srcData = new NativeArray<byte>(src, Allocator.Persistent);
        job.dstData = new NativeArray<ulong>(src.Length / 8, Allocator.Persistent);
        sw1.Restart();
        jobHandle = job.Schedule(src.Length / 8, 12);
        isRun = true;
    }

    private void LateUpdate()
    {
        if (isRun && jobHandle.IsCompleted)
        {
            jobHandle.Complete();
            sw0.Stop();
            sw1.Stop();
            UnityEngine.Debug.Log($"sw0: {sw0.ElapsedMilliseconds}");
            UnityEngine.Debug.Log($"sw1: {sw1.ElapsedMilliseconds}");
            UnityEngine.Debug.Log("");
            job.srcData.Dispose();
            job.dstData.Dispose();

            isRun = false;
        }
    }

    [BurstCompile]
    private struct ConvertBytesToIntsJob : IJobParallelFor
    {
        [ReadOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<byte> srcData;

        [WriteOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<ulong> dstData;
        public unsafe void Execute(int index)
        {
            dstData[index] = *((ulong*)srcData.GetUnsafeReadOnlyPtr() + index);  
        }
    }

    int[] _InBuffer;
    int[] _OutBuffer;

    static int srcWidth;
    static int srcHeight;
    static int pixelPerCell;
    static int pixelFormat;
    static int offset;

    static int dstWidth = srcWidth * pixelPerCell * pixelFormat;
    static int dstHeight = srcHeight * pixelPerCell;

    void ExecuteCells(int index)
    {
        ExecuteCell(_InBuffer[index] & 0xFF, index);
        ExecuteCell((_InBuffer[index] >> 8) & 0xFF, index + 1);
        ExecuteCell((_InBuffer[index] >> 16) & 0xFF, index + 2);
        ExecuteCell((_InBuffer[index] >> 24) & 0xFF, index + 3);
    }

    void ExecuteCell(int cell, int index)
    {
        int srcY = index / srcWidth;
        int srcX = index - srcY * srcWidth;

        int[] dstPtr = new int[pixelPerCell];
        for (int indexPtr = 0; indexPtr < pixelPerCell; indexPtr++)
        {
            dstPtr[indexPtr] = srcY * pixelPerCell * dstWidth + indexPtr * dstWidth + srcX * pixelPerCell * pixelFormat;
        }

        for (int indexPixelX = 0; indexPixelX < pixelPerCell; indexPixelX++)
        {
            for (int indexPixelY = 0; indexPixelY < pixelPerCell; indexPixelY++)
            {
                _OutBuffer[dstPtr[indexPixelX] + indexPixelY * pixelFormat] = cell * 255 << 15 | 0xFF;
            }
        }
    }
}
