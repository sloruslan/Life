using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameLife
{
    private int _cellsPerHorizontal, _cellsPerVertical;
    private int _startChanceOfLifeForCell;
    private int _xmax, _xmax100, _xmax001;
    private int _ymax, _ymax100, _ymax001;
    public GameLife(int cellsByHorizontal, int ñellsByVertical, int startChanceOfLifeForCell = 50)
    {
        _cellsPerHorizontal = cellsByHorizontal;
        _cellsPerVertical = ñellsByVertical;
        _startChanceOfLifeForCell = startChanceOfLifeForCell;
        _xmax = _cellsPerHorizontal - 1;
        _ymax = _cellsPerVertical - 1;
        _xmax100 = _xmax - 1;
        _xmax001 = _xmax + 1;
        _ymax100 = _ymax - 1;
        _ymax001 = _ymax + 1;
    }
    public CellsBase FirstGeneration(CellsBase inputCells)
    {
        for (int i = 0; i < _cellsPerHorizontal; i++)
        {
            for (int j = 0; j < _cellsPerVertical; j++)
            {
                inputCells[i, j] = (byte)((Random.Range(0, 101) <= _startChanceOfLifeForCell) ? 1 : 0);
            }
        }

        return inputCells;
    }

    public CellsBase NextGeneration(CellsBase inputCells)
    {
        var newField = new CellsBase(_cellsPerHorizontal, _cellsPerVertical);

        RecalcNewField(0, 0, inputCells, newField, SosediCornerCount(inputCells, ESide.TL));
        RecalcNewField(_xmax, 0, inputCells, newField, SosediCornerCount(inputCells, ESide.TR));
        RecalcNewField(_xmax, _ymax, inputCells, newField, SosediCornerCount(inputCells, ESide.BR));
        RecalcNewField(0, _ymax, inputCells, newField, SosediCornerCount(inputCells, ESide.BL));

        for (int x = 1; x < _cellsPerHorizontal - 1; x++)
        {
            RecalcNewField(x, 0, inputCells, newField, SosediCount(x, inputCells, ESide.Top));
            RecalcNewField(x, _ymax, inputCells, newField, SosediCount(x, inputCells, ESide.Bottom));
        }
        for (int y = 1; y < _cellsPerVertical - 1; y++)
        {
            RecalcNewField(0, y, inputCells, newField, SosediCount(y, inputCells, ESide.Left));
            RecalcNewField(_xmax, y, inputCells, newField, SosediCount(y, inputCells, ESide.Right));
        }
        
        for (int x = 1; x < _cellsPerHorizontal - 1; x++)
        {
            for (int y = 1; y < _cellsPerVertical - 1; y++)
            {
                RecalcNewField(x, y, inputCells, newField, SosediCount(x, y, inputCells));
            }
        }

        return newField;
    }

    
    private void RecalcNewField(int x, int y, CellsBase inputCells, CellsBase newField, int countSosedi)
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
    
    
    private void RecalcNewFieldNew(int x, int y, CellsBase inputCells, CellsBase newField, int countSosedi)
    {
        if ((countSosedi == 3) || (inputCells[x, y] == 1 && countSosedi == 2))
            newField[x, y] = 1;
        else
            newField[x, y] = 0;
    }
    

    private int SosediCount(int x, int y, CellsBase inputCells)
    {
        var x100 = x - 1;
        var x010 = x;
        var x001 = x + 1;
        var y100 = y - 1;
        var y010 = y;
        var y001 = y + 1;

        return inputCells[x010, y100 ] + inputCells[x001, y100] + inputCells[x001, y010] + inputCells[x001, y001] + inputCells[x010, y001] + inputCells[x100, y001] + inputCells[x100, y010] + inputCells[x100, y100];
    }



    private int SosediCount(int index, CellsBase inputCells, ESide side)
    {
        var res = 0;

        var p100 = index - 1;
        var p010 = index;
        var p001 = index + 1;

        switch (side)
        {
            case ESide.Top:
                res = inputCells[p001, 0] + inputCells[p001, 1] + inputCells[p010, 1] + inputCells[p100, 1] + inputCells[p100, 0] + inputCells[p001, _ymax] + inputCells[p010, _ymax] + inputCells[p100, _ymax];
                break;
            case ESide.Right:
                res = inputCells[_xmax, p001] + inputCells[_xmax - 1,p001] + inputCells[_xmax-1, p010] + inputCells[_xmax-1, p100] + inputCells[_xmax, p100] + inputCells[0, p100] + inputCells[0, p010] + inputCells[0, p001];
                break;
            case ESide.Bottom:
                res = inputCells[p001, _ymax] + inputCells[p001, _ymax - 1] + inputCells[p010, _ymax - 1] + inputCells[p100, _ymax - 1] + inputCells[p100, _ymax] + inputCells[p100, 0] + inputCells[p010, 0] + inputCells[p001, 0];
                break;
            case ESide.Left:
                res = inputCells[0, p001] + inputCells[1, p001] + inputCells[1, p010] + inputCells[1, p100] + inputCells[0, p100] + inputCells[_xmax, p100] + inputCells[_xmax, p010] + inputCells[_xmax, p001];
                break;
        }

        return res;
    }

    private int SosediCornerCount(CellsBase inputCells, ESide side)
    {
        int res = 0;
        switch (side)
        {
            case ESide.TL:
                res = inputCells[1, 0] + inputCells[1, 1] + inputCells[0, 1] + 
                        inputCells[_xmax, 0] + inputCells[_xmax, 1] + inputCells[0, _ymax] + inputCells[1, _ymax] + inputCells[_xmax, _ymax];
                break;
            case ESide.TR:
                res = inputCells[_xmax - 1, 0] + inputCells[_xmax - 1, 1] + inputCells[_xmax, 1] +
                         inputCells[0, 0] + inputCells[0, 1] + inputCells[_xmax, _ymax] + inputCells[_xmax - 1, _ymax] + inputCells[0, _ymax];
                break;
            case ESide.BR:
                res = inputCells[_xmax - 1, _ymax] + inputCells[_xmax - 1, _ymax - 1] + inputCells[_xmax, _ymax - 1] + 
                        inputCells[0, _ymax] + inputCells[0, _ymax - 1] + inputCells[_xmax, 0] + inputCells[_xmax - 1, 0] + inputCells[0, 0];
                break;
            case ESide.BL:
                res = inputCells[0, _ymax - 1] + inputCells[1, _ymax - 1] + inputCells[1, _ymax] + 
                        inputCells[0, 0] + inputCells[1, 0] + inputCells[_xmax, _ymax] + inputCells[_xmax, _ymax - 1] + inputCells[_xmax, 0];
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
