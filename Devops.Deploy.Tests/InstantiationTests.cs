using Devops.Deploy.Clients;
using Devops.Deploy.Injectors;
using Devops.Deploy.Interfaces;
using Devops.Deploy.Interfaces.Clients;
using Devops.Deploy.Objects;
using Ninject;
using System.Net.Http.Headers;
using Thirdparty.Interfaces;

namespace Devops.Deploy.Tests
{
    [TestFixture]
    public class InstantiationTests
    {
        private static ILogger Logger;
        private StandardKernel DevopsDeployKernel;
        private StandardKernel ThirdPartyKernel;


        ITransformClient TransformClient;
        IReleaseClient ReleaseClient;
        IDeploymentClient DeploymentClient;
        IProjectClient ProjectClient;
        IEnvironmentClient EnvironmentClient;
        IInstanceClient InstanceClient;



        [SetUp]
        public void SetUp()
        {
            ThirdPartyKernel = new StandardKernel(new ThirdpartyInterfaceMapper());
            Logger = ThirdPartyKernel.Get<ILoggerClient>().Logger;

            DevopsDeployKernel = new StandardKernel(new InterfaceMapper(Logger));
            Logger.Info("Starting up Tests");


            TransformClient = DevopsDeployKernel.Get<ITransformClient>();

            ReleaseClient = DevopsDeployKernel.Get<IReleaseClient>();
            DeploymentClient = DevopsDeployKernel.Get<IDeploymentClient>();
            ProjectClient = DevopsDeployKernel.Get<IProjectClient>();
            EnvironmentClient = DevopsDeployKernel.Get<IEnvironmentClient>();

            InstanceClient = DevopsDeployKernel.Get<IInstanceClient>();

            //arrange for releases
            
            using StreamReader releasesFile = File.OpenText("JSONSource/Releases.json");
            var releasesJSON = releasesFile.ReadToEnd();

            Logger.Info("arrange for releases: Loading List");
            ReleaseClient.AssignReleases(releasesJSON, TransformClient.Transform);

            
            using StreamReader deploymentsFile = File.OpenText("JSONSource/Deployments.json");
            var deploymentsJSON = deploymentsFile.ReadToEnd();
           
            Logger.Info("arrange for deployments: Loading List");
            DeploymentClient.AssignDeployments(deploymentsJSON, TransformClient.Transform);

            
            using StreamReader environmentsFile = File.OpenText("JSONSource/Environments.json");
            var environmentJSON = environmentsFile.ReadToEnd();

            Logger.Info("arrange for Environments: Loading List");
            EnvironmentClient.AssignEnvironments(environmentJSON, TransformClient.Transform);

            
            using StreamReader projectFile = File.OpenText("JSONSource/Projects.json");
            var projectJSON = projectFile.ReadToEnd();

            Logger.Info("arrange for Projects: Loading List");
            ProjectClient.AssignProjects(projectJSON, TransformClient.Transform);

            InstanceClient.LoadJson(deploymentsJSON, projectJSON, releasesJSON, environmentJSON);


            ReleaseClient.AssignDeploymentsToRelevantRelease(DeploymentClient.SortDeployments()).SortReleases();
            ProjectClient.AssignReleasesToRelevantProject(ReleaseClient.SortReleases());
            EnvironmentClient.AssignDeploymentsToRelevantEnvironment(DeploymentClient.SortDeployments(), ReleaseClient.SortReleases());

            

            //Loaded Lists

        }
        private void LoadSeparateClients()
        { 
            
        }



        //[**Projects**](#project) can have zero or more [**releases**](#release), which can be released to an [**environment**](#environment) by creating a [**deployment**](#deployment).
        [Test]
        public void When_Project_Initiated_Must_Have_Zero_or_MOre_Releases()
        {

            //Releases cannot be null
            //Value to return Release Count

            Assert.IsNotNull(ProjectClient.Projects);
            Assert.IsNotNull(ProjectClient.Projects.FirstOrDefault().Releases);
            Assert.IsNotNull(ProjectClient.Projects.FirstOrDefault().RealeaseCount);
        }

