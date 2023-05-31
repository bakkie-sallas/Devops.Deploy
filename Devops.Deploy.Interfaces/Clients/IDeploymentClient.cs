using Thirdparty.Interfaces;

namespace Devops.Deploy.Interfaces.Clients
{
    public interface IDeploymentClient : IClient
    {
        public List<IDeployment> Deployments { get; }
        public IDeploymentClient AssignDeployments(string json, ITransform Transform);
        public IDeploymentClient AssignDeployments(List<IDeployment> Deployments);
        public IDeploymentClient SortDeployments();
    }
}
