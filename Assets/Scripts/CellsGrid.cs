using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenerateTextureScript))]
public class CellsGrid : MonoBehaviour
{
    private GenerateTextureScript _texture;

    public int CellsByHorizontal = -1;
    public int CellsByVertical = -1;

    [HideInInspector]
    public int PixelsByHorizontal;
    [HideInInspector]
    public int PixelsByVercital;
    public int PixelsByCell = 8;

    public void Awake()
    {
        PixelsByHorizontal = Screen.width - Screen.width % PixelsByCell;
        PixelsByVercital = Screen.height - Screen.height % PixelsByCell;

        CellsByHorizontal = PixelsByHorizontal / PixelsByCell;
        CellsByVertical = PixelsByVercital / PixelsByCell;
    }

    private void Start()
    {
        _texture = GetComponent<GenerateTextureScript>();

        StartGame();
    }

    public bool[,] _fields;
   
    public int numDensity;

    [Range(0f, 2f)]
    public float timeOfTick = 0.1f;
    private void StartGame()
    {

        _fields = new bool[CellsByHorizontal, CellsByVertical];

        for (int i = 0; i < CellsByHorizontal; i++)
        {
            for (int j = 0; j < CellsByVertical; j++)
            {
                _fields[i, j] = Random.Range(0, numDensity+1) == 0;
            }
        }

        StartCoroutine(TickRate());
    }

    private void NextGeneration()
    {
        _texture.SetTextureColor(0);

        var newFiled = new bool[CellsByHorizontal, CellsByVertical];

        for (int x = 0; x < CellsByHorizontal; x++)
        {
            for (int y = 0; y < CellsByVertical; y++)
            {
                var countSosedi = SosediCount(x, y);
                var isLife = _fields[x, y];

                if (isLife)
                {
                    if ((countSosedi == 2) | (countSosedi == 3))
                        newFiled[x, y] = true;
                    else 
                        newFiled[x, y] = false;
                }
                else
                {
                    if (countSosedi == 3)
                        newFiled[x, y] = true;
                    else 
                        newFiled[x, y] = false;
                }

                if (isLife)
                {
                    _texture.SetCellColor(x, y, 1);
                }
            }

        }

        _texture.Apply();
        _fields = newFiled;
    }

    private int SosediCount(int x, int y)
    {
        var res = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var col = x + i;
                var row = y + j;

                if ((col < 0) || (row < 0) || (col >= CellsByHorizontal) || (row >= CellsByVertical) || ((col == x) && (row == y)))
                {
                    continue;
                }

                if (_fields[col, row]) res++;
            }
        }
        return res;
    }

    private IEnumerator TickRate()
    {
        yield return new WaitForSeconds(timeOfTick);

        NextGeneration();

        StartCoroutine(TickRate());
    }
}