        [Test]
        public void When_Lists_Loaded_Counts_Correct()
        {
            Assert.That(ReleaseClient.Releases.Count(), Is.EqualTo(8));
            Assert.That(ProjectClient.Projects.Count(), Is.EqualTo(2));
            Assert.That(EnvironmentClient.Environments.Count(), Is.EqualTo(2));
            Assert.That(DeploymentClient.Deployments.Count(), Is.EqualTo(10));
        }


        [Test]
        public void When_Deployment_Initialised_Must_Have_Environment_ID()
        {
            //Action to load applicable dpeloyments for every release
            ReleaseClient.Releases.ForEach(release => release.AssignDeployments(DeploymentClient.Deployments));

            //looking for any deployment that has been assigned to a release and matches an environment.
            var ReleaseWithDeploymentMatchingEnvironments = ReleaseClient.Releases.FirstOrDefault(release =>
            release.Deployments.Any(deployment =>
                EnvironmentClient.Environments.Any(environment => environment.Id == deployment.EnvironmentId)
                )
            );
            Assert.IsNotNull(ReleaseWithDeploymentMatchingEnvironments);
        }


        [Test]
        public void When_Deployment_Initialised_Must_Have_Environment_And_ReleaseScope()
        {
            //Action to load applicable dpeloyments for every release
            ReleaseClient.Releases.ForEach(release => release.AssignDeployments(DeploymentClient.Deployments));


            //looking for any deployment that has been assigned to a release and matches an environment.



            //Deployment 1 and 3 has same release 1
            //Deployment 2 and 4 has same release 2
            //Deployment 6,7 and 9 has same release 6
            //Deployment 5,8 and 10 hashas single different releases
            //total:6 deployments with matching releases.
            var DeploymentsMatchingReleases = ReleaseClient.Releases.Where(release =>
                release.Deployments.Any(deployment => deployment.ReleaseId == release.Id)
            );
            Assert.That(DeploymentsMatchingReleases.Count(), Is.EqualTo(6));

            //Ensure they all have environments loaded
            var DeploymentsMatchingReleasesWithEnvironments = ReleaseClient.Releases.Count(release =>
                release.Deployments.Any(deployment =>
                    EnvironmentClient.Environments.Any(environment => environment.Id == deployment.EnvironmentId)
                )
            );
            Assert.That(DeploymentsMatchingReleasesWithEnvironments, Is.EqualTo(6));

            // Releases without dpeloys: 3 and 4
            var DeploymentsNotMatchingReleases = ReleaseClient.Releases.Where(release =>
                !release.Deployments.Any()
            );

            Assert.That(DeploymentsNotMatchingReleases.Count(), Is.EqualTo(2));

        }


        [Test]
        public void When_Release_Instantiated_And_Deployment_Added_Then_Has_Been_Released()
        {
            //Loading Deployments. A deployments need Environments to be Valid
            ReleaseClient.Releases.ForEach(release => release.AssignDeployments(DeploymentClient.Deployments).ValidateAssignedDeployments(EnvironmentClient.Environments));


            //6 releases have deployments. Deployment 4 is assigned to release 2.It mentions environment 3, which is not in the list of Environments provided. We can not 
            //verify if it has been released. 5 Releases can be verified
            Assert.That(ReleaseClient.Releases.Count(release => release.HasBeenReleased), Is.EqualTo(5));
        }
        [Test]
        public void When_Release_Instantiated_And_NO_Deployment_Added_Then_Has_NOT_Been_Released()
        {
            //Loading Deployments. A deployments need Environments to be Valid
            ReleaseClient.Releases.ForEach(release => release.AssignDeployments(DeploymentClient.Deployments).ValidateAssignedDeployments(EnvironmentClient.Environments));


            //Deployment requires the release and the environment

            Assert.That(ReleaseClient.Releases.Count(release => !release.HasBeenReleased), Is.EqualTo(3));
        }

