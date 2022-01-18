using Common.Models;

namespace Common.Services;

public interface IStorageService
{
    List<(string, HeadingsDataModel2)> HeadingModels { get; set; }
}