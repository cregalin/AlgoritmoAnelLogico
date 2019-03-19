using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Interfaces
{
    public interface IProcedure
    {
        long Identifier { get; set; }
        bool Manager { get; set; }

        bool SendRequest();
        bool ReceiveRequest(long identifier);
        void BeginElection();
        void BeginElection(long identifier);
        bool UpdateManager(IProcedure newManager);
    }
}
