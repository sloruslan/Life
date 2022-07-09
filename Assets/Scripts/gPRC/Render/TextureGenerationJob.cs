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
    private bool _isExist;
    private bool _isRun = false;

    [Range(1, 50)]
    public int InnerLoop = 50;

    public UnityAction<NativeArray<byte>> WaitTextureGenerationJobCompletedEvent { get; set; }

    public void RunTextureGenerationJob(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat, int offset)
    {
        if (_isExist)
            _textureDataJob.UpdateValue(srcData, pixelPerCell, pixelFormat, offset);
        else
            _textureDataJob = TextureDataJob.CreateTextureDataJob(srcData, srcWidth, srcHeight, pixelPerCell, pixelFormat, offset);

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
        if (_isExist)
        {
            _textureDataJob.Dispose();
            _isExist = false;
        }
    }


    [BurstCompile]
    private struct TextureDataJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<byte> srcData;

        private int srcWidth;
        private int srcHeight;
        public int pixelPerCell;
        public int pixelFormat;
        public int offset;

        [WriteOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<byte> dstData;

        

        private int dstWidth;
        private int dstHeight;

        [BurstCompile]
        public void Execute(int index)
        {
            int srcY = index / srcWidth;
            int srcX = index - srcY * srcHeight;

            NativeArray<int> dstPtr = new NativeArray<int>(pixelPerCell, Allocator.Persistent);
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

        public static TextureDataJob CreateTextureDataJob(byte[] srcData, int srcWidth, int srcHeight, int pixelPerCell, int pixelFormat, int offset)
        {
            TextureDataJob textureDataJob = new TextureDataJob();

            textureDataJob.srcData = new NativeArray<byte>(srcData, Allocator.Persistent);
            textureDataJob.dstData = new NativeArray<byte>(srcData.Length * pixelFormat, Allocator.Persistent);

            textureDataJob.srcWidth = srcWidth;
            textureDataJob.srcHeight = srcHeight;
            textureDataJob.pixelPerCell = pixelPerCell;
            textureDataJob.pixelFormat = pixelFormat;
            textureDataJob.offset = offset;

            textureDataJob.dstWidth = srcWidth * pixelPerCell * pixelFormat;
            textureDataJob.dstHeight = srcHeight * pixelPerCell;

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
