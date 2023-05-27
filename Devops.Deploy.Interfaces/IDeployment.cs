namespace Devops.Deploy.Interfaces
{
    internal interface IDeployment
    {
        string Id { get; set; }

        DateTime DeployedAt { get; set; }

        string ReleaseId { get; }
        string EnvironmentId { get; }

        IRelease Release { get; }

        bool Loaded { get; }

        bool Load(string JSON);
    }
}
