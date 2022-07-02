using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grpc.Core;
using Protos;
using System.Linq;
using TextureGenerationMethods;

public class GameLoop : MonoBehaviour
{
    [Range(0f, 2f)]
    public float TimeOfTick = 0.1f;
    public int PixelsPerCell = 8;

    private GameRender _gameRender;

    public int CellsPerHorizontal = -1;
    public int CellsPerVertical = -1;
    
    public int PixelsPerHorizontal;
    public int PixelsPerVercital;
    

    [Range(0, 100)]
    public int StartChanceOfLifeForCell = 50;

    private Channel _channel;
    private GameService.GameServiceClient _client;
    private Coroutine _gameLoopCoroutine = null;
    public void Init()//Awake()
    {
        if (CellsPerHorizontal <= 0)
        {
            PixelsPerHorizontal = PixelsPerHorizontal > 0 ? PixelsPerHorizontal : Screen.width - Screen.width % PixelsPerCell;
            CellsPerHorizontal = PixelsPerHorizontal / PixelsPerCell;
        }
        else
        {
            PixelsPerHorizontal = PixelsPerHorizontal > 0 ? PixelsPerHorizontal : CellsPerHorizontal * PixelsPerCell;
        }

        if (CellsPerVertical <= 0)
        {
            PixelsPerVercital = PixelsPerVercital > 0 ? PixelsPerVercital : Screen.height - Screen.height % PixelsPerCell;
            CellsPerVertical = PixelsPerVercital / PixelsPerCell;
        }
        else
        {
            PixelsPerVercital = PixelsPerVercital > 0 ? PixelsPerVercital : CellsPerVertical * PixelsPerCell;
        }
        
        
        _gameRender = GetComponent<GameRender>();
        _gameRender.Init(this);

        Logger.Text = "=========================>>";
        Logger.Text = "GameLoop::Init is completed";

        StartGame();
    }

    public void Init(float timeOfTick, int pixelsPerCell, int cellsPerHorizontal, int cellsPerVertical, int pixelsPerHorizontal, int pixelsPerVercital)
    {
        TimeOfTick = timeOfTick;
        PixelsPerCell = pixelsPerCell;
        CellsPerHorizontal = cellsPerHorizontal;
        CellsPerVertical = cellsPerVertical;
        PixelsPerHorizontal = pixelsPerHorizontal;
        PixelsPerVercital = pixelsPerVercital;

        if (CellsPerHorizontal <= 0)
        {
            PixelsPerHorizontal = PixelsPerHorizontal > 0 ? PixelsPerHorizontal : Screen.width - Screen.width % PixelsPerCell;
            CellsPerHorizontal = PixelsPerHorizontal / PixelsPerCell;
        }
        else
        {
            PixelsPerHorizontal = PixelsPerHorizontal > 0 && PixelsPerHorizontal >= CellsPerHorizontal * PixelsPerCell ? PixelsPerHorizontal : CellsPerHorizontal * PixelsPerCell;
        }

        if (CellsPerVertical <= 0)
        {
            PixelsPerVercital = PixelsPerVercital > 0 ? PixelsPerVercital : Screen.height - Screen.height % PixelsPerCell;
            CellsPerVertical = PixelsPerVercital / PixelsPerCell;
        }
        else
        {
            PixelsPerVercital = PixelsPerVercital > 0 && PixelsPerVercital >= CellsPerVertical * PixelsPerCell ? PixelsPerVercital : CellsPerVertical * PixelsPerCell;
        }


        _gameRender = GetComponent<GameRender>();
        _gameRender.Init(this);

        Logger.Text = "=========================>>";
        Logger.Text = "GameLoop::Init is completed";

        StartGame();
    }

    private void StartGame()
    {
        
        Logger.Text = "GameLoop::StartGame()";
        try
        {
            if (_channel == null || _client == null)
            {   //http://46.72.251.132:7355
                //_channel = Channel.ForAddress("http://46.72.251.132:7355");
                _channel = new Channel("46.72.251.132:7355", ChannelCredentials.Insecure);
                Logger.Text = "GameLoop::StartGame: _channel created";
                _client = new GameService.GameServiceClient(_channel);
                Logger.Text = "GameLoop::StartGame: _client created";
            }

            var res = _client.SettingApplication(new Settings() { Horizontal = CellsPerHorizontal, Vertical = CellsPerVertical, Density = StartChanceOfLifeForCell });
            Logger.Text = "GameLoop::StartGame: _client.SettingApplication completed";

            //TextureRefresh(TextureData.GetTextureData(res.Array.ToByteArray()));
            TextureRefresh(TextureGeneration.GetTextureDataGreen(res.Array.ToArray(), CellsPerHorizontal, CellsPerVertical, PixelsPerCell, 3, _checkLandSpaceOrientation));
        }
        catch (System.Exception ex)
        {
            Logger.Text = "Exception GameLoop::StartGame: " + ex.Message;
            Logger.SetActive(true);
            return;
        }

        StopAndRemoveCoroutine(_gameLoopCoroutine);

        _gameLoopCoroutine = StartCoroutine(TickRate());
    }

    private IEnumerator TickRate()
    {
        if (TimeOfTick == 0f)
            yield return null;
        else
            yield return new WaitForSeconds(TimeOfTick);

        try
        {
            var res = _client.StartGame(new ClearMessage());

            

            TextureRefresh(TextureGeneration.GetTextureDataGreen(res.Array.ToArray(), CellsPerHorizontal, CellsPerVertical, PixelsPerCell, 3, _checkLandSpaceOrientation));
        }
        catch (System.Exception ex)
        {
            Logger.Text = "Exception GameLoop::StartGame: " + ex.Message;
            Logger.SetActive(true);
            StopAndRemoveCoroutine(_gameLoopCoroutine);
            yield break;
        }

        _gameLoopCoroutine = StartCoroutine(TickRate());
    }

    private void StopAndRemoveCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private bool _checkLandSpaceOrientation
    {
        //get => !(Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight);
        get => true;
    }

    private void TextureRefresh(byte[] data)
    {
        _gameRender.SetTextureColorStreamApply(data);
    }
}
