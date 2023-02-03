using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XReceiptService.Common
{
    public class Globals
    {
        public static int isShutDown { get; set; } = 0;
        public static int utilizeMain { get; set; } = 0;
        public static long TimeOutFlow { get; set; } = 28;
        public static string Version { get; set; }
        public delegate void FinishStartAppEventHandler(object sender);
        public static event FinishStartAppEventHandler FinishStartApp;

        public static void RaizeFinishStartApp(object sender)
        {
            var onEvent = FinishStartApp;
            if (!Check(onEvent)) return;
            try
            {
                onEvent(sender);
            }
            catch
            {
            }
        }
        private static bool Check(Delegate onEvent)
        {
            return onEvent != null;
        }
    }
}