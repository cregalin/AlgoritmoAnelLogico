using Rules;
using Rules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IRing anelLogico = new Ring();

            anelLogico.CreateProcedures();
            anelLogico.ExecuteRequest();
            anelLogico.InactivateManager();
            anelLogico.InactivateProcedure();
        }
    }
}
