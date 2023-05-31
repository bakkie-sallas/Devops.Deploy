using Thirdparty.Interfaces;

namespace Devops.Deploy.Interfaces.Clients
{
    public interface IInstanceClient : IClient
    {
        public IDeploymentClient DeploymentClient { get; set; }
        public IProjectClient ProjectClient { get; set; }
        public IReleaseClient ReleaseClient { get; set; }
        public IEnvironmentClient EnvironmentClient { get; set; }
        public ITransformClient TransformClient { get; set; }


        public IInstanceClient LoadJson(string DeploymentJSON, string ProjectJSON, string ReleaseJSON, string EnvironmentJSON);
        public IInstanceClient SetReleaseLimit(int MaximumReleases = -1);

    }
}
