using Thirdparty.Interfaces;

namespace Devops.Deploy.Interfaces.Clients
{
    public interface IProjectClient : IClient
    {
        public List<IProject> Projects { get; }
        

        public IProjectClient AssignProjects(string json, ITransform Transform);
        public void AssignReleasesToRelevantProject(List<IRelease> Releases);
        
    }
}
