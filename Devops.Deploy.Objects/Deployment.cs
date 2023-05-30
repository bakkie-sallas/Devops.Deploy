using Devops.Deploy.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using Thirdparty.Interfaces;

namespace Devops.Deploy.Objects
{
    public class Deployment : IDeployment
    {
        public string Id { get; set; }
        public DateTime DeployedAt { get; set; }

        public string ReleaseId { get; set; }

        public string EnvironmentId { get; set; }

        public bool HasValidEnvironment { get; set; }
        public ILogger Logger { get; }

        public Deployment()
        {

        }

        public Deployment(ILogger Logger):this()
        {
            this.Logger = Logger;
        }

        public Deployment(IDeployment Deployment, ILogger Logger) : this(Logger)
        {
            MapProperties(Deployment);
        }

        public void ValidateEnvironments( List<IEnvironment> AvailableEnvironments)
        { 
            HasValidEnvironment = AvailableEnvironments.Any(environment=> environment.Id== EnvironmentId);
            Logger.Info($"Deployment {Id} is assigned to Environment {EnvironmentId}. Validity:: {HasValidEnvironment}");
        }

        public IDeployment MapProperties(IDeployment Deployment)
        {
            this.Id = Deployment.Id;
            this.DeployedAt= Deployment.DeployedAt;
            this.ReleaseId = Deployment.ReleaseId;
            this.EnvironmentId = Deployment.EnvironmentId;
            this.HasValidEnvironment= Deployment.HasValidEnvironment;
            return this;
        }
     
    }
}