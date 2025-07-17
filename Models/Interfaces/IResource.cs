namespace YssWebstoreApi.Models.Interfaces
{
    public interface IResource : IEntity
    {
        long Size { get; set; }
        string Path { get; set; }
    }
}
