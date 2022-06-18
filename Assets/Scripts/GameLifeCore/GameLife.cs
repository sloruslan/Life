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

    public GameLife(int cellsByHorizontal, int ñellsByVertical, int startChanceOfLifeForCell = 50)
    {
        _cellsPerHorizontal = cellsByHorizontal;
        _cellsPerVertical = ñellsByVertical;
        _startChanceOfLifeForCell = startChanceOfLifeForCell;
    }
    public CellsBase FirstGeneration(CellsBase inputCells)
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

    public CellsBase NextGeneration(CellsBase inputCells)
    {
        var newField = new CellsBase(_cellsPerHorizontal, _cellsPerVertical);

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

    private int SosediCount(int x, int y, CellsBase inputCells)
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
