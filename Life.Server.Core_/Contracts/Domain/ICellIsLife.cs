﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Server.Core.Contracts.Domain
{
    public interface ICellIsLife
    {
        public bool IsLife { get; set; }
    }
}
