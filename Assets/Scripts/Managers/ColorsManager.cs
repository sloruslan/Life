using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorsManager : MonoBehaviour
{
    [SerializeField]
    private List<Color> _colors;

    private List<Color[]> _colorArraysList;
    private int _cellSize;

    public int CountOfColorsArray => _colors.Count;

    public void Init(int cellSize)
    {
        SetArraysSize(_cellSize = cellSize);
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
            if (_colorArraysList == null)
                SetArraysSize(_cellSize);

            if (index >= _colorArraysList.Count)
                return _colorArraysList[0];

            return _colorArraysList[index];
        }
    }
}
