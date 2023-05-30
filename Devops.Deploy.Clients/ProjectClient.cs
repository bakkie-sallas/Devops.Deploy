using Devops.Deploy.Interfaces;
using Devops.Deploy.Interfaces.Clients;
using Thirdparty.Interfaces;

namespace Devops.Deploy.Clients
{
    /*The client will execute the methods for the injected object. 
     * The client just forms a little in between to not tightly couple 
     * millions of little methods strung through the calling libs 
     * to the object directly*/

    public class ProjectClient : IProjectClient
    {
        public List<IProject> Projects => projects;
        private List<IProject> projects;
        public ILogger Logger { get; set; }

       

        public ProjectClient(ILogger Logger)
        {
            this.Logger = Logger;
        }
        
        public  IProjectClient AssignProjects(string json, ITransform Transform)
        {
            projects = Transform.GetProjects(json);
            return this;
        }

        public void AssignReleasesToRelevantProject(IReleaseClient ReleaseClient, int MaximumReleases = -1)
        {
            projects.ForEach(project => { project.AssignReleases(ReleaseClient.Releases.Limit(MaximumReleases)); });
        }
        
    }
}