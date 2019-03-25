using Rules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// Retorna um processo aleatório
        /// </summary>
        public static IProcedure GetRandomProcedure(this IList<IProcedure> activeProcedures)
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
        /// <summary>
        /// Retorna o processo coordenador
        /// </summary>
        public static IProcedure RetrieveManager(this IList<IProcedure> activeProcedures)
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

        /// <summary>
        /// Retorna um novo identificador único
        /// </summary>
        public static long GetNewIdentifier(this IList<IProcedure> activeProcedures, long ident)
        {
            long identifier = ident == 0 ? new Random().Next(1000, 9999) : ident;

            if (activeProcedures.Select(proc => proc.Identifier).Contains(identifier))
                identifier = GetNewIdentifier(activeProcedures, identifier + 10);

            return identifier;
        }
    }
}
