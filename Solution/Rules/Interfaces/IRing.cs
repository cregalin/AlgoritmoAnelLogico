﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Interfaces
{
    public interface IRing
    {
        Object SynchronizedLock { get; }

        void CreateProcedures();
        void ExecuteRequest();
        void InactivateManager();
        void InactivateProcedure();
    }
}
