using Devops.Deploy.Interfaces;
using Newtonsoft.Json;
using Thirdparty.Interfaces;

namespace Devops.Deploy.Objects
{
    public class Environment : IEnvironment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<IDeployment> Deployments  => deployments;
        public List<IRelease> Releases => releases;


        private List<IDeployment> deployments;
        private List<IRelease> releases;
        private List<string> releaseIds;
        public ILogger Logger { get; }
        public Environment()
        {

        }

        public Environment(ILogger Logger):this()
        {
            this.Logger = Logger;
        }

        public Environment(IEnvironment Environment, ILogger Logger) : this(Logger)
        {
            
            MapProperties(Environment);
        }
       
        public IEnvironment MapProperties(IEnvironment Environment)
        {
            this.Id = Environment.Id;
            this.Name = Environment.Name;
            deployments = Environment.Deployments;
            return this;
        }

   
        public IEnvironment AssignDeploymentsAndReleases(List<IDeployment> Deployments, List<IRelease> Releases)
        {
            Logger.Info($"Assigning Deployments to Environment:: {this.Id}");
            deployments = Deployments.Where(deployment=> deployment.EnvironmentId == this.Id).ToList();
            Logger.Info($"Assigning Releases to Deployments:: {this.Id}");
            releases = Releases.Where(release => deployments.Select(deployment => deployment.ReleaseId).Contains(release.Id)).ToList();
            return this;
        }
    }
}

  


