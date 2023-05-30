using Thirdparty.Interfaces;

namespace Devops.Deploy.Interfaces.Clients
{
    public interface IEnvironmentClient : IClient
    {
        public List<IEnvironment> Environments { get; }
        
        public IEnvironmentClient AssignEnvironments(string json, ITransform Transform);
        public void AssignDeploymentsToRelevantEnvironment(IDeploymentClient DeploymentClient, IReleaseClient ReleaseClient, int MaximumReleases = -1);

    }
}
