using Devops.Deploy.Interfaces.Clients;
using Thirdparty.Interfaces;
namespace Devops.Deploy.Clients
{
    /*The client will execute the methods for the injected object. 
     * The client just forms a little in between to not tightly couple 
     * millions of little methods strung through the calling libs 
     * to the object directly*/

    public class LoggerClient:ILoggerClient
    {
        public ILogger Logger { get; set; }
        public LoggerClient(ILogger Logger)
        {
            this.Logger = Logger;
        }
    }
}
