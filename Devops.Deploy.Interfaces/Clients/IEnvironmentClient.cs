using Thirdparty.Interfaces;

namespace Devops.Deploy.Interfaces.Clients
{
    public interface IEnvironmentClient : IClient
    {
        public List<IEnvironment> Environments { get; }
        
        public IEnvironmentClient AssignEnvironments(string json, ITransform Transform);
        public void AssignDeploymentsAndReleasesToRelevantEnvironment(List<IDeployment> Deployments, List<IRelease> Releases);
    }
}
