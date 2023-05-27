using Devops.Deploy.Interfaces;
using Devops.Deploy.Objects;
using Ninject.Modules;

namespace Devops.Deploy.Injectors
{

    /*Mapper will map objects to their corresponding interfaces
     * we will call the kernel to find these mappings to instantiate these */

    public class InterfaceMapper : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IProject>().To<Project>();
        }
    }
}