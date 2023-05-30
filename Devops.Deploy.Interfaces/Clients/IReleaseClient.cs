using Thirdparty.Interfaces;

namespace Devops.Deploy.Interfaces.Clients
{
    public interface IReleaseClient : IClient
    {
        public List<IRelease> Releases { get; }
        public IReleaseClient AssignReleases(string json, ITransform Transform);
        IReleaseClient AssignDeploymentsToRelevantRelease(List<IDeployment> Deployments);
        public IReleaseClient SortReleases();
        public IReleaseClient GetLatest_N(int MaximumReleases);
    }
}
