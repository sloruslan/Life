using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grpc.Core;
using Protos;
using System.Linq;
using TextureGenerationMethods;
using Unity.Collections;

public class GameLoop : MonoBehaviour
{

    public bool IsRendererTexture;

    [Range(0, 32)]
    public int OffsetColor = 15;

    [Range(0f, 2f)]
    public float TimeOfTick = 0.1f;
    public int PixelsPerCell = 8;

    private GameRender _gameRender;
    private TextureGenerationJob _textureGenerationJob;
    private TextureDataUIntComputeShaderControl _shaderSpriteControl;
    private TextureDataTexture2DComputeShaderControl _shaderMaterialControl;

    public int CellsPerHorizontal = -1;
    public int CellsPerVertical = -1;
    
    public int PixelsPerHorizontal;
    public int PixelsPerVercital;
    

    [Range(0, 100)]
    public int StartChanceOfLifeForCell = 50;

    private Channel _channel;
    private GameService.GameServiceClient _client;
    private Coroutine _gameLoopCoroutine = null;

    public ETypeOfTextureGeneration TypeOfTextureGeneration;

    public enum ETypeOfTextureGeneration
    {
        Mono,
        Job,
        Shader
    }

    public void Init()//Awake()
    {
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

        if (_gameRender == null)
        {
            _gameRender = GetComponent<GameRender>();
            _gameRender.Init(this, IsRendererTexture);
        }

        if (_textureGenerationJob == null || _textureGenerationJob._isExist)
        {
            _textureGenerationJob = GetComponent<TextureGenerationJob>();
            _textureGenerationJob.WaitTextureGenerationJobCompletedEvent += OnWaitTextureGenerationJobCompleted;
        }

        if (!IsRendererTexture && _shaderSpriteControl == null)
        {
            _shaderSpriteControl = GetComponent<TextureDataUIntComputeShaderControl>();
        }

        if (IsRendererTexture && _shaderMaterialControl == null)
        {
            _shaderMaterialControl = GetComponent<TextureDataTexture2DComputeShaderControl>();
        }

        Logger.Text = $"TimeOfTick: {TimeOfTick}";
        Logger.Text = $"PixelsPerCell: {PixelsPerCell}";
        Logger.Text = $"CellsPerHorizontal: {CellsPerHorizontal}";
        Logger.Text = $"CellsPerVertical: {CellsPerVertical}";
        Logger.Text = $"PixelsPerHorizontal: {PixelsPerHorizontal}";
        Logger.Text = $"PixelsPerVercital: {PixelsPerVercital}";


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

        Init();
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

                var options = new List<ChannelOption>()
                {
                    new ChannelOption(ChannelOptions.MaxReceiveMessageLength, 8192 * 8192),
                    new ChannelOption(ChannelOptions.MaxSendMessageLength, 8192 * 8192)
                };

                _channel = new Channel("46.72.251.132:7355", ChannelCredentials.Insecure, options);
                Logger.Text = "GameLoop::StartGame: _channel created";
                _client = new GameService.GameServiceClient(_channel);
                Logger.Text = "GameLoop::StartGame: _client created";
            }


            DeviceOrientationManager.SetStartDeviceOrientationSimple();
            var res = _client.SettingApplication(new Settings() { Horizontal = CellsPerHorizontal, Vertical = CellsPerVertical, Density = StartChanceOfLifeForCell });
            Logger.Text = "GameLoop::StartGame: _client.SettingApplication completed";

