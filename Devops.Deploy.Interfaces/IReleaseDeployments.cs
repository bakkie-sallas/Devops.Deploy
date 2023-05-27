namespace Devops.Deploy.Interfaces
{
    internal interface IReleaseDeployments:IRelease
    {
        List<IDeployment> Deployments { get; set; }
    }
}
