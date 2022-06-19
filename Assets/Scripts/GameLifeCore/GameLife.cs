using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLifeBoolsArray
{
    private int _cellsPerHorizontal, _cellsPerVertical;
    
    private int _startChanceOfLifeForCell;

    public GameLifeBoolsArray(int cellsByHorizontal, int ñellsByVertical, int startChanceOfLifeForCell = 50)
    {
        _cellsPerHorizontal = cellsByHorizontal;
        _cellsPerVertical = ñellsByVertical;
        _startChanceOfLifeForCell = startChanceOfLifeForCell;

        
    }
    public bool[,] FirstGeneration(bool[,] inputCells)
    {
        for (int i = 0; i < _cellsPerHorizontal; i++)
        {
            for (int j = 0; j < _cellsPerVertical; j++)
            {
                inputCells[i, j] = Random.Range(0, 101) < _startChanceOfLifeForCell;
            }
        }

        return inputCells;
    }

    public bool[,] NextGeneration(bool[,] inputCells)
    {
        var newField = new bool[_cellsPerHorizontal, _cellsPerVertical];

        for (int x = 0; x < _cellsPerHorizontal; x++)
        {
            for (int y = 0; y < _cellsPerVertical; y++)
            {
                var countSosedi = SosediCount(x, y, inputCells);

                if (inputCells[x, y])
                {
                    if ((countSosedi == 2) | (countSosedi == 3))
                        newField[x, y] = true;
                    else
                        newField[x, y] = false;
                }
                else
                {
                    if (countSosedi == 3)
                        newField[x, y] = true;
                    else
                        newField[x, y] = false;
                }
            }
        }

        return newField;
    }

    private int SosediCount(int x, int y, bool[,] inputCells)
    {
        var res = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var col = x + i;
                var row = y + j;

                if ((col < 0) || (row < 0) || (col >= _cellsPerHorizontal) || (row >= _cellsPerVertical) || ((col == x) && (row == y)))
                {
                    continue;
                }

                if (inputCells[col, row]) res++;
            }
        }
        return res;
    }
}


public class GameLife
{
    private int _cellsPerHorizontal, _cellsPerVertical;
    private int _startChanceOfLifeForCell;
    private int _xmax, _ymax;
    public GameLife(int cellsByHorizontal, int ñellsByVertical, int startChanceOfLifeForCell = 50)
    {
        _cellsPerHorizontal = cellsByHorizontal;
        _cellsPerVertical = ñellsByVertical;
        _startChanceOfLifeForCell = startChanceOfLifeForCell;
        _xmax = _cellsPerHorizontal - 1;
        _ymax = _cellsPerVertical - 1;
    }
    public CellsBase FirstGeneration(CellsBase inputCells)
    {
        for (int i = 0; i < _cellsPerHorizontal; i++)
        {
            for (int j = 0; j < _cellsPerVertical; j++)
            {
                inputCells[i, j] = (byte)((Random.Range(0, 101) < _startChanceOfLifeForCell) ? 1 : 0);
            }
        }

        return inputCells;
    }

    public CellsBase NextGeneration(CellsBase inputCells)
    {
        var newField = new CellsBase(_cellsPerHorizontal, _cellsPerVertical);

        RecalcNewField(0, 0, ref inputCells, ref newField, SosediCornerCount(ref inputCells, ESide.TL));
        RecalcNewField(_xmax, 0, ref inputCells, ref newField, SosediCornerCount(ref inputCells, ESide.TR));
        RecalcNewField(_xmax, _ymax, ref inputCells, ref newField, SosediCornerCount(ref inputCells, ESide.BR));
        RecalcNewField(0, _ymax, ref inputCells, ref newField, SosediCornerCount(ref inputCells, ESide.BL));

        for (int x = 1; x < _cellsPerHorizontal - 1; x++)
        {
            RecalcNewField(x, 0, ref inputCells, ref newField, SosediCount(x, ref inputCells, ESide.Top));
            RecalcNewField(x, _ymax, ref inputCells, ref newField, SosediCount(x, ref inputCells, ESide.Bottom));
        }
        for (int y = 1; y < _cellsPerVertical - 1; y++)
        {
            RecalcNewField(0, y, ref inputCells, ref newField, SosediCount(y, ref inputCells, ESide.Left));
            RecalcNewField(_xmax, y, ref inputCells, ref newField, SosediCount(y, ref inputCells, ESide.Right));
        }
        
        for (int x = 1; x < _cellsPerHorizontal - 1; x++)
        {
            for (int y = 1; y < _cellsPerVertical - 1; y++)
            {
                RecalcNewField(x, y, ref inputCells, ref newField, SosediCount(x, y, ref inputCells));
            }
        }

        return newField;
    }

    private void RecalcNewField(int x, int y, ref CellsBase inputCells, ref CellsBase newField, int countSosedi)
    {
        if (inputCells[x, y] != 0)
        {
            if ((countSosedi == 2) | (countSosedi == 3))
                newField[x, y] = 1;
            else
                newField[x, y] = 0;
        }
        else
        {
            if (countSosedi == 3)
                newField[x, y] = 1;
            else
                newField[x, y] = 0;
        }
    }

    private int SosediCount(int x, int y, ref CellsBase inputCells)
    {
        var x100 = x - 1;
        var x010 = x;
        var x001 = x + 1;
        var y100 = y - 1;
        var y010 = y;
        var y001 = y + 1;

        return inputCells[x010, y100 ] + inputCells[x001, y100] + inputCells[x001, y010] + inputCells[x001, y001] + inputCells[x010, y001] + inputCells[x100, y001] + inputCells[x100, y010] + inputCells[x100, y100];
    }



    private int SosediCount(int index, ref CellsBase inputCells, ESide side)
    {
        var res = 0;

        var p100 = index - 1;
        var p010 = index;
        var p001 = index + 1;

        switch (side)
        {
            case ESide.Top:
                res = inputCells[p001, 0] + inputCells[p001, 1] + inputCells[p010, 1] + inputCells[p100, 1] + inputCells[p100, 0];
                break;
            case ESide.Right:
                res = inputCells[_xmax, p001] + inputCells[_xmax - 1,p001] + inputCells[_xmax-1, p010] + inputCells[_xmax-1, p100] + inputCells[_xmax, p100];
                break;
            case ESide.Bottom:
                res = inputCells[p001, _ymax] + inputCells[p001, _ymax - 1] + inputCells[p010, _ymax - 1] + inputCells[p100, _ymax - 1] + inputCells[p100, 0];
                break;
            case ESide.Left:
                res = inputCells[0, p001] + inputCells[1, p001] + inputCells[1, p010] + inputCells[1, p100] + inputCells[0, p100];
                break;
        }

        return res;
    }

    private int SosediCornerCount(ref CellsBase inputCells, ESide side)
    {
        int res = 0;
        switch (side)
        {
            case ESide.TL:
                res = inputCells[1, 0] + inputCells[1, 1] + inputCells[0, 1];
                break;
            case ESide.TR:
                res = inputCells[_xmax - 1, 0] + inputCells[_xmax - 1, 1] + inputCells[_xmax, 1];
                break;
            case ESide.BR:
                res = inputCells[_xmax - 1, _ymax] + inputCells[_xmax - 1, _ymax - 1] + inputCells[_xmax, _ymax - 1];
                break;
            case ESide.BL:
                res = inputCells[0, _ymax - 1] + inputCells[1, _ymax - 1] + inputCells[1, _ymax];
                break;
        }
        return res;
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
