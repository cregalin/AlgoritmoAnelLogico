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
        public object SynchronizedLock => _internalLock;
        #endregion Properties

        #region Time control
        private readonly int ADD = 30000;
        private readonly int REQUEST = 25000;
        private readonly int INACTIVATE_MANAGER = 100000;
        private readonly int INACTIVATE_PROCEDURE = 80000;
        #endregion Time control

        public void CreateProcedures()
        {
            Thread creator = CreateProceduresThread();
            creator.Start();
        }

        private Thread CreateProceduresThread()
        {
            return new Thread(new ThreadStart(CreateProcedureStart));
        }

        private void CreateProcedureStart()
        {
            while (true)
            {
                lock (SynchronizedLock)
                {
                    IProcedure newProcedure = CreateProcedure();
                    Console.WriteLine(string.Format("Processo {0} criado.", newProcedure.Identifier));
                }

                try
                {
                    Thread.Sleep(ADD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Erro ao criar novo processo: {0}", ex.Message));
                }
            }
        }

        private IProcedure CreateProcedure()
        {
            long identifier = GetNewIdentifier(ActiveProcedures);
            bool isManager = ActiveProcedures.Count < 1;

            return new Procedure(identifier, isManager);
        }

        private long GetNewIdentifier(IList<IProcedure> activeProcedures)
        {
            long identifier = new Random().Next(1000, 9999);

            while (activeProcedures.Select(proc => proc.Identifier == identifier).Any())
                identifier += 10;

            return identifier;
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
