using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Business
{
    public class CommonHelper
    {
        public static Stopwatch TimerStart()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            return stopwatch;
        }

        public static string TimerEnd(Stopwatch watch)
        {
            watch.Stop();
            return ((double)watch.ElapsedMilliseconds).ToString();
        }
    }
}
