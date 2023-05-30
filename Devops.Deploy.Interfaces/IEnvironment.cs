namespace Devops.Deploy.Interfaces
{
    public interface IEnvironment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<IDeployment> Deployments { get;  }
        public List<IRelease> Releases { get; }
        public IEnvironment MapProperties(IEnvironment Environment);
        public IEnvironment AssignDeploymentsAndReleases(List<IDeployment> Deployments, List<IRelease> Releases);


    }
}
