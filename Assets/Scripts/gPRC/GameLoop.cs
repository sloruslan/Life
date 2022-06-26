using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grpc.Net.Client;
using Life.Shared.Domain.Protos;

public class GameLoop : MonoBehaviour
{
    [Range(0f, 2f)]
    public float TimeOfTick = 0.1f;

    private GameRender _gameRender;

    public int CellsPerHorizontal = -1;
    public int CellsPerVertical = -1;

    public int PixelsPerHorizontal;
    public int PixelsPerVercital;
    public int PixelsPerCell = 8;

    [Range(0, 100)]
    public int StartChanceOfLifeForCell = 50;

    private GrpcChannel _channel;
    private GameService.GameServiceClient _client;



    public void Awake()
    {
        PixelsPerHorizontal = PixelsPerHorizontal > 0 ? PixelsPerHorizontal : Screen.width - Screen.width % PixelsPerCell;
        PixelsPerVercital = PixelsPerVercital > 0 ? PixelsPerVercital : Screen.height - Screen.height % PixelsPerCell;

        CellsPerHorizontal = PixelsPerHorizontal / PixelsPerCell;
        CellsPerVertical = PixelsPerVercital / PixelsPerCell;
        
        _gameRender = GetComponent<GameRender>();
        _gameRender.Init(this);
    }

    private CellsBase _field;

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        


        TextureRefresh();
        StartCoroutine(TickRate());
    }
    /*
    private void ButtonFirstGeneration_Click(object sender, EventArgs e)
    {
        _cellsVM = new CellsViewModel(pictureBox1, _width, _height, _sizeCell);
        _cells = new CellStateField(_width, _height);

        try
        {

            if (_channel == null || _client == null)
            {   //http://46.72.251.132:7355
                _channel = GrpcChannel.ForAddress("http://46.72.251.132:7355");
                _client = new GameService.GameServiceClient(_channel);
            }

            var res = _client.SettingApplication(new Settings() { Horizontal = _width, Vertical = _height, Density = _density });

            _cells.CopyFrom(res.Array.ToArray());

            _cellsVM.RefreshImage(_cells);
            //_cellsVM.RefreshImageInvoke(_cells);
        }
        catch
        {
            StopGame();
        }
    }
    */
    private IEnumerator TickRate()
    {
        if (TimeOfTick == 0f)
            yield return null;
        else
            yield return new WaitForSeconds(TimeOfTick);


        TextureRefresh();
        StartCoroutine(TickRate());

    }

    private void TextureRefresh()
    {
        _gameRender.SetTextureColorStreamApply(_field);
    }
}
