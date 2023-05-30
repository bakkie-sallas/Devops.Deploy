namespace Devops.Deploy.Interfaces
{
    public interface ITransform
    {
        public List<IDeployment> GetDeployments(string jsonString);
        public List<IEnvironment> GetEnvironments(string jsonString);

        public List<IProject> GetProjects(string jsonString);
        public List<IRelease> GetReleases(string jsonString);
    }
}
