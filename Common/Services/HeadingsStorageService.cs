using Common.Models;

namespace Common.Services;

public class HeadingsStorageService : IStorageService
{
    public List<(string category, HeadingsDataModel? model)> HeadingModels { get; set; } = new();
}