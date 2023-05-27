using Devops.Deploy.Interfaces;
namespace Devops.Deploy.Tests
{
    [TestFixture]
    public class InstantiationTests
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [SetUp]
        public void SetUp()
        {
            LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("NLog.config");

            logger.Info("Starting up Tests");
        }

        //[**Projects**](#project) can have zero or more [**releases**](#release), which can be released to an [**environment**](#environment) by creating a [**deployment**](#deployment).
        [Test]
        public void When_Project_Initiated_MustHave_Zero_or_MOre_Releases()
        {
            IProject Project;
            
            
            //Releases cannot be null
            //Value to return Release Count



        }
        [Test]
        public void When_Project_Initiated_Can_Create_Deployment()
        {
            //Needs CreateDeployment method
        }
        [Test]
        public void When_Deployment_Instantiated_Then_Can_Release_To_Environment()
        {
            //Deployment requires the release and the environment
        }
        [Test]
        /// /A **release** can have zero or more **deployments** for a **project**
        public void When_Project_Instantiated_Releases_Must_Have_ZeroOrMore_Deployments()
        {
            //Deployments cannot be null
            //Value to return Deployment Count
        }
        [Test]
        /// /A **release** can have zero or more **deployments** for an **environment**.  
        public void When_Environment_Instantiated_Releases_Must_Have_ZeroOrMore_Deployments()
        {
            //Deployments cannot be null
            //Value to return Deployment Count
        }
        [Test]
        //For each **environment** combination, keep `n` **releases** that have most recently been deployed, where `n` is the number of releases to keep. 
        public void When_Environment_Instantiated_N_Releases_Must_Be_Available()
        {
            //Requires Method to Load specific amount of releases
        }
        [Test]
        //For each **project**, keep `n` **releases** that have most recently been deployed, where `n` is the number of releases to keep. 
        public void When_Project_Instantiated_N_Releases_Must_Be_Available()
        {
            //Requires Method to Load specific amount of releases
        }

        [Test]
        //For each **environment** combination, keep `n` **releases** that have most recently been deployed, where `n` is the number of releases to keep. 
        public void When_Environment_Instantiated_Releases_Must_Be_Sorted_From_Most_Recent()
        {
            //check that previous item is later than current
        }

        [Test]
        //For each **project**, keep `n` **releases** that have most recently been deployed, where `n` is the number of releases to keep. 
        public void When_Project_Instantiated_N_Releases_Must_Be_Sorted_From_Most_Recent()
        {
            //check that previous item is later than current
        }


        [Test]
        //For each **project**, keep `n` **releases** that have most recently been deployed, where `n` is the number of releases to keep. 
        public void When_Project_And_Environment_Instantiated_N_Releases_Must_Be_Available()
        {
            //Requires Method to Load specific amount of releases
            //Note That N is shared between Projects and Environments
            //can probably share a static list of releases
        }

        [Test]
        //note: A **release** is considered to have "been deployed" if the release _has one or more_ **deployments**.
        public void When_Release_Instantiated_With_Deployments_Is_Deployed()
        {
            //Requires IsDeployed boolean
        }

        [Test]
        //note: A **release** is considered to have "been deployed" if the release _has one or more_ **deployments**.
        public void When_Release_Instantiated_WithOut_Deployments_Is_Not_Deployed()
        {
            //Requires IsDeployed boolean
        }


        [Test]
        /*#### Test Data
        | Project-1 | Environment-1 |
        | ------------- | ------------- |
        | `Release-1` (Version: `1.0.0`, Created: `2000-01-01T08:00:00`)  | `Deployment-1` (DeployedAt: `2000-01-01T10:00:00`) |
        
        ##### Expected Result
        
        - `Release-1` kept because it was the most recently deployed to `Environment-1`*/
        public void Test1()
        {

        }



        /*#### Test Case: 2 Releases deployed to the same environment, Keep 1

        ##### Test Data
        | Project-1 | Environment-1 |
        | ------------- | ------------- |
        | `Release-1` (Version: `1.0.0`, Created: `2000-01-01T08:00:00`)  | `Deployment-2` (DeployedAt: `2000-01-01T11:00:00`) |
        | `Release-2` (Version: `1.0.1`, Created: `2000-01-01T09:00:00`)  | `Deployment-1` (DeployedAt: `2000-01-01T10:00:00`) |

        ##### Expected Result

        - `Release-1` kept because it was the most recently deployed to `Environment-1`*/


        /*#### Test Case: 2 Releases deployed to different environments, Keep 1

        ##### Test Data
        | Project-1 | Environment-1 | Environment-2 |
        | ------------- | ------------- | ------------- |
        | `Release-1` (Version: `1.0.0`, Created: `2000-01-01T08:00:00`)  | | `Deployment-2` (DeployedAt: `2000-01-02T11:00:00`) |
        | `Release-2` (Version: `1.0.1`, Created: `2000-01-01T09:00:00`)  | `Deployment-1` (DeployedAt: `2000-01-01T10:00:00`) | |

        ##### Expected Result

        - `Release-1` kept because it was the most recently deployed to `Environment-2`
        - `Release-2` kept because it was the most recently deployed to `Environment-1`*/

    }
}