        [Test]
        /// /A **release** can have zero or more **deployments** for an **environment**.  
        public void When_Environment_Instantiated_Must_Have_ZeroOrMore_Deployments()
        {

            //Deployments cannot be null
            //Value to return Deployment Count
            Assert.IsNotNull(EnvironmentClient.Environments.FirstOrDefault().Deployments);
        }



        [Test]
        /// /A **release** can have zero or more **deployments** for an **environment**.  
        public void When_Environments_are_Instantiated_Must_Have_ZeroOrMore_Deployments()
        {

            //Deployments cannot be null
            //Value to return Deployment Count
            Assert.IsFalse(EnvironmentClient.Environments.Any(environment => environment.Deployments == null));
        }

        [Test]
        /// /A **release** can have zero or more **deployments** for an **environment**.  
        public void When_Projects_Are_Instantiated_Releases_Must_Have_ZeroOrMore_Deployments()
        {

            //Deployments cannot be null
            //Value to return Deployment Count
            Assert.IsFalse(ProjectClient.Projects.Any(project => project.Releases.Any(release => release.Deployments == null)));
        }


        [Test]
        //For each **project**, keep `n` **releases** that have most recently been deployed, where `n` is the number of releases to keep. 
        public void When_Project_Instantiated_N_Releases_Must_Be_Available()
        {
            //Requires Method to Load specific amount of releases
            Assert.IsNotNull(ProjectClient.Projects.Any(environment => environment.Releases.Any()));
        }

        [Test]
        //For each **environment** combination, keep `n` **releases** that have most recently been deployed, where `n` is the number of releases to keep. 
        public void When_Environment_Instantiated_N_Releases_Must_Be_Sorted_From_Most_Recent()
        {
            int NumberReleasesToTake = 5;

            EnvironmentClient.LimitDeployments(DeploymentClient, ReleaseClient, NumberReleasesToTake);
            var releasesAssigned = EnvironmentClient.Environments.SelectMany(environment => environment.Releases);
            Assert.Greater(GetApplicableDate(releasesAssigned.FirstOrDefault()), GetApplicableDate(releasesAssigned.LastOrDefault()));


            var MaximumReleasesAssignedTOEnvironment = EnvironmentClient.Environments.Max(environment => environment.Releases.Distinct().Count());

            Assert.That(MaximumReleasesAssignedTOEnvironment, Is.EqualTo(5));//Release-1 and Release-6 are present in both environments. Env 2 only has two releases. so 5 is max from first
        }


        [Test]
        //For each **project**, keep `n` **releases** that have most recently been deployed, where `n` is the number of releases to keep. 
        public void When_Project_Instantiated_N_Releases_Must_Be_Sorted_From_Most_Recent()
        {
            int NumberReleasesToTake = 5;

            ProjectClient.AssignReleasesToRelevantProject(ReleaseClient, NumberReleasesToTake);
            var releasesAssigned = ProjectClient.Projects.SelectMany(project => project.Releases);
            Assert.GreaterOrEqual(GetApplicableDate(releasesAssigned.FirstOrDefault()), GetApplicableDate(releasesAssigned.LastOrDefault()));

            var MaximumReleasesAssignedToProject = ProjectClient.Projects.SelectMany(project => project.Releases).Distinct().Count();
            Assert.That(MaximumReleasesAssignedToProject, Is.EqualTo(NumberReleasesToTake));
        }


        [Test]
        //For each **project**, keep `n` **releases** that have most recently been deployed, where `n` is the number of releases to keep. 
        public void When_Project_And_Environment_Instantiated_N_Releases_Must_Be_Available()
        {
            //Requires Method to Load specific amount of releases
            //Note That N is shared between Projects and Environments
            int NumberReleasesToTake = 4;
            InstanceClient.SetReleaseLimit(NumberReleasesToTake);
            InstanceAsserts(NumberReleasesToTake, NumberReleasesToTake);

        }
     

