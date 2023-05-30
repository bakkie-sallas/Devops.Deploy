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
            releases = Transform.GetReleases(json).OrderByDescending(release => release.Deployments.FirstOrDefault()).ToList();
            return this;
        }

        public IReleaseClient AssignDeploymentsToRelevantRelease(List<IDeployment> Deployments)
        {
            if (releases == null)
                throw new Exception("There are no releases to assign the deployments to");

            releases.ForEach(release => { release.AssignDeployment(Deployments); });
            return this;
        }

        public IReleaseClient SortReleases()
        {
            releases = releases.OrderByDescending(release => release.Deployments.FirstOrDefault()).ToList();
            return this;
        }
        public IReleaseClient GetLatest_N(int MaximumReleases)
        {
            releases = releases.Take(MaximumReleases).ToList();
            return this;
        }

    }
}