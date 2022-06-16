using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CellsGrid))]
public class ColorsManager : MonoBehaviour
{
    [SerializeField]
    private List<Color> _colors;

    private CellsGrid _cellsGrid;
    private List<Color[]> _colorArraysList;

    public int CountOfColorsArray => _colors.Count;

    private void Awake()
    {
        _cellsGrid = GetComponent<CellsGrid>();
        SetArraysSize(_cellsGrid.PixelsByCell);
    }

    public void SetArraysSize(int cellSize)
    {
        if (_colorArraysList == null)
            _colorArraysList = new List<Color[]>();
        else
            _colorArraysList.Clear();

        foreach (var color in _colors)
        {
            var colorArray = new Color[cellSize * cellSize];
            for (int index = 0; index < colorArray.Length; index++)
                    colorArray[index] = color;

            _colorArraysList.Add(colorArray);
        }
    }

    public Color[] this[int index]
    {
        get
        {
            if (index >= _colorArraysList.Count)
                return _colorArraysList[0];

            return _colorArraysList[index];
        }
    }
}