        [Test]
        /*#### Test Data
        | Project-1 | Environment-1 |
        | ------------- | ------------- |
        | `Release-1` (Version: `1.0.0`, Created: `2000-01-01T08:00:00`)  | `Deployment-1` (DeployedAt: `2000-01-01T10:00:00`) |
        
        ##### Expected Result
        
        - `Release-1` kept because it was the most recently deployed to `Environment-1`*/
        public void When_Given_1_Project_1_ENvironment_and_1_Release_Should_Return_1()
        {
            int numberToKeep = 1;
            var singleDeployment =  InstanceClient.DeploymentClient.Deployments.FirstOrDefault();
            singleDeployment.Id = "Deployment-1";
            singleDeployment.DeployedAt = Convert.ToDateTime("2000-01-01T10:00:00");
            singleDeployment.ReleaseId = "Release-1";
            singleDeployment.EnvironmentId = "Environment-1";
            InstanceClient.DeploymentClient.AssignDeployments(new List<IDeployment> { singleDeployment});

            var singleRelease = InstanceClient.ReleaseClient.Releases.FirstOrDefault();
            singleRelease.Id = "Release-1";
            singleRelease.ProjectId = "Project-1";
            singleRelease.Created = Convert.ToDateTime("2000-01-01T08:00:00");
            singleRelease.AssignDeployments(InstanceClient.DeploymentClient.Deployments);
            InstanceClient.ReleaseClient.AssignReleases(new List<IRelease> { singleRelease });

            var singleEnvironment = InstanceClient.EnvironmentClient.Environments.FirstOrDefault();
            singleEnvironment.Id = "Environment-1";
            singleEnvironment.AssignDeployments(InstanceClient.DeploymentClient.Deployments);
            singleEnvironment.AssignReleases(InstanceClient.ReleaseClient.Releases);
            InstanceClient.EnvironmentClient.AssignEnvironments(new List<IEnvironment> { singleEnvironment });

            var singleProject = InstanceClient.ProjectClient.Projects.FirstOrDefault();
            singleProject.Id = "Project-1";
            singleProject.AssignReleases(InstanceClient.ReleaseClient.Releases);
            InstanceClient.ProjectClient.AssignProjects(new List<IProject> { singleProject });

            InstanceClient.SetReleaseLimit(numberToKeep);
            InstanceAsserts(numberToKeep, numberToKeep);
            Assert.IsTrue(ScanLogFileFor(new List<string>() { @"'Release-1' kept because it was the most recently deployed to 'Environment-1'" }));
        }

