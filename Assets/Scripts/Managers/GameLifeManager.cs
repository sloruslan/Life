using System.Collections;
using UnityEngine;
using System.Diagnostics;
using Unity.Jobs;

[RequireComponent(typeof(GenerateTextureManager))]
[RequireComponent(typeof(GameLifeJob))]
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

    [Range(0, 100)]
    public int StartChanceOfLifeForCell = 50;

    private GameLife _gameLife;
    private GameLifeJob _gameLifeJob;
    private JobHandle _jobHandle;

    public ETypeOfRender typeRender;
    public ETypeOfCalc typeCalc;

    public enum ETypeOfCalc
    {
        Mono,
        Job
    }

    public enum ETypeOfRender
    {
        Array,
        Stream
    }

    

    public void Awake()
    {
        PixelsPerHorizontal = Screen.width - Screen.width % PixelsPerCell;
        PixelsPerVercital = Screen.height - Screen.height % PixelsPerCell;

        CellsPerHorizontal = PixelsPerHorizontal / PixelsPerCell;
        CellsPerVertical = PixelsPerVercital / PixelsPerCell;

        _texture = GetComponent<GenerateTextureManager>();
        _texture.Init(this);

        _gameLifeJob = GetComponent<GameLifeJob>();
    }

    private CellsBase _field;

    private void Start()
    {
        StartGame();
    }

    

    private void StartGame()
    {
        _gameLife = new GameLife(CellsPerHorizontal, PixelsPerVercital, StartChanceOfLifeForCell);
        _gameLifeJob.Init(CellsPerHorizontal, PixelsPerVercital, StartChanceOfLifeForCell);

        _field = new CellsBase(CellsPerHorizontal, PixelsPerVercital);

        if (typeCalc == ETypeOfCalc.Mono)
            _field = _gameLife.FirstGeneration(_field);
        else
            _field = _gameLifeJob.FirstGeneration(_field);

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

        if (typeCalc == ETypeOfCalc.Mono)
        {
            _field = _gameLife.NextGeneration(_field);
            TextureRefresh();
            StartCoroutine(TickRate());
        }
        else
        {
            _ = _gameLifeJob.NextGeneration(_field);
        }
        sw.Stop();
        UnityEngine.Debug.Log("NextGeneration:" + sw.ElapsedMilliseconds); 
    }

    private void TextureRefresh()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();


        if (typeRender == ETypeOfRender.Stream)
            _texture.SetTextureColorStreamApply(_field);
        else
            _texture.SetTextureColorApply(_field);

        //_texture.Apply();


        sw.Stop();
        UnityEngine.Debug.Log("TextureRefresh:" + sw.ElapsedMilliseconds);
    }
}


