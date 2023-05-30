namespace Devops.Deploy.Interfaces
{
    public interface IProject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<IRelease> Releases { get; }
        public Int16? RealeaseCount { get; }
        public IProject MapProperties(IProject Project);
        public IProject AssignReleases(List<IRelease> Releases);


    }

}
