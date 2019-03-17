using Rules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules
{
    public class Ring : IRing
    {
        private readonly static IList<IProcedure> _activeProcedures = new List<IProcedure>();

        public static IList<IProcedure> ActiveProcedures => _activeProcedures;
    }
}
