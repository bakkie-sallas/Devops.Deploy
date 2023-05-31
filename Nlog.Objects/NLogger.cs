using NLog;

namespace Nlog.Objects
{
    public class NLogger:Thirdparty.Interfaces.ILogger
    {

        private static Logger Logger = LogManager.GetCurrentClassLogger();
        public Guid InstanceId { get; set; }
        
        // public string FileName => Logger.
        public NLogger():this("log.config")
        {
            //if no nlog.config was provided,this will break because there is no log.config. I just added this here to demonstrate the concept
           
        }


        public NLogger(string logConfig)
        {
            LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(logConfig);
            InstanceId = Guid.NewGuid();
            
        }

        public void Info(string message,object obj = null)
        {
            Logger.Info($"{InstanceId}  :: {message}");
        }
        public void Error(string message, object obj = null)
        {
            Logger.Info($"{InstanceId}  :: {message}");
        }

        public string CompleteLog()
        {
            string LogFileName  = ((NLog.Targets.FileTarget)Logger.Factory.Configuration.AllTargets[0]).FileName.ToString();
            Logger.Factory.Shutdown();
            return LogFileName;
        }
    }
}