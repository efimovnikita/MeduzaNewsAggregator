using Common.Models;

namespace Common.Services;

public class HeadingsStorageService : IStorageService
{
    public List<(string, HeadingsDataModel2?)> HeadingModels { get; set; } = new();
}