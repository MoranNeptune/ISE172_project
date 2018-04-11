using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.PresentationLayer;
using MileStoneClient.Logger;

namespace MileStoneClient
{
    class Program
    {
        static void Main(string[] args)
        {
            /*there are no rules for what each log represents, so for now:
             *
             * debug - (least fatal error type)
             *         at the start of the prog once the project was debugged and starting
             * 
             * info - 
             *         system info (for now: how the system works, end of prog, maybe add logins)
             *         
             * warn - 
             *         ??
             *         
             * error - 
             *         ??
             *         
             * fatal - (most fatal error type)
             *         system crush
             *         
             */
            try
            {
                Log.Instance.debug("Debugged successfully");//log
                Log.Instance.info("Chat Room initiated successfully");//log
                Presentation p = new Presentation();
                p.Main(args);
                Log.Instance.info("Chat Room closed - logged out and exit system");//log
            }
            catch (Exception e)
            {
                if (e.Source != null)
                    Log.Instance.fatal("Fatal error - System crush");
            }


        }
    }
}
