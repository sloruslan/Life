using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

public class GameLifeJobBackup : MonoBehaviour
{
    private JobHandle handle;
    public CellsBase outputData;

    [ReadOnly]
    public NativeArray<byte> inputArray;
    [WriteOnly]
    public NativeArray<byte> outputArray;

    public UnityAction<CellsBase> WaitJobCompletedEvent { get; set; }

    public void CreateJob(CellsBase inputCells, CellsBase newField) 
    {
        NextGenerationJob nexGenJob = new NextGenerationJob();

        if (outputArray.Length == 0)
            inputArray = new NativeArray<byte>(inputCells.Array, Allocator.Persistent);
        else
            inputArray.CopyFrom(inputCells.Array);

        if (outputArray.Length == 0)
            outputArray = new NativeArray<byte>(inputCells.Array, Allocator.Persistent);
        else
            outputArray.CopyFrom(newField.Array);

        nexGenJob.inputArray = inputArray;
        nexGenJob.outputArray = outputArray;

        handle = nexGenJob.Schedule(inputArray.Length, 1);
        handle.Complete();
        StartCoroutine(WaitJobCompleted());
    }

    private void OnDisable()
    {
        inputArray.Dispose();
        outputArray.Dispose();
    }

    private IEnumerator WaitJobCompleted()
    {

        if (handle.IsCompleted)
        {
        //    Debug.Log("handle.IsCompleted");
        //    outputData = new CellsBase(outputArray.ToArray());
        //    WaitJobCompletedEvent?.Invoke(outputData);
        }
        else
            yield return null;
    }

    public struct NextGenerationJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<byte> inputArray;
        [WriteOnly]
        public NativeArray<byte> outputArray;

        public int w;
        public int h;

        public int _xmax;
        public int _xmax1;
        public int _ymax;
        public int _ymax1;

