namespace Devops.Deploy.Interfaces
{
    internal interface IRelease
    {
        string Id { get; set; }
        string ProjectId { get; set; }
        string Version { get; set; }
        DateTime Created { get; set; }

        IProject Project { get;}
    }
}
