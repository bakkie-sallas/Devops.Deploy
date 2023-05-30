using Devops.Deploy.Clients;
using Devops.Deploy.Interfaces;
using Devops.Deploy.Interfaces.Clients;
using Devops.Deploy.Objects;
using Ninject;
using Ninject.Modules;
using Thirdparty.Interfaces;

namespace Devops.Deploy.Injectors
{

    /*Mapper will map objects to their corresponding interfaces
     * we will call the kernel to find these mappings to instantiate these */

    public class InterfaceMapper : NinjectModule
    {
        ILogger Logger;
        public InterfaceMapper(ILogger Logger) { this.Logger = Logger; }

        public override void Load()
        {
            this.Bind<ITransform>().To<Transform>().WithConstructorArgument(Logger);

            this.Bind<IProject>().To<Project>().WithConstructorArgument(Logger);
            this.Bind<IDeployment>().To<Deployment>().WithConstructorArgument(Logger);
            this.Bind<IRelease>().To<Release>().WithConstructorArgument(Logger);
            this.Bind<IEnvironment>().To<Objects.Environment>().WithConstructorArgument(Logger);

            /*Suggestions:
  1) Ensure that you have not declared a dependency for IRelease on any implementations of the service.
  2) Consider combining the services into a single one to remove the cycle.
  3) Use property injection instead of constructor injection, and implement IInitializable
     if you need initialization logic to be run after property values have been injected.
'*/

            this.Bind<ITransformClient>().To<TransformClient>().WithConstructorArgument(Kernel.Get<ITransform>());


            this.Bind<IDeploymentClient>().To<DeploymentClient>().WithConstructorArgument(Logger);
            this.Bind<IReleaseClient>().To<ReleaseClient>().WithConstructorArgument(Logger);
            this.Bind<IEnvironmentClient>().To<EnvironmentClient>().WithConstructorArgument(Logger);
            this.Bind<IProjectClient>().To<ProjectClient>().WithConstructorArgument(Logger);



            var DeploymentClient = Kernel.Get<IDeploymentClient>();
            var ReleaseClient = Kernel.Get<IReleaseClient>();
            var EnvironmentClient = Kernel.Get<IEnvironmentClient>();
            var ProjectClient = Kernel.Get<IProjectClient>();


            this.Bind<IInstanceClient>().To<InstanceClient>()
                .WithConstructorArgument(Logger)
                .WithConstructorArgument(DeploymentClient)
                .WithConstructorArgument(ProjectClient)
                .WithConstructorArgument(ReleaseClient)
                .WithConstructorArgument(EnvironmentClient);
        }
    }
}