using Thirdparty.Interfaces;

namespace Devops.Deploy.Interfaces.Clients
{
    public interface IReleaseClient : IClient
    {
        public List<IRelease> Releases { get; }
        public IReleaseClient AssignReleases(string json, ITransform Transform);
        public IReleaseClient AssignReleases(List<IRelease> Releases);
        IReleaseClient AssignDeploymentsToRelevantRelease(IDeploymentClient DeploymentClient);
        public IReleaseClient SortReleases();
        public List<IRelease> GetLatest_N(List<IRelease> Releases, int MaximumReleases);
    }
}
