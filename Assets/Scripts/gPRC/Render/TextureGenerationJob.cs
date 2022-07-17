using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using UnityEngine.Events;

public class TextureGenerationJob : MonoBehaviour
{

    private TextureDataJob _textureDataJob;
    private JobHandle _jobHandle;
    public bool _isExist;
    private bool _isRun = false;

    [Range(1, 200)]
    public int InnerLoop = 50;

    public UnityAction<NativeArray<byte>> WaitTextureGenerationJobCompletedEvent { get; set; }

    public void RunTextureGenerationJob(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat, int offset)
    {
        if (_isExist)
            _textureDataJob.UpdateValue(srcData, pixelPerCell, pixelFormat, offset);
        else
        {
            _textureDataJob = TextureDataJob.CreateTextureDataJob(srcData, srcWidth, srcHeight, pixelPerCell, pixelFormat, offset);
            _isExist = true;
        }
        _jobHandle = _textureDataJob.Schedule(srcData.Length, InnerLoop);
        _isRun = true;
    }

    private void LateUpdate()
    {
        if (_isRun && _jobHandle.IsCompleted)
        {
            _isRun = false;
            _jobHandle.Complete();
            WaitTextureGenerationJobCompletedEvent?.Invoke(_textureDataJob.dstData);
        }
    }

    private void OnDisable()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (_isExist)
        {
            if (!_jobHandle.IsCompleted)
                _jobHandle.Complete();
            _textureDataJob.Dispose();
            _isExist = false;
        }
    }


    [BurstCompile]
    private struct TextureDataJob : IJobParallelFor
    {
        [ReadOnly]
        [NoAlias]
        public NativeArray<byte> srcData;

        [ReadOnly]
        private int srcWidth;
        [ReadOnly]
        private int srcHeight;
        [ReadOnly]
        public int pixelPerCell;
        [ReadOnly]
        public int pixelFormat;
        public int offset;
        [WriteOnly]
        [NativeDisableParallelForRestriction]
        [NoAlias]
        public NativeArray<byte> dstData;


        [ReadOnly]
        private int dstWidth;
        [ReadOnly]
        private int dstHeight;


        public void Execute(int index)
        {
            int srcY = index / srcWidth;
            int srcX = index - srcY * srcWidth;

            NativeArray<int> dstPtr = new NativeArray<int>(pixelPerCell, Allocator.Temp);
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

            dstPtr.Dispose();
        }
        

       // public void Execute(int index)
       // {
       //     dstData[index * 3 + 1] = (byte)(srcData[index] * 255);
       // }



        public static TextureDataJob CreateTextureDataJob(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat, int offset)
        {
            TextureDataJob textureDataJob = new TextureDataJob();

            textureDataJob.srcData = new NativeArray<byte>(srcData, Allocator.Persistent);


            textureDataJob.srcWidth = srcWidth;
            textureDataJob.srcHeight = srcHeight;
            textureDataJob.pixelPerCell = pixelPerCell;
            textureDataJob.pixelFormat = pixelFormat;
            textureDataJob.offset = offset;

            textureDataJob.dstWidth = srcWidth * pixelPerCell * pixelFormat;
            textureDataJob.dstHeight = srcHeight * pixelPerCell;

            textureDataJob.dstData = new NativeArray<byte>(textureDataJob.dstWidth * textureDataJob.dstHeight, Allocator.Persistent);

            return textureDataJob;
        }

        public void UpdateValue(byte[] srcData, int pixelPerCell, int pixelFormat, int offset)
        {
            this.srcData.CopyFrom(srcData);
            this.pixelPerCell = pixelPerCell;
            this.pixelFormat = pixelFormat;
            this.offset = offset;

            this.dstWidth = srcWidth * pixelPerCell * pixelFormat;
            this.dstHeight = srcHeight * pixelPerCell;
        }

        public void Dispose()
        {
            this.srcData.Dispose();
            this.dstData.Dispose();
        }
    }
}
