using Devices;
using Devices.Repositories;
using Driver.Infrastructure.AppVision;

namespace Driver.Infrastructure
{
    // TODO 
    internal class AppVisionDataAccess : IAppVariablesRootRepository
    {
        private readonly SDK _sdk;
        public AppVisionDataAccess() 
        {
            _sdk = new SDK();
         }

        public Task<AppVariablesRoot> GetVariablesRootAsync()
        {
            return Task.FromResult(new AppVariablesRoot(new List<AppVariable>()));
        }

        public Task SaveAsync(AppVariablesRoot appVariablesRoot)
        {
            _sdk.DeleteAllVariable();

            foreach (var variableRow in appVariablesRoot.Variables)
            {
                _sdk.Create(VariableRow.ToAppVisionModel(variableRow));
            }

            return Task.CompletedTask;
        }
    }
}
