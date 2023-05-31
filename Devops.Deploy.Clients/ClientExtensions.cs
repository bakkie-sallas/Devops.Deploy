using Devops.Deploy.Interfaces;
using Thirdparty.Interfaces;

namespace Devops.Deploy.Clients
{
    public static class ClientExtensions
    {

        //We don't have enironment scope on these
        public static List<IRelease> Limit(this List<IRelease> Releases, int NumberOfReleases, ILogger Logger)
        {
            NumberOfReleases = NumberOfReleases < 0 || NumberOfReleases > Releases.Count() ? Releases.Count() : NumberOfReleases;
            Logger.Info($"Releases will be chopped to {NumberOfReleases}");
            return Releases.OrderByDescending(releases=>releases.DeploymentOrCreated).Take(NumberOfReleases).ToList();
        }

        public static List<IDeployment> Limit(this List<IDeployment> Deployments,int NumberOfReleases, string EnvironmentID, ILogger Logger)
        {
            Deployments = Deployments.Where(deployment => deployment.EnvironmentId == EnvironmentID).ToList();
            var ReleaseIds = Deployments.OrderByDescending(deployment => deployment.DeployedAt).Select(deployment => deployment.ReleaseId).Distinct().ToList();



            if (NumberOfReleases < 0 || NumberOfReleases > ReleaseIds.Count())
                NumberOfReleases = ReleaseIds.Count();
            else
            {
                Logger.Info($"Releases will be chopped to {NumberOfReleases}");
                //- `Release - 1` kept because it was the most recently deployed to `Environment - 1`*/
                ReleaseIds = ReleaseIds.Take(NumberOfReleases).ToList();
                ReleaseIds.ForEach(releaseId => Logger.Info($"- '{releaseId}' kept because it was the most recently deployed to '{EnvironmentID}'"));
            }

            return Deployments.Where(deployment => ReleaseIds.Contains(deployment.ReleaseId)).ToList();
        }
    }
}
