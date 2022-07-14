using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Burst;
using System.Diagnostics;

public class ConvertBytesToIntsJobScript : MonoBehaviour
{
    bool isRun = false;
    JobHandle jobHandle;
    ConvertBytesToIntsJob job;
    Stopwatch sw0 = new Stopwatch();
    Stopwatch sw1 = new Stopwatch();
    private unsafe void Start()
    {
        byte[] bytes = new byte[8192 * 8192];
        bytes[0] = 0;
        bytes[1] = 1;
        bytes[2] = 2;
        bytes[3] = 3;
        bytes[4] = 4;
        bytes[5] = 5;
        bytes[6] = 6;
        bytes[7] = 7;
        ulong[] longs = new ulong[bytes.Length / 8];

        sw0.Start();
        sw1.Start();
        fixed (void* srcData = &bytes[0])
        {
            for (int i = 0; i < longs.Length; i++)
            {
                longs[i] = *((ulong*)srcData + i);
            }
        }

        sw0.Stop();
        sw1.Stop();
        UnityEngine.Debug.Log($"sw0: {sw0.ElapsedMilliseconds}");
        UnityEngine.Debug.Log($"sw1: {sw1.ElapsedMilliseconds}");

        job = new ConvertBytesToIntsJob();

        sw0.Restart();
        job.srcData = new NativeArray<byte>(bytes, Allocator.Persistent);
        job.dstData = new NativeArray<ulong>(bytes.Length / 8, Allocator.Persistent);
        sw1.Restart();
        jobHandle = job.Schedule(bytes.Length / 8, 12);
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
}
