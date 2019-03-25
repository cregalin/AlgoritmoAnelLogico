using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Enums
{
    public class ProcessType
    {
        private ProcessType(string value) { Value = value; }

        public string Value { get; set; }

        public static ProcessType InactivateProcedure { get { return new ProcessType("inativar o processo"); } }
        public static ProcessType InactivateManager { get { return new ProcessType("inativar o coordenador"); } }
        public static ProcessType ExecuteRequest { get { return new ProcessType("executar uma requisição"); } }
        public static ProcessType CreateProcedure { get { return new ProcessType("criar um novo processo"); } }
    }
}