        [Test]
        /*#### Test Case: 2 Releases deployed to the same environment, Keep 1

        ##### Test Data
        | Project-1 | Environment-1 |
        | ------------- | ------------- |
        | `Release-1` (Version: `1.0.0`, Created: `2000-01-01T08:00:00`)  | `Deployment-2` (DeployedAt: `2000-01-01T11:00:00`) |
        | `Release-2` (Version: `1.0.1`, Created: `2000-01-01T09:00:00`)  | `Deployment-1` (DeployedAt: `2000-01-01T10:00:00`) |

        ##### Expected Result

        - `Release-1` kept because it was the most recently deployed to `Environment-1`*/
        public void When_Given_1_Project_1_ENvironment_and_2_Release_Should_Return_1()
        {
            int numberToKeep = 1;
            var deployments = InstanceClient.DeploymentClient.Deployments.Take(2).ToList();
            deployments[0].Id = "Deployment-1";
            deployments[0].EnvironmentId = "Environment-1";
            deployments[0].DeployedAt = Convert.ToDateTime("2000-01-01T10:00:00");
            deployments[0].ReleaseId = "Release-2";

            deployments[1].Id = "Deployment-2";
            deployments[1].EnvironmentId = "Environment-1";
            deployments[1].DeployedAt = Convert.ToDateTime("2000-01-01T11:00:00");
            deployments[1].ReleaseId = "Release-1";
            InstanceClient.DeploymentClient.AssignDeployments(new List<IDeployment> { deployments[0], deployments[1] });

            var releases = InstanceClient.ReleaseClient.Releases.Take(2).ToList();
            releases[0].Id = "Release-1";
            releases[0].ProjectId = "Project-1";
            releases[0].Version = "1.0.1";
            releases[0].Created = Convert.ToDateTime("2000-01-01T08:00:00");
            releases[0].AssignDeployments(InstanceClient.DeploymentClient.Deployments);

            releases[1].Id = "Release-2";
            releases[1].ProjectId = "Project-1";
            releases[1].Version = "1.0.0";
            releases[1].Created = Convert.ToDateTime("2000-01-01T09:00:00");
            releases[1].AssignDeployments(InstanceClient.DeploymentClient.Deployments);



            InstanceClient.ReleaseClient.AssignReleases(new List<IRelease> { releases[0], releases[1] });
             
            var singleEnvironment = InstanceClient.EnvironmentClient.Environments.FirstOrDefault();
            singleEnvironment.Id = "Environment-1";
            singleEnvironment.AssignDeployments(InstanceClient.DeploymentClient.Deployments);
            singleEnvironment.AssignReleases(InstanceClient.ReleaseClient.Releases);
            InstanceClient.EnvironmentClient.AssignEnvironments(new List<IEnvironment> { singleEnvironment });

            var singleProject = InstanceClient.ProjectClient.Projects.FirstOrDefault();
            singleProject.Id = "Project-1";
            singleProject.AssignReleases(InstanceClient.ReleaseClient.Releases);
            InstanceClient.ProjectClient.AssignProjects(new List<IProject> { singleProject });

            InstanceClient.SetReleaseLimit(numberToKeep);
            InstanceAsserts(numberToKeep,numberToKeep);
            Assert.IsTrue(ScanLogFileFor(new List<string>() { @"'Release-1' kept because it was the most recently deployed to 'Environment-1'" }));

        }

        [Test]
        /*#### Test Case: 2 Releases deployed to different environments, Keep 1

        ##### Test Data
        | Project-1 | Environment-1 | Environment-2 |
        | ------------- | ------------- | ------------- |
        | `Release-1` (Version: `1.0.0`, Created: `2000-01-01T08:00:00`)  | | `Deployment-2` (DeployedAt: `2000-01-02T11:00:00`) -> release 2|
        | `Release-2` (Version: `1.0.1`, Created: `2000-01-01T09:00:00`)  | `Deployment-1` (DeployedAt: `2000-01-01T10:00:00`) -> release1 | |
        //this test data is incomplete because it does not actually specidy where the dpeloyments go. But I am working from the jsons provided to 
        //link them 
        ##### Expected Result

        - `Release-1` kept because it was the most recently deployed to `Environment-2`
        - `Release-2` kept because it was the most recently deployed to `Environment-1`*/
        public void When_Given_1_Project_2_ENvironment_and_2_Release_Should_Return_2()
        {
            int numberToKeep = 2;
            var deployments = InstanceClient.DeploymentClient.Deployments.Take(2).ToList();
            deployments[0].Id = "Deployment-1";
            deployments[0].EnvironmentId = "Environment-1";
            deployments[0].DeployedAt = Convert.ToDateTime("2000-01-01T10:00:00");
            deployments[0].ReleaseId = "Release-2";

            deployments[1].Id = "Deployment-2";
            deployments[1].EnvironmentId = "Environment-2";
            deployments[1].DeployedAt = Convert.ToDateTime("2000-01-01T11:00:00");
            deployments[1].ReleaseId = "Release-1";
            InstanceClient.DeploymentClient.AssignDeployments(new List<IDeployment> { deployments[0], deployments[1] });

            var releases = InstanceClient.ReleaseClient.Releases.Take(2).ToList();
            releases[0].Id = "Release-1";
            releases[0].ProjectId = "Project-1";
            releases[0].Version = "1.0.1";
            releases[0].Created = Convert.ToDateTime("2000-01-01T08:00:00");
            releases[0].AssignDeployments(InstanceClient.DeploymentClient.Deployments);

            releases[1].Id = "Release-2";
            releases[1].ProjectId = "Project-1";
            releases[1].Version = "1.0.0";
            releases[1].Created = Convert.ToDateTime("2000-01-01T09:00:00");
            releases[1].AssignDeployments(InstanceClient.DeploymentClient.Deployments);



            InstanceClient.ReleaseClient.AssignReleases(new List<IRelease> { releases[0], releases[1] });

            var environments = InstanceClient.EnvironmentClient.Environments.Take(2).ToList();
            environments[0].Id = "Environment-1";
            environments[0].AssignDeployments(InstanceClient.DeploymentClient.Deployments);
            environments[0].AssignReleases(InstanceClient.ReleaseClient.Releases);

            environments[1].Id = "Environment-2";
            environments[1].AssignDeployments(InstanceClient.DeploymentClient.Deployments);
            environments[1].AssignReleases(InstanceClient.ReleaseClient.Releases);

            InstanceClient.EnvironmentClient.AssignEnvironments(new List<IEnvironment> { environments[0], environments[1] });

            var singleProject = InstanceClient.ProjectClient.Projects.FirstOrDefault();
            singleProject.Id = "Project-1";
            singleProject.AssignReleases(InstanceClient.ReleaseClient.Releases);
            InstanceClient.ProjectClient.AssignProjects(new List<IProject> { singleProject });


            int NumberReleasesToValidateForEnvironment = 1;// Because there is one per environment
            int NumberReleasesToValidateForProject = 2;// Because there is one per environment
            InstanceClient.SetReleaseLimit(numberToKeep);
            InstanceAsserts(NumberReleasesToValidateForEnvironment, NumberReleasesToValidateForProject);
            Assert.IsTrue(ScanLogFileFor(new List<string>() { @"'Release-1' kept because it was the most recently deployed to 'Environment-2'",
            @"'Release-2' kept because it was the most recently deployed to 'Environment-1'" }));

        }




