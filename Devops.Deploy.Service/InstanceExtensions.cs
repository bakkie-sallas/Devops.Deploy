using Devops.Deploy.Interfaces;
using Newtonsoft.Json;

namespace Devops.Deploy.Service
{
    public static class InstanceExtensions
    {
        public static IEnumerable<IRelease> Releases { get;set; }
    }
}