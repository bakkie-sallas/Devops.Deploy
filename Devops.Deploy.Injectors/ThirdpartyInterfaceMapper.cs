using Thirdparty.Interfaces;
using Ninject.Modules;
using Nlog.Objects;
using Devops.Deploy.Clients;
using Devops.Deploy.Interfaces.Clients;
using NLog;

namespace Devops.Deploy.Injectors
{

    /*Mapper will map objects to their corresponding interfaces
     * we will call the kernel to find these mappings to instantiate these */

    public class ThirdpartyInterfaceMapper : NinjectModule
    {
        public override void Load()
        {
            
            this.Bind<Thirdparty.Interfaces.ILogger>().To<NLogger>().WithConstructorArgument("logConfig", "nlog.config");
            this.Bind<ILoggerClient>().To<LoggerClient>();
        }
    }
}