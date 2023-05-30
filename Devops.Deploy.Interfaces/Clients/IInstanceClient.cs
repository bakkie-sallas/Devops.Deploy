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

        public IInstanceClient LoadJson(string GeneralJSON);

        public IInstanceClient LoadJson(string DeploymentJSON = null, string ProjectJSON = null, string ReleaseJSON = null, string EnvironmentJSON = null);
        public IInstanceClient WithReleaseLimiter(int MaximumReleases = -1);

    }
}
