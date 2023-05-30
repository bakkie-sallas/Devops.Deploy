﻿using Devops.Deploy.Interfaces;
using Devops.Deploy.Interfaces.Clients;
using Thirdparty.Interfaces;

namespace Devops.Deploy.Clients
{
    /*The client will execute the methods for the injected object. 
     * The client just forms a little in between to not tightly couple 
     * millions of little methods strung through the calling libs 
     * to the object directly*/

    public class EnvironmentClient : IEnvironmentClient
    {
        public List<IEnvironment> Environments => environments;
        private List<IEnvironment> environments;
        public ILogger Logger { get; set; }



        public EnvironmentClient(ILogger Logger)
        {
            this.Logger = Logger;
        }
        public IEnvironmentClient AssignEnvironments(string json, ITransform Transform)
        {
            environments = Transform.GetEnvironments(json); 
            return this;
        }
        public void AssignDeploymentsToRelevantEnvironment(IDeploymentClient DeploymentClient, IReleaseClient ReleaseClient, int MaximumReleases = -1)
        {
            environments.ForEach(environment => { environment.AssignDeployments(DeploymentClient.Deployments.Limit(MaximumReleases,Logger)).AssignReleases(ReleaseClient.Releases); });
        }

    }
}