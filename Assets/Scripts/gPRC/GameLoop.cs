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

        FirstGeneration();
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

        FirstGeneration();
    }

    private void FirstGeneration()
    {
        //StopAndRemoveCoroutine(ref _gameLoopCoroutine);
        StopAndRemoveCoroutine();

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


            StartDeviceOrientationSimple = GetDeviceOrientationSimple;
            var res = _client.SettingApplication(new Settings() { Horizontal = CellsPerHorizontal, Vertical = CellsPerVertical, Density = StartChanceOfLifeForCell });
            Logger.Text = "GameLoop::StartGame: _client.SettingApplication completed";


            TextureRefresh(TextureGeneration.GetTextureDataGreenParallelFor(res.Array.ToArray(), CellsPerHorizontal, CellsPerVertical, PixelsPerCell, 3));
        }
        catch (System.Exception ex)
        {
            Logger.Text = "Exception StartGame::TickRate:Message " + ex.Message;
            Logger.Text = "Exception StartGame::TickRate:Source " + ex.Source;
            Logger.Text = "Exception StartGame::TickRate:StackTrace " + ex.StackTrace;
            Logger.Text = "Exception StartGame::TickRate:HelpLink " + ex.HelpLink;
            Logger.Text = "StartDeviceOrientationSimple " + StartDeviceOrientationSimple.ToString();
            Logger.Text = "GetDeviceOrientationSimple " + GetDeviceOrientationSimple.ToString();
            Logger.Text = "CurrentDeviceOrientationSimple " + CurrentDeviceOrientationSimple.ToString();
            Logger.SetActive(true);
            return;
        }
    }

    public void GameLoopControl()
    {
        if (_gameLoopCoroutine == null)
            _gameLoopCoroutine = StartCoroutine(TickRate());
        else
            StopAndRemoveCoroutine();
            //StopAndRemoveCoroutine(ref _gameLoopCoroutine);
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

            TextureRefresh(TextureGeneration.GetTextureDataGreenParallelFor(res.Array.ToArray(), CellsPerHorizontal, CellsPerVertical, PixelsPerCell, 3));
        }
        catch (System.Exception ex)
        {
            Logger.Text = "Exception GameLoop::TickRate:Message " + ex.Message;
            Logger.Text = "Exception GameLoop::TickRate:Source " + ex.Source;
            Logger.Text = "Exception GameLoop::TickRate:StackTrace " + ex.StackTrace;
            Logger.Text = "Exception GameLoop::TickRate:HelpLink " + ex.HelpLink;
            Logger.Text = "StartDeviceOrientationSimple " + StartDeviceOrientationSimple.ToString();
            Logger.Text = "GetDeviceOrientationSimple " + GetDeviceOrientationSimple.ToString();
            Logger.Text = "CurrentDeviceOrientationSimple " + CurrentDeviceOrientationSimple.ToString();
            Logger.SetActive(true);
            StopAndRemoveCoroutine();
            //StopAndRemoveCoroutine(ref _gameLoopCoroutine);
            yield break;
        }

        _gameLoopCoroutine = StartCoroutine(TickRate());
    }

    private void StopAndRemoveCoroutine(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private void StopAndRemoveCoroutine()
    {
        if (_gameLoopCoroutine != null)
        {
            StopCoroutine(_gameLoopCoroutine);
            _gameLoopCoroutine = null;
        }
    }

    public bool IsWasRotationDevice
    {
        get => StartDeviceOrientationSimple == GetDeviceOrientationSimple;
    }
    public DeviceOrientationSimple GetDeviceOrientationSimple
    {
        get
        {
            if (Input.deviceOrientation == DeviceOrientation.LandscapeRight || Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
                return CurrentDeviceOrientationSimple = DeviceOrientationSimple.Landscape;
            else if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
                return CurrentDeviceOrientationSimple = DeviceOrientationSimple.Portrait;
            else
                return CurrentDeviceOrientationSimple;
        }
    }

    private DeviceOrientationSimple _currentDeviceOrientationSimple = DeviceOrientationSimple.Portrait;
    public DeviceOrientationSimple CurrentDeviceOrientationSimple
    {
        get => _currentDeviceOrientationSimple;
        set
        {
            if (_currentDeviceOrientationSimple == value) return;

            _currentDeviceOrientationSimple = value;
        }
    }

    private DeviceOrientationSimple _startDeviceOrientationSimple;
    public DeviceOrientationSimple StartDeviceOrientationSimple
    {
        get => _startDeviceOrientationSimple;
        set
        {
            if (_startDeviceOrientationSimple == value) return;

            _startDeviceOrientationSimple = value;
        }
    }

    public enum DeviceOrientationSimple
    {
        Portrait,
        Landscape
    }


    private void TextureRefresh(byte[] data)
    {
        _gameRender.SetTextureColorStreamApply(data);
    }
}
