using Devops.Deploy.Interfaces;
using Devops.Deploy.Interfaces.Clients;
using Thirdparty.Interfaces;

namespace Devops.Deploy.Clients
{
    /*The client will execute the methods for the injected object. 
     * The client just forms a little in between to not tightly couple 
     * millions of little methods strung through the calling libs 
     * to the object directly*/

    public class DeploymentClient : IDeploymentClient
    {
        public List<IDeployment> Deployments => deployments;
        private List<IDeployment> deployments;
        public ILogger Logger { get; set; }



        public DeploymentClient(ILogger Logger)
        {
            this.Logger = Logger;
        }
        public IDeploymentClient AssignDeployments(string json, ITransform Transform)
        {
            deployments = Transform.GetDeployments(json);
            return this;
        }

        public IDeploymentClient SortDeployments()
        {
            deployments = deployments.OrderByDescending(deployment => deployment.DeployedAt).ToList();
            return this;
        }
    }
}