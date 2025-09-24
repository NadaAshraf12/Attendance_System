namespace CleanArch.Domain.Repositories.Command.Base
{
    public interface ISoftDeleteRepository<T> where T : class
    {
        Task SoftDeleteAsync(Guid id);
    }
}
