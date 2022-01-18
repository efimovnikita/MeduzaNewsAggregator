using Common.Models;

namespace Common.Services;

public interface IStorageService
{
    List<(string category, HeadingsDataModel2? model)> HeadingModels { get; set; }
}