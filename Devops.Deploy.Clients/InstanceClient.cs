using Devops.Deploy.Interfaces;
using Devops.Deploy.Interfaces.Clients;
using Thirdparty.Interfaces;

namespace Devops.Deploy.Clients
{
    public  class InstanceClient : IInstanceClient
    {
        public IDeploymentClient DeploymentClient { get; set; }
        public IProjectClient ProjectClient { get; set; }
        public IReleaseClient ReleaseClient { get; set; }
        public IEnvironmentClient EnvironmentClient { get; set; }
        public ITransformClient TransformClient { get; set; }
 
        public ILogger Logger { get; set; }

        public InstanceClient(ILogger Logger, ITransformClient TransformClient,IDeploymentClient DeploymentClient, IProjectClient ProjectClient, IReleaseClient ReleaseClient, IEnvironmentClient EnvironmentClient)
        {
            this.Logger = Logger;
            this.DeploymentClient = DeploymentClient;
            this.ProjectClient = ProjectClient;
            this.EnvironmentClient= EnvironmentClient;
            this.ReleaseClient = ReleaseClient;
            this.TransformClient = TransformClient;
        }

        public IInstanceClient LoadJson(string DeploymentJSON , string ProjectJSON, string ReleaseJSON, string EnvironmentJSON)
        {
            
            Logger.Info("Arrange for deployments: Loading List");
            DeploymentClient.AssignDeployments(DeploymentJSON, TransformClient.Transform);
            
            Logger.Info("Arrange for releases: Loading List");
            ReleaseClient.AssignReleases(ReleaseJSON, TransformClient.Transform);

            Logger.Info("Arrange for Projects: Loading List");
            ProjectClient.AssignProjects(ProjectJSON, TransformClient.Transform);

            Logger.Info("Arrange for Environments: Loading List");
            EnvironmentClient.AssignEnvironments(EnvironmentJSON, TransformClient.Transform);

            return AssignReleasesDeployments();
        }
        public IInstanceClient SetReleaseLimit(int MaximumReleases = -1)
        {
            EnvironmentClient.LimitDeployments(DeploymentClient.SortDeployments(), ReleaseClient.SortReleases(), MaximumReleases);
            ReleaseClient.AssignDeploymentsToRelevantRelease(DeploymentClient.SortDeployments()).SortReleases();
            ProjectClient.AssignReleasesToRelevantProject(ReleaseClient.SortReleases(),MaximumReleases);
           
            return this;
        }
        private IInstanceClient AssignReleasesDeployments()
        {
            ReleaseClient.AssignDeploymentsToRelevantRelease(DeploymentClient.SortDeployments()).SortReleases();
            ProjectClient.AssignReleasesToRelevantProject(ReleaseClient.SortReleases());
            EnvironmentClient.AssignDeploymentsToRelevantEnvironment(DeploymentClient.SortDeployments(), ReleaseClient.SortReleases());
            return this;
        }

    }
}
