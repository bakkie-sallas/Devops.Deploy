namespace Devops.Deploy.Interfaces.Clients
{
    public interface ITransformClient : IClient
    {
        public ITransform Transform { get; set; }
    }
}
