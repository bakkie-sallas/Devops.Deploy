using Devops.Deploy.Interfaces;
using Devops.Deploy.Interfaces.Clients;
using Thirdparty.Interfaces;

namespace Devops.Deploy.Clients
{
    /*The client will execute the methods for the injected object. 
     * The client just forms a little in between to not tightly couple 
     * millions of little methods strung through the calling libs 
     * to the object directly*/

    public class ReleaseClient : IReleaseClient
    {
        public List<IRelease> Releases => releases;
        private List<IRelease> releases;
        public ILogger Logger { get; set; }

        public ReleaseClient(ILogger Logger)
        {
            this.Logger = Logger;
        }
        
        
        
        public IReleaseClient AssignReleases(string json, ITransform Transform)
        {
            AssignReleases(Transform.GetReleases(json));
            return this;
        }
        public IReleaseClient AssignReleases(List<IRelease> Releases)
        {
            releases = Releases.OrderByDescending(release => release.DeploymentOrCreated).ToList();
            return this;
        }

        public IReleaseClient AssignDeploymentsToRelevantRelease(IDeploymentClient DeploymentClient)
        {
            if (releases == null)
                throw new Exception("There are no releases to assign the deployments to");

            releases.ForEach(release => { release.AssignDeployments(DeploymentClient.Deployments); });
            return this;
        }

        public IReleaseClient SortReleases()
        {
            releases = releases.OrderByDescending(release => release.DeploymentOrCreated).ToList();
            return this;
        }

       
        public List<IRelease> GetLatest_N(List<IRelease> Releases,int MaximumReleases)
        {
            releases = releases.Limit(MaximumReleases, Logger);
            return releases;
        }

    }
}