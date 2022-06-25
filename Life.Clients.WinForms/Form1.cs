using Grpc.Net.Client;
using Life.Clients.WinForms.Domain;
using Life.Clients.WinForms.ViewModel;
using Life.Server.Infrastructure.Managers;
using Life.Shared.Protos;

namespace Life.Clients.WinForms
{
    public partial class Form1 : Form
    {
        private int _width;
        private int _height;
        private int _sizeCell;
        private int _density;
        private CellsViewModel _cellsVM;
        private CellStateField _cells;
        private GrpcChannel _channel;
        private GameService.GameServiceClient _client;
        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void ButtonFirstGeneration_Click(object sender, EventArgs e)
        {
            _sizeCell = (int)NUDSizeCell.Value;
            _width = pictureBox1.Width / _sizeCell;
            _height = pictureBox1.Height / _sizeCell;
            _density = (int)NUDDensity.Value;

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
            }
            catch
            {
                StopGame();
            }
        }

        private void StartGame()
        {
            if (TimerTicks.Enabled)
            {
                return;
            }

            TimerTicks.Start();
        }

        private void StopGame()
        {
            TimerTicks.Stop();
        }

        private void TimerTickRate(object sender, EventArgs e)
        {

            try
            {
                var res = _client.StartGame(new ClearMessage());

                _cells.CopyFrom(res.Array.ToArray());

                _cellsVM.RefreshImage(_cells);
            }
            catch
            {
                StopGame();
            }
        }
    }
}