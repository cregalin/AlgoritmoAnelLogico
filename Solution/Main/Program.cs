using Rules;
using Rules.Interfaces;
using Rules.Utils;
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
            var log = new Log();
            log.Timer.Start();
            IRing anelLogico = new Ring(log);

            anelLogico.CreateProcedures();
            anelLogico.ExecuteRequest();
            anelLogico.InactivateManager();
            anelLogico.InactivateProcedure();
        }
    }
}
