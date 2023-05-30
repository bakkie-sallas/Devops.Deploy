using Thirdparty.Interfaces;

namespace Devops.Deploy.Interfaces.Clients
{
    public interface IClient
    {
        public ILogger Logger { get; set; }

    }
}
