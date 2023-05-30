using Thirdparty.Interfaces;

namespace Devops.Deploy.Interfaces.Clients
{
    public interface IInstanceClient : IClient
    {
        public IDeploymentClient DeploymentClient { get; set; }
        public IProjectClient ProjectClient { get; set; }
        public IReleaseClient ReleaseClient { get; set; }
        public IEnvironmentClient EnvironmentClient { get; set; }

        public string DeploymentJSON { get; set; }
        public string ProjectJSON { get; set; }
        public string ReleaseJSON { get; set; }
        public string EnvironmentJSON { get; set; }

    }
}
