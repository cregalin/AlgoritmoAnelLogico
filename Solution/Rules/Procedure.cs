using Rules.Interfaces;
using Rules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules
{
    public class Procedure : IProcedure
    {
        public long Identifier { get; set; }

        public bool Manager { get; set; }

        private readonly ILog _log;

        public Procedure(ILog log, long id, bool manager)
        {
            _log = log;
            Identifier = id;
            Manager = manager;
        }

        public bool SendRequest()
        {
            return ProcessRequest(this.Identifier);
        }

        private bool ProcessRequest(long identifier)
        {
            IProcedure managerProcedure = Ring.ActiveProcedures.RetrieveManager();
            if (managerProcedure != null)
                return managerProcedure.ReceiveRequest(identifier);

            return false;
        }

        public bool ReceiveRequest(long identifier)
        {
            _log.requisicaoRecebida(identifier);
            try
            {
                // Faz algo com a requisição
                _log.requisicaoTratada(identifier);
                return true;
            }
            catch (Exception ex)
            {
                _log.erroAoTratarRequisicao(identifier, ex);
                return false;
            }
        }

        public void BeginElection()
        {
            BeginElection(this.Identifier);
        }

        public void BeginElection(long identifier)
        {
            _log.eleicaoIniciada(identifier);

            _log.processosAtivos();

            IProcedure newManager = RetriveManagerBiggestIndentifier();
            UpdateManager(newManager);

            _log.eleicaoTerminada(identifier);
        }

        private IProcedure RetriveManagerBiggestIndentifier()
        {
            return Ring.ActiveProcedures.OrderBy(proc => proc.Identifier).Last();
        }

        /// <summary>
        /// Defini o processo como coordenador
        /// </summary>
        public bool UpdateManager(IProcedure newManager)
        {
            try
            {

                SetProceduresAsManager(newManager.Identifier);
                _log.demaisProcessosComoNaoCoordenadores();
            }
            catch (Exception ex)
            {
                _log.erroAoAtualizarOCoorenador(ex);
            }

            return newManager.Manager;
        }

        private void SetProceduresAsManager(long managerIdentifier) =>
            Ring.ActiveProcedures.
            Where(proc =>
            {
                return proc.Manager == true &&
                proc.Identifier == managerIdentifier;
            }).ToList().
                ForEach(proc =>
                {
                    if (proc.Identifier == managerIdentifier)
                    {
                        _log.novoCoordenador(managerIdentifier);
                        proc.Manager = true;
                    }
                    else
                        proc.Manager = false;
                });
    }
}
