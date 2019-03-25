using Rules.Enums;
using Rules.Interfaces;
using Rules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Rules
{
    public class Ring : IRing
    {
        private readonly ILog _log;
        public Ring(ILog log)
        {
            _log = log;
        }

        #region Properties
        private readonly static IList<IProcedure> _activeProcedures = new List<IProcedure>();
        public static IList<IProcedure> ActiveProcedures { get { return _activeProcedures; } }

        private readonly object _internalLock = new Object();
        public object SynchronizedLock { get { return _internalLock; } }
        #endregion Properties

        #region Time control
        private const int ADD = 30000;
        private const int REQUEST = 25000;
        private const int INACTIVATE_MANAGER = 100000;
        private const int INACTIVATE_PROCEDURE = 80000;
        #endregion Time control

        /// <summary>
        /// Cria um novo processo e adiciona na lista de processos com um intervalo de tempo de 30 segundos.
        /// </summary>
        public void CreateProcedures() =>
            new Thread(new ThreadStart(CreateProcedureStart)).Start();

        private void CreateProcedureStart() =>
            startProcess(ADD, ProcessType.CreateProcedure, () =>
            {
                    IProcedure newProcedure = CreateProcedure();

                    ActiveProcedures.Add(newProcedure);

                    _log.processoCriado(newProcedure.Identifier);
            }, false);

        /// <summary>
        /// Envia uma requisição a um processo aleatório em um intervalo de tempo de 25 segundos,
        /// se não for recebida inicia uma nova eleição.
        /// </summary>
        public void ExecuteRequest() =>
            new Thread(new ThreadStart(ExecuteRequestStart)).Start();

        private void ExecuteRequestStart() =>
            startProcess(REQUEST, ProcessType.ExecuteRequest, () =>
            {
                if (ActiveProcedures.Any())
                {
                    IProcedure procedure = ActiveProcedures.GetRandomProcedure();
                    if (procedure != null)
                    {
                        _log.processoFezUmaRequisicao(procedure.Identifier);
                        bool recieved = procedure.SendRequest();

                        if (!recieved)
                        {
                            _log.naoFoiObtidaNenhumaRespostaParaARequisicao();
                            procedure.BeginElection();
                        }
                    }
                }
            });

        /// <summary>
        /// Inativa o coordenador existente em um intervalo de 100 segundos
        /// </summary>
        public void InactivateManager() =>
            new Thread(new ThreadStart(InactivateManagerStart)).Start();
        private void InactivateManagerStart() =>
            startProcess(INACTIVATE_MANAGER, ProcessType.InactivateManager, () =>
            {
                if (ActiveProcedures.Any())
                {
                    IProcedure managerProcedure = ActiveProcedures.RetrieveManager();
                    if (managerProcedure != null)
                        InactivateProcedure(managerProcedure);
                }
            });

        /// <summary>
        /// Inativa um processo aleatório existente em um intervalo de 80 segundos
        /// </summary>
        public void InactivateProcedure() =>
            new Thread(new ThreadStart(InactivateProcedureStart)).Start();

        private void InactivateProcedureStart() =>
            startProcess(INACTIVATE_PROCEDURE, ProcessType.InactivateProcedure, () =>
            {
                if (ActiveProcedures.Any())
                {
                    IProcedure randomProcedure = ActiveProcedures.GetRandomProcedure();
                    if (randomProcedure != null)
                        InactivateProcedure(randomProcedure);
                }
            });

        /// <summary>
        /// Inativa o processo especificado da lista de processo
        /// </summary>
        private void InactivateProcedure(IProcedure procedure)
        {
            ActiveProcedures.Remove(procedure);
            _log.processoInativado(procedure.Identifier);
        }

        /// <summary>
        /// Inicia um processo com um intervalo de tempo especificado e sincronizado com o objeto SynchronizedLock
        /// </summary>
        private void startProcess(int ms, ProcessType processType, Action del, bool sleepFirst = true)
        {
            while (true)
            {
                if (sleepFirst)
                    try
                    {
                        Thread.Sleep(ms);
                    }
                    catch (Exception ex)
                    {
                        _log.error(ex, processType);
                    }

                lock (SynchronizedLock)
                {
                    del.Invoke();
                }
                if (!sleepFirst)
                    try
                    {
                        Thread.Sleep(ms);
                    }
                    catch (Exception ex)
                    {
                        _log.error(ex, processType);
                    }
            }
        }

        private IProcedure CreateProcedure()
        {
            long identifier = ActiveProcedures.GetNewIdentifier(0);
            bool isManager = ActiveProcedures.Count < 1;

            return new Procedure(_log, identifier, isManager);
        }
    }
}
