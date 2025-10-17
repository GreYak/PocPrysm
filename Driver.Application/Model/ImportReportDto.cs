
namespace Driver.Application.Model
{
    public record ImportReportDto (int ExternalDevicesCount, int NewAppVariablesCount, int UpdatedAppVariablesCount)
    {
        public string Summary => $"{ExternalDevicesCount} devices have been found to create {NewAppVariablesCount} variables into AppVision and to update {UpdatedAppVariablesCount} others.";
    }
}
