namespace Devops.Deploy.Interfaces
{
    public interface IRelease
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Version { get; set; }
        public DateTime Created { get; set; }


        public DateTime DeploymentOrCreated { get;}
        public List<IDeployment> Deployments { get;  }
        public bool HasBeenReleased { get; }
        public IRelease AssignDeployment(List<IDeployment> Deployments);
        public IRelease ValidateAssignedDeployments(List<IEnvironment> Environments);

        public IRelease MapProperties(IRelease Release);
       
    }
}
