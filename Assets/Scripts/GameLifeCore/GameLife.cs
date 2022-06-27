using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameLife
{
    private int _cellsPerHorizontal, _cellsPerVertical;
    private int _startChanceOfLifeForCell;
    private int _xmax, _xmax1;
    private int _ymax, _ymax1;
    public GameLife(int cellsByHorizontal, int сellsByVertical, int startChanceOfLifeForCell = 50)
    {
        _cellsPerHorizontal = cellsByHorizontal;
        _cellsPerVertical = сellsByVertical;
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

    public CellsBase NextGeneration(CellsBase inputCells)
    {
        var newField = new CellsBase(_cellsPerHorizontal, _cellsPerVertical);

        RecalcNewField(0, inputCells, newField, SosediCornerCount(inputCells, ESide.TL));
        RecalcNewField(_xmax, inputCells, newField, SosediCornerCount(inputCells, ESide.TR));
        RecalcNewField(_xmax + _ymax * _cellsPerHorizontal, inputCells, newField, SosediCornerCount(inputCells, ESide.BR));
        RecalcNewField(_ymax * _cellsPerHorizontal, inputCells, newField, SosediCornerCount(inputCells, ESide.BL));

        for (int x = 1; x < _cellsPerHorizontal - 1; x++)
        {
            RecalcNewField(x, inputCells, newField, SosediCount(x, inputCells, ESide.Top));
            RecalcNewField(x + _ymax * _cellsPerHorizontal, inputCells, newField, SosediCount(x, inputCells, ESide.Bottom));
        }
        for (int y = 1; y < _cellsPerVertical - 1; y++)
        {
            RecalcNewField(y * _cellsPerHorizontal, inputCells, newField, SosediCount(y, inputCells, ESide.Left));
            RecalcNewField(_xmax + y * _cellsPerHorizontal, inputCells, newField, SosediCount(y, inputCells, ESide.Right));
        }

        for (int x = 1; x < _cellsPerHorizontal - 1; x++)
        {
            for (int y = 1; y < _cellsPerVertical - 1; y++)
            {
                RecalcNewField(x + y * _cellsPerHorizontal, inputCells, newField, SosediCount(x, y, inputCells));
            }
        }

        return newField;
    }

    /// <summary>
    /// Выбор судьбы клетки в зависимоси от количества соседей. Аналогично тому, что ниже, просто другая форма записи
    /// </summary>
    private void RecalcNewField(int index, CellsBase inputCells, CellsBase newField, int countSosedi)
    {
        if (inputCells[index] != 0)
        {
            if ((countSosedi == 2) | (countSosedi == 3))
                newField[index] = 1;
            else
                newField[index] = 0;
        }
        else
        {
            if (countSosedi == 3)
                newField[index] = 1;
            else
                newField[index] = 0;
        }
    }

    /// <summary>
    /// Выбор судьбы клетки в зависимоси от количества соседей. Аналогично тому, что выше, просто другая форма записи
    /// </summary>
    private void RecalcNewFieldNew(int index, CellsBase inputCells, CellsBase newField, int countSosedi)
    {
        if ((countSosedi == 3) || (inputCells[index] == 1 && countSosedi == 2))
            newField[index] = 1;
        else
            newField[index] = 0;
    }


    private int SosediCount(int x, int y, CellsBase inputCells)
    {
        var x100 = x - 1;
        var x010 = x;
        var x001 = x + 1;
        var y100 = y - 1;
        var y010 = y;
        var y001 = y + 1;

        return inputCells[x010 + y100 * _cellsPerHorizontal] + inputCells[x001 + y100 * _cellsPerHorizontal] + inputCells[x001 + y010 * _cellsPerHorizontal] + 
            inputCells[x001 + y001 * _cellsPerHorizontal] + inputCells[x010 + y001 * _cellsPerHorizontal] + inputCells[x100 + y001 * _cellsPerHorizontal] + 
            inputCells[x100 + y010 * _cellsPerHorizontal] + inputCells[x100 + y100 * _cellsPerHorizontal];
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
                res = inputCells[p001] + inputCells[p001 + _cellsPerHorizontal] + inputCells[p010 + _cellsPerHorizontal] + 
                    inputCells[p100 + _cellsPerHorizontal] + inputCells[p100] + inputCells[p001 + _ymax * _cellsPerHorizontal] + 
                    inputCells[p010 + _ymax * _cellsPerHorizontal] + inputCells[p100 + _ymax * _cellsPerHorizontal];
                break;
            case ESide.Right:
                res = inputCells[_xmax + p001 * _cellsPerHorizontal] + inputCells[_xmax1 + p001 * _cellsPerHorizontal] + inputCells[_xmax1 + p010 * _cellsPerHorizontal] + 
                    inputCells[_xmax1 + p100 * _cellsPerHorizontal] + inputCells[_xmax + p100 * _cellsPerHorizontal] + inputCells[p100 * _cellsPerHorizontal] + 
                    inputCells[p010 * _cellsPerHorizontal] + inputCells[0 + p001 * _cellsPerHorizontal];
                break;
            case ESide.Bottom:
                res = inputCells[p001 + _ymax * _cellsPerHorizontal] + inputCells[p001 + _ymax1 * _cellsPerHorizontal] + inputCells[p010 + _ymax1 * _cellsPerHorizontal] + 
                    inputCells[p100 + _ymax1 * _cellsPerHorizontal] + inputCells[p100 + _ymax * _cellsPerHorizontal] + inputCells[p100] + 
                    inputCells[p010] + inputCells[p001];
                break;
            case ESide.Left:
                res = inputCells[p001 * _cellsPerHorizontal] + inputCells[1 + p001 * _cellsPerHorizontal] + inputCells[1 + p010 * _cellsPerHorizontal] +
                    inputCells[1 + p100 * _cellsPerHorizontal] + inputCells[p100 * _cellsPerHorizontal] + inputCells[_xmax + p100 * _cellsPerHorizontal] + 
                    inputCells[_xmax + p010 * _cellsPerHorizontal] + inputCells[_xmax + p001 * _cellsPerHorizontal];
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
                res = inputCells[1] + inputCells[1 + _cellsPerHorizontal] + inputCells[_cellsPerHorizontal] +
                        inputCells[_xmax] + inputCells[_xmax + _cellsPerHorizontal] + inputCells[_ymax * _cellsPerHorizontal] + 
                        inputCells[1 + _ymax * _cellsPerHorizontal] + inputCells[_xmax + _ymax * _cellsPerHorizontal];
                break;
            case ESide.TR:
                res = inputCells[_xmax1] + inputCells[_xmax1 + _cellsPerHorizontal] + inputCells[_xmax + _cellsPerHorizontal] +
                         inputCells[0] + inputCells[_cellsPerHorizontal] + inputCells[_xmax + _ymax * _cellsPerHorizontal] + 
                         inputCells[_xmax1 + _ymax * _cellsPerHorizontal] + inputCells[_ymax * _cellsPerHorizontal];
                break;
            case ESide.BR:
                res = inputCells[_xmax1 + _ymax * _cellsPerHorizontal] + inputCells[_xmax1 + _ymax1 * _cellsPerHorizontal] + inputCells[_xmax + _ymax1 * _cellsPerHorizontal] +
                        inputCells[_ymax * _cellsPerHorizontal] + inputCells[_ymax1 * _cellsPerHorizontal] + inputCells[_xmax] + 
                        inputCells[_xmax1] + inputCells[0];
                break;
            case ESide.BL:
                res = inputCells[_ymax1 * _cellsPerHorizontal] + inputCells[1 + _ymax1 * _cellsPerHorizontal] + inputCells[1 + _ymax * _cellsPerHorizontal] +
                        inputCells[0] + inputCells[1] + inputCells[_xmax + _ymax * _cellsPerHorizontal] + 
                        inputCells[_xmax + _ymax1 * _cellsPerHorizontal] + inputCells[_xmax];
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
