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
            MapProperties(Deployment.Id, Deployment.DeployedAt, Deployment.ReleaseId, Deployment.EnvironmentId);
        }
        public Deployment(string Id, DateTime DeployedAt, string ReleaseId, string EnvironmentId)
        {
            MapProperties(Id, DeployedAt, ReleaseId, EnvironmentId);
        }
        public void ValidateEnvironments( List<IEnvironment> AvailableEnvironments)
        { 
            HasValidEnvironment = AvailableEnvironments.Any(environment=> environment.Id== EnvironmentId);
            Logger.Info($"Deployment {Id} is assigned to Environment {EnvironmentId}. Validity:: {HasValidEnvironment}");
        }

        private IDeployment MapProperties(string Id, DateTime DeployedAt, string ReleaseId, string EnvironmentId)
        {
            this.Id = Id;
            this.DeployedAt= DeployedAt;
            this.ReleaseId = ReleaseId;
            this.EnvironmentId = EnvironmentId;
            return this;
        }
     
    }
}