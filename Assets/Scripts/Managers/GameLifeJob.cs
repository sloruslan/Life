using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

public class GameLifeJob : MonoBehaviour
{
    private int _cellsPerHorizontal, _cellsPerVertical;
    private int _startChanceOfLifeForCell;
    public void Init(int cellsByHorizontal, int ñellsByVertical, int startChanceOfLifeForCell = 50)
    {
        _cellsPerHorizontal = cellsByHorizontal;
        _cellsPerVertical = ñellsByVertical;
        _startChanceOfLifeForCell = startChanceOfLifeForCell;
    }

    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

    private JobHandle handle;
    public CellsBase outputData;

    [ReadOnly]
    public NativeArray<byte> inputArray;
    [WriteOnly]
    public NativeArray<byte> outputArrayValue;
    [WriteOnly]
    public NativeArray<byte> outputArrayColor;

    public UnityAction<CellsBase> WaitJobCompletedEvent { get; set; }

    [Range(1, 50)]
    public int InnerLoop = 5;

    public void CreateJob(CellsBase inputCells) 
    {
        NextGenerationJob nexGenJob = new NextGenerationJob();

        sw.Restart();
        sw.Start();

        if (inputArray.Length == 0)
            inputArray = new NativeArray<byte>(inputCells.Array, Allocator.Persistent);
        else
            inputArray.CopyFrom(inputCells.Array);

        

        if (outputArrayValue.Length == 0)
            outputArrayValue = new NativeArray<byte>(inputCells.Array.Length, Allocator.Persistent);

        if (outputArrayColor.Length == 0)
            outputArrayColor = new NativeArray<byte>(inputCells.Array.Length * 3, Allocator.Persistent);

        nexGenJob.inputArray = inputArray;
        nexGenJob.outputArrayValue = outputArrayValue;
        nexGenJob.outputArrayColor = outputArrayColor;
        nexGenJob.w = _cellsPerHorizontal;
        nexGenJob.h = _cellsPerVertical;

        
        

        handle = nexGenJob.Schedule(inputArray.Length, InnerLoop);
        
        handle.Complete();

        sw.Stop();
        Debug.Log("CreateJob:" + sw.ElapsedMilliseconds);
        StartCoroutine(WaitJobCompleted());
    }

    private void OnDisable()
    {
        inputArray.Dispose();
        outputArrayValue.Dispose();
        outputArrayColor.Dispose();
    }

    private IEnumerator WaitJobCompleted()
    {
        if (handle.IsCompleted)
        {
            WaitJobCompletedEvent?.Invoke(new CellsBase(outputArrayValue.ToArray(), outputArrayColor.ToArray()));
        }
        else
            yield return null;
    }

    public struct NextGenerationJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<byte> inputArray;
        [WriteOnly]
        public NativeArray<byte> outputArrayValue;
        [WriteOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<byte> outputArrayColor;

        public int w;
        public int h;

        public void Execute(int index)
        {
            int y = index / w;
            int x = index - y * w;

            byte value = RecalcNewFieldNew(index, inputArray, SosediCount(inputArray, x, y, w, h));
            outputArrayValue[index] = value;
            outputArrayColor[index * 3 + 1] = value == 1 ? (byte)255 : (byte)0;

            byte RecalcNewFieldNew(int index, NativeArray<byte> inputCells, int countSosedi)
            {
                if ((countSosedi == 3) || (inputCells[index] == 1 && countSosedi == 2))
                    return 1;
                else
                    return 0;
            }

            
            int SosediCount(NativeArray<byte> inputCells, int x, int y, int w, int h)
            {
                var res = 0;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        var col = x + i;
                        var row = y + j;

                        if ((col < 0) || (row < 0) || (col >= w) || (row >= h) || ((col == x) && (row == y)))
                        {
                            continue;
                        }

                        if (inputCells[col + row * w] == 1) res++;
                    }
                }
                return res;
            }
        }
    }

    public CellsBase FirstGeneration(CellsBase inputCells)
    {
        for (int i = 0; i < _cellsPerHorizontal; i++)
        {
            for (int j = 0; j < _cellsPerVertical; j++)
            {
                inputCells[i + j * _cellsPerHorizontal] = (byte)((Random.Range(0, 101) <= _startChanceOfLifeForCell) ? 1 : 0);
            }
        }

        return inputCells;
    }

    public void NextGeneration(CellsBase inputCells)
    {
        CreateJob(inputCells);
    }
}