        public void Execute(int index)
        {
            /*
            outputArray[index] = RecalcNewField(0, inputArray, SosediCornerCount(inputCells, ESide.TL));
            outputArray[index] = RecalcNewField(_xmax, inputArray, SosediCornerCount(inputCells, ESide.TR));
            outputArray[index] = RecalcNewField(_xmax + _ymax * w, inputArray, SosediCornerCount(inputCells, ESide.BR));
            outputArray[index] = RecalcNewField(_ymax * w, inputArray, newField, SosediCornerCount(inputCells, ESide.BL));

            for (int x = 1; x < w - 1; x++)
            {
                outputArray[index] = RecalcNewField(x, inputCells, newField, SosediCount(x, inputCells, ESide.Top));
                outputArray[index] = RecalcNewField(x + _ymax * w, inputCells, newField, SosediCount(x, inputCells, ESide.Bottom));
            }
            for (int y = 1; y < _cellsPerVertical - 1; y++)
            {
                outputArray[index] = RecalcNewField(y * w, inputCells, newField, SosediCount(y, inputCells, ESide.Left));
                outputArray[index] = RecalcNewField(_xmax + y * w, inputCells, newField, SosediCount(y, inputCells, ESide.Right));
            }

            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    outputArray[index] = RecalcNewField(x + y * w, inputArray, outputArray, SosediCount(x, y, inputArray, w));
                }
            }


            byte RecalcNewField(int index, NativeArray<byte> inputCells, int countSosedi)
            {
                if (inputCells[index] != 0)
                {
                    if ((countSosedi == 2) | (countSosedi == 3))
                        return 1;
                    else
                        return 0;
                }
                else
                {
                    if (countSosedi == 3)
                        return 1;
                    else
                        return 0;
                }
            }

            int SosediCount(int x, int y, NativeArray<byte> inputCells, int w)
            {
                var x100 = x - 1;
                var x010 = x;
                var x001 = x + 1;
                var y100 = y - 1;
                var y010 = y;
                var y001 = y + 1;

                return inputCells[x010 + y100 * w] + inputCells[x001 + y100 * w] + inputCells[x001 + y010 * w] +
                    inputCells[x001 + y001 * w] + inputCells[x010 + y001 * w] + inputCells[x100 + y001 * w] +
                    inputCells[x100 + y010 * w] + inputCells[x100 + y100 * w];
            }

            int SosediCount(int x, int y, NativeArray<byte> inputCells, int w)
            {
                var x100 = x - 1;
                var x010 = x;
                var x001 = x + 1;
                var y100 = y - 1;
                var y010 = y;
                var y001 = y + 1;

                return inputCells[x010 + y100 * w] + inputCells[x001 + y100 * w] + inputCells[x001 + y010 * w] +
                    inputCells[x001 + y001 * w] + inputCells[x010 + y001 * w] + inputCells[x100 + y001 * w] +
                    inputCells[x100 + y010 * w] + inputCells[x100 + y100 * w];
           
            }
            
        



            int SosediCount(int index, NativeArray<byte> inputCells, ESide side, int w, int _xmax, int _xmax1, int _ymax, int _ymax1)
            {
                var res = 0;

                var p100 = index - 1;
                var p010 = index;
                var p001 = index + 1;

                switch (side)
                {
                    case ESide.Top:
                        res = inputCells[p001] + inputCells[p001 + w] + inputCells[p010 + w] +
                            inputCells[p100 + w] + inputCells[p100] + inputCells[p001 + _ymax * w] +
                            inputCells[p010 + _ymax * w] + inputCells[p100 + _ymax * w];
                        break;
                    case ESide.Right:
                        res = inputCells[_xmax + p001 * w] + inputCells[_xmax1 + p001 * w] + inputCells[_xmax1 + p010 * w] +
                            inputCells[_xmax1 + p100 * w] + inputCells[_xmax + p100 * w] + inputCells[p100 * w] +
                            inputCells[p010 * w] + inputCells[0 + p001 * w];
                        break;
                    case ESide.Bottom:
                        res = inputCells[p001 + _ymax * w] + inputCells[p001 + _ymax1 * w] + inputCells[p010 + _ymax1 * w] +
                            inputCells[p100 + _ymax1 * w] + inputCells[p100 + _ymax * w] + inputCells[p100] +
                            inputCells[p010] + inputCells[p001];
                        break;
                    case ESide.Left:
                        res = inputCells[p001 * w] + inputCells[1 + p001 * w] + inputCells[1 + p010 * w] +
                            inputCells[1 + p100 * w] + inputCells[p100 * w] + inputCells[_xmax + p100 * w] +
                            inputCells[_xmax + p010 * w] + inputCells[_xmax + p001 * w];
                        break;
                }

                return res;
            }

            int SosediCornerCount(NativeArray<byte> inputCells, ESide side, int w, int _xmax, int _xmax1, int _ymax, int _ymax1)
            {
                int res = 0;
                switch (side)
                {
                    case ESide.TL:
                        res = inputCells[1] + inputCells[1 + w] + inputCells[w] +
                                inputCells[_xmax] + inputCells[_xmax + w] + inputCells[_ymax * w] +
                                inputCells[1 + _ymax * w] + inputCells[_xmax + _ymax * w];
                        break;
                    case ESide.TR:
                        res = inputCells[_xmax1] + inputCells[_xmax1 + w] + inputCells[_xmax + w] +
                                 inputCells[0] + inputCells[w] + inputCells[_xmax + _ymax * w] +
                                 inputCells[_xmax1 + _ymax * w] + inputCells[_ymax * w];
                        break;
                    case ESide.BR:
                        res = inputCells[_xmax1 + _ymax * w] + inputCells[_xmax1 + _ymax1 * w] + inputCells[_xmax + _ymax1 * w] +
                                inputCells[_ymax * w] + inputCells[_ymax1 * w] + inputCells[_xmax] +
                                inputCells[_xmax1] + inputCells[0];
                        break;
                    case ESide.BL:
                        res = inputCells[_ymax1 * w] + inputCells[1 + _ymax1 * w] + inputCells[1 + _ymax * w] +
                                inputCells[0] + inputCells[1] + inputCells[_xmax + _ymax * w] +
                                inputCells[_xmax + _ymax1 * w] + inputCells[_xmax];
                        break;
                }
                return res;
            }
            */
        }
    }



    private int _cellsPerHorizontal, _cellsPerVertical;
    private int _startChanceOfLifeForCell;
    private int _xmax, _xmax1;
    private int _ymax, _ymax1;
    public void Init(int cellsByHorizontal, int ñellsByVertical, int startChanceOfLifeForCell = 50)
    {
        _cellsPerHorizontal = cellsByHorizontal;
        _cellsPerVertical = ñellsByVertical;
        _startChanceOfLifeForCell = startChanceOfLifeForCell;
        _xmax = _cellsPerHorizontal - 1;
        _ymax = _cellsPerVertical - 1;
        _xmax1 = _xmax - 1;
        _ymax1 = _ymax - 1;
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
        var newField = new CellsBase(_cellsPerHorizontal, _cellsPerVertical);

        CreateJob(inputCells, newField);
    }

    private void RecalcNewFieldNew(int index, CellsBase inputCells, CellsBase newField, int countSosedi)
    {
        if ((countSosedi == 3) || (inputCells[index] == 1 && countSosedi == 2))
            newField[index] = 1;
        else
            newField[index] = 0;
    }

    

    enum ESide
    {
        Top,
        Right,
        Bottom,
        Left,
        TL,
        TR,
        BR,
        BL
    }
}