            TextureRefresh(res.Array);
        }
        catch (System.Exception ex)
        {
            ExeptionLog(ex);
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
    }

    public bool showTickRateTime;
    public bool showServerRequestTime;
    public bool showTextureGenTime;

    private System.Diagnostics.Stopwatch tickRateTime = new System.Diagnostics.Stopwatch();
    private System.Diagnostics.Stopwatch serverRequestTime = new System.Diagnostics.Stopwatch();
    private System.Diagnostics.Stopwatch textureGenTime = new System.Diagnostics.Stopwatch();

    private IEnumerator TickRate()
    {
        if (tickRateTime.IsRunning)
        {
            tickRateTime.Stop();
            if (showTickRateTime)
                Debug.Log($"tickRateTime: {tickRateTime.ElapsedMilliseconds}");
        }


        if (TimeOfTick == 0f)
            yield return null;
        else
            yield return new WaitForSeconds(TimeOfTick);

        tickRateTime.Restart();

        try
        {
            serverRequestTime.Restart();
            var res = _client.StartGame(new ClearMessage());

            serverRequestTime.Stop();
            if (showServerRequestTime)
                Debug.Log($"serverRequestTime: {serverRequestTime.ElapsedMilliseconds}");

            TextureRefresh(res.Array);

            switch (TypeOfTextureGeneration)
            {
                case ETypeOfTextureGeneration.Mono:
                case ETypeOfTextureGeneration.Shader:
                    {
                        _gameLoopCoroutine = StartCoroutine(TickRate());
                    }
                    break;

                default:
                    break;
            }
        }
        catch (System.Exception ex)
        {
            ExeptionLog(ex);
            Logger.SetActive(true);
            StopAndRemoveCoroutine();
            yield break;
        }
    }


   

    private void StopAndRemoveCoroutine()
    {
        if (_gameLoopCoroutine != null)
        {
            StopCoroutine(_gameLoopCoroutine);
            _gameLoopCoroutine = null;
            _textureGenerationJob.Dispose();
        }
    }

    private void TextureRefresh(Google.Protobuf.ByteString srcData)
    {
        switch (TypeOfTextureGeneration)
        {

            case ETypeOfTextureGeneration.Mono:
                {
                    textureGenTime.Restart();
                    SetTextureColorStreamApply(TextureGeneration.GetTextureDataParallel(srcData.ToArray(), CellsPerHorizontal, CellsPerVertical, PixelsPerCell, 3, OffsetColor));
                    //SetTextureColorStreamApply(TextureGeneration.GetTextureDataParallelIter(srcData.ToArray()));
                    textureGenTime.Stop();
                    if (showTextureGenTime)
                        Debug.Log($"textureGenTime Mono: {textureGenTime.ElapsedMilliseconds}");
                }
                break;

            case ETypeOfTextureGeneration.Job:
                {
                    textureGenTime.Restart();
                    _textureGenerationJob.RunTextureGenerationJob(srcData.ToArray(), CellsPerHorizontal, CellsPerVertical, PixelsPerCell, 3, OffsetColor);
                }
                break;

            case ETypeOfTextureGeneration.Shader:
                {
                    textureGenTime.Restart();

                    if (IsRendererTexture)
                    {
                        _shaderMaterialControl.CreateShader(_gameRender.RenderTexture, srcData.ToArray(), CellsPerHorizontal, PixelsPerCell);
                        if (showTextureGenTime)
                            Debug.Log($"textureGenTime Shader CreateShader: {textureGenTime.ElapsedMilliseconds}");
                    }
                    else
                    {
                        var dst = _shaderSpriteControl.TextureDataGeneration(srcData.ToArray(), CellsPerHorizontal, PixelsPerCell);
                        if (showTextureGenTime)
                            Debug.Log($"textureGenTime Shader TextureDataGeneration: {textureGenTime.ElapsedMilliseconds}");
                        SetTextureColorStreamApply(dst);
                        if (showTextureGenTime)
                            Debug.Log($"textureGenTime Shader SetTextureColorStreamApply: {textureGenTime.ElapsedMilliseconds}");
                    }
                    textureGenTime.Stop();
                    
                }
                break;
            default:
                break;

        }
    }


    private void SetTextureColorStreamApply(byte[] textureData)
    {
        _gameRender.SetTextureColorStreamApply(textureData);
    }

    private void SetTextureColorStreamApply(int[] textureData)
    {
        _gameRender.SetTextureColorStreamApply(textureData);
    }

    private void SetTextureColorStreamApply(uint[] textureData)
    {
        _gameRender.SetTextureColorStreamApply(textureData);
    }

    private void SetTextureColorStreamApply(NativeArray<byte> textureData)
    //private void SetTextureColorStreamApply(NativeArray<int> textureData)
    {
        _gameRender.SetTextureColorStreamApply(textureData);
    }

    private void OnWaitTextureGenerationJobCompleted(NativeArray<byte> textureData)
    //private void OnWaitTextureGenerationJobCompleted(NativeArray<int> textureData)
    {
        textureGenTime.Stop();
        if (showTextureGenTime)
            Debug.Log($"textureGenTime Job: {textureGenTime.ElapsedMilliseconds}");

        SetTextureColorStreamApply(textureData);

        if (_gameLoopCoroutine != null)
            StartCoroutine(TickRate());
    }

    private void ExeptionLog(System.Exception ex)
    {
        Logger.Text = "Exception GameLoop::TickRate:Message " + ex.Message;
        Logger.Text = "Exception GameLoop::TickRate:Source " + ex.Source;
        Logger.Text = "Exception GameLoop::TickRate:StackTrace " + ex.StackTrace;
        Logger.Text = "Exception GameLoop::TickRate:HelpLink " + ex.HelpLink;
        Logger.Text = "StartDeviceOrientationSimple " + DeviceOrientationManager.StartDeviceOrientationSimple.ToString();
        Logger.Text = "GetDeviceOrientationSimple " + DeviceOrientationManager.GetDeviceOrientationSimple.ToString();
        Logger.Text = "CurrentDeviceOrientationSimple " + DeviceOrientationManager.CurrentDeviceOrientationSimple.ToString();
    }
}
