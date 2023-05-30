using Devops.Deploy.Interfaces;
using Newtonsoft.Json;
using Thirdparty.Interfaces;

namespace Devops.Deploy.Objects
{
    public class Transform : ITransform
    {
        ILogger Logger;

        public Transform(ILogger Logger) 
        { 
            this.Logger = Logger; 
        }
        public List<IDeployment> GetDeployments(string jsonString)
        {
            Logger.Info("Converting json to Deploy list");
            var jsonItems = JsonConvert.DeserializeObject<List<Deployment>>(jsonString);
            var objList = jsonItems.Select(jsonItem => new Deployment(jsonItem, Logger)).ToList<IDeployment>();
            return objList; throw new NotImplementedException();
        }

        public List<IEnvironment> GetEnvironments(string jsonString)
        {
            Logger.Info("Converting json to Environment list");
            var jsonItems = JsonConvert.DeserializeObject<List<Environment>>(jsonString);
            var objList = jsonItems.Select(jsonItem => new Environment(jsonItem, Logger)).ToList<IEnvironment>();
            return objList;
        }

        public List<IProject> GetProjects(string jsonString)
        {
            Logger.Info("Converting json to Project list");
            var jsonItems = JsonConvert.DeserializeObject<List<Project>>(jsonString);
            var objList = jsonItems.Select(jsonItem => new Project(jsonItem, Logger)).ToList<IProject>();
            return objList;
        }

        public List<IRelease> GetReleases(string jsonString)
        {
            Logger.Info("Converting json to Release list");
            var jsonItems = JsonConvert.DeserializeObject<List<Release>>(jsonString);
            var objList = jsonItems.Select(jsonItem => new Release(jsonItem, Logger)).ToList<IRelease>();
            return objList;
        }
    }
}
