using Rules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Rules
{
    public class Ring : IRing
    {
        #region Properties
        private readonly static IList<IProcedure> _activeProcedures = new List<IProcedure>();
        public static IList<IProcedure> ActiveProcedures { get { return _activeProcedures; } }

        private readonly object _internalLock = new Object();
        public object SynchronizedLock { get { return _internalLock; } }
        #endregion Properties

        #region Time control
        private readonly int ADD = 3000;
        private readonly int REQUEST = 2500;
        private readonly int INACTIVATE_MANAGER = 10000;
        private readonly int INACTIVATE_PROCEDURE = 8000;
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

                    ActiveProcedures.Add(newProcedure);

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
            long identifier = GetNewIdentifier(ActiveProcedures, 0);
            bool isManager = ActiveProcedures.Count < 1;

            return new Procedure(identifier, isManager);
        }

        private long GetNewIdentifier(IList<IProcedure> activeProcedures, long ident)
        {
            long identifier = ident == 0 ? new Random().Next(1000, 9999) : ident;

            if (activeProcedures.Select(proc => proc.Identifier).Contains(identifier))
                identifier = GetNewIdentifier(activeProcedures, identifier + 10);

            return identifier;
        }

        public void ExecuteRequest()
        {
            Thread executor = ExecuteRequestThread();
            executor.Start();
        }

        private Thread ExecuteRequestThread()
        {
            return new Thread(new ThreadStart(ExecuteRequestStart));
        }

        private void ExecuteRequestStart()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(REQUEST);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Erro ao executar requisição: {0}", ex.Message));
                }

                lock (SynchronizedLock)
                {
                    if (ActiveProcedures.Any())
                    {
                        IProcedure procedure = GetRandomProcedure(ActiveProcedures);
                        if (procedure != null)
                        {
                            Console.WriteLine(string.Format("Processo {0} fez uma requisição.", procedure.Identifier));
                            bool recieved = procedure.SendRequest();

                            if (!recieved)
                            {
                                Console.WriteLine(string.Format("Não foi obtida nenhuma resposta para a requisição."));
                                procedure.BeginElection();
                            }
                        }
                    }
                }
            }
        }

        private IProcedure GetRandomProcedure(IList<IProcedure> activeProcedures)
        {
            int index = new Random().Next(activeProcedures.Count);

            IProcedure randomProcedure = activeProcedures[index];

            if (!randomProcedure.Manager)
                return randomProcedure;
            else if (activeProcedures.Count > 1)
                return GetRandomProcedure(activeProcedures);
            else
                return null;
        }

        public void InactivateManager()
        {
            Thread inactivator = InactivateManagerThread();
            inactivator.Start();
        }

        private Thread InactivateManagerThread()
        {
            return new Thread(new ThreadStart(InactivateManagerStart));
        }

        private void InactivateManagerStart()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(INACTIVATE_MANAGER);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Erro ao inativar coordenador: {0}", ex.Message));
                }

                lock (SynchronizedLock)
                {
                    if (ActiveProcedures.Any())
                    {
                        IProcedure managerProcedure = RetrieveManager(ActiveProcedures);
                        if (managerProcedure != null)
                            InactivateProcedure(managerProcedure);
                    }
                }
            }
        }

        public static IProcedure RetrieveManager()
        {
            return RetrieveManager(ActiveProcedures);
        }

        public static IProcedure RetrieveManager(IList<IProcedure> activeProcedures)
        {
            try
            {
                return activeProcedures.First(proc => proc.Manager == true);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void InactivateProcedure()
        {
            Thread inactivator = InactivateProcedureThread();
            inactivator.Start();
        }

        private Thread InactivateProcedureThread()
        {
            return new Thread(new ThreadStart(InactivateProcedureStart));
        }

        private void InactivateProcedureStart()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(INACTIVATE_PROCEDURE);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Erro ao inativar processo: {0}", ex.Message));
                }

                lock (SynchronizedLock)
                {
                    if (ActiveProcedures.Any())
                    {
                        IProcedure randomProcedure = GetRandomProcedure(ActiveProcedures);
                        if (randomProcedure != null)
                            InactivateProcedure(randomProcedure);
                    }
                }
            }
        }

        private void InactivateProcedure(IProcedure procedure)
        {
            ActiveProcedures.Remove(procedure);
            Console.WriteLine(string.Format("Processo {0} inativado.", procedure.Identifier));
        }
    }
}
