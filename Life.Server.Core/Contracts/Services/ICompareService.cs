using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Server.Core.Contracts.Services
{
    public interface ICompareService
    {
        public List<Task<int>> State { get; set; }

        public bool AddComparer();
        
    }
}
