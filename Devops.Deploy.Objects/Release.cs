using Devops.Deploy.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http.Headers;
using Thirdparty.Interfaces;

namespace Devops.Deploy.Objects
{
    public class Release : IRelease
    {

        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Version { get; set; }
        public DateTime Created { get; set; }
        public List<IDeployment> Deployments => deployments;
        private List<IDeployment> deployments;

        public bool HasBeenReleased => Deployments.Any() && Deployments.All(deployment => deployment.HasValidEnvironment);

        public ILogger Logger { get; }
        public DateTime DeploymentOrCreated { get => this.Deployments!=null && this.Deployments.Any() == true ?
                                                this.Deployments.FirstOrDefault().DeployedAt :
                                                this.Created;
                                            }

        public Release()
        {
            deployments = new List<IDeployment>();
        }

        public Release(ILogger Logger) : this()
        {
            this.Logger = Logger;
        }

        public Release(IRelease Release, ILogger Logger) : this(Logger)
        {
            MapProperties(Release);
        }

        public IRelease AssignDeployment(List<IDeployment> Deployments)
        {
            deployments = Deployments.OrderByDescending(deployment=>deployment.DeployedAt).Where(deployment => deployment.ReleaseId == Id).ToList();
            deployments.ForEach(x => { Logger.Info($"Deployment {x.Id} is assigned to Release {Id}"); });
            return this;
        }
        public IRelease ValidateAssignedDeployments(List<IEnvironment> Environments)
        {
            this.Deployments.ForEach(deployment => deployment.ValidateEnvironments(Environments));
            return this;
        }

        public IRelease MapProperties(IRelease Release)
        {
            this.Id = Release.Id;
            this.ProjectId = Release.ProjectId;
            this.Version = Release.Version;
            this.Created = Release.Created;
            return this;
        }

    }
}