using Common.Models;

namespace Common.Services;

public class HeadingsStorageService : IStorageService
{
    public List<HeadingModel> HeadingModels { get; set; } = new();
}