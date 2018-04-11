using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.PresentationLayer;
using MileStoneClient.Logger;

namespace MileStoneClient
{
    /// <summary>
    /// This Chat Room belt in order to supplay a way that peoples can communicate with each other.
    /// The project has 4 layers : presentation layer, business layer, presistent layer and communication layer.
    /// Each layer has is oun namespace.
    /// <version> 1.0
    /// </summary>
    class Program
    {
        /// <summary>
        /// This is the main method Of the project.
        /// </summary>
        /// <param name="args"></param>
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
             */

            Log.Instance.debug("Debugged successfully");//log
            Log.Instance.info("Chat Room initiated successfully");//log
            Presentation p = new Presentation(); // creating new presentation 
            p.Main(args);
            Log.Instance.info("Chat Room closed - logged out and exit system");//log

        }
    }
}
