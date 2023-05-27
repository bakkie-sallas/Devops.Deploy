namespace Devops.Deploy.Interfaces
{
    internal interface IProjectReleases:IProject
    {
        List<IRelease> Releases { get; set; }
    }
}