        private void InstanceAsserts(int NumberReleasesToValidateForEnvironment, int NumberReleasesToValidateForProject)
        {
            //Environment Date comparison
            InstanceClient.EnvironmentClient.Environments.ForEach(environment =>
            {
                Assert.GreaterOrEqual(
                    GetApplicableDate(environment.Releases.FirstOrDefault()),
                    GetApplicableDate(environment.Releases.LastOrDefault()));
            });


            var MaximumReleasesAssignedTOEnvironment = InstanceClient.EnvironmentClient.Environments.Max(environment => environment.Releases.Count());
            Assert.That(MaximumReleasesAssignedTOEnvironment, Is.EqualTo(NumberReleasesToValidateForEnvironment));


            //Project Date comparison
            InstanceClient.ProjectClient.Projects.ForEach(project =>
            {
                Assert.GreaterOrEqual(
                    GetApplicableDate(project.Releases.FirstOrDefault()),
                    GetApplicableDate(project.Releases.LastOrDefault()));
            });

            var MaximumReleasesAssignedToProject = InstanceClient.ProjectClient.Projects.SelectMany(project => project.Releases).Distinct().Count();
            Assert.That(MaximumReleasesAssignedToProject, Is.EqualTo(NumberReleasesToValidateForProject));
        }
        private DateTime GetApplicableDate(IRelease Release)
        {
            if (Release == null)
                return new DateTime();
            if (!Release.Deployments.Any())
                return Release.Created;

            return Release.Deployments.FirstOrDefault().DeployedAt;
        }


        private bool ScanLogFileFor(List<string> searchTexts)
        {
            var retVal = false;
            var logLines = File.ReadLines(
                InstanceClient.Logger.CompleteLog().Replace("${level}","info")).Reverse().ToList();
            searchTexts.ForEach(searchText =>
            {
                foreach (var logLine in logLines)
                {
                    if (logLine.Contains(searchText))
                    {
                        retVal = true;
                        break;
                    }
                }
            });
            return retVal;
        }
    }
}