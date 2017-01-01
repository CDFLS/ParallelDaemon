using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ParallelDaemon.Utils
{
    public class ThreadUtils
    {
        public static void StartOperation(Action action)
        {
            Thread newThread = new Thread(() => { action(); });
            newThread.Start();
        }
    }
}
