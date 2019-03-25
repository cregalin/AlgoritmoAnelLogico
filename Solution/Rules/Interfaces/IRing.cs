using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Interfaces
{
    public interface IRing
    {
        Object SynchronizedLock { get; }

        /// <summary>
        /// Cria um novo processo e adiciona na lista de processos com um intervalo de tempo de 30 segundos.
        /// </summary>
        void CreateProcedures();

        /// <summary>
        /// Envia uma requisição a um processo aleatório em um intervalo de tempo de 25 segundos,
        /// se não for recebida inicia uma nova eleição.
        /// </summary>
        void ExecuteRequest();

        /// <summary>
        /// Inativa o coordenador existente em um intervalo de 100 segundos
        /// </summary>
        void InactivateManager();

        /// <summary>
        /// Inativa um processo aleatório existente em um intervalo de 80 segundos
        /// </summary>
        void InactivateProcedure();
    }
}
