using Google.Protobuf;
using Grpc.Core;
using Life.Server.Core.Domain;
using Life.Server.Infrastructure.Managers;
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
        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
        }
        //http://localhost:5231;https://localhost:7232;
        public override async Task<SettingsResponse> SettingApplication(Settings request, ServerCallContext context)
        {
            var res = new SettingsResponse() { Message = "Поле создано" };

            //GameLoop<Cell>.Cells = new Cell[request.Cols, request.Rows];

            await Task.Run(() =>
            {
                if (!GameLoop.Cells.ContainsKey(context.Peer))
                {
                    GameLoop.Cells.Add(context.Peer, GameLoop.StartGeneration(request.Horizontal, request.Vertical, request.Density));
                }
                else
                {
                    GameLoop.Cells[context.Peer] = GameLoop.StartGeneration(request.Horizontal, request.Vertical, request.Density);
                }

                res.Array = ByteString.CopyFrom(GameLoop.GetByteArray(GameLoop.Cells[context.Peer]));
            });
                
            return res;
        }

        public override async Task<Frame> StartGame(ClearMessage request, ServerCallContext context)
        {
            var res = new Frame();

            await Task.Run(() =>
            {
                GameLoop.Cells[context.Peer] = GameLoop.NextGeneration(GameLoop.Cells[context.Peer]);

                res.Array = ByteString.CopyFrom(GameLoop.GetByteArray(GameLoop.Cells[context.Peer]));
            });

            return res;
        }

    }
}
