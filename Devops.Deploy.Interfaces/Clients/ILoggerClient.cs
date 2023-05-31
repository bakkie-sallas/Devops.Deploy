using Thirdparty.Interfaces;

namespace Devops.Deploy.Interfaces.Clients
{
    public interface ILoggerClient
    {
       public ILogger Logger { get; set; }

    }
}
