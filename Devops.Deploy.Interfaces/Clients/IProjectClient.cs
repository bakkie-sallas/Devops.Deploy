using Thirdparty.Interfaces;

namespace Devops.Deploy.Interfaces.Clients
{
    public interface IProjectClient : IClient
    {
        public List<IProject> Projects { get; }
        

        public IProjectClient AssignProjects(string json, ITransform Transform);
        public IProjectClient AssignProjects(List<IProject> Projects);
        public void AssignReleasesToRelevantProject(IReleaseClient ReleaseClient, int MaximumReleases = -1);
        
    }
}
