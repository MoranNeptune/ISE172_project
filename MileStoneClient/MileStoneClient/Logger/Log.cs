using System;
using log4net;
using log4net.Config;

namespace MileStoneClient.Logger
{
    //Singleton class Log
    class Log
    {
        private static Log instance = null;
        private static readonly object padlock = new object();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private constructor for singleton
        private Log()
        {

        }

        public static Log Instance
        {
            get
            {   //only if there is no instance lock object, otherwise return instance
                if (instance == null)
                {
                    lock (padlock) // senario: n threads in here,
                    {              //locking the first and others going to sleep till the first get new Instance
                        if (instance == null)  // rest n-1 threads no need new instance because its not null anymore.
                        {
                            instance = new Log();
                        }
                    }
                }
                return instance;
            }
        }

        public ILog logger { get { return log; } }

        public void info(String str)
        {
            log.Info(str);
        }

        public void error(String str)
        {
            log.Error(str);
        }

        public void warn(String str)
        {
            log.Warn(str);
        }

        public void fatal(String str)
        {
            log.Fatal(str);
        }

        public void debug(String str)
        {
            log.Debug(str);
        }
    }
}
