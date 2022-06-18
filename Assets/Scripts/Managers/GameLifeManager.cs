using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Diagnostics;


[RequireComponent(typeof(GenerateTextureManager))]
public class GameLifeManager : MonoBehaviour
{
    [Range(0f, 2f)]
    public float TimeOfTick = 0.1f;

    private GenerateTextureManager _texture;

    public int CellsPerHorizontal = -1;
    public int CellsPerVertical = -1;

    public int PixelsPerHorizontal;
    public int PixelsPerVercital;
    public int PixelsPerCell = 8;

    public int StartChanceOfLifeForCell = 50;

    private GameLife _gameLife;
    private GameLifeBoolsArray _gameLifeB;

    public void Awake()
    {
        PixelsPerHorizontal = Screen.width - Screen.width % PixelsPerCell;
        PixelsPerVercital = Screen.height - Screen.height % PixelsPerCell;

        CellsPerHorizontal = PixelsPerHorizontal / PixelsPerCell;
        CellsPerVertical = PixelsPerVercital / PixelsPerCell;

        _texture = GetComponent<GenerateTextureManager>();
        _texture.Init(this);
    }

    private bool[,] _fieldBoolsArray;
    private CellsBase _field;

    private void Start()
    {   
        StartGame();
    }

    public enum ETypeOfArray
    {
        CellsBase,
        BoolsArray
    }

    public ETypeOfArray type;

    private void StartGame()
    {
        _gameLife = new GameLife(CellsPerHorizontal, PixelsPerVercital, StartChanceOfLifeForCell);
        _gameLifeB = new GameLifeBoolsArray(CellsPerHorizontal, PixelsPerVercital, StartChanceOfLifeForCell);
        _fieldBoolsArray = new bool[CellsPerHorizontal, PixelsPerVercital];

        _field = new CellsBase(CellsPerHorizontal, PixelsPerVercital);

        if (type == ETypeOfArray.BoolsArray)
            _fieldBoolsArray = _gameLifeB.FirstGeneration(_fieldBoolsArray);
        else
            _field = _gameLife.FirstGeneration(_field);

        TextureRefresh();

        StartCoroutine(TickRate());
    }

    private IEnumerator TickRate()
    {
        if (TimeOfTick == 0f)
            yield return null;
        else
            yield return new WaitForSeconds(TimeOfTick);

        Stopwatch sw = new Stopwatch();
        sw.Start();
        
        if (type == ETypeOfArray.BoolsArray)
            _fieldBoolsArray = _gameLifeB.NextGeneration(_fieldBoolsArray);
        else
            _field = _gameLife.NextGeneration(_field);
        
        sw.Stop();
        UnityEngine.Debug.Log("NextGeneration:" + sw.ElapsedMilliseconds);

        TextureRefresh();

        StartCoroutine(TickRate());
    }

    private void TextureRefresh()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        if (type == ETypeOfArray.CellsBase)
        {
            /*
              for (int x = 0; x < CellsPerHorizontal; x++)
                  for (int y = 0; y < CellsPerVertical; y++)
                      _texture.SetCellColor(x, y, _field[x, y] ? 1 : 0);
            */
            _texture.SetTextureColorApply(_field);  
        }
        else
        {
            for (int x = 0; x < CellsPerHorizontal; x++)
                for (int y = 0; y < CellsPerVertical; y++)
                    _texture.SetCellColor(x, y, _fieldBoolsArray[x, y] ? 1 : 0);
            _texture.Apply();
        }
        
        
        sw.Stop();
        UnityEngine.Debug.Log("TextureRefresh:" + sw.ElapsedMilliseconds);
    }
}


