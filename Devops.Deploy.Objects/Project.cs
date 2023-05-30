using Devops.Deploy.Interfaces;
using System.Linq;
using Thirdparty.Interfaces;

namespace Devops.Deploy.Objects
{
    public class Project : IProject

    {
        
        public string Id { get; set; }
        public string Name { get; set; }
        public List<IRelease> Releases => releases;
        public ILogger Logger { get; }


        private List<IRelease> releases;
        
        public short? RealeaseCount => Releases==null?null:(short?)Releases.Count;
        public Project()
        {

        }

        public Project(ILogger Logger):this()
        {
            this.Logger = Logger;
        }

        public Project(IProject Project, ILogger Logger) : this(Logger)
        {
            MapProperties(Project);
        }
        

        public IProject MapProperties(IProject Project)
        {
            this.Id=Project.Id;
            this.Name=Project.Name;
            return this;
        }

        public IProject AssignReleases(List<IRelease> Releases)
        {
            Logger.Info($"Assigning Releases to Project:: {this.Id}");

            releases = Releases.Where(release => release.ProjectId == this.Id).ToList();
            return this;
        }

    }
}