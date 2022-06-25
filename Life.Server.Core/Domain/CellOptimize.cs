using Life.Server.Core.Contracts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Server.Core.Domain
{
    public struct CellOptimize : ICellState
    {
        public byte State { get; set; }
    }
}
