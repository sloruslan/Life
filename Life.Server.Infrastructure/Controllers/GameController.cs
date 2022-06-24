using Google.Protobuf;
using Grpc.Core;
using Life.Server.Core.Domain;
using Life.Server.Infrastructure.Managers;
using Life.Shared.Domain;
using Life.Shared.Protos;
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

        public override async Task<SettingsResponse> SettingApplication(Settings request, ServerCallContext context)
        {
            var res = new SettingsResponse() { Message = "Поле создано" };

            //GameLoop<Cell>.Cells = new Cell[request.Cols, request.Rows];

            var t = ClassOfData.Age;

            await Task.Run(() =>
            {
                if (!GameLoop<Cell>.Cells.ContainsKey(context.Peer))
                {
                    GameLoop<Cell>.Cells.Add(context.Peer, GameLoop<Cell>.StartGeneration(request.Horizontal, request.Vertical, request.Density));
                }
                else
                {
                    GameLoop<Cell>.Cells[context.Peer] = GameLoop<Cell>.StartGeneration(request.Horizontal, request.Vertical, request.Density);
                }
                
                //Cells field = new Cells(GameLoop<Cell>.Cells[context.Peer] = GameLoop<Cell>.StartGeneration(request.Horizontal, request.Vertical, request.Density)); 
                Cells field = new Cells(GameLoop<Cell>.Cells[context.Peer]); 

                res.Array = ByteString.CopyFrom(field.State);
            });
                
            return res;
        }

        public override async Task<Frame> StartGame(ClearMessage request, ServerCallContext context)
        {
            var res = new Frame();

            await Task.Run(() =>
            {
                Cells field = new Cells(GameLoop<Cell>.Cells[context.Peer] = GameLoop<Cell>.NextGeneration(GameLoop<Cell>.Cells[context.Peer]));

                res.Array = ByteString.CopyFrom(field.State);
            });

            return res;
        }

    }
}
