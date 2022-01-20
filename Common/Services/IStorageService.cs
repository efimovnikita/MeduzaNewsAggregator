using Common.Models;

namespace Common.Services;

public interface IStorageService
{
    List<(string category, HeadingsDataModel? model)> HeadingModels { get; set; }
}