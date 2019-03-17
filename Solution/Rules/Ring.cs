using Rules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rules
{
    public class Ring : IRing
    {
        #region Properties
        private readonly static IList<IProcedure> _activeProcedures = new List<IProcedure>();
        public static IList<IProcedure> ActiveProcedures => _activeProcedures;

        private readonly object _internalLock = new Object();
        public object Lock => _internalLock;
        #endregion Properties

        #region Time control
        private readonly long ADD = 30000;
        private readonly long REQUEST = 25000;
        private readonly long INACTIVATE_MANAGER = 100000;
        private readonly long INACTIVATE_PROCEDURE = 80000;
        #endregion Time control
        
        public void CreateProcedures()
        {
            Thread creator = CreateProceduresThread();
            creator.Start();
        }

        private Thread CreateProceduresThread()
        {
            return new Thread(new ThreadStart(CreateProcedure));
        }

        private void CreateProcedure()
        {
            while (true)
            {
                lock(Lock)
                {
                    // Criar novos processos
                }
            }
        }

        public void ExecuteRequest()
        {
            throw new NotImplementedException();
        }

        public void InactivateManager()
        {
            throw new NotImplementedException();
        }

        public void InactivateProcedure()
        {
            throw new NotImplementedException();
        }
    }
}
