namespace Devops.Deploy.Interfaces
{
    public interface IDeployment
    {
        public string Id { get; set; }

        public DateTime DeployedAt { get; set; }

        public string ReleaseId { get; }
        public string EnvironmentId { get; }

        public bool HasValidEnvironment { get; set; }
     
        public void ValidateEnvironments(List<IEnvironment> AvailableEnvironments);
        public IDeployment MapProperties(IDeployment Deployment);
    

    }
}
