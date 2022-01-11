using Common.Models;

namespace Common.Services;

public interface IStorageService
{
    List<HeadingModel> HeadingModels { get; set; }
}