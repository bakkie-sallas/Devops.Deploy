namespace Devops.Deploy.Interfaces
{
    internal interface IEnvironmentDeployments : IEnvironment
    {
        List<IDeployment> Deployments { get; set; }
    }
}
