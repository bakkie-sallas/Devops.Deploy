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
        private string deploymentJSON;
        private string projectJSON;
        private string releaseJSON;
        private string environmentJSON;
        private string generalJSON;

        private int releaseLimiter;
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

        public IInstanceClient LoadJson(string GeneralJSON)
        {
            InitAll();
            return this;
        }

        public IInstanceClient LoadJson(string DeploymentJSON = null, string ProjectJSON = null, string ReleaseJSON = null, string EnvironmentJSON = null)
        {
            deploymentJSON= DeploymentJSON;
            projectJSON= ProjectJSON;
            releaseJSON= ReleaseJSON;
            environmentJSON= EnvironmentJSON;

            InitAll();
            return this;
        }
        public IInstanceClient WithReleaseLimiter(int MaximumReleases = -1)
        {
            releaseLimiter= MaximumReleases;
            ProjectClient.AssignReleasesToRelevantProject(ReleaseClient, releaseLimiter);
            EnvironmentClient.AssignDeploymentsToRelevantEnvironment(DeploymentClient, ReleaseClient, MaximumReleases);
            return this;
        }

        private void InitAll()
        {
            //arrange for releases
            Logger.Info("arrange for releases: Loading List");
            ReleaseClient.AssignReleases(releaseJSON, TransformClient.Transform);

            Logger.Info("arrange for deployments: Loading List");
            DeploymentClient.AssignDeployments(deploymentJSON, TransformClient.Transform);

            Logger.Info("arrange for Environments: Loading List");
            EnvironmentClient.AssignEnvironments(environmentJSON, TransformClient.Transform);

            Logger.Info("arrange for Projects: Loading List");
            ProjectClient.AssignProjects(projectJSON, TransformClient.Transform);


            ReleaseClient.AssignDeploymentsToRelevantRelease(DeploymentClient.SortDeployments()).SortReleases();
            ProjectClient.AssignReleasesToRelevantProject(ReleaseClient.SortReleases());
            EnvironmentClient.AssignDeploymentsToRelevantEnvironment(DeploymentClient.SortDeployments(), ReleaseClient.SortReleases());
        }
    }
}
