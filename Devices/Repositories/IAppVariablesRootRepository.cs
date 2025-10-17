namespace Devices.Repositories
{
    public interface IAppVariablesRootRepository
    {
        Task<AppVariablesRoot> GetVariablesRootAsync();
        Task SaveAsync(AppVariablesRoot appVariablesRoot);
    }
}
