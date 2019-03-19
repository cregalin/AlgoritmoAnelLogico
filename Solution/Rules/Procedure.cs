using Rules.Interfaces;
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

        public readonly string SEPARATOR = new string('-', 70);

        public Procedure(long id) : this(id, false)
        {
        }

        public Procedure(long id, bool manager)
        {
            Identifier = id;
            Manager = manager;
        }

        public bool SendRequest()
        {
            return ProcessRequest(this.Identifier);
        }

        private bool ProcessRequest(long identifier)
        {
            IProcedure managerProcedure = Ring.RetrieveManager();
            if (managerProcedure != null)
                return managerProcedure.ReceiveRequest(identifier);

            return false;
        }

        public bool ReceiveRequest(long identifier)
        {
            Console.WriteLine(string.Format("Requeisição do processo {0} recebida pelo coordenador ({1}).", identifier, Ring.RetrieveManager().Identifier));
            try
            {
                // Faz algo com a requisição 
                Console.WriteLine(string.Format("Requeisição do processo {0} tratada.", identifier));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Erro ao tratar requisição do processo {0}: {1}", identifier, ex.Message));
                return false;
            }
        }

        public void BeginElection()
        {
            BeginElection(this.Identifier);
        }

        public void BeginElection(long identifier)
        {
            Console.WriteLine(SEPARATOR);
            Console.WriteLine(string.Format("Eleição iniciada pelo processo {0}", identifier));
            Console.WriteLine(SEPARATOR);

            Console.WriteLine(string.Format("{0} processos ativos", Ring.ActiveProcedures.Count));

            IProcedure newManager = RetriveManagerBiggestIndentifier();
            UpdateManager(newManager);

            Console.WriteLine(SEPARATOR);
            Console.WriteLine(string.Format("Eleição terminada, o processo {0} é o novo coordenador.", newManager.Identifier));
            Console.WriteLine(SEPARATOR);
        }

        private IProcedure RetriveManagerBiggestIndentifier()
        {
            return Ring.ActiveProcedures.OrderBy(proc => proc.Identifier).Last();
        }

        public bool UpdateManager(IProcedure newManager)
        {
            try
            {
                newManager.Manager = true;
                Console.WriteLine(string.Format("Processo {0} definido como coordenador.", newManager.Identifier));

                SetProceduresAsManager(newManager.Identifier, false);
                Console.WriteLine("Demais processos marcados como não-coordenadores.");
            }
            catch (Exception ex)
            {
                newManager.Manager = false;
                Console.WriteLine(string.Format("Erro ao atualizar coordenador: {0}", ex.Message));
            }

            return newManager.Manager;
        }

        private void SetProceduresAsManager(long managerIdentifier, bool isManager)
        {
            IList<IProcedure> procedures = Ring.ActiveProcedures.Where(proc => proc.Identifier != managerIdentifier && proc.Manager != isManager).ToList();

            foreach (IProcedure procedure in procedures)
                procedure.Manager = isManager;
        }
    }
}
