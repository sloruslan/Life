using Google.Protobuf;
using Grpc.Core;
using Life.Server.Core.Contracts.Services;
using Life.Server.Core.Domain;
using Life.Shared.Domain.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Life.Server.Infrastructure.Controllers
{
    public class GameController : GameService.GameServiceBase
    {
        private readonly ILogger<GameController> _logger;
        private IGameLoop<CellOptimize> _gameLoop;
        public GameController(ILogger<GameController> logger, IGameLoop<CellOptimize> gameLoop)
        {
            _logger = logger;
            _gameLoop = gameLoop;
        }
        //http://localhost:5231;https://localhost:7232;
        public override async Task<SettingsResponse> SettingApplication(Settings request, ServerCallContext context)
        {
            var res = new SettingsResponse() { Message = "Поле создано" };

            //GameLoop<Cell>.Cells = new Cell[request.Cols, request.Rows];

            await Task.Run(() =>
            {
                if (!_gameLoop.Cells.ContainsKey(context.Peer))
                {
                    _gameLoop.Cells.Add(context.Peer, _gameLoop.StartGeneration(request.Horizontal, request.Vertical, request.Density));
                }
                else
                {
                    _gameLoop.Cells[context.Peer] = _gameLoop.StartGeneration(request.Horizontal, request.Vertical, request.Density);
                }

                res.Array = ByteString.CopyFrom(_gameLoop.GetByteArray(_gameLoop.Cells[context.Peer]));
            });
                
            return res;
        }

        public override async Task<Frame> StartGame(ClearMessage request, ServerCallContext context)
        {
            var res = new Frame();

            await Task.Run(() =>
            {
                _gameLoop.Cells[context.Peer] = _gameLoop.NextGeneration(_gameLoop.Cells[context.Peer]);

                res.Array = ByteString.CopyFrom(_gameLoop.GetByteArray(_gameLoop.Cells[context.Peer]));
            });

            return res;
        }

    }
}